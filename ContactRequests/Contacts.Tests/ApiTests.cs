using System.Net;
using System.Net.Http.Json;
using Contacts.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Contacts.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Create_Then_Get_Works_With_ApiKey()
    {
        var client = _factory.CreateClient();
        var dto = new CreateContactRequestDto(
            Name: "Test User",
            Phone: "050-1234567",
            Email: "test@example.com",
            Departments: new() { "Support", "IT" },
            Description: "Hello"
        );

        var req = new HttpRequestMessage(HttpMethod.Post, "/api/contacts")
        {
            Content = JsonContent.Create(dto)
        };
        req.Headers.Add("X-Api-Key", "dev-secret");

        var createResp = await client.SendAsync(req);
        Assert.Equal(HttpStatusCode.Created, createResp.StatusCode);

        var list = await client.GetFromJsonAsync<List<ContactResponseDto>>("/api/contacts");
        Assert.NotNull(list);
        Assert.True(list!.Count >= 1);
    }
}
