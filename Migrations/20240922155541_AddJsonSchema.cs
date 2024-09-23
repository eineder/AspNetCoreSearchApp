using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreSearchApp.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JsonSchemaId",
                table: "JsonDocuments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JsonSchemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SchemaName = table.Column<string>(type: "TEXT", nullable: false),
                    SchemaData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JsonSchemas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JsonDocuments_JsonSchemaId",
                table: "JsonDocuments",
                column: "JsonSchemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_JsonDocuments_JsonSchemas_JsonSchemaId",
                table: "JsonDocuments",
                column: "JsonSchemaId",
                principalTable: "JsonSchemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JsonDocuments_JsonSchemas_JsonSchemaId",
                table: "JsonDocuments");

            migrationBuilder.DropTable(
                name: "JsonSchemas");

            migrationBuilder.DropIndex(
                name: "IX_JsonDocuments_JsonSchemaId",
                table: "JsonDocuments");

            migrationBuilder.DropColumn(
                name: "JsonSchemaId",
                table: "JsonDocuments");
        }
    }
}
