﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;
[Table("Product")]
public class Product : IValidatableObject
{
    [Key]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "o nome e obrigatorio")]//Iniciar mapear a propriedade Name para uma coluna Not Null
    [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 caracteres", MinimumLength = 5)]//Define tamanho em Bytes

    public string Name { get; set; }

    [Required]//Inciar mapear a propriedade Description para uma coluna Not Null
    [StringLength(300, ErrorMessage = "A descriçao deve ter no máximo {1} caracteres")]//Define tamanho em Bytes
    public string? Description { get; set; }

    [Required]//Iniciar mapear a propriedade Price para uma coluna Not Null
    [Column(TypeName = "decimal(10,2)")]//Define tamanho e quantidade de digitos antes e depois da virgula
    [Range(1, 10000, ErrorMessage = "O preço dewve ser entre {1} e {2}")]
    public decimal Price { get; set; }

    [Required]//Iniciar mapear a propriedade ImageUrl para uma coluna Not Null
    [StringLength(300)]//Define tamanho em Bytes
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime DateRegistration { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    //Validação a nível de modelo
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

        if (!string.IsNullOrEmpty(this.Name))
        {
            var firstLetter = this.Name.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                yield return new
                ValidationResult("Priemira letra deve ser maiuscula", new[]
                {
                    nameof(this.Name)
                });
            }
        }
    }
}
