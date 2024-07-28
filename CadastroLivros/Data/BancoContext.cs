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
        public DbSet<FormaCompra> FormaCompras { get; set; }
        public DbSet<PrecoLivro> PrecoLivros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assunto>(entity =>
            {
                entity.HasKey(e => e.CodAs).HasName("PK__Assunto__F41597616E10CD35");

                entity.ToTable("Assunto");

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.CodAu).HasName("PK__Autor__F4159767F9692936");

                entity.ToTable("Autor");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FormaCompra>(entity =>
            {
                entity.HasKey(e => e.CodCom).HasName("PK__FormaCom__98E6D1F216219E02");

                entity.ToTable("FormaCompra");

                entity.Property(e => e.Desconto).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.Codl).HasName("PK__Livro__A25C5ABF3F3B7DA5");

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
                    .UsingEntity<Dictionary<string, object>>(
                        "LivroAssunto",
                        r => r.HasOne<Assunto>().WithMany()
                            .HasForeignKey("AssuntoCodAs")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__LivroAssu__Assun__6754599E"),
                        l => l.HasOne<Livro>().WithMany()
                            .HasForeignKey("LivroCodl")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__LivroAssu__Livro__66603565"),
                        j =>
                        {
                            j.HasKey("LivroCodl", "AssuntoCodAs");
                            j.ToTable("LivroAssunto");
                        });

                entity.HasMany(d => d.Autores).WithMany(p => p.Livros)
                    .UsingEntity<Dictionary<string, object>>(
                        "LivroAutor",
                        r => r.HasOne<Autor>().WithMany()
                            .HasForeignKey("AutorCodAu")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__LivroAuto__Autor__6383C8BA"),
                        l => l.HasOne<Livro>().WithMany()
                            .HasForeignKey("LivroCodl")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__LivroAuto__Livro__628FA481"),
                        j =>
                        {
                            j.HasKey("LivroCodl", "AutorCodAu");
                            j.ToTable("LivroAutor");
                        });
            });

            modelBuilder.Entity<PrecoLivro>(entity =>
            {
                entity.HasKey(e => e.CodPrecoLivro).HasName("PK__PrecoLiv__0F5E640291B5047A");

                entity.ToTable("PrecoLivro");

                entity.Property(e => e.DataFim).HasColumnType("datetime");
                entity.Property(e => e.DataInicio).HasColumnType("datetime");
                entity.Property(e => e.Valor).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.FormaCompra).WithMany(p => p.PrecoLivros)
                    .HasForeignKey(d => d.FormaCompraId)
                    .HasConstraintName("FK__PrecoLivr__Forma__6D0D32F4");

                entity.HasOne(d => d.Livro).WithMany(p => p.PrecoLivros)
                    .HasForeignKey(d => d.LivroCodl)
                    .HasConstraintName("FK__PrecoLivr__Livro__6C190EBB");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
