using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "TicketAttachments",
                newName: "FileUrl");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "TicketAttachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UploadedBy",
                table: "TicketAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TicketAttachments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_UserId",
                table: "TicketAttachments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_Users_UserId",
                table: "TicketAttachments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Users_UserId",
                table: "TicketAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TicketAttachments_UserId",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "UploadedBy",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TicketAttachments");

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "TicketAttachments",
                newName: "FilePath");
        }
    }
}
