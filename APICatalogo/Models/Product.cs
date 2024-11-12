using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models;
[Table("Product")]
public class Product
{
    [Key]
    public int ProductID { get; set; }

    [Required]//Iniciar mapear a propriedade Name para uma coluna Not Null
    [StringLength(80)]//Define tamanho em Bytes
    public string? Name { get; set; }

    [Required]//Inciar mapear a propriedade Description para uma coluna Not Null
    [StringLength(300)]//Define tamanho em Bytes
    public string? Description { get; set; }

    [Required]//Iniciar mapear a propriedade Price para uma coluna Not Null
    [Column(TypeName = "decimal(10,2)")]//Define tamanho e quantidade de digitos antes e depois da virgula
    public decimal Price { get; set; }

    [Required]//Iniciar mapear a propriedade ImageUrl para uma coluna Not Null
    [StringLength(300)]//Define tamanho em Bytes
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime DateRegistration { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

   
}
