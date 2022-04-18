using Microsoft.EntityFrameworkCore.Migrations;

namespace CA.DAL.Migrations
{
    public partial class SymptomIdadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Symptoms_SymptomId",
                table: "Criterias");

            migrationBuilder.AlterColumn<int>(
                name: "SymptomId",
                table: "Criterias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Symptoms_SymptomId",
                table: "Criterias",
                column: "SymptomId",
                principalTable: "Symptoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Symptoms_SymptomId",
                table: "Criterias");

            migrationBuilder.AlterColumn<int>(
                name: "SymptomId",
                table: "Criterias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Symptoms_SymptomId",
                table: "Criterias",
                column: "SymptomId",
                principalTable: "Symptoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
