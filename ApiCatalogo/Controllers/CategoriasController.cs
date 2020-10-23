using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
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

        public CategoriasController(IUnityOfWork context, IConfiguration config)
        {
            _unitOfWork = context;
            _configuration = config;
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
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _unitOfWork.CategoriaRepository.GetCategoriasProduto().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _unitOfWork.CategoriaRepository.Get()
                .ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository
                .GetById(c => c.CategoriaId == id);
                
            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _unitOfWork.CategoriaRepository.Add(categoria);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria == null)
                return NotFound();

            _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();

            return categoria;
        }
    }
}
