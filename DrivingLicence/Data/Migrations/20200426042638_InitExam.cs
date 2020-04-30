using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DrivingLicence.Data.Migrations
{
    public partial class InitExam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    ExamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    Time = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.ExamId);
                });

            migrationBuilder.CreateTable(
                name: "Consequences",
                columns: table => new
                {
                    ConsequenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmitedDate = table.Column<DateTime>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    CorrectCount = table.Column<int>(nullable: false),
                    WrongCount = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consequences", x => x.ConsequenceId);
                    table.ForeignKey(
                        name: "FK_Consequences_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    ChoiceA = table.Column<string>(nullable: true),
                    ChoiceB = table.Column<string>(nullable: true),
                    ChoiceC = table.Column<string>(nullable: true),
                    ChoiceD = table.Column<string>(nullable: true),
                    Answers = table.Column<string>(nullable: true),
                    Media = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                    table.ForeignKey(
                        name: "FK_Quizzes_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consequences_ExamId",
                table: "Consequences",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ExamId",
                table: "Quizzes",
                column: "ExamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consequences");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "Exams");
        }
    }
}
