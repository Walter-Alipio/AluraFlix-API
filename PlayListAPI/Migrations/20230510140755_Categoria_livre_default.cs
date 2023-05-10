using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayListAPI.Migrations
{
    /// <inheritdoc />
    public partial class Categorialivredefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Categorias_CategoriaId",
                table: "Videos");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99999",
                column: "ConcurrencyStamp",
                value: "0ac28c93-fc9a-4f42-9888-da79624d836e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "999998",
                column: "ConcurrencyStamp",
                value: "306e49c3-2b41-46fc-8bc0-87d4937a56f8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99999",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1e82c4af-b0e2-47c3-811d-65acdb61335d", "AQAAAAEAACcQAAAAEG2u9a2q3VoWD8r2mel+OyIFA1EwSOus5kpEvk8tF46zDWdurNhryMFTfMqpueZaXw==" });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Cor", "Title" },
                values: new object[] { 1, "#FFFFFF", "LIVRE" });

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Categorias_CategoriaId",
                table: "Videos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Categorias_CategoriaId",
                table: "Videos");

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99999",
                column: "ConcurrencyStamp",
                value: "4065f49b-a4eb-47ae-8708-41659f39ed66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "999998",
                column: "ConcurrencyStamp",
                value: "9d5d4710-7466-4a20-be56-b6420032ba7f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99999",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6ad1fb69-ec96-484a-90f2-2094bf608ed7", "AQAAAAEAACcQAAAAEPP/U+CsUw/mFdycZdyTcudKfwmVIDVIrGqE1vHcxLyL5XUm+S6UE2bKvHT0GhBA5A==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Categorias_CategoriaId",
                table: "Videos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
