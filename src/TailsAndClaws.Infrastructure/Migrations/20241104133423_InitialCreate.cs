using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailsAndClaws.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dogs-app");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "dogs",
                schema: "dogs-app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, computedColumnSql: "UPPER(name)", stored: true),
                    tail_length_in_meters = table.Column<decimal>(type: "numeric(6,3)", precision: 6, scale: 3, nullable: false),
                    weight_in_kg = table.Column<decimal>(type: "numeric(8,3)", precision: 8, scale: 3, nullable: false),
                    color = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dogs", x => x.id);
                    table.CheckConstraint("CHK_dogs_tail_length_in_meters_must_be_greater_than_zero", "tail_length_in_meters > 0");
                    table.CheckConstraint("CHK_dogs_weight_in_kg_must_be_greater_than_zero", "weight_in_kg > 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_dogs_normalized_name",
                schema: "dogs-app",
                table: "dogs",
                column: "normalized_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dogs",
                schema: "dogs-app");
        }
    }
}
