namespace DAL;

using Domain;

public interface ISavedCheckersGameRepository
{
    List<CheckersGame> GetSavedCheckersGameList();

    CheckersGame GetCheckersGame(int id);
    
    // create and update
    CheckersGame SaveCheckersGame(CheckersGame savedGame);
    
    // Delete 
    void DeleteCheckersGame(int id);
}