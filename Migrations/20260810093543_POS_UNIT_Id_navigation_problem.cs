using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_API.Migrations
{
    /// <inheritdoc />
    public partial class POS_UNIT_Id_navigation_problem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POS_Products_POS_Customers_UnitId",
                schema: "dbo",
                table: "POS_Products");

            migrationBuilder.DropForeignKey(
                name: "FK_POS_Products_POS_Units_POSUnitId",
                schema: "dbo",
                table: "POS_Products");

            migrationBuilder.DropIndex(
                name: "IX_POS_Products_POSUnitId",
                schema: "dbo",
                table: "POS_Products");

            migrationBuilder.DropColumn(
                name: "POSUnitId",
                schema: "dbo",
                table: "POS_Products");

            migrationBuilder.AddForeignKey(
                name: "FK_POS_Products_POS_Units_UnitId",
                schema: "dbo",
                table: "POS_Products",
                column: "UnitId",
                principalSchema: "dbo",
                principalTable: "POS_Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POS_Products_POS_Units_UnitId",
                schema: "dbo",
                table: "POS_Products");

            migrationBuilder.AddColumn<int>(
                name: "POSUnitId",
                schema: "dbo",
                table: "POS_Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POS_Products_POSUnitId",
                schema: "dbo",
                table: "POS_Products",
                column: "POSUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_POS_Products_POS_Customers_UnitId",
                schema: "dbo",
                table: "POS_Products",
                column: "UnitId",
                principalSchema: "dbo",
                principalTable: "POS_Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_POS_Products_POS_Units_POSUnitId",
                schema: "dbo",
                table: "POS_Products",
                column: "POSUnitId",
                principalSchema: "dbo",
                principalTable: "POS_Units",
                principalColumn: "Id");
        }
    }
}
