using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
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
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            var categoria = _unitOfWork.CategoriaRepository.GetCategoriasProduto().ToList();
            var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);

            return categoriaDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categoria =  _unitOfWork.CategoriaRepository.Get().ToList();
            var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);

            return categoriaDto;

        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository
                .GetById(c => c.CategoriaId == id);
                
            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoriaDTO)
        {
            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            _unitOfWork.CategoriaRepository.Add(categoria);
            _unitOfWork.Commit();

            var categoriaDtoResponse = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDtoResponse);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
                return BadRequest();

            var categoria = _mapper.Map<Categoria>(categoriaDTO);
            _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();

            var categoriaDtoResponse = _mapper.Map<CategoriaDTO>(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDtoResponse);
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria == null)
                return NotFound();

            _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return categoriaDto;
        }
    }
}
