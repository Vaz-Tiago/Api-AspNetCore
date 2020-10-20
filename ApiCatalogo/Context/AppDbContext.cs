using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Context
{
    // Necessário herdar de DbContext -> EF CORE
    public class AppDbContext : DbContext
    {
        // Define um contexto para posteriormente registra-lo como um serviço.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {   }
        
        // Define quais models representa cada tabela
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
