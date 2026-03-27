using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class rename_tutor_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "years_experience",
                table: "tutor",
                newName: "month_experience");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "month_experience",
                table: "tutor",
                newName: "years_experience");
        }
    }
}
