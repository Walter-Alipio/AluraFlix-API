using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayListAPI.Migrations
{
  /// <inheritdoc />
  public partial class VideoCategorias : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Categorias",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            Cor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Categorias", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Videos",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
            Url = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            CategoriaId = table.Column<int>(type: "int", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Videos", x => x.Id);
            table.ForeignKey(
                      name: "FK_Videos_Categorias_CategoriaId",
                      column: x => x.CategoriaId,
                      principalTable: "Categorias",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.SetNull);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Videos_CategoriaId",
          table: "Videos",
          column: "CategoriaId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Videos");

      migrationBuilder.DropTable(
          name: "Categorias");
    }
  }
}
