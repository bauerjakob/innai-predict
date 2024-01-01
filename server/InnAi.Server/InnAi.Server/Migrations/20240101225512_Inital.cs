using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InnAi.Server.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Default = table.Column<bool>(type: "boolean", nullable: false),
                    PrecipitationMapSize = table.Column<int>(type: "integer", nullable: false),
                    NumberOfInnLevels = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AiModelResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AiModelId = table.Column<int>(type: "integer", nullable: false),
                    PredictionValues = table.Column<string>(type: "text", nullable: false),
                    ActualValues = table.Column<string>(type: "text", nullable: false),
                    AverageDeviation = table.Column<double>(type: "double precision", nullable: false),
                    PercentageDeviation = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiModelResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AiModelResults_AiModels_AiModelId",
                        column: x => x.AiModelId,
                        principalTable: "AiModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AiModelResults_AiModelId",
                table: "AiModelResults",
                column: "AiModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AiModelResults_DateTime_AiModelId",
                table: "AiModelResults",
                columns: new[] { "DateTime", "AiModelId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AiModels_ExternalId",
                table: "AiModels",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiModelResults");

            migrationBuilder.DropTable(
                name: "AiModels");
        }
    }
}
