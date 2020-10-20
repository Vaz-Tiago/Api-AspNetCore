using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string Descricao { get; set; }

        [Required]
        public double Preco { get; set; }

        [Required]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }

        public float Estoque { get; set; }
        
        public DateTime DataCadastro { get; set; }

        // Definindo a FK
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }
    }
}
