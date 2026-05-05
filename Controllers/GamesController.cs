using GameRentalSystem.Data;
using GameRentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameRentalSystem.Controllers;

public class GamesController : Controller
{
    private readonly AppDbContext _context;

    public GamesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? genre, int? players, int? playTime)
    {
        var gamesQuery = _context.Games
            .Include(g => g.Reviews)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(genre))
        {
            gamesQuery = gamesQuery.Where(g => g.Genre == genre);
        }

        if (players.HasValue && players > 0)
        {
            gamesQuery = gamesQuery.Where(g => g.NumberOfPlayers == players.Value);
        }

        if (playTime.HasValue && playTime > 0)
        {
            gamesQuery = gamesQuery.Where(g => g.PlayTime <= playTime.Value);
        }

        ViewBag.Genres = await _context.Games.Select(g => g.Genre).Distinct().ToListAsync();
        return View(await gamesQuery.ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var game = await _context.Games
            .Include(g => g.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Users");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Game game)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Users");
        }

        if (!ModelState.IsValid)
        {
            return View(game);
        }

        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Users");
        }

        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Game game)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Users");
        }

        if (id != game.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(game);
        }

        _context.Update(game);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Users");
        }

        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Users");
        }

        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return RedirectToAction(nameof(Index));
        }

        // Delete children first so MySQL/InnoDB does not hit FK / cascade issues (simple student approach).
        var reviews = await _context.Reviews.Where(r => r.GameId == id).ToListAsync();
        _context.Reviews.RemoveRange(reviews);

        var rentals = await _context.Rentals.Where(r => r.GameId == id).ToListAsync();
        _context.Rentals.RemoveRange(rentals);

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("UserRole") == "Admin";
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
        return RedirectToAction("MyHistory", "Rentals");
    }
}
