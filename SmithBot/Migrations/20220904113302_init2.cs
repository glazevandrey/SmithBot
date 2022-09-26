using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmithBot.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "WithdrawOrders");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "ForBotBalance",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "NFTType",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NFTs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "NFTs",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OwnerTelegramId",
                table: "NFTs",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "OwnerWallet",
                table: "NFTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "OwnerTelegramId",
                table: "NFTs");

            migrationBuilder.DropColumn(
                name: "OwnerWallet",
                table: "NFTs");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "NFTs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "ForBotBalance",
                table: "NFTs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "NFTs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NFTType",
                table: "NFTs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "NFTs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "NFTs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    NFTType = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionId = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    WaitPayment = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawOrders",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawOrders", x => x.UserId);
                });
        }
    }
}
