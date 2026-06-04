using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySongs.Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddTagType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagType",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0); // 0 = General
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagType",
                table: "Tags");
        }
    }
}
