using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zorgdossier.Migrations
{
    /// <inheritdoc />
    public partial class ContactAdvice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "ContactAdvice",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                DossierId = table.Column<int>(type: "INTEGER", nullable: false),
                Advice = table.Column<string>(type: "TEXT", maxLength: 720, nullable: false),
                ContactAdviceText = table.Column<string>(type: "TEXT", maxLength: 320, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContactAdvice", x => x.Id);
                table.ForeignKey(
                    name: "FK_ContactAdvice_Dossier_DossierId",
                    column: x => x.DossierId,
                    principalTable: "Dossier",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex(
            name: "IX_ContactAdvice_DossierId",
            table: "ContactAdvice",
            column: "DossierId",
            unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "ContactAdvice");
        }
    }
}
