using PlayListAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PlayListAPI.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }
        public DbSet<Video> Videos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Relação 1:n
            builder.Entity<Video>()
              .HasOne(video => video.Categoria)
              .WithMany(categoia => categoia.Videos)
              .HasForeignKey(video => video.CategoriaId)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);
            //adicionado para cria��o da migra��o.
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

    }
}