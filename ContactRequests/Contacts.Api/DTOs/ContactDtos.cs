using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.DTOs;

public record CreateContactRequestDto(
    [Required, MaxLength(100)] string Name,
    [Required, Phone, MaxLength(30)] string Phone,
    [Required, EmailAddress, MaxLength(200)] string Email,
    [Required, MinLength(1)] List<string> Departments,
    [Required, MaxLength(2000)] string Description
);

public record UpdateContactRequestDto(
    [Required, MaxLength(100)] string Name,
    [Required, Phone, MaxLength(30)] string Phone,
    [Required, EmailAddress, MaxLength(200)] string Email,
    [Required, MinLength(1)] List<string> Departments,
    [Required, MaxLength(2000)] string Description
);

public record ContactResponseDto(
    Guid Id, string Name, string Phone, string Email, List<string> Departments, string Description, DateTime CreatedAtUtc
);

