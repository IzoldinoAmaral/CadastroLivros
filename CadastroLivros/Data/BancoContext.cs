using CadastroLivros.DTOs;
using CadastroLivros.Models;
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
        public DbSet<LivroRelatorioDto> VwLivroRelatorio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LivroRelatorioDto>(entity =>
            {
                entity.HasNoKey(); 
                entity.ToView("vw_LivroRelatorio"); 
            });

            

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

                entity.Property(e => e.Ativo) 
                .IsRequired()
                .HasDefaultValue(true);
            });

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.CodAu);
                entity.ToTable("Autor");
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Ativo) 
                    .IsRequired()
                    .HasDefaultValue(true);
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

                entity.HasMany(d => d.Assuntos).WithMany(p => p.Livros)
                    .UsingEntity<LivroAssunto>(
                        j => j.HasOne(la => la.Assunto)
                              .WithMany()
                              .HasForeignKey(la => la.AssuntoCodAs)
                              .OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne(la => la.Livro)
                              .WithMany()
                              .HasForeignKey(la => la.LivroCodl)
                              .OnDelete(DeleteBehavior.Cascade),
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
                              .OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne(la => la.Livro)
                              .WithMany()
                              .HasForeignKey(la => la.LivroCodl)
                              .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey(la => new { la.LivroCodl, la.AutorCodAu });
                            j.ToTable("LivroAutor");
                        });

                OnModelCreatingPartial(modelBuilder);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    } }
