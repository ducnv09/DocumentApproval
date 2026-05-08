using BE.Domain.Entities;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace BE.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                var admin = new User("admin", "System Administrator", "admin@gmail.com", true);
                admin.SetPassword(BC.HashPassword("123"));
                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
