using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PlayListAPI.Models;

namespace PlayListAPI.Data.EntityConfiguration;

public class VideoEntityTypeConfiguration : IEntityTypeConfiguration<Video>
{
  public void Configure(EntityTypeBuilder<Video> builder)
  {
    builder.ToTable("Videos");

    //Relação 1:n
    builder
      .HasOne(video => video.Categoria)
      .WithMany(categoia => categoia.Videos)
      .HasForeignKey(video => video.CategoriaId)
      .IsRequired(false)
      .OnDelete(DeleteBehavior.SetNull);

  }
}