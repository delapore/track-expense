﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    Recipient = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Currency", "Recipient", "TransactionDate", "Type" },
                values: new object[] { 1L, 300.0, "CHF", "Alice", new DateTime(2020, 9, 26, 0, 0, 0, 0, DateTimeKind.Local), 1 });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Currency", "Recipient", "TransactionDate", "Type" },
                values: new object[] { 2L, 200.0, "EUR", "Bob", new DateTime(2020, 9, 27, 0, 0, 0, 0, DateTimeKind.Local), 2 });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Currency", "Recipient", "TransactionDate", "Type" },
                values: new object[] { 3L, 100.0, "USD", "Carol", new DateTime(2020, 9, 27, 0, 0, 0, 0, DateTimeKind.Local), 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");
        }
    }
}
