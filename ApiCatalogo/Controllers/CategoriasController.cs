using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    // Comentado o login para facilitar teste da documentação

    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CategoriasController(IUnityOfWork context, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = context;
            _configuration = config;
            _mapper = mapper;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["autor"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];
            return $"Autor: {autor}. Conexão: {conexao}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetCategoriasProduto();
            var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);

            return categoriaDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery]PaginationParameters parameters)
        {
            var categoria = _unitOfWork.CategoriaRepository.GetCategorias(parameters);

            var metadata = new
            {
                categoria.TotalCount,
                categoria.PageSize,
                categoria.CurrentPage,
                categoria.TotalPages,
                categoria.HasNext,
                categoria.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);

            return categoriaDto;
        }

        /// <summary>
        /// Obtem uma categoria pelo seu Id
        /// </summary>
        /// <param name="id">Código da categoria</param>
        /// <returns>Objetos Categoria</returns>
        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository
                .GetById(c => c.CategoriaId == id);
                
            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/categorias
        ///     {
        ///         "categoriaId": 1,
        ///         "nome": "categoria 1",
        ///         "imagemUrl": "http://teste.com/i.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDTO">Objeto Categoria</param>
        /// <returns>Retorna a categoria incluida</returns>
        /// <remarks>Retorna um objeto categoria incluído</remarks>
        [HttpPost]
        public async  Task<ActionResult> Post([FromBody] Categoria categoriaDTO)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            _unitOfWork.CategoriaRepository.Add(categoria);
            await _unitOfWork.Commit();

            var categoriaDtoResponse = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDtoResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Categoria categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.Commit();

            var categoriaDtoResponse = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDtoResponse);
        }

        [HttpDelete("{id}")]
        public async  Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria == null)
                return NotFound();

            _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }
    }
}
