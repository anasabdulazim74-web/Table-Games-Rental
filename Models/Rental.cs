using System.ComponentModel.DataAnnotations;

namespace GameRentalSystem.Models;

public class Rental
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }

    [Required]
    public int GameId { get; set; }
    public Game? Game { get; set; }

    [Display(Name = "Rent Date")]
    public DateTime RentDate { get; set; } = DateTime.Now;

    [Display(Name = "Return Date")]
    public DateTime? ReturnDate { get; set; }

    public Review? Review { get; set; }
}
