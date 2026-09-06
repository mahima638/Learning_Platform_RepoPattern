using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningPlatformRepoPattern.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    sub_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sub_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    mid = table.Column<int>(type: "int", nullable: false),
                    sub_amount = table.Column<double>(type: "float", nullable: false),
                    subStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subThumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.sub_id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Master_course_mid",
                        column: x => x.mid,
                        principalTable: "Master_course",
                        principalColumn: "mid");
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionSubCourses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sub_id = table.Column<int>(type: "int", nullable: false),
                    sid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionSubCourses", x => x.id);
                    table.ForeignKey(
                        name: "FK_SubscriptionSubCourses_Subscriptions_sub_id",
                        column: x => x.sub_id,
                        principalTable: "Subscriptions",
                        principalColumn: "sub_id");
                    table.ForeignKey(
                        name: "FK_SubscriptionSubCourses_sub_course_sid",
                        column: x => x.sid,
                        principalTable: "sub_course",
                        principalColumn: "sid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_mid",
                table: "Subscriptions",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionSubCourses_sid",
                table: "SubscriptionSubCourses",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionSubCourses_sub_id",
                table: "SubscriptionSubCourses",
                column: "sub_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionSubCourses");

            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
