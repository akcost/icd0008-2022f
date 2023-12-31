using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Db;

public class GameRepositoryDb : BaseRepository, IGameRepository
{
    public GameRepositoryDb(AppDbContext dbContext) : base(dbContext)
    {
    }
    
    public List<CheckersGame> GetAll()
    {
        return Ctx.CheckersGames
            .Include(o => o.CheckersOption)
            .OrderBy(o => o.StartedAt)
            .ToList();
    }

    public CheckersGame? GetGame(int? id)
    {
        return Ctx.CheckersGames
            .Include(g => g.CheckersOption)
            .Include(g => g.CheckersGameStates)
            .FirstOrDefault(g => g.Id == id);
    }

    public CheckersGame AddGame(CheckersGame game)
    {
        Ctx.CheckersGames.Add(game);
        Ctx.SaveChanges();

        return game;
    }

}