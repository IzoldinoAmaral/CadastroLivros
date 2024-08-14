using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroLivros.Migrations
{
    /// <inheritdoc />
    public partial class AjustesRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assunto",
                columns: table => new
                {
                    CodAs = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assunto", x => x.CodAs);
                });

            migrationBuilder.CreateTable(
                name: "Autor",
                columns: table => new
                {
                    CodAu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autor", x => x.CodAu);
                });

            migrationBuilder.CreateTable(
                name: "FormaCompra",
                columns: table => new
                {
                    CodCom = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaCompra", x => x.CodCom);
                });

            migrationBuilder.CreateTable(
                name: "Livro",
                columns: table => new
                {
                    Codl = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Editora = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false),
                    Edicao = table.Column<int>(type: "int", nullable: false),
                    AnoPublicacao = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    PrecoBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livro", x => x.Codl);
                });

            migrationBuilder.CreateTable(
                name: "LivroAssunto",
                columns: table => new
                {
                    LivroCodl = table.Column<int>(type: "int", nullable: false),
                    AssuntoCodAs = table.Column<int>(type: "int", nullable: false),
                    AssuntoCodAs1 = table.Column<int>(type: "int", nullable: true),
                    LivroCodl1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroAssunto", x => new { x.LivroCodl, x.AssuntoCodAs });
                    table.ForeignKey(
                        name: "FK_LivroAssunto_Assunto_AssuntoCodAs",
                        column: x => x.AssuntoCodAs,
                        principalTable: "Assunto",
                        principalColumn: "CodAs");
                    table.ForeignKey(
                        name: "FK_LivroAssunto_Assunto_AssuntoCodAs1",
                        column: x => x.AssuntoCodAs1,
                        principalTable: "Assunto",
                        principalColumn: "CodAs");
                    table.ForeignKey(
                        name: "FK_LivroAssunto_Livro_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livro",
                        principalColumn: "Codl");
                    table.ForeignKey(
                        name: "FK_LivroAssunto_Livro_LivroCodl1",
                        column: x => x.LivroCodl1,
                        principalTable: "Livro",
                        principalColumn: "Codl");
                });

            migrationBuilder.CreateTable(
                name: "LivroAutor",
                columns: table => new
                {
                    LivroCodl = table.Column<int>(type: "int", nullable: false),
                    AutorCodAu = table.Column<int>(type: "int", nullable: false),
                    AutorCodAu1 = table.Column<int>(type: "int", nullable: true),
                    LivroCodl1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroAutor", x => new { x.LivroCodl, x.AutorCodAu });
                    table.ForeignKey(
                        name: "FK_LivroAutor_Autor_AutorCodAu",
                        column: x => x.AutorCodAu,
                        principalTable: "Autor",
                        principalColumn: "CodAu");
                    table.ForeignKey(
                        name: "FK_LivroAutor_Autor_AutorCodAu1",
                        column: x => x.AutorCodAu1,
                        principalTable: "Autor",
                        principalColumn: "CodAu");
                    table.ForeignKey(
                        name: "FK_LivroAutor_Livro_LivroCodl",
                        column: x => x.LivroCodl,
                        principalTable: "Livro",
                        principalColumn: "Codl");
                    table.ForeignKey(
                        name: "FK_LivroAutor_Livro_LivroCodl1",
                        column: x => x.LivroCodl1,
                        principalTable: "Livro",
                        principalColumn: "Codl");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LivroAssunto_AssuntoCodAs",
                table: "LivroAssunto",
                column: "AssuntoCodAs");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAssunto_AssuntoCodAs1",
                table: "LivroAssunto",
                column: "AssuntoCodAs1");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAssunto_LivroCodl1",
                table: "LivroAssunto",
                column: "LivroCodl1");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAutor_AutorCodAu",
                table: "LivroAutor",
                column: "AutorCodAu");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAutor_AutorCodAu1",
                table: "LivroAutor",
                column: "AutorCodAu1");

            migrationBuilder.CreateIndex(
                name: "IX_LivroAutor_LivroCodl1",
                table: "LivroAutor",
                column: "LivroCodl1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormaCompra");

            migrationBuilder.DropTable(
                name: "LivroAssunto");

            migrationBuilder.DropTable(
                name: "LivroAutor");

            migrationBuilder.DropTable(
                name: "Assunto");

            migrationBuilder.DropTable(
                name: "Autor");

            migrationBuilder.DropTable(
                name: "Livro");
        }
    }
}
