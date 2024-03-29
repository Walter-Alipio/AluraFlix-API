﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlayListAPI.Data;

#nullable disable

namespace PlayListAPI.Migrations
{
  [DbContext(typeof(AppDbContext))]
  [Migration("20221226165404_Video-Categorias")]
  partial class VideoCategorias
  {
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "7.0.1")
          .HasAnnotation("Relational:MaxIdentifierLength", 128);

      SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

      modelBuilder.Entity("PlayListAPI.Models.Categoria", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Cor")
                      .IsRequired()
                      .HasMaxLength(10)
                      .HasColumnType("nvarchar(10)");

            b.Property<string>("Title")
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnType("nvarchar(50)");

            b.HasKey("Id");

            b.ToTable("Categorias");
          });

      modelBuilder.Entity("PlayListAPI.Models.Video", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("int");

            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<int>("CategoriaId")
                      .HasColumnType("int");

            b.Property<string>("Description")
                      .IsRequired()
                      .HasMaxLength(5000)
                      .HasColumnType("nvarchar(max)");

            b.Property<string>("Title")
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnType("nvarchar(100)");

            b.Property<string>("Url")
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnType("nvarchar(100)");

            b.HasKey("Id");

            b.HasIndex("CategoriaId");

            b.ToTable("Videos");
          });

      modelBuilder.Entity("PlayListAPI.Models.Video", b =>
          {
            b.HasOne("PlayListAPI.Models.Categoria", "Categoria")
                      .WithMany("Videos")
                      .HasForeignKey("CategoriaId")
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);

            b.Navigation("Categoria");
          });

      modelBuilder.Entity("PlayListAPI.Models.Categoria", b =>
          {
            b.Navigation("Videos");
          });
#pragma warning restore 612, 618
    }
  }
}
