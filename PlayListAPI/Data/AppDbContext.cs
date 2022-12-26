using PlayListAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PlayListAPI.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      //Relação 1:n
      builder.Entity<Video>()
        .HasOne(video => video.Categoria)
        .WithMany(categoia => categoia.Videos)
        .HasForeignKey(video => video.CategoriaId)
        .IsRequired(false)
        .OnDelete(DeleteBehavior.SetNull);

    }

    public DbSet<Video> Videos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
  }
}