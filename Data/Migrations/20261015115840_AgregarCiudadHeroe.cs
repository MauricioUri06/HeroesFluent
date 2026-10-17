using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeroesFluent.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCiudadHeroe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ciudad",
                table: "Heroes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciudad",
                table: "Heroes");
        }
    }
}
