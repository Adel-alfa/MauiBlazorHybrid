using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MauiBlazorHybrid.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class StudentQuizQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedOn",
                table: "studentQuizzes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "studentQuizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StudentQuizQuestions",
                columns: table => new
                {
                    StudentQuizId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    StudentQuizId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentQuizQuestions", x => new { x.StudentQuizId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_StudentQuizQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentQuizQuestions_studentQuizzes_StudentQuizId",
                        column: x => x.StudentQuizId,
                        principalTable: "studentQuizzes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentQuizQuestions_studentQuizzes_StudentQuizId1",
                        column: x => x.StudentQuizId1,
                        principalTable: "studentQuizzes",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGzAuRaZegsvS6t40G0eNhmTFxMZT4PeG3JGVjq3miEPVrW/cIqZROplDaCEmItBsQ==");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizQuestions_QuestionId",
                table: "StudentQuizQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizQuestions_StudentQuizId1",
                table: "StudentQuizQuestions",
                column: "StudentQuizId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentQuizQuestions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "studentQuizzes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedOn",
                table: "studentQuizzes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM0tt1LhZGEe8KL7K4S0QVjWksWE/MFzS9w9H57uLtlUUtDS1xtFZuKaiE51wO5Kkw==");
        }
    }
}
