using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aulas.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstVersionOfModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Logotype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CellPhone = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    StudentNumber = table.Column<int>(type: "int", nullable: true),
                    TuitionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DegreeFK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Degrees_DegreeFK",
                        column: x => x.DegreeFK,
                        principalTable: "Degrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CurricularYear = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    DegreeFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Degrees_DegreeFK",
                        column: x => x.DegreeFK,
                        principalTable: "Degrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseProfessor",
                columns: table => new
                {
                    CoursesListId = table.Column<int>(type: "int", nullable: false),
                    ProfessorsListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProfessor", x => new { x.CoursesListId, x.ProfessorsListId });
                    table.ForeignKey(
                        name: "FK_CourseProfessor_AppUsers_ProfessorsListId",
                        column: x => x.ProfessorsListId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProfessor_Courses_CoursesListId",
                        column: x => x.CoursesListId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    StudentFK = table.Column<int>(type: "int", nullable: false),
                    CourseFK = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => new { x.StudentFK, x.CourseFK });
                    table.ForeignKey(
                        name: "FK_Registrations_AppUsers_StudentFK",
                        column: x => x.StudentFK,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrations_Courses_CourseFK",
                        column: x => x.CourseFK,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_DegreeFK",
                table: "AppUsers",
                column: "DegreeFK");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProfessor_ProfessorsListId",
                table: "CourseProfessor",
                column: "ProfessorsListId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DegreeFK",
                table: "Courses",
                column: "DegreeFK");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_CourseFK",
                table: "Registrations",
                column: "CourseFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseProfessor");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Degrees");
        }
    }
}
