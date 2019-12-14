using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.LiveHub.Infrastrue.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lc_account",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    account_no = table.Column<string>(type: "varchar(20)", nullable: true),
                    account_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    open_id = table.Column<string>(type: "varchar(64)", nullable: true),
                    nick_name = table.Column<string>(type: "varchar(32)", nullable: true),
                    avatar = table.Column<string>(type: "varchar(500)", nullable: true),
                    department = table.Column<string>(type: "varchar(50)", nullable: true),
                    password = table.Column<string>(type: "varchar(32)", nullable: true),
                    status = table.Column<int>(nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", nullable: true),
                    mail_address = table.Column<string>(type: "varchar(32)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lc_account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lc_role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    role_name = table.Column<string>(type: "varchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lc_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lc_business",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from = table.Column<string>(type: "varchar(50)", nullable: true),
                    to = table.Column<string>(type: "varchar(50)", nullable: true),
                    sign_date = table.Column<DateTime>(nullable: false),
                    create_date = table.Column<DateTime>(nullable: false),
                    business_type = table.Column<int>(nullable: false),
                    am_pm = table.Column<int>(nullable: false),
                    account_id = table.Column<long>(nullable: false),
                    remark = table.Column<string>(type: "varchar(100)", nullable: true),
                    status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lc_business", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lc_business_lc_account_account_id",
                        column: x => x.account_id,
                        principalTable: "lc_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lh_game_score",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    score = table.Column<int>(nullable: false),
                    create_time = table.Column<DateTime>(nullable: false),
                    account_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lh_game_score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lh_game_score_lc_account_account_id",
                        column: x => x.account_id,
                        principalTable: "lc_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lh_sign_in",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sign_text = table.Column<string>(type: "varchar(100)", nullable: true),
                    sign_date = table.Column<DateTime>(nullable: false),
                    account_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lh_sign_in", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lh_sign_in_lc_account_account_id",
                        column: x => x.account_id,
                        principalTable: "lc_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lh_todolist",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(64)", nullable: true),
                    status = table.Column<int>(nullable: false),
                    @checked = table.Column<bool>(name: "checked", nullable: false),
                    order = table.Column<int>(nullable: true),
                    type = table.Column<int>(nullable: false),
                    account_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lh_todolist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lh_todolist_lc_account_account_id",
                        column: x => x.account_id,
                        principalTable: "lc_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lc_user_role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lc_user_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lc_user_role_lc_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "lc_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lc_user_role_lc_account_UserId",
                        column: x => x.UserId,
                        principalTable: "lc_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "lc_role",
                columns: new[] { "Id", "role_name" },
                values: new object[] { 1L, "助理" });

            migrationBuilder.InsertData(
                table: "lc_role",
                columns: new[] { "Id", "role_name" },
                values: new object[] { 2L, "普通用户" });

            migrationBuilder.CreateIndex(
                name: "IX_lc_business_account_id",
                table: "lc_business",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_lc_user_role_RoleId",
                table: "lc_user_role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_lc_user_role_UserId",
                table: "lc_user_role",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_lh_game_score_account_id",
                table: "lh_game_score",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_lh_sign_in_account_id",
                table: "lh_sign_in",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_lh_todolist_account_id",
                table: "lh_todolist",
                column: "account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lc_business");

            migrationBuilder.DropTable(
                name: "lc_user_role");

            migrationBuilder.DropTable(
                name: "lh_game_score");

            migrationBuilder.DropTable(
                name: "lh_sign_in");

            migrationBuilder.DropTable(
                name: "lh_todolist");

            migrationBuilder.DropTable(
                name: "lc_role");

            migrationBuilder.DropTable(
                name: "lc_account");
        }
    }
}
