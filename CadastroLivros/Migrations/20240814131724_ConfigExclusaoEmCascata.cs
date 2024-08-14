using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroLivros.Migrations
{
    /// <inheritdoc />
    public partial class ConfigExclusaoEmCascata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAssunto_Assunto_AssuntoCodAs",
                table: "LivroAssunto");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAssunto_Livro_LivroCodl",
                table: "LivroAssunto");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutor_Autor_AutorCodAu",
                table: "LivroAutor");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutor_Livro_LivroCodl",
                table: "LivroAutor");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAssunto_Assunto_AssuntoCodAs",
                table: "LivroAssunto",
                column: "AssuntoCodAs",
                principalTable: "Assunto",
                principalColumn: "CodAs",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAssunto_Livro_LivroCodl",
                table: "LivroAssunto",
                column: "LivroCodl",
                principalTable: "Livro",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutor_Autor_AutorCodAu",
                table: "LivroAutor",
                column: "AutorCodAu",
                principalTable: "Autor",
                principalColumn: "CodAu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutor_Livro_LivroCodl",
                table: "LivroAutor",
                column: "LivroCodl",
                principalTable: "Livro",
                principalColumn: "Codl",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAssunto_Assunto_AssuntoCodAs",
                table: "LivroAssunto");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAssunto_Livro_LivroCodl",
                table: "LivroAssunto");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutor_Autor_AutorCodAu",
                table: "LivroAutor");

            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutor_Livro_LivroCodl",
                table: "LivroAutor");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAssunto_Assunto_AssuntoCodAs",
                table: "LivroAssunto",
                column: "AssuntoCodAs",
                principalTable: "Assunto",
                principalColumn: "CodAs");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAssunto_Livro_LivroCodl",
                table: "LivroAssunto",
                column: "LivroCodl",
                principalTable: "Livro",
                principalColumn: "Codl");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutor_Autor_AutorCodAu",
                table: "LivroAutor",
                column: "AutorCodAu",
                principalTable: "Autor",
                principalColumn: "CodAu");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutor_Livro_LivroCodl",
                table: "LivroAutor",
                column: "LivroCodl",
                principalTable: "Livro",
                principalColumn: "Codl");
        }
    }
}
