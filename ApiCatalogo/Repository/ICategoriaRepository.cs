using ApiCatalogo.Models;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProduto();
    }
}
