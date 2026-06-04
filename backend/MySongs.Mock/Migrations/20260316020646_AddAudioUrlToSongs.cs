using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySongs.Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddAudioUrlToSongs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "Songs");
        }
    }
}
