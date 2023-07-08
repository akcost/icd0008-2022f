using Domain;

namespace DAL;

public interface IGameStateRepository : IBaseRepository
{
    void AddState(CheckersGame state);
    void GetState(int id);
    // void GetLatestStateForGame(int gameId);
    
}