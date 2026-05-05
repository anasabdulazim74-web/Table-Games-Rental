using GameRentalSystem.Data;
using GameRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameRentalSystem.Controllers;

public class RentalsController : Controller
{
    private readonly AppDbContext _context;

    public RentalsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RentGame(int gameId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Users");
        }

        var gameExists = await _context.Games.AnyAsync(g => g.Id == gameId);
        if (!gameExists)
        {
            return NotFound();
        }

        var rental = new Rental
        {
            GameId = gameId,
            UserId = userId.Value,
            RentDate = DateTime.Now
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(MyHistory));
    }

    public async Task<IActionResult> MyHistory()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Users");
        }

        var rentals = await _context.Rentals
            .Include(r => r.Game)
            .Include(r => r.Review)
            .Where(r => r.UserId == userId.Value)
            .OrderByDescending(r => r.RentDate)
            .ToListAsync();

        return View(rentals);
    }

    public async Task<IActionResult> RentalHistory()
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Users");
        }

        var rentals = await _context.Rentals
            .Include(r => r.User)
            .Include(r => r.Game)
            .Include(r => r.Review)
            .OrderByDescending(r => r.RentDate)
            .ToListAsync();

        return View(rentals);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnGame(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Users");
        }

        var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId.Value);
        if (rental == null)
        {
            return NotFound();
        }

        rental.ReturnDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(MyHistory));
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("UserRole") == "Admin";
    }
}
