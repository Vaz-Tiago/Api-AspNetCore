//using ApiCatalogo.Context;
//using ApiCatalogo.Filters;
//using ApiCatalogo.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ApiCatalogo.Controllers
//{
//    [Route("api/[Controller]")]
//    [ApiController]
//    public class ProdutosController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        public ProdutosController(AppDbContext context)
//        {
//            _context = context;
//        }

//        //// Adicionar essa barra no método ignora o mapeamento pré-definido pelo controller
//        //[HttpGet("{valor:alpha:length(5)}")]
//        //public ActionResult<Produto> GetFirst()
//        //{
//        //    return _context.Produtos.FirstOrDefault();
//        //}

//        // Tornando assíncrono
//        [HttpGet]
//        [ServiceFilter(typeof(ApiLoggingFilter))]
//        public async Task<ActionResult<IEnumerable<Produto>>> Get()
//        {
//            // As NoTracking, melhora a performance das buscas, pois como não vai ocorrer nenhuma alteração no resultado
//            // Não é necessário manter um instancia do objeto em memória.
//            return await _context.Produtos.AsNoTracking().ToListAsync();
//        }

//        // Define um name, pois pode ser utilizado como retorno de outros métodos, apenas informando o name
//        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
//        public async Task<ActionResult<Produto>> Get([FromQuery]int id)
//        {
//            var produto = await _context.Produtos.AsNoTracking()
//                .FirstOrDefaultAsync(p => p.ProdutoId == id);

//            if (produto == null)
//                return NotFound();

//            return produto;
//        }

//        [HttpPost]
//        public ActionResult Post([FromBody]Produto produto)
//        {
//            // Validação
//            // Como esse código é muito utilizado, a partir da versão 2.1, ela é feita automaticamente.
//            // Ma é necessário utilizar o [ApiController]
//            //if (!ModelState.IsValid)
//            //    return BadRequest(ModelState);

//            _context.Produtos.Add(produto);
//            _context.SaveChanges();

//            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
//        }

//        [HttpPut("{id}")]
//        public ActionResult Put(int id, [FromBody]Produto produto)
//        {
//            if (id != produto.ProdutoId)
//                return BadRequest();

//            _context.Entry(produto).State = EntityState.Modified;
//            _context.SaveChanges();
//            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
//        }

//        [HttpDelete("{id}")]
//        public ActionResult<Produto> Delete(int id)
//        {
//            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

//            if (produto == null)
//                return NotFound();

//            _context.Produtos.Remove(produto);
//            _context.SaveChanges();

//            return produto;
//        }
//    }
//}
