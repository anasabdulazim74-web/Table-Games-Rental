using System.ComponentModel.DataAnnotations;

namespace GameRentalSystem.Models;

public class Game
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Display(Name = "Number of Players")]
    [Range(1, 20)]
    public int NumberOfPlayers { get; set; }

    [Display(Name = "Play Time (Minutes)")]
    [Range(10, 500)]
    public int PlayTime { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    public List<Rental> Rentals { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
