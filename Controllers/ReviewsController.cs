using GameRentalSystem.Data;
using GameRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameRentalSystem.Controllers;

public class ReviewsController : Controller
{
    private readonly AppDbContext _context;

    public ReviewsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Create(int rentalId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Users");
        }

        var rental = await _context.Rentals
            .Include(r => r.Game)
            .Include(r => r.Review)
            .FirstOrDefaultAsync(r => r.Id == rentalId && r.UserId == userId.Value);

        if (rental == null)
        {
            return NotFound();
        }

        if (rental.Review != null)
        {
            return RedirectToAction("MyHistory", "Rentals");
        }

        if (rental.ReturnDate == null)
        {
            TempData["Message"] = "Return the game first, then you can leave a review.";
            return RedirectToAction("MyHistory", "Rentals");
        }

        ViewBag.RentalId = rental.Id;
        ViewBag.GameName = rental.Game?.Name;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int gameId, int rating, string? comment)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Users");
        }

        // Find a rental for this user and game that has been returned and doesn't have a review yet
        var rental = await _context.Rentals
            .Include(r => r.Review)
            .FirstOrDefaultAsync(r => r.GameId == gameId && r.UserId == userId.Value && r.ReturnDate != null && r.Review == null);

        if (rental == null)
        {
            TempData["Message"] = "You must rent and return this game before leaving a review, or you already reviewed it.";
            return RedirectToAction("Details", "Games", new { id = gameId });
        }

        if (rating < 1 || rating > 5)
        {
            TempData["Message"] = "Rating must be between 1 and 5.";
            return RedirectToAction("Details", "Games", new { id = gameId });
        }

        var review = new Review
        {
            RentalId = rental.Id,
            UserId = userId.Value,
            GameId = gameId,
            Rating = rating,
            Comment = comment
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", "Games", new { id = gameId });
    }
}
