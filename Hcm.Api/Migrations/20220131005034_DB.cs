using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hcm.Api.Migrations
{
    public partial class DB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "departments",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 64, nullable: false),
                    country = table.Column<string>(fixedLength: true, maxLength: 3, nullable: false),
                    city = table.Column<string>(maxLength: 128, nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 64, nullable: false),
                    username = table.Column<string>(maxLength: 128, nullable: false),
                    password = table.Column<string>(maxLength: 1024, nullable: false),
                    phone = table.Column<string>(maxLength: 16, nullable: true),
                    email = table.Column<string>(maxLength: 128, nullable: false),
                    role = table.Column<int>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 64, nullable: false),
                    avatar = table.Column<string>(nullable: true),
                    first_name = table.Column<string>(maxLength: 64, nullable: false),
                    last_name = table.Column<string>(maxLength: 64, nullable: false),
                    phone = table.Column<string>(maxLength: 16, nullable: false),
                    email = table.Column<string>(maxLength: 128, nullable: false),
                    country = table.Column<string>(fixedLength: true, maxLength: 3, nullable: false),
                    address_line = table.Column<string>(maxLength: 512, nullable: true),
                    post_code = table.Column<string>(maxLength: 32, nullable: true),
                    city = table.Column<string>(maxLength: 128, nullable: false),
                    user_id = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 64, nullable: false),
                    job_title = table.Column<string>(maxLength: 32, nullable: false),
                    start_date = table.Column<DateTime>(nullable: false),
                    end_date = table.Column<DateTime>(nullable: true),
                    employee_id = table.Column<string>(nullable: false),
                    department_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_assignments_departments_department_id",
                        column: x => x.department_id,
                        principalSchema: "dbo",
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assignments_employees_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "dbo",
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "salaries",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 64, nullable: false),
                    currency = table.Column<string>(fixedLength: true, maxLength: 3, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    assignment_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaries", x => x.id);
                    table.ForeignKey(
                        name: "FK_salaries_assignments_assignment_id",
                        column: x => x.assignment_id,
                        principalSchema: "dbo",
                        principalTable: "assignments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "users",
                columns: new[] { "id", "email", "password", "phone", "role", "username" },
                values: new object[] { "9f66a868-85f5-49b7-b869-a3d8952ae78e", "administrator@hcm.com", "6b45f1416832256171cbfe03140a4cab", "+359878121212121", 1, "administrator@hcm.com" });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_department_id",
                schema: "dbo",
                table: "assignments",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_employee_id",
                schema: "dbo",
                table: "assignments",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_email",
                schema: "dbo",
                table: "employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_phone",
                schema: "dbo",
                table: "employees",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_user_id",
                schema: "dbo",
                table: "employees",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_salaries_assignment_id",
                schema: "dbo",
                table: "salaries",
                column: "assignment_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salaries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "assignments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "departments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "employees",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");
        }
    }
}
