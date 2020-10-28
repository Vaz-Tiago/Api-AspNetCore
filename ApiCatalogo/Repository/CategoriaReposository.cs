using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public class CategoriaReposository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaReposository(AppDbContext context) : base(context){ }

        public PagedList<Categoria> GetCategorias(PaginationParameters parameters)
        {
            return PagedList<Categoria>
                .ToPagedList(Get()
                .OrderBy(on => on.CategoriaId), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProduto()
        {
            return await Get().Include(p => p.Produtos).ToListAsync();
        }
    }
}
