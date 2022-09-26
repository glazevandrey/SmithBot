using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmithBot.Migrations
{
    public partial class addWallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(nullable: true),
                    Balance = table.Column<double>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Stage = table.Column<int>(nullable: false),
                    ReferalUrl = table.Column<string>(nullable: true),
                    ReferrerUserId = table.Column<long>(nullable: true),
                    Activated = table.Column<bool>(nullable: false),
                    Subscriber = table.Column<bool>(nullable: false),
                    Wallet = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_BotUsers_BotUsers_ReferrerUserId",
                        column: x => x.ReferrerUserId,
                        principalTable: "BotUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NFTs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<long>(nullable: false),
                    ImageId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    ForBotBalance = table.Column<bool>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    NFTType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NFTs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionId = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    NFTType = table.Column<string>(nullable: true),
                    WaitPayment = table.Column<bool>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOrders",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOrders", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotUsers_ReferrerUserId",
                table: "BotUsers",
                column: "ReferrerUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotUsers");

            migrationBuilder.DropTable(
                name: "NFTs");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "WithdrawOrders");
        }
    }
}
