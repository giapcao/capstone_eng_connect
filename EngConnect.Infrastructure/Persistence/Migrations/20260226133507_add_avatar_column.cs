using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_avatar_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "student",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "student");
        }
    }
}
