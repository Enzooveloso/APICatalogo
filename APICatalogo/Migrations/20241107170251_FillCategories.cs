using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class FillCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorys(Name, ImageUrl) Values ('Bebidas','bebidas.jpg')");
            mb.Sql("Insert into Categorys(Name, ImageUrl) Values ('Lanches','lanches.jpg')");
            mb.Sql("Insert into Categorys(Name, ImageUrl) Values ('Sobremesas','sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("delete from Categorys");
        }
    }
}
