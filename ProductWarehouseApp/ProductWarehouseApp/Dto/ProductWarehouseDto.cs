using System.ComponentModel.DataAnnotations;

namespace ProductWarehouseApp.Dto;

public class ProductWarehouseDto
{
    [Required]
    public int IdProduct { get; set; }
    
    [Required]
    public int IdWarehouse { get; set; }
    
    [Required]
    [Range(1,Int32.MaxValue, ErrorMessage = "Amount must be bigger than 0")]
    public int Amount { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
}