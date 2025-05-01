using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostServiceLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedUtcOnTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_UserId",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Восстанавливаем FOREIGN KEY при откате миграции
            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade); // Или другой ReferentialAction
        }
    }
}
