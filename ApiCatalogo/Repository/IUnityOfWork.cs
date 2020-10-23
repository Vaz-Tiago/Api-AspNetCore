using System.Threading.Tasks;

namespace ApiCatalogo.Repository
{
    public interface IUnityOfWork
    {
        // Uma instancia de cada repositório criado, caso um novo seja criado, é necessário adicionar aqui!
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }

        Task Commit();
    }
}
