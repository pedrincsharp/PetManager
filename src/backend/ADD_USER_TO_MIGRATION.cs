/**
 * Migration para suportar UserId em RefreshToken
 * 
 * Para criar a migration, execute:
 * dotnet ef migrations add AddUserIdToRefreshToken -p src/backend/PetManager.Infrastructure -s src/backend/PetManager.Api
 * 
 * Depois aplique com:
 * dotnet ef database update -p src/backend/PetManager.Infrastructure -s src/backend/PetManager.Api
 * 
 * Ou use o script PowerShell:
 * src/backend/migrate.ps1
 */

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<System.Guid>(
                name: "api_key_id",
                table: "refresh_tokens",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(System.Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<System.Guid>(
                name: "user_id",
                table: "refresh_tokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_tokens_users_user_id",
                table: "refresh_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_tokens_users_user_id",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "refresh_tokens");

            migrationBuilder.AlterColumn<System.Guid>(
                name: "api_key_id",
                table: "refresh_tokens",
                type: "uuid",
                nullable: false,
                defaultValue: new System.Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(System.Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
