using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatalogo.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        // Responsabilidade da Classe iniciar a listagem de produtos
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }

        // Relacionamento, Uma categoria pode conter vários produtos
        // Basta adicionar uma coleção de produtos
        public ICollection<Produto> Produtos { get; set; }
    }
}
