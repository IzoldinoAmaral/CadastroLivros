﻿using CadastroLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data
{
    public partial class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }
        public DbSet<LivroAutor> LivroAutores { get; set; }
        public DbSet<LivroAssunto> LivroAssuntos { get; set; }
        public DbSet<FormaCompra> FormaCompras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LivroAutor>()
                .HasKey(la => new { la.LivroCodl, la.AutorCodAu });

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAutores)
                .HasForeignKey(la => la.LivroCodl)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.LivrosAutores)
                .HasForeignKey(la => la.AutorCodAu)
                .OnDelete(DeleteBehavior.Cascade);  

            // Mapeamento para LivroAssunto
            modelBuilder.Entity<LivroAssunto>()
                .HasKey(la => new { la.LivroCodl, la.AssuntoCodAs });

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAssuntos)
                .HasForeignKey(la => la.LivroCodl)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Assunto)
                .WithMany(a => a.LivroAssuntos)
                .HasForeignKey(la => la.AssuntoCodAs)
                .OnDelete(DeleteBehavior.Cascade);  



            modelBuilder.Entity<Assunto>(entity =>
            {
                entity.HasKey(e => e.CodAs);
                entity.ToTable("Assunto");
                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.CodAu);
                entity.ToTable("Autor");
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FormaCompra>(entity =>
            {
                entity.HasKey(e => e.CodCom);
                entity.ToTable("FormaCompra");
                entity.Property(e => e.Desconto).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.Codl);
                entity.ToTable("Livro");

                entity.Property(e => e.AnoPublicacao)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();
                entity.Property(e => e.Editora)
                    .HasMaxLength(40)
                    .IsUnicode(false);
                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                // Configurando as relações muitos-para-muitos com LivroAutor e LivroAssunto
                entity.HasMany(d => d.Assuntos).WithMany(p => p.Livros)
                    .UsingEntity<LivroAssunto>(
                        j => j.HasOne(la => la.Assunto)
                              .WithMany()
                              .HasForeignKey(la => la.AssuntoCodAs)
                              .OnDelete(DeleteBehavior.ClientSetNull),
                        j => j.HasOne(la => la.Livro)
                              .WithMany()
                              .HasForeignKey(la => la.LivroCodl)
                              .OnDelete(DeleteBehavior.ClientSetNull),
                        j =>
                        {
                            j.HasKey(la => new { la.LivroCodl, la.AssuntoCodAs });
                            j.ToTable("LivroAssunto");
                        });

                entity.HasMany(d => d.Autores).WithMany(p => p.Livros)
                    .UsingEntity<LivroAutor>(
                        j => j.HasOne(la => la.Autor)
                              .WithMany()
                              .HasForeignKey(la => la.AutorCodAu)
                              .OnDelete(DeleteBehavior.ClientSetNull),
                        j => j.HasOne(la => la.Livro)
                              .WithMany()
                              .HasForeignKey(la => la.LivroCodl)
                              .OnDelete(DeleteBehavior.ClientSetNull),
                        j =>
                        {
                            j.HasKey(la => new { la.LivroCodl, la.AutorCodAu });
                            j.ToTable("LivroAutor");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //public partial class BancoContext : DbContext
        //{
        //    public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        //    public DbSet<Livro> Livros { get; set; }
        //    public DbSet<Autor> Autores { get; set; }
        //    public DbSet<Assunto> Assuntos { get; set; }
        //    public DbSet<LivroAutor> LivroAutores { get; set; }
        //    public DbSet<LivroAssunto> LivroAssuntos { get; set; }
        //    public DbSet<FormaCompra> FormaCompras { get; set; }        

        //    protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //        modelBuilder.Entity<Assunto>(entity =>
        //        {
        //            entity.HasKey(e => e.CodAs);

        //            entity.ToTable("Assunto");

        //            entity.Property(e => e.Descricao)
        //                .IsRequired()
        //                .HasMaxLength(20)
        //                .IsUnicode(false);
        //        });


        //        modelBuilder.Entity<Autor>(entity =>
        //        {
        //            entity.HasKey(e => e.CodAu);

        //            entity.ToTable("Autor");

        //            entity.Property(e => e.Nome)
        //                .IsRequired()
        //                .HasMaxLength(40)
        //                .IsUnicode(false);
        //        });

        //        modelBuilder.Entity<LivroAutor>()
        //        .HasKey(la => new { la.LivroCodl, la.AutorCodAu });

        //        //modelBuilder.Entity<LivroAutor>()
        //        //    .HasOne(la => la.Livro)
        //        //    .WithMany(l => l.LivroAutores)
        //        //    .HasForeignKey(la => la.LivroCodl);

        //        //modelBuilder.Entity<LivroAutor>()
        //        //    .HasOne(la => la.Autor)
        //        //    .WithMany(a => a.LivrosAutores)
        //        //    .HasForeignKey(la => la.AutorCodAu);

        //        modelBuilder.Entity<LivroAssunto>() 
        //            .HasKey(la => new { la.LivroCodl, la.AssuntoCodAs });

        //        //modelBuilder.Entity<LivroAssunto>()
        //        //    .HasOne(la => la.Livro)
        //        //    .WithMany(l => l.LivroAssuntos)
        //        //    .HasForeignKey(la => la.LivroCodl);

        //        //modelBuilder.Entity<LivroAssunto>()
        //        //    .HasOne(la => la.Assunto)
        //        //    .WithMany(a => a.LivroAssuntos)
        //        //    .HasForeignKey(la => la.AssuntoCodAs);

        //        modelBuilder.Entity<FormaCompra>(entity =>
        //        {
        //            entity.HasKey(e => e.CodCom);

        //            entity.ToTable("FormaCompra");

        //            entity.Property(e => e.Desconto).HasColumnType("decimal(5, 2)");
        //            entity.Property(e => e.Descricao)
        //                .IsRequired()
        //                .HasMaxLength(20)
        //                .IsUnicode(false);
        //        });

        //        modelBuilder.Entity<Livro>(entity =>
        //        {
        //            entity.HasKey(e => e.Codl);

        //            entity.ToTable("Livro");

        //            entity.Property(e => e.AnoPublicacao)
        //                .HasMaxLength(4)
        //                .IsUnicode(false)
        //                .IsFixedLength();
        //            entity.Property(e => e.Editora)
        //                .HasMaxLength(40)
        //                .IsUnicode(false);
        //            entity.Property(e => e.Titulo)
        //                .IsRequired()
        //                .HasMaxLength(40)
        //                .IsUnicode(false);

        //            entity.HasMany(d => d.Assuntos).WithMany(p => p.Livros)
        //                .UsingEntity<Dictionary<string, object>>(
        //                    "LivroAssunto",
        //                    r => r.HasOne<Assunto>().WithMany()
        //                        .HasForeignKey("AssuntoCodAs")
        //                        .OnDelete(DeleteBehavior.ClientSetNull),

        //                    l => l.HasOne<Livro>().WithMany()
        //                        .HasForeignKey("LivroCodl")
        //                        .OnDelete(DeleteBehavior.ClientSetNull),

        //                    j =>
        //                    {
        //                        j.HasKey("LivroCodl", "AssuntoCodAs");
        //                        j.ToTable("LivroAssunto");
        //                    });

        //            entity.HasMany(d => d.Autores).WithMany(p => p.Livros)
        //                .UsingEntity<Dictionary<string, object>>(
        //                    "LivroAutor",
        //                    r => r.HasOne<Autor>().WithMany()
        //                        .HasForeignKey("AutorCodAu")
        //                        .OnDelete(DeleteBehavior.ClientSetNull),

        //                    l => l.HasOne<Livro>().WithMany()
        //                        .HasForeignKey("LivroCodl")
        //                        .OnDelete(DeleteBehavior.ClientSetNull),

        //                    j =>
        //                    {
        //                        j.HasKey("LivroCodl", "AutorCodAu");
        //                        j.ToTable("LivroAutor");
        //                    });
        //        });

        //        OnModelCreatingPartial(modelBuilder);
        //    }

        //    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        //}

    }
}
