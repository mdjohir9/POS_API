using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_API.Migrations
{
    /// <inheritdoc />
    public partial class initialdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "HrdCompanyInfo",
                schema: "dbo",
                columns: table => new
                {
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyType = table.Column<bool>(type: "bit", nullable: true),
                    HeadOfficeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameBangla = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressBangla = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessType = table.Column<short>(type: "smallint", nullable: true),
                    MultipleBranch = table.Column<bool>(type: "bit", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartCardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weekend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNoType = table.Column<bool>(type: "bit", nullable: true),
                    FlatCode = table.Column<short>(type: "smallint", nullable: true),
                    CardNoDigits = table.Column<short>(type: "smallint", nullable: true),
                    AttMachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PfcountDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsLeaveAuthority = table.Column<bool>(type: "bit", nullable: true),
                    IsOdauthority = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrdCompanyInfo", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "dbo",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAccessLevel = table.Column<int>(type: "int", nullable: true),
                    Ordering = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(150)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UserRoleID = table.Column<int>(type: "int", nullable: false),
                    IsGuestUser = table.Column<bool>(type: "bit", nullable: true),
                    IsApprovingAuthority = table.Column<bool>(type: "bit", nullable: true),
                    ReferenceID = table.Column<string>(type: "varchar(50)", nullable: true),
                    AdditionalPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemovedPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAccessPermission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    IsAdministrator = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BaseEntity",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompanyId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    POSCategory_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VATPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsBatchRequired = table.Column<bool>(type: "bit", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LotNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ManufacturingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    POSProductBatch_PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReceiveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvailableQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SupplierCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SupplierName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    POSSupplier_Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    POSSupplier_Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PurchaseNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupplierId = table.Column<long>(type: "bigint", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    POSSalesPaymentMethod_Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseEntity_BaseEntity_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseEntity_BaseEntity_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseEntity_BaseEntity_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseEntity_BaseEntity_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseEntity_BaseEntity_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseEntity_BaseEntity_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseEntity_HrdCompanyInfo_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "HrdCompanyInfo",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntity_HrdCompanyInfo_CompanyId1",
                        column: x => x.CompanyId1,
                        principalSchema: "dbo",
                        principalTable: "HrdCompanyInfo",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "POS_PurchaseDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseMasterId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    POSProductBatchId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POS_PurchaseDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POS_PurchaseDetails_BaseEntity_POSProductBatchId",
                        column: x => x.POSProductBatchId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_POS_PurchaseDetails_BaseEntity_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_POS_PurchaseDetails_BaseEntity_PurchaseMasterId",
                        column: x => x.PurchaseMasterId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POS_SalesDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesMasterId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    POSProductBatchId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POS_SalesDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POS_SalesDetails_BaseEntity_POSProductBatchId",
                        column: x => x.POSProductBatchId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_POS_SalesDetails_BaseEntity_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_POS_SalesDetails_BaseEntity_SalesMasterId",
                        column: x => x.SalesMasterId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POS_SalesPayments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POS_SalesPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POS_SalesPayments_BaseEntity_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_POS_SalesPayments_BaseEntity_SalesId",
                        column: x => x.SalesId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "POS_StockLedgers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: false),
                    InQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POS_StockLedgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POS_StockLedgers_BaseEntity_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "BaseEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_BrandId",
                schema: "dbo",
                table: "BaseEntity",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_CategoryId",
                schema: "dbo",
                table: "BaseEntity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_CompanyId",
                schema: "dbo",
                table: "BaseEntity",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_CompanyId1",
                schema: "dbo",
                table: "BaseEntity",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_CustomerId",
                schema: "dbo",
                table: "BaseEntity",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_ProductId",
                schema: "dbo",
                table: "BaseEntity",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_SupplierId",
                schema: "dbo",
                table: "BaseEntity",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_UnitId",
                schema: "dbo",
                table: "BaseEntity",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_PurchaseDetails_POSProductBatchId",
                schema: "dbo",
                table: "POS_PurchaseDetails",
                column: "POSProductBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_PurchaseDetails_ProductId",
                schema: "dbo",
                table: "POS_PurchaseDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_PurchaseDetails_PurchaseMasterId",
                schema: "dbo",
                table: "POS_PurchaseDetails",
                column: "PurchaseMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_SalesDetails_POSProductBatchId",
                schema: "dbo",
                table: "POS_SalesDetails",
                column: "POSProductBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_SalesDetails_ProductId",
                schema: "dbo",
                table: "POS_SalesDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_SalesDetails_SalesMasterId",
                schema: "dbo",
                table: "POS_SalesDetails",
                column: "SalesMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_SalesPayments_PaymentMethodId",
                schema: "dbo",
                table: "POS_SalesPayments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_SalesPayments_SalesId",
                schema: "dbo",
                table: "POS_SalesPayments",
                column: "SalesId");

            migrationBuilder.CreateIndex(
                name: "IX_POS_StockLedgers_ProductId",
                schema: "dbo",
                table: "POS_StockLedgers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POS_PurchaseDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "POS_SalesDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "POS_SalesPayments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "POS_StockLedgers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BaseEntity",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "HrdCompanyInfo",
                schema: "dbo");
        }
    }
}
