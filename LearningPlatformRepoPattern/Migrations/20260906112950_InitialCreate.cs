using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningPlatformRepoPattern.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Master_course",
                columns: table => new
                {
                    mid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mstatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mthumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Master_course", x => x.mid);
                });

            migrationBuilder.CreateTable(
                name: "TopicProgress",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    tid = table.Column<int>(type: "int", nullable: false),
                    sid = table.Column<int>(type: "int", nullable: false),
                    mcq_passed = table.Column<bool>(type: "bit", nullable: false),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicProgress", x => x.ProgressId);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "sub_course",
                columns: table => new
                {
                    sid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mid = table.Column<int>(type: "int", nullable: false),
                    SubCourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sstatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    samount = table.Column<decimal>(type: "decimal(18,2)", precision: 9, scale: 2, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MasterCourseId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_course", x => x.sid);
                    table.ForeignKey(
                        name: "FK_sub_course_Master_course_MasterCourseId1",
                        column: x => x.MasterCourseId1,
                        principalTable: "Master_course",
                        principalColumn: "mid");
                    table.ForeignKey(
                        name: "FK_sub_course_Master_course_mid",
                        column: x => x.mid,
                        principalTable: "Master_course",
                        principalColumn: "mid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "my_courses",
                columns: table => new
                {
                    mcid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sid = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_my_courses", x => x.mcid);
                    table.ForeignKey(
                        name: "FK_my_courses_sub_course_sid",
                        column: x => x.sid,
                        principalTable: "sub_course",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_my_courses_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    tid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mid = table.Column<int>(type: "int", nullable: false),
                    sid = table.Column<int>(type: "int", nullable: false),
                    tname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    videoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tstatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tthumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.tid);
                    table.ForeignKey(
                        name: "FK_Topic_Master_course_mid",
                        column: x => x.mid,
                        principalTable: "Master_course",
                        principalColumn: "mid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topic_sub_course_sid",
                        column: x => x.sid,
                        principalTable: "sub_course",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    material_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mid = table.Column<int>(type: "int", nullable: false),
                    sid = table.Column<int>(type: "int", nullable: false),
                    tid = table.Column<int>(type: "int", nullable: false),
                    assignment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.material_id);
                    table.ForeignKey(
                        name: "FK_Material_Master_course_mid",
                        column: x => x.mid,
                        principalTable: "Master_course",
                        principalColumn: "mid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Material_Topic_tid",
                        column: x => x.tid,
                        principalTable: "Topic",
                        principalColumn: "tid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Material_sub_course_sid",
                        column: x => x.sid,
                        principalTable: "sub_course",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mcq",
                columns: table => new
                {
                    mcq_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_id = table.Column<int>(type: "int", nullable: false),
                    question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mcq", x => x.mcq_id);
                    table.ForeignKey(
                        name: "FK_Mcq_Material_material_id",
                        column: x => x.material_id,
                        principalTable: "Material",
                        principalColumn: "material_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Material_mid",
                table: "Material",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_Material_sid",
                table: "Material",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_Material_tid",
                table: "Material",
                column: "tid");

            migrationBuilder.CreateIndex(
                name: "IX_Mcq_material_id",
                table: "Mcq",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_my_courses_sid",
                table: "my_courses",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_my_courses_user_id",
                table: "my_courses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_sub_course_MasterCourseId1",
                table: "sub_course",
                column: "MasterCourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_sub_course_mid",
                table: "sub_course",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_mid",
                table: "Topic",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_sid",
                table: "Topic",
                column: "sid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mcq");

            migrationBuilder.DropTable(
                name: "my_courses");

            migrationBuilder.DropTable(
                name: "TopicProgress");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropTable(
                name: "sub_course");

            migrationBuilder.DropTable(
                name: "Master_course");
        }
    }
}
