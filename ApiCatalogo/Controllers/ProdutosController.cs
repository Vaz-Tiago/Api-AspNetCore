using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await _unitOfWork.ProdutoRepository.Get().ToListAsync();
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
