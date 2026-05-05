using System.ComponentModel.DataAnnotations;

namespace GameRentalSystem.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(60)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "User";

    public List<Rental> Rentals { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
