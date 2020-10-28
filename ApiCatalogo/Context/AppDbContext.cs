using ApiCatalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Context
{
    // Necessário herdar de DbContext -> EF CORE
    // Necessário herdar de IdentityDbContext -> EF CORE -> Para gerar estrutura de login
    public class AppDbContext : IdentityDbContext
    {
        // Define um contexto para posteriormente registra-lo como um serviço.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {   }
        
        // Define quais models representa cada tabela
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
