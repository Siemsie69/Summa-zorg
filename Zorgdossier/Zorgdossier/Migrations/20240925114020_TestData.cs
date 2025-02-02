using Microsoft.EntityFrameworkCore.Migrations;
using Zorgdossier.Databases;

#nullable disable

namespace Zorgdossier.Migrations
{
    /// <inheritdoc />
    public partial class TestData : Migration
    {
        /// <inheritdoc />
        //Deze methode wordt gebruikt om de methode 'TestData' in de Seeder class uit te voeren, zodat de database testdata krijgt.
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            new Seeder().TestData();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}