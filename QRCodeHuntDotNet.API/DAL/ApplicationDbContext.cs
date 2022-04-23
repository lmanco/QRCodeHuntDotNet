using Microsoft.EntityFrameworkCore;
using QRCodeHuntDotNet.API.DAL.Models;

namespace QRCodeHuntDotNet.API.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
    }
}
