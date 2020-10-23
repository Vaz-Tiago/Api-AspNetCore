using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public class CategoriaReposository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaReposository(AppDbContext context) : base(context){ }

        public IEnumerable<Categoria> GetCategoriasProduto()
        {
            return Get().Include(p => p.Produtos);
        }
    }
}
