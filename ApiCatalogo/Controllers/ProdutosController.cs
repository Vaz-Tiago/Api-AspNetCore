using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _UnitOfWork;
        public ProdutosController(IUnityOfWork context)
        {
            _UnitOfWork = context;
        }

        [HttpGet("menor-preco")]
        public ActionResult<IEnumerable<Produto>> GetPRodutosPrecos()
        {
            return _UnitOfWork.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _UnitOfWork.ProdutoRepository.Get().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            _UnitOfWork.ProdutoRepository.Add(produto);
            _UnitOfWork.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _UnitOfWork.ProdutoRepository.Update(produto);
            _UnitOfWork.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _UnitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            _UnitOfWork.ProdutoRepository.Delete(produto);
            _UnitOfWork.Commit();

            return produto;
        }
    }
}
