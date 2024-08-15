﻿// <auto-generated />
using System;
using CadastroLivros.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CadastroLivros.Migrations
{
    [DbContext(typeof(BancoContext))]
    partial class BancoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CadastroLivros.DTOs.LivroRelatorioDto", b =>
                {
                    b.Property<string>("AnoPublicacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Assuntos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Edicao")
                        .HasColumnType("int");

                    b.Property<string>("Editora")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeAutor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrecoBase")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TituloLivro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable((string)null);

                    b.ToView("vw_LivroRelatorio", (string)null);
                });

            modelBuilder.Entity("CadastroLivros.Models.Assunto", b =>
                {
                    b.Property<int>("CodAs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodAs"));

                    b.Property<bool>("Ativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("CodAs");

                    b.ToTable("Assunto", (string)null);
                });

            modelBuilder.Entity("CadastroLivros.Models.Autor", b =>
                {
                    b.Property<int>("CodAu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodAu"));

                    b.Property<bool>("Ativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.HasKey("CodAu");

                    b.ToTable("Autor", (string)null);
                });

            modelBuilder.Entity("CadastroLivros.Models.FormaCompra", b =>
                {
                    b.Property<int>("CodCom")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodCom"));

                    b.Property<decimal>("Desconto")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("CodCom");

                    b.ToTable("FormaCompra", (string)null);
                });

            modelBuilder.Entity("CadastroLivros.Models.Livro", b =>
                {
                    b.Property<int>("Codl")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Codl"));

                    b.Property<string>("AnoPublicacao")
                        .IsRequired()
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("char(4)")
                        .IsFixedLength();

                    b.Property<int>("Edicao")
                        .HasColumnType("int");

                    b.Property<string>("Editora")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.Property<decimal>("PrecoBase")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.HasKey("Codl");

                    b.ToTable("Livro", (string)null);
                });

            modelBuilder.Entity("CadastroLivros.Models.LivroAssunto", b =>
                {
                    b.Property<int>("LivroCodl")
                        .HasColumnType("int");

                    b.Property<int>("AssuntoCodAs")
                        .HasColumnType("int");

                    b.Property<int?>("AssuntoCodAs1")
                        .HasColumnType("int");

                    b.Property<int?>("LivroCodl1")
                        .HasColumnType("int");

                    b.HasKey("LivroCodl", "AssuntoCodAs");

                    b.HasIndex("AssuntoCodAs");

                    b.HasIndex("AssuntoCodAs1");

                    b.HasIndex("LivroCodl1");

                    b.ToTable("LivroAssunto", (string)null);
                });

            modelBuilder.Entity("CadastroLivros.Models.LivroAutor", b =>
                {
                    b.Property<int>("LivroCodl")
                        .HasColumnType("int");

                    b.Property<int>("AutorCodAu")
                        .HasColumnType("int");

                    b.Property<int?>("AutorCodAu1")
                        .HasColumnType("int");

                    b.Property<int?>("LivroCodl1")
                        .HasColumnType("int");

                    b.HasKey("LivroCodl", "AutorCodAu");

                    b.HasIndex("AutorCodAu");

                    b.HasIndex("AutorCodAu1");

                    b.HasIndex("LivroCodl1");

                    b.ToTable("LivroAutor", (string)null);
                });

            modelBuilder.Entity("CadastroLivros.Models.LivroAssunto", b =>
                {
                    b.HasOne("CadastroLivros.Models.Assunto", "Assunto")
                        .WithMany()
                        .HasForeignKey("AssuntoCodAs")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CadastroLivros.Models.Assunto", null)
                        .WithMany("LivroAssuntos")
                        .HasForeignKey("AssuntoCodAs1");

                    b.HasOne("CadastroLivros.Models.Livro", "Livro")
                        .WithMany()
                        .HasForeignKey("LivroCodl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CadastroLivros.Models.Livro", null)
                        .WithMany("LivroAssuntos")
                        .HasForeignKey("LivroCodl1");

                    b.Navigation("Assunto");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("CadastroLivros.Models.LivroAutor", b =>
                {
                    b.HasOne("CadastroLivros.Models.Autor", "Autor")
                        .WithMany()
                        .HasForeignKey("AutorCodAu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CadastroLivros.Models.Autor", null)
                        .WithMany("LivrosAutores")
                        .HasForeignKey("AutorCodAu1");

                    b.HasOne("CadastroLivros.Models.Livro", "Livro")
                        .WithMany()
                        .HasForeignKey("LivroCodl")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CadastroLivros.Models.Livro", null)
                        .WithMany("LivroAutores")
                        .HasForeignKey("LivroCodl1");

                    b.Navigation("Autor");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("CadastroLivros.Models.Assunto", b =>
                {
                    b.Navigation("LivroAssuntos");
                });

            modelBuilder.Entity("CadastroLivros.Models.Autor", b =>
                {
                    b.Navigation("LivrosAutores");
                });

            modelBuilder.Entity("CadastroLivros.Models.Livro", b =>
                {
                    b.Navigation("LivroAssuntos");

                    b.Navigation("LivroAutores");
                });
#pragma warning restore 612, 618
        }
    }
}
