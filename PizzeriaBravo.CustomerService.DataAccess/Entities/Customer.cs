using System.ComponentModel.DataAnnotations;
using PizzeriaBravo.CustomerService.DataAccess.Interfaces;

namespace PizzeriaBravo.CustomerService.DataAccess.Entities;

public class Customer : IEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = null!;
}