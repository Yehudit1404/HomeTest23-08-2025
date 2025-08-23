using System.ComponentModel.DataAnnotations;
using Contacts.Api.Data;
using Contacts.Api.DTOs;
using Contacts.Api.Models;
using Contacts.Api.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// EF InMemory כדי שהפרויקט ירוץ בלי DB אמיתי
builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("contacts-db"));

// דו"ח חודשי (סימולציית Stored Procedure)
builder.Services.AddScoped<IMonthlyReportService, MonthlyReportService>();

// Swagger + סכימת אבטחה ל-API Key
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contacts API", Version = "v1" });

    // הגדרת API Key בכותרת X-Api-Key
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Enter the API key (e.g., dev-secret). The header name is X-Api-Key.",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS – התאם לפי הצורך (למשל פרונט מקומי)
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowLocal", p =>
        p.WithOrigins("http://localhost:5173", "http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// API Key (בדמו: appsettings.json; ברירת מחדל dev-secret)
var apiKey = builder.Configuration["ApiKeys:Primary"] ?? "dev-secret";

var app = builder.Build();

// Error Handling בסיסי (ProblemDetails)
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async ctx =>
    {
        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var problem = Results.Problem(
            title: "Unexpected error",
            statusCode: StatusCodes.Status500InternalServerError,
            instance: ctx.Request.Path,
            extensions: new Dictionary<string, object?> { ["traceId"] = ctx.TraceIdentifier }
        );
        await problem.ExecuteAsync(ctx);
    });
});

app.UseCors("AllowLocal");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// פעולות כתיבה דורשות X-Api-Key
static bool IsWriteMethod(HttpContext ctx) =>
    HttpMethods.IsPost(ctx.Request.Method) ||
    HttpMethods.IsPut(ctx.Request.Method) ||
    HttpMethods.IsDelete(ctx.Request.Method) ||
    HttpMethods.IsPatch(ctx.Request.Method);

app.Use(async (ctx, next) =>
{
    if (IsWriteMethod(ctx))
    {
        if (!ctx.Request.Headers.TryGetValue("X-Api-Key", out var key) || string.IsNullOrWhiteSpace(key))
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await ctx.Response.WriteAsync("Missing X-Api-Key");
            return;
        }
        if (!string.Equals(key, apiKey, StringComparison.Ordinal))
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            await ctx.Response.WriteAsync("Invalid API key");
            return;
        }
    }
    await next();
});

// עזר ל-DataAnnotations במינימל API
static (bool ok, Dictionary<string, string[]>? errors) Validate<T>(T model)
{
    var vc = new ValidationContext(model!);
    var results = new List<ValidationResult>();
    var ok = Validator.TryValidateObject(model!, vc, results, validateAllProperties: true);
    if (ok) return (true, null);
    var errors = results
        .GroupBy(r => r.MemberNames.FirstOrDefault() ?? "")
        .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage ?? "Invalid").ToArray());
    return (false, errors);
}

// CRUD
app.MapGet("/api/contacts", async ([FromServices] AppDbContext db) =>
{
    var data = await db.ContactRequests
        .OrderByDescending(c => c.CreatedAtUtc)
        .Select(c => new ContactResponseDto(c.Id, c.Name, c.Phone, c.Email, c.Departments, c.Description, c.CreatedAtUtc))
        .ToListAsync();
    return Results.Ok(data);
});

app.MapGet("/api/contacts/{id:guid}", async ([FromServices] AppDbContext db, Guid id) =>
{
    var c = await db.ContactRequests.FindAsync(id);
    return c is null
        ? Results.NotFound()
        : Results.Ok(new ContactResponseDto(c.Id, c.Name, c.Phone, c.Email, c.Departments, c.Description, c.CreatedAtUtc));
});

app.MapPost("/api/contacts", async ([FromServices] AppDbContext db, [FromBody] CreateContactRequestDto dto) =>
{
    var (ok, errors) = Validate(dto);
    if (!ok) return Results.ValidationProblem(errors!);

    var entity = new ContactRequest
    {
        Name = dto.Name,
        Phone = dto.Phone,
        Email = dto.Email,
        Departments = dto.Departments,
        Description = dto.Description,
        CreatedAtUtc = DateTime.UtcNow
    };
    db.ContactRequests.Add(entity);
    await db.SaveChangesAsync();
    return Results.Created($"/api/contacts/{entity.Id}", new ContactResponseDto(
        entity.Id, entity.Name, entity.Phone, entity.Email, entity.Departments, entity.Description, entity.CreatedAtUtc
    ));
});

app.MapPut("/api/contacts/{id:guid}", async ([FromServices] AppDbContext db, Guid id, [FromBody] UpdateContactRequestDto dto) =>
{
    var (ok, errors) = Validate(dto);
    if (!ok) return Results.ValidationProblem(errors!);

    var entity = await db.ContactRequests.FindAsync(id);
    if (entity is null) return Results.NotFound();

    entity.Name = dto.Name;
    entity.Phone = dto.Phone;
    entity.Email = dto.Email;
    entity.Departments = dto.Departments;
    entity.Description = dto.Description;

    await db.SaveChangesAsync();
    return Results.Ok(new ContactResponseDto(
        entity.Id, entity.Name, entity.Phone, entity.Email, entity.Departments, entity.Description, entity.CreatedAtUtc
    ));
});

app.MapDelete("/api/contacts/{id:guid}", async ([FromServices] AppDbContext db, Guid id) =>
{
    var entity = await db.ContactRequests.FindAsync(id);
    if (entity is null) return Results.NotFound();
    db.ContactRequests.Remove(entity);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// דו"ח חודשי (סימולציית Stored Procedure)
app.MapGet("/api/reports/monthly/{year:int}", async ([FromServices] IMonthlyReportService svc, int year) =>
{
    var rows = await svc.GetMonthlyReportAsync(year);
    return Results.Ok(rows);
});

app.Run();

// הערה: המחלקה החלקית Program נמצאת בקובץ נפרד Program.Public.cs עבור הטסטים
