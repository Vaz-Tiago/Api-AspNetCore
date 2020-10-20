using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiCatalogo.Migrations
{
    public partial class PopulateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Categorias
            migrationBuilder.Sql(@"
                INSERT INTO Categorias 
                    (Nome, ImagemUrl) 
                VALUES 
                    ('Bebidas', 'http://www.macoratti.net/Imagens/1.jpg')
                "
            );
            migrationBuilder.Sql(@"
                INSERT INTO Categorias 
                    (Nome, ImagemUrl) 
                VALUES 
                    ('Lanches', 'http://www.macoratti.net/Imagens/2.jpg')
                "
            );

            migrationBuilder.Sql(@"
                INSERT INTO Categorias 
                    (Nome, ImagemUrl) 
                VALUES 
                    ('Sobremesas', 'http://www.macoratti.net/Imagens/3.jpg')
                "
            );
            #endregion

            #region Produtos
            // Para pegar o id da categoria é necessário fazer o select
            migrationBuilder.Sql(@"
                INSERT INTO Produtos 
                    (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)
                VALUES
                    ('Coca-Cola Diet', 'Refrigerante de Cola', 5.45, 'http://www.macoratti.net/Imagens/coca.jpg', 50, now(), (SELECT CategoriaID FROM Categorias WHERE Nome='Bebidas'))
                "
            );

            migrationBuilder.Sql(@"
                INSERT INTO Produtos 
                    (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)
                VALUES
                    ('Lanche de Atum', 'Lanche de atum com maionese', 7.90, 'http://www.macoratti.net/Imagens/atum.jpg', 30, now(), (SELECT CategoriaID FROM Categorias WHERE Nome='Lanches'))
                "
            );

            migrationBuilder.Sql(@"
                INSERT INTO Produtos 
                    (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId)
                VALUES
                    ('Pudim', 'Pudim de leite condensado 100g', 6.25, 'http://www.macoratti.net/Imagens/pudim.jpg', 25, now(), (SELECT CategoriaID FROM Categorias WHERE Nome='Sobremesas'))
                "
            );

            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categorias");
            migrationBuilder.Sql("DELETE FROM Produtos");
        }
    }
}
