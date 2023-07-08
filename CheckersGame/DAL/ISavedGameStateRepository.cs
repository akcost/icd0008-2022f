using Domain;

namespace DAL;

public interface ISavedGameStateRepository
{
    // CRUD Create Read Update Delete
    // read
    List<string> GetSavedGameStateList();
    CheckersGameState GetCheckersGameState(int id);
    
    // create and update
    int SaveGameState(int id, CheckersGameState savedGameState);

    List<CheckersGameState> GetSavedGameStateListByCheckersGame(CheckersGame checkersGame);

    // Delete 
    void DeleteSavedGameState(int id);
}