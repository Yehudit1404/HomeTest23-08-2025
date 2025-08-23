using System.ComponentModel.DataAnnotations;

namespace Contacts.Api.Models;

public class ContactRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = default!;

    [Required, MinLength(1)]
    public List<string> Departments { get; set; } = new();

    [Required, MaxLength(2000)]
    public string Description { get; set; } = default!;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
