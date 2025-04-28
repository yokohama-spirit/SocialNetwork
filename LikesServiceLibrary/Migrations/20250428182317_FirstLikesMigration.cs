using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LikesServiceLibrary.Migrations
{
    /// <inheritdoc />
    public partial class FirstLikesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    PostId = table.Column<string>(type: "TEXT", nullable: true),
                    CommentId = table.Column<string>(type: "TEXT", nullable: true),
                    ReplyId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
