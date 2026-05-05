using System.ComponentModel.DataAnnotations;

namespace GameRentalSystem.Models;

public class Review
{
    public int Id { get; set; }

    [Required]
    public int RentalId { get; set; }
    public Rental? Rental { get; set; }

    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }

    [Required]
    public int GameId { get; set; }
    public Game? Game { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(500)]
    public string? Comment { get; set; }
}
