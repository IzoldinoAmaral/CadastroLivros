# Cadastro de Livros

O projeto tem por objetivo faze rum cadastro de livro

## 🚀 Como executar o projeto

Para que você possa ver em funcionamento o projeto é preciso que você:
* Clone esse projeto em um diretório da sua maquina.
* Uma IDE para rodar projeto .Net, de preferência Visual Studio 2022.
* Banco de dados SQLServer Express Instalado
* Edite o appsettings.json com sua connectionString, com as config do seu Banco
* Abrir o projeto e executar o seguinte comando no terminal **_dotnet ef database update_**
* Execute o script abaixo no SSMS ou ferramenta semelhante:
```
USE cadastroLivros;

CREATE VIEW vw_LivroRelatorio AS
SELECT	a.Nome AS NomeAutor,
		l.Titulo AS TituloLivro,
		l.Editora,
		l.PrecoBase,
		l.Edicao,
		l.AnoPublicacao,
		COALESCE(STRING_AGG(ass.Descricao, ', '), '') AS Assuntos
FROM LivroAutor la
INNER JOIN Livro l ON la.LivroCodl = l.Codl
INNER JOIN Autor a ON la.AutorCodAu = a.CodAu
LEFT JOIN LivroAssunto lass ON l.Codl = lass.LivroCodl
LEFT JOIN  Assunto ass ON lass.AssuntoCodAs = ass.CodAs
GROUP BY	a.Nome, 
			l.Titulo, 
			l.Editora, 
			l.PrecoBase, 
			l.Edicao, 
			l.AnoPublicacao;

```

* Após isso ja pode buildar e executar o projeto

