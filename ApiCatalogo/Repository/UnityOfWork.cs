using ApiCatalogo.Context;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private ProdutoRepository _produtoRepository;
        private CategoriaReposository _categoryRepository;
        public AppDbContext _context;

        public UnityOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository { 
            get 
            {
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
            } 
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoryRepository ??= new CategoriaReposository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
