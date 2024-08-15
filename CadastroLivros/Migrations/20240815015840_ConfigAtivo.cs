using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroLivros.Migrations
{
    /// <inheritdoc />
    public partial class ConfigAtivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Autor",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Assunto",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Autor");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Assunto");
        }
    }
}
