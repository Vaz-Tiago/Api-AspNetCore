using ApiCatalogo.Context;
using ApiCatalogo.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        // Cria um construtor para passar o contexto para a classe base
        public ProdutoRepository(AppDbContext context) : base(context) { }

        // Como a classe base já faz a implementação básica do CRUD, só é necessário implementar o método específico
        // Da interface IProdutoRepository
        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(p => p.Preco).ToList();
        }
    }
}
