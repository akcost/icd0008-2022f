using Domain;

namespace DAL.Db;

public class SavedCheckersGameRepositoryDb : ISavedCheckersGameRepository
{
    private readonly AppDbContext _dbContext;
    
    public SavedCheckersGameRepositoryDb(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Name { get; } = "DB";
    public List<CheckersGame> GetSavedCheckersGameList()
    {
        return _dbContext
            .CheckersGames
            .OrderBy(g => g.Id)
            // .Select(g => g.GameName)
            .ToList();
    }

    public CheckersGame GetCheckersGame(int id) => 
        _dbContext
            .CheckersGames
            .First(checkersGame => checkersGame.Id == id);

    public CheckersGame SaveCheckersGame(CheckersGame savedGame)
    {
        var gamesFromDb = _dbContext
            .CheckersGames
            .FirstOrDefault(o => o.Id == savedGame.Id);
        if (gamesFromDb == null)
        {
            _dbContext.CheckersGames.Add(savedGame);
            _dbContext.SaveChanges();
            return savedGame;
        }
        
        gamesFromDb.GameName = savedGame.GameName;
        gamesFromDb.Player1Name = savedGame.Player1Name;
        gamesFromDb.Player2Name = savedGame.Player2Name;
        gamesFromDb.Player1Type = savedGame.Player1Type;
        gamesFromDb.Player2Type = savedGame.Player2Type;
        gamesFromDb.GameOverAt = savedGame.GameOverAt;
        gamesFromDb.GameWonByPlayer = savedGame.GameWonByPlayer;
        gamesFromDb.CheckersOption = savedGame.CheckersOption;
        gamesFromDb.CheckersGameStates = savedGame.CheckersGameStates;
        
        _dbContext.SaveChanges();
        return savedGame;
    }

    public void DeleteCheckersGame(int id)
    {
        var gamesFromDb = GetCheckersGame(id);
        _dbContext.CheckersGames.Remove(gamesFromDb);
        _dbContext.SaveChanges();
    }
}