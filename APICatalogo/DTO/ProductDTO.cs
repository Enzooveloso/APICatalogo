using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.DTO
{
    public class ProductDTO
    {

        
        public int ProductID { get; set; }

        [Required]//Iniciar mapear a propriedade Name para uma coluna Not Null
        [StringLength(80)]//Define tamanho em Bytes
        public string Name { get; set; }

        [Required]//Inciar mapear a propriedade Description para uma coluna Not Null
        [StringLength(300)]//Define tamanho em Bytes
        public string? Description { get; set; }

        [Required]//Iniciar mapear a propriedade Price para uma coluna Not Null
        public decimal Price { get; set; }

        [Required]//Iniciar mapear a propriedade ImageUrl para uma coluna Not Null
        [StringLength(300)]//Define tamanho em Bytes
        public string? ImageUrl { get; set; }
       
        public int CategoryId { get; set; }

    
    }
}
