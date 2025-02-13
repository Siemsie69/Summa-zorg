using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zorgdossier.Migrations
{
    /// <inheritdoc />
    public partial class ComplaintsSymptoms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "ComplaintsSymptoms",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                DossierId = table.Column<int>(type: "INTEGER", nullable: false),
                ComplaintsSymptomsSummary = table.Column<string>(type: "TEXT", maxLength: 720, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ComplaintsSymptoms", x => x.Id);
                table.ForeignKey(
                    name: "FK_ComplaintsSymptoms_Dossier_DossierId",
                    column: x => x.DossierId,
                    principalTable: "Dossier",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex(
            name: "IX_ComplaintsSymptoms_DossierId",
            table: "ComplaintsSymptoms",
            column: "DossierId",
            unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "ComplaintsSymptoms");
        }
    }
}
