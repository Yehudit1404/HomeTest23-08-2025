using Contacts.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ContactRequest> ContactRequests => Set<ContactRequest>();
}
