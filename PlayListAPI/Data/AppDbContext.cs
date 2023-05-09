using PlayListAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PlayListAPI.Data.EntityConfiguration;
using Microsoft.AspNetCore.Identity;

namespace PlayListAPI.Data
{
  public class AppDbContext : IdentityDbContext
  {
    private readonly IConfiguration _configuration;
    public AppDbContext(DbContextOptions<AppDbContext> opt, IConfiguration configuration) : base(opt)
    {
      _configuration = configuration;
    }
    public DbSet<Video> Videos { get; set; } = null!;
    public DbSet<Categoria> Categorias { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
      //adicionado para cria��o da migra��o.
      base.OnModelCreating(builder);

      builder.Entity<IdentityUser>().HasData(new IdentityUser
      {
        Id = "99999",
        UserName = "admin",
        NormalizedUserName = "ADMIN",
        Email = "admin@example.com",
        NormalizedEmail = "ADMIN@EXAMPLE.COM",
        EmailConfirmed = true,
        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "MySecurePassword123!"),
        SecurityStamp = string.Empty
      });

      builder.Entity<IdentityUserRole<string>>()
      .HasData(
           new IdentityUserRole<string> { RoleId = "99999", UserId = "99999" }
      );

      builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

  }
}