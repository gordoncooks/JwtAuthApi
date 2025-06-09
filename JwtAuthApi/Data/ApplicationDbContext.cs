using Microsoft.EntityFrameworkCore;

namespace JwtAuthApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<JwtAuthApi.Models.User> Users { get; set; }
        public DbSet<JwtAuthApi.Models.RefreshToken> RefreshTokens { get; set; }
    }
}