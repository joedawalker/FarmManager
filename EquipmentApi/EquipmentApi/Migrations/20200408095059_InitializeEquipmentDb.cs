using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EquipmentApi.Migrations
{
    public partial class InitializeEquipmentDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    HoursUsed = table.Column<TimeSpan>(nullable: false),
                    Mileage = table.Column<double>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    RecuringType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Instructions = table.Column<string>(nullable: true),
                    ScheduleId = table.Column<int>(nullable: true),
                    NextService = table.Column<DateTime>(nullable: true),
                    EquipmentItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceItems_EquipmentItems_EquipmentItemId",
                        column: x => x.EquipmentItemId,
                        principalTable: "EquipmentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceItems_MaintenanceSchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "MaintenanceSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    LogType = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    UpdatedById = table.Column<int>(nullable: true),
                    HoursUsedSnapshot = table.Column<TimeSpan>(nullable: false),
                    MileageSnapshot = table.Column<double>(nullable: false),
                    MaintenanceItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceLog_MaintenanceItems_MaintenanceItemId",
                        column: x => x.MaintenanceItemId,
                        principalTable: "MaintenanceItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceLog_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequiredEquipmentParts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    MaintenanceItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequiredEquipmentParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequiredEquipmentParts_MaintenanceItems_MaintenanceItemId",
                        column: x => x.MaintenanceItemId,
                        principalTable: "MaintenanceItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequiredEquipmentParts_EquipmentParts_PartId",
                        column: x => x.PartId,
                        principalTable: "EquipmentParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItems_EquipmentItemId",
                table: "MaintenanceItems",
                column: "EquipmentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItems_ScheduleId",
                table: "MaintenanceItems",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLog_MaintenanceItemId",
                table: "MaintenanceLog",
                column: "MaintenanceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLog_UpdatedById",
                table: "MaintenanceLog",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredEquipmentParts_MaintenanceItemId",
                table: "RequiredEquipmentParts",
                column: "MaintenanceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequiredEquipmentParts_PartId",
                table: "RequiredEquipmentParts",
                column: "PartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceLog");

            migrationBuilder.DropTable(
                name: "RequiredEquipmentParts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MaintenanceItems");

            migrationBuilder.DropTable(
                name: "EquipmentParts");

            migrationBuilder.DropTable(
                name: "EquipmentItems");

            migrationBuilder.DropTable(
                name: "MaintenanceSchedules");
        }
    }
}
