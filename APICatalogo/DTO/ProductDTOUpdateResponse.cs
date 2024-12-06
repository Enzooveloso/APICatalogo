using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO;

public class ProductDTOUpdateResponse
{
    public int ProductID { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime DateRegistration { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
