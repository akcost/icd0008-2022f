using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CheckersGames;

public class LaunchGame : PageModel
{
    
    private readonly IGameRepository _repo;

    public LaunchGame(IGameRepository repo)
    {
        _repo = repo;
    }

    public int GameId { get; set; }

    
    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return RedirectToPage("/Index", new {error = "No id!"});
        }
        
        var game = _repo.GetGame(id);
        
        if (game == null)
        {
            return RedirectToPage("/Index", new {error = "No game found!"});
        }

        // is it 2 player game
        if (game.Player1Type == EPlayerType.Human && game.Player2Type == EPlayerType.Human)
        {
            // create 2 links - tab1, tab2
            GameId = game.Id;
            
            return Page();
        }
        
        // it is a single player game
        return RedirectToPage("Play", new  {id = game.Id, playerNo = 0});

    }
}