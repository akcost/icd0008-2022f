using Domain;

namespace DAL.Db;

public class SavedGameStateRepositoryDb : ISavedGameStateRepository
{
    
    private readonly AppDbContext _dbContext;
    
    public SavedGameStateRepositoryDb(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Name { get; } = "DB";
    
    public List<string> GetSavedGameStateList() => _dbContext
        .CheckersGameStates
        .OrderBy(gs => gs.Id)
        .Select(gs => gs.Id.ToString())
        .ToList();

    public List<CheckersGameState> GetSavedGameStateListByCheckersGame(CheckersGame checkersGame) => _dbContext
        .CheckersGameStates
        .OrderByDescending(gs => gs.CreatedAt)
        .Where(gs => gs.CheckersGame == checkersGame)
        .Select(gs => gs)
        .ToList();

    public void DeleteSavedGameState(int id)
    {
        var gameState = GetCheckersGameState(id);
        _dbContext.CheckersGameStates.Remove(gameState);
        _dbContext.SaveChanges();
    }

    public CheckersGameState GetCheckersGameState(int id) => _dbContext
        .CheckersGameStates
        .First(gs => gs.Id == id);

    public int SaveGameState(int id, CheckersGameState savedGameState)
    {
        // var max = _dbContext.CheckersGameStates.Max(s => s.Id);
        // savedGameState.Id = max + 1;

        _dbContext.CheckersGameStates.Add(savedGameState);
        _dbContext.SaveChanges();
        return savedGameState.Id;
    }
}