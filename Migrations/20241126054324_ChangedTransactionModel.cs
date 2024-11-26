using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infinicare_Ojash_Devkota.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTransactionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverAccountId",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "SenderAccountId",
                table: "TransactionDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverAccountId",
                table: "TransactionDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SenderAccountId",
                table: "TransactionDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
