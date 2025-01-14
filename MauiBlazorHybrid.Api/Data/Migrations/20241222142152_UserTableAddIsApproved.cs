using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiBlazorHybrid.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserTableAddIsApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsApproved", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEM0tt1LhZGEe8KL7K4S0QVjWksWE/MFzS9w9H57uLtlUUtDS1xtFZuKaiE51wO5Kkw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFpXV8XdOPXrXUz4lNY9NnjBu3GQXsIPFdoCLFUCeJ4NXFhMeLyWfNEhS7ZgExFSgg==");
        }
    }
}
