using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterPrediction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlertSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertSettings",
                table: "AlertSettings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertSettings",
                table: "AlertSettings",
                columns: new[] { "RegionId", "DisasterType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertSettings",
                table: "AlertSettings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertSettings",
                table: "AlertSettings",
                column: "RegionId");
        }
    }
}
