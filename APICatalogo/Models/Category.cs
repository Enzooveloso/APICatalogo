using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;
[Table("Categorys")]//Define a entidade Category
public class Category
{
    public Category()
    {
        Products = new Collection<Product>();
    }
    
    [Key]//Define aa propriedade CategoryId como Primarykey no MySQL
    public int CategoryId { get; set; }

    [Required]//Iniciar mapear a propriedade Name para uma coluna Not Null
    [StringLength(80)]//Define tamanho em Bytes
    public string? Name { get; set; }

    [Required]//Iniciar mapear a propriedade ImageUrl para uma coluna Not Null
    [StringLength(300)]//Define tamanho em Bytes
    public string? ImageUrl { get; set; }
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
    

}

