# Cadastro de Livros

O projeto tem por objetivo faze rum cadastro de livro

## 🚀 Como executar o projeto

Para que você possa ver em funcionamento o projeto é preciso que você:
* Clone esse projeto em um diretório da sua maquina.
* Uma IDE para rodar projeto .Net, de preferência Visual Studio 2022.
* Banco de dados SQLServer Express Instalado
* Edite o appsettings.json com sua connectionString, com as config do seu Banco
   e execute o script abaixo:
```
-- Cria o banco de dados cadastroLivros
CREATE DATABASE cadastroLivros;

-- Utiliza o banco de dados recém-criado
USE cadastroLivros;

CREATE TABLE Livro (
    Codl INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(40) NOT NULL,
    Editora VARCHAR(40),
	PrecoBase DECIMAL(10,2) NOT NULL,
    Edicao INT,
    AnoPublicacao CHAR(4)
);

CREATE TABLE Autor (
    CodAu INT PRIMARY KEY IDENTITY(1,1),
    Nome VARCHAR(40) NOT NULL
);

CREATE TABLE Assunto (
    CodAs INT PRIMARY KEY IDENTITY(1,1),
    Descricao VARCHAR(20) NOT NULL
);

CREATE TABLE LivroAutor (
    LivroCodl INT FOREIGN KEY REFERENCES Livro(Codl),
    AutorCodAu INT FOREIGN KEY REFERENCES Autor(CodAu),
    CONSTRAINT PK_LivroAutor PRIMARY KEY (LivroCodl, AutorCodAu)
);

CREATE TABLE LivroAssunto (
    LivroCodl INT FOREIGN KEY REFERENCES Livro(Codl),
    AssuntoCodAs INT FOREIGN KEY REFERENCES Assunto(CodAs),
    CONSTRAINT PK_LivroAssunto PRIMARY KEY (LivroCodl, AssuntoCodAs)
);

CREATE TABLE FormaCompra (
    CodCom INT PRIMARY KEY IDENTITY(1,1),
    Descricao VARCHAR(20) NOT NULL,
    Desconto DECIMAL(5,2) 
);

```
* Após isso ja pode buildar e executar o projeto

