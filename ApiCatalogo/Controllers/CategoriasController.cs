using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public CategoriasController(AppDbContext context, IConfiguration config, ILogger<CategoriasController> logger)
        {
            _context = context;
            _configuration = config;
            _logger = logger;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["autor"];
            // Para navegar dentro de um objeto basta adicionar : que adentra um nível
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];
            return $"Autor: {autor}. Conexão: {conexao}";
        }

        // Injetando um serviço direto na rota
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("============================= GET api/categorias/produtos =============================");
            return _context.Categorias.AsNoTracking()
                .Include(c => c.Produtos)
                .ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("============================= GET api/categorias =============================");
            return _context.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            _logger.LogInformation($"============================= GET api/categorias/{id} =============================");
            var categoria = _context.Categorias.AsNoTracking()
                .Include(p=> p.Produtos)
                .FirstOrDefault(c => c.CategoriaId == id);
                
            if (categoria == null)
            {
                _logger.LogInformation($"============================= GET api/categorias/{id} - NOT FOUND =============================");
                return NotFound();
            }

            return categoria;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return categoria;
        }
    }
}
