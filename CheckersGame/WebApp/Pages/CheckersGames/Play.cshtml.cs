using System.Diagnostics;
using System.Drawing.Printing;
using System.Text.Json;
using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Pages.CheckersGames;

public class Play : PageModel
{
    private readonly IGameRepository _repo;

    public Play(IGameRepository repo)
    {
        _repo = repo;
    }

    public int PlayerNo { get; set; }
    public CheckersBrain Brain { get; set; } = default!;
    
    public CheckersGame CheckersGame { get; set; } = default!;
    
    public int GameId { get; set; }


    public async Task<IActionResult> OnGet(int? id, int? playerNo, int? x, int? y, int? newX, int? newY)
    {
        var game = _repo.GetGame(id);

        if (game == null || game.CheckersOption == null)
        {
            return NotFound();
        }
        
        if (playerNo == null || playerNo.Value < 0 || playerNo.Value > 2)
        {
            return RedirectToPage("/Index", new {error = "No player number, or wrong player number."});
        }

        GameId = id!.Value;
        // playerNo 0 - first player. first player is always white.
        // playerNo 1 - second player. second player is always black.
        // playerNo 2 - play as both players.
        PlayerNo = playerNo.Value;

        CheckersGame = game;
        CheckersGameState checkersGameState = game.CheckersGameStates!.LastOrDefault()!;

        if (checkersGameState == null)
        {
            Brain = new CheckersBrain(game.CheckersOption, null);
        }
        else
        {
            CheckersState cState = JsonSerializer.Deserialize<CheckersState>(checkersGameState.SerializedGameState)!;
            // var gameBoard =
            //     JsonSerializer.Deserialize<EGamePiece?[][]>(checkersGameState.SerializedGameState)!;
            var checkersUnSerializedState = new CheckersState
            {
                GameBoard = cState.GameBoard,
                NextMoveByBlack = cState.NextMoveByBlack
            };
            Brain = new CheckersBrain(game.CheckersOption, checkersUnSerializedState);
        }

        if (game.GameWonByPlayer != null)
        {
            Console.WriteLine();
        }
        
        if (newX != null && newY != null)
        {
            if (x != null && y != null)
            {
                if ((game.Player1Type == EPlayerType.Ai && !Brain.NextMoveByBlack()) ||
                    (game.Player2Type == EPlayerType.Ai && Brain.NextMoveByBlack()))
                {
                    Console.WriteLine("AI making a move");
                    Brain.MakeAMoveByAi();
                }
                else
                {
                    Brain.MakeAMove((int)x, (int)y, (int)newX, (int)newY);
                }


                if ((game.Player1Type == EPlayerType.Ai && !Brain.NextMoveByBlack()) ||
                    (game.Player2Type == EPlayerType.Ai && Brain.NextMoveByBlack()))
                {
                    Console.WriteLine("AI making a move");
                    Brain.MakeAMoveByAi();
                }


                game.CheckersGameStates!.Add(new CheckersGameState()
                {
                    SerializedGameState = Brain.GetSerializedGameState(),
                });

                _repo.SaveChanges();
            }
        }
        else
        {
            if ((game.Player1Type == EPlayerType.Ai && !Brain.NextMoveByBlack()) ||
                (game.Player2Type == EPlayerType.Ai && Brain.NextMoveByBlack()))
            {
                Console.WriteLine("AI making a move again");
                Brain.MakeAMoveByAi();
            }
        }

        return Page();
    }

    public void EndGame()
    {
        _repo.SaveChanges();
    }
}