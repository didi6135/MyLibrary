using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtzarSfarim.Migrations
{
    /// <inheritdoc />
    public partial class addGenreId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "BookModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "BookModel");
        }
    }
}
