using GameRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GameRentalSystem.Data;

/// <summary>
/// Simple sample data for testing the site (student project style).
/// </summary>
public static class DbSeed
{
    public static void ApplySampleDataIfNeeded(AppDbContext db)
    {
        // Default admin account (same as before — one place for a student demo).
        if (!db.Users.Any(u => u.Role == "Admin"))
        {
            db.Users.Add(new User
            {
                FullName = "Admin User",
                Email = "admin@boardgames.com",
                Password = "admin123",
                Role = "Admin"
            });
            db.SaveChanges();
        }

        // Only seed games/rentals/reviews once — if there are already games, stop here.
        if (db.Games.Any())
            return;

        // Two normal users for demos (passwords are plain on purpose for a class project).
        if (!db.Users.Any(u => u.Email == "alice@student.com"))
        {
            db.Users.Add(new User
            {
                FullName = "Alice Student",
                Email = "alice@student.com",
                Password = "12345",
                Role = "User"
            });
        }

        if (!db.Users.Any(u => u.Email == "bob@student.com"))
        {
            db.Users.Add(new User
            {
                FullName = "Bob Student",
                Email = "bob@student.com",
                Password = "12345",
                Role = "User"
            });
        }

        db.SaveChanges();

        var games = new List<Game>
        {
            new()
            {
                Name = "Chess",
                Genre = "Strategy",
                NumberOfPlayers = 2,
                PlayTime = 60,
                Description = "Classic two-player strategy. Capture the king to win.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6f/ChessSet.jpg/640px-ChessSet.jpg"
            },
            new()
            {
                Name = "Monopoly",
                Genre = "Family",
                NumberOfPlayers = 8,
                PlayTime = 120,
                Description = "Buy properties, collect rent, and try to bankrupt your friends.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/78/Monopoly_board_game_%282007%29.jpg/640px-Monopoly_board_game_%282007%29.jpg"
            },
            new()
            {
                Name = "Catan",
                Genre = "Strategy",
                NumberOfPlayers = 4,
                PlayTime = 90,
                Description = "Collect resources, build roads and settlements, and reach 10 victory points.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/a/a3/Catan-2015-boxart.jpg/440px-Catan-2015-boxart.jpg"
            },
            new()
            {
                Name = "UNO",
                Genre = "Card",
                NumberOfPlayers = 10,
                PlayTime = 30,
                Description = "Match colors and numbers. First to empty their hand wins the round.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Uno_cards_deck.jpg/640px-Uno_cards_deck.jpg"
            },
            new()
            {
                Name = "Scrabble",
                Genre = "Word",
                NumberOfPlayers = 4,
                PlayTime = 90,
                Description = "Build words on the board using letter tiles for the highest score.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5c/Scrabble_game_in_progress.jpg/640px-Scrabble_game_in_progress.jpg"
            },
            new()
            {
                Name = "Risk",
                Genre = "Strategy",
                NumberOfPlayers = 6,
                PlayTime = 120,
                Description = "Conquer territories with dice rolls and armies until you control the world.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/80/Risk_board_game_%282007%29.jpg/640px-Risk_board_game_%282007%29.jpg"
            },
            new()
            {
                Name = "Clue",
                Genre = "Mystery",
                NumberOfPlayers = 6,
                PlayTime = 45,
                Description = "Move through the mansion and solve who did it, with what weapon, and where.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/6/61/Cluedo_Cards.jpg/440px-Cluedo_Cards.jpg"
            },
            new()
            {
                Name = "Ticket to Ride",
                Genre = "Family",
                NumberOfPlayers = 5,
                PlayTime = 60,
                Description = "Claim train routes across a map and complete destination tickets.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/9/92/Ticket_to_Ride_Board_Game_Box_EN.jpg/440px-Ticket_to_Ride_Board_Game_Box_EN.jpg"
            },
            new()
            {
                Name = "Jenga",
                Genre = "Skill",
                NumberOfPlayers = 8,
                PlayTime = 20,
                Description = "Pull wooden blocks from the tower without making it fall.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2e/Jenga_game.jpg/640px-Jenga_game.jpg"
            },
            new()
            {
                Name = "Pandemic",
                Genre = "Cooperative",
                NumberOfPlayers = 4,
                PlayTime = 45,
                Description = "Work together as a team to stop diseases before they spread worldwide.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/3/3e/Pandemic_game.jpg/440px-Pandemic_game.jpg"
            }
        };

        db.Games.AddRange(games);
        db.SaveChanges();

        var alice = db.Users.First(u => u.Email == "alice@student.com");
        var bob = db.Users.First(u => u.Email == "bob@student.com");
        var admin = db.Users.First(u => u.Role == "Admin");

        var chess = db.Games.First(g => g.Name == "Chess");
        var monopoly = db.Games.First(g => g.Name == "Monopoly");
        var catan = db.Games.First(g => g.Name == "Catan");
        var uno = db.Games.First(g => g.Name == "UNO");
        var scrabble = db.Games.First(g => g.Name == "Scrabble");
        var risk = db.Games.First(g => g.Name == "Risk");

        var r1 = new Rental
        {
            UserId = alice.Id,
            GameId = chess.Id,
            RentDate = DateTime.Now.AddDays(-14),
            ReturnDate = DateTime.Now.AddDays(-12)
        };
        var r2 = new Rental
        {
            UserId = alice.Id,
            GameId = monopoly.Id,
            RentDate = DateTime.Now.AddDays(-8),
            ReturnDate = DateTime.Now.AddDays(-6)
        };
        var r3 = new Rental
        {
            UserId = bob.Id,
            GameId = catan.Id,
            RentDate = DateTime.Now.AddDays(-5),
            ReturnDate = DateTime.Now.AddDays(-3)
        };
        var r4 = new Rental
        {
            UserId = bob.Id,
            GameId = uno.Id,
            RentDate = DateTime.Now.AddDays(-2),
            ReturnDate = null
        };
        var r5 = new Rental
        {
            UserId = admin.Id,
            GameId = scrabble.Id,
            RentDate = DateTime.Now.AddDays(-20),
            ReturnDate = DateTime.Now.AddDays(-18)
        };
        var r6 = new Rental
        {
            UserId = bob.Id,
            GameId = risk.Id,
            RentDate = DateTime.Now.AddDays(-30),
            ReturnDate = DateTime.Now.AddDays(-28)
        };

        db.Rentals.AddRange(r1, r2, r3, r4, r5, r6);
        db.SaveChanges();

        db.Reviews.AddRange(
            new Review
            {
                RentalId = r1.Id,
                UserId = alice.Id,
                GameId = chess.Id,
                Rating = 5,
                Comment = "Great game, learned a lot from playing with a friend."
            },
            new Review
            {
                RentalId = r2.Id,
                UserId = alice.Id,
                GameId = monopoly.Id,
                Rating = 4,
                Comment = "Takes a long time but fun with family."
            },
            new Review
            {
                RentalId = r3.Id,
                UserId = bob.Id,
                GameId = catan.Id,
                Rating = 5,
                Comment = "My favorite board game. Trading makes it exciting."
            },
            new Review
            {
                RentalId = r5.Id,
                UserId = admin.Id,
                GameId = scrabble.Id,
                Rating = 3,
                Comment = "Good for word lovers. A bit slow for me."
            },
            new Review
            {
                RentalId = r6.Id,
                UserId = bob.Id,
                GameId = risk.Id,
                Rating = 4,
                Comment = "Epic battles. Setup takes a while."
            }
        );

        db.SaveChanges();
    }
}
