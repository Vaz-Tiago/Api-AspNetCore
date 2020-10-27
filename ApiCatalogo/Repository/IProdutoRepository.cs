using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        // Caso a entidade não possua nenhum método específico, apenas a classe vazia é suficiente para funcionar o CRUD

        // Caso tenha algo específico, deve ser adicionado para a classe concreta implementar
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
        Task<IEnumerable<Produto>> GetProdutos(ProdutosParameters produtosParameters);
    }
}
