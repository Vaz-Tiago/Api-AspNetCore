using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public class CategoriaReposository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaReposository(AppDbContext context) : base(context){ }

        public async Task<IEnumerable<Categoria>> GetCategoriasProduto()
        {
            return await Get().Include(p => p.Produtos).ToListAsync();
        }
    }
}
