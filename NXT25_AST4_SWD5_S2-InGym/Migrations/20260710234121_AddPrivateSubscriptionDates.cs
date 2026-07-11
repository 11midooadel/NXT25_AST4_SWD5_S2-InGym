using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXT25_AST4_SWD5_S2InGym.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivateSubscriptionDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "MemberPrivateSubs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MemberPrivateSubs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "MemberPrivateSubs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "MemberPrivateSubs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MemberPrivateSubs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MemberPrivateSubs");
        }
    }
}
