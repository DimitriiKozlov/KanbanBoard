using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KanbanBoard.Migrations
{
    public partial class updateEF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_States",
                table: "States");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "States");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "States",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "States",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Cards",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatePriority",
                table: "Cards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_States",
                table: "States",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_StatePriority",
                table: "Cards",
                column: "StatePriority");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_States_StatePriority",
                table: "Cards",
                column: "StatePriority",
                principalTable: "States",
                principalColumn: "Priority",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_States_StatePriority",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_States",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Cards_StatePriority",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "States");

            migrationBuilder.DropColumn(
                name: "StatePriority",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "States",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "States",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Cards",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "Cards",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_States",
                table: "States",
                column: "Id");
        }
    }
}
