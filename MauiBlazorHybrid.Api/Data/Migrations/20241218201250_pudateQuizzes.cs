using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiBlazorHybrid.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class pudateQuizzes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFpXV8XdOPXrXUz4lNY9NnjBu3GQXsIPFdoCLFUCeJ4NXFhMeLyWfNEhS7ZgExFSgg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Quizzes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPLeA6MF1NtKqvvLW2XlTmWH6lg9Nde5eo5yIOKEHeycGDoR1CcbvpeQX23DIVTFNw==");
        }
    }
}
