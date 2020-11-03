using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [EnableCors("PertmitirApiRequest")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProdutosController(IUnityOfWork context, IMapper mapper)
        {
            _unitOfWork = context;
            _mapper = mapper;
        }

        [HttpGet("menor-preco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetPRodutosPrecos()
        {
            var produtos = await _unitOfWork.ProdutoRepository.GetProdutosPorPreco();

            // É Só chamar o método map, que o mapeamento para DTO será feito.
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] PaginationParameters produtosParameters)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

            _unitOfWork.ProdutoRepository.Add(produto);
            await _unitOfWork.Commit();

            var produtoDtoResponse = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDtoResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);

            _unitOfWork.ProdutoRepository.Update(produto);
            await _unitOfWork.Commit();

            var produtoDtoResponse = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDtoResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound();

            _unitOfWork.ProdutoRepository.Delete(produto);
            await _unitOfWork.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }
    }
}
