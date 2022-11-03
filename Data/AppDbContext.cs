using AluraPlayList.Models;
using Microsoft.EntityFrameworkCore;

namespace AluraPlayList.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Video> Videos { get; set; }
  }
}