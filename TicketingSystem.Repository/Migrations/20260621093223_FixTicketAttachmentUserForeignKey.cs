using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketAttachmentUserForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Users_UserId",
                table: "TicketAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TicketAttachments_UserId",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TicketAttachments");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_UploadedBy",
                table: "TicketAttachments",
                column: "UploadedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachments_Users_UploadedBy",
                table: "TicketAttachments",
                column: "UploadedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachments_Users_UploadedBy",
                table: "TicketAttachments");

            migrationBuilder.DropIndex(
                name: "IX_TicketAttachments_UploadedBy",
                table: "TicketAttachments");

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
    }
}
