using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace Hostsol.TFSTB.WebFrontend.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_IdentityRole_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropPrimaryKey(name: "PK_IdentityRole", table: "AspNetRoles");
            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationRole",
                table: "AspNetRoles",
                column: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_ApplicationRole_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_ApplicationRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_ApplicationRole_RoleId", table: "AspNetRoleClaims");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_ApplicationRole_RoleId", table: "AspNetUserRoles");
            migrationBuilder.DropPrimaryKey(name: "PK_ApplicationRole", table: "AspNetRoles");
            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRole",
                table: "AspNetRoles",
                column: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_IdentityRole_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_IdentityRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}
