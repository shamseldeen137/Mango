﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.EmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class sourcesmb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrokerSource",
                table: "EmailLoggers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokerSource",
                table: "EmailLoggers");
        }
    }
}
