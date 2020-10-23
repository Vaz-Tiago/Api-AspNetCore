using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        // Cria um construtor para passar o contexto para a classe base
        public ProdutoRepository(AppDbContext context) : base(context) { }

        // Como a classe base já faz a implementação básica do CRUD, só é necessário implementar o método específico
        // Da interface IProdutoRepository
        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(p => p.Preco).ToListAsync();
        }
    }
}
