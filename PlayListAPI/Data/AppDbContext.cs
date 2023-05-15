using PlayListAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

      builder.Entity<Categoria>().HasData(new Categoria
      {
        Id = 1,
        Title = "LIVRE",
        Cor = "#FFFFFF"
      });

      builder.Entity<Video>().HasQueryFilter(v => v.DeletedAt == null);

      builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      HandleVideoDelete();
      return await base.SaveChangesAsync();
    }

    private void HandleVideoDelete()
    {
      var entities = ChangeTracker.Entries()
                          .Where(e => e.State == EntityState.Deleted);

      foreach (var entity in entities)
      {
        if (entity.Entity is Video)
        {
          entity.State = EntityState.Modified;
          var video = entity.Entity as Video;
          video!.DeletedAt = DateTime.UtcNow;
        }
      }
    }
  }
}