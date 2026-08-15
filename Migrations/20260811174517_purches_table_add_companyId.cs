using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_API.Migrations
{
    /// <inheritdoc />
    public partial class purches_table_add_companyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "dbo",
                table: "POS_PurchaseMasters",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "dbo",
                table: "POS_PurchaseMasters");
        }
    }
}
