﻿using CadastroLivros.Interfaces.Repositorios;
using CadastroLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroLivros.Data.Repositorio
{
    public class LivroRepositorio : ILivroRepositorio
    {
        private readonly BancoContext _bancoContext;
        public LivroRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public async Task<bool> AdicionarAsync(Livro livro)
        {

            _bancoContext.Livros.Add(livro);
            
            await _bancoContext.SaveChangesAsync();

            if (livro.AutoresSelecionados != null && livro.AutoresSelecionados.Any())
            {
                foreach (var autorId in livro.AutoresSelecionados)
                {
                    _bancoContext.LivroAutores.Add(new LivroAutor
                    {
                        LivroCodl = livro.Codl,
                        AutorCodAu = autorId
                    });
                }
            }

            if (livro.AssuntosSelecionados != null && livro.AssuntosSelecionados.Any())
            {
                foreach (var assuntoId in livro.AssuntosSelecionados)
                {
                    _bancoContext.LivroAssuntos.Add(new LivroAssunto
                    {
                        LivroCodl = livro.Codl,
                        AssuntoCodAs = assuntoId
                    });
                }
            }

            await _bancoContext.SaveChangesAsync();

            return true;
            
        }

        public async Task<Livro> Atualizar(Livro livro)
        {         
            _bancoContext.Livros.Update(livro);
            await _bancoContext.SaveChangesAsync();
            return livro;           
        }

        public async Task<bool> BuscarPorNomeAsync(string titulo)
        {

            return await _bancoContext.Livros.AnyAsync(l => l.Titulo == titulo);    
            
        }

        public async Task<Livro> BuscarPorCodAsync(int cod)
        {
            return await _bancoContext.Livros.Include(l => l.LivroAutores)
                        .Include(l => l.LivroAssuntos)
                        .FirstOrDefaultAsync(c => c.Codl == cod);
        }

        public async Task<IEnumerable<Livro>> BuscarTodosAsync()
        {
            return await _bancoContext.Livros
                        .Include(l => l.Autores) 
                        .Include(l => l.Assuntos)
                        .ToListAsync();
                
        }

        public async Task<bool> DeletarAsync(Livro livro)
        {
            if (livro.LivroAssuntos != null)
            {
                 _bancoContext.LivroAssuntos.RemoveRange(livro.LivroAssuntos);
            }
            _bancoContext.Livros.Remove(livro);
            await _bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarListaAssuntosAsync(IList<int> assuntoIds)
        {
            var livroAssuntos = await _bancoContext.LivroAssuntos
                                        .Where(la => assuntoIds.Contains(la.AssuntoCodAs))
                                        .ToListAsync();

            _bancoContext.LivroAssuntos.RemoveRange(livroAssuntos);
            await _bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletarListaAutoresAsync(IList<int> autorIds)
        {
            var livroAutores = await _bancoContext.LivroAutores
                                        .Where(la => autorIds.Contains(la.AutorCodAu))
                                        .ToListAsync();

            // Remover os itens encontrados
            _bancoContext.LivroAutores.RemoveRange(livroAutores);
            await _bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Autor>> BuscarTodosAutoresAsync()
        {
            return await _bancoContext.Autores.ToListAsync();
        }

        public async Task<IEnumerable<Assunto>> BuscarTodosAssuntosAsync()
        {
            return await _bancoContext.Assuntos.ToListAsync();
        }

        public Task<IEnumerable<Livro>> ListarDetalhesAsync(int cod)
        {
            throw new NotImplementedException();
        }
    }
}
