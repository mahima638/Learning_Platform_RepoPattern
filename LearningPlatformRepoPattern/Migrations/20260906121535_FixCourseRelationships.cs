using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningPlatformRepoPattern.Migrations
{
    /// <inheritdoc />
    public partial class FixCourseRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Master_course_mid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Topic_tid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_sub_course_sid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcq_Material_material_id",
                table: "Mcq");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_course_Master_course_mid",
                table: "sub_course");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Master_course_mid",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_sub_course_sid",
                table: "Topic");

            migrationBuilder.CreateIndex(
                name: "IX_my_courses_sid",
                table: "my_courses",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_my_courses_user_id",
                table: "my_courses",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Master_course_mid",
                table: "Material",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Topic_tid",
                table: "Material",
                column: "tid",
                principalTable: "Topic",
                principalColumn: "tid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_sub_course_sid",
                table: "Material",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mcq_Material_material_id",
                table: "Mcq",
                column: "material_id",
                principalTable: "Material",
                principalColumn: "material_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_my_courses_sub_course_sid",
                table: "my_courses",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_my_courses_user_user_id",
                table: "my_courses",
                column: "user_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_course_Master_course_mid",
                table: "sub_course",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Master_course_mid",
                table: "Topic",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_sub_course_sid",
                table: "Topic",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Master_course_mid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Topic_tid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_sub_course_sid",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Mcq_Material_material_id",
                table: "Mcq");

            migrationBuilder.DropForeignKey(
                name: "FK_my_courses_sub_course_sid",
                table: "my_courses");

            migrationBuilder.DropForeignKey(
                name: "FK_my_courses_user_user_id",
                table: "my_courses");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_course_Master_course_mid",
                table: "sub_course");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Master_course_mid",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_sub_course_sid",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_my_courses_sid",
                table: "my_courses");

            migrationBuilder.DropIndex(
                name: "IX_my_courses_user_id",
                table: "my_courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Master_course_mid",
                table: "Material",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Topic_tid",
                table: "Material",
                column: "tid",
                principalTable: "Topic",
                principalColumn: "tid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_sub_course_sid",
                table: "Material",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mcq_Material_material_id",
                table: "Mcq",
                column: "material_id",
                principalTable: "Material",
                principalColumn: "material_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_course_Master_course_mid",
                table: "sub_course",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Master_course_mid",
                table: "Topic",
                column: "mid",
                principalTable: "Master_course",
                principalColumn: "mid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_sub_course_sid",
                table: "Topic",
                column: "sid",
                principalTable: "sub_course",
                principalColumn: "sid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
