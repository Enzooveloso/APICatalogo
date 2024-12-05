using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }

    [Required]//Iniciar mapear a propriedade Name para uma coluna Not Null
    [StringLength(80)]//Define tamanho em Bytes
    public string? Name { get; set; }

    [Required]//Iniciar mapear a propriedade ImageUrl para uma coluna Not Null
    [StringLength(300)]//Define tamanho em Bytes
    public string? ImageUrl { get; set; }
}
