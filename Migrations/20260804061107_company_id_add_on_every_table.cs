using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_API.Migrations
{
    /// <inheritdoc />
    public partial class company_id_add_on_every_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "HrdCompanyInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Id",
                schema: "dbo",
                table: "HrdCompanyInfo",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
