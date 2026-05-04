using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aulas.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTuitionFeeDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TuitionFee",
                table: "AppUsers",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TuitionFee",
                table: "AppUsers",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,2)",
                oldPrecision: 9,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
