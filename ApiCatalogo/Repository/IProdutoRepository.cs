using ApiCatalogo.Models;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        // Caso a entidade não possua nenhum método específico, apenas a classe vazia é suficiente para funcionar o CRUD

        // Caso tenha algo específico, deve ser adicionado para a classe concreta implementar
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
