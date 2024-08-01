using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtzarSfarim.Migrations
{
    /// <inheritdoc />
    public partial class add_test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenreModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShelfModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    FreeSpace = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelfModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShelfModel_GenreModel_GenreId",
                        column: x => x.GenreId,
                        principalTable: "GenreModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookWidth = table.Column<int>(type: "int", nullable: false),
                    BookHeight = table.Column<int>(type: "int", nullable: false),
                    SetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShelfId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookModel_ShelfModel_ShelfId",
                        column: x => x.ShelfId,
                        principalTable: "ShelfModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookModel_ShelfId",
                table: "BookModel",
                column: "ShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_ShelfModel_GenreId",
                table: "ShelfModel",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookModel");

            migrationBuilder.DropTable(
                name: "ShelfModel");

            migrationBuilder.DropTable(
                name: "GenreModel");
        }
    }
}
