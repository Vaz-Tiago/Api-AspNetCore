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
        private readonly IUnityOfWork _unitOfWork;
        public ProdutosController(IUnityOfWork context)
        {
            _unitOfWork = context;
        }

        [HttpGet("menor-preco")]
        public ActionResult<IEnumerable<Produto>> GetPRodutosPrecos()
        {
            return _unitOfWork.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _unitOfWork.ProdutoRepository.Get().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            _unitOfWork.ProdutoRepository.Add(produto);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();

            return produto;
        }
    }
}
