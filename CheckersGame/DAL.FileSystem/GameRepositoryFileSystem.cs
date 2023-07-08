using Domain;

namespace DAL.FileSystem;

public class GameRepositoryFileSystem : IGameRepository
{
    private const string FileExtension = "json";
    private readonly string _optionsDirectory = "." + System.IO.Path.DirectorySeparatorChar + "options";
    private readonly string _gameDirectory = "." + System.IO.Path.DirectorySeparatorChar + "game";

    public List<string> GetGameOptionsList()
    {
        CheckOrCreateDirectory();
        
        var res = new List<string>();
        
        foreach (var fileName in Directory.GetFileSystemEntries(_optionsDirectory, "*." + FileExtension))
        {
            res.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
        }

        return res;
    }

    public CheckersOption GetGameOptions(string id)
    {
        var fileContent = System.IO.File.ReadAllText(GetFileName(id));
        var options = System.Text.Json.JsonSerializer.Deserialize<CheckersOption>(fileContent);
        if (options == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        return options;
    }

    public void SaveGameOptions(string id, CheckersOption option)
    {
        CheckOrCreateDirectory();
            
        var fileContent = System.Text.Json.JsonSerializer.Serialize(option);
        System.IO.File.WriteAllText(GetFileName(id), fileContent);
    }

    public void DeleteGameOptions(string id)
    {
        System.IO.File.Delete(GetFileName(id));
    }

    private string GetFileName(string id)
    {
        return _optionsDirectory +
               System.IO.Path.DirectorySeparatorChar +
               id + "." + FileExtension;
    }

    private void CheckOrCreateDirectory()
    {
        if (!System.IO.Directory.Exists(_gameDirectory))
        {
            System.IO.Directory.CreateDirectory(_gameDirectory);
        }
    }

    public List<string> GetGamesList()
    {
        CheckOrCreateDirectory();
        
        var res = new List<string>();
        
        foreach (var fileName in Directory.GetFileSystemEntries(_gameDirectory, "*." + FileExtension))
        {
            res.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
        }

        return res;
    }

    public CheckersGameState GetGame(string id)
    {
        var fileContent = System.IO.File.ReadAllText(GetFileName(id));
        var gameState = System.Text.Json.JsonSerializer.Deserialize<CheckersGameState>(fileContent);
        if (gameState == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }

        return gameState;
    }

    public void SaveGame(string id, CheckersGameState gameState)
    {
        CheckOrCreateDirectory();
            
        var fileContent = System.Text.Json.JsonSerializer.Serialize(gameState);
        System.IO.File.WriteAllText(GetFileName(id), fileContent);
    }

    public void DeleteGame(string id)
    {
        throw new NotImplementedException();
    }

    public string? Name { get; }
    public void SaveChanges()
    {
        throw new NotImplementedException();
    }

    public List<CheckersGame> GetAll()
    {
        throw new NotImplementedException();
    }

    public CheckersGame? GetGame(int? id)
    {
        throw new NotImplementedException();
    }

    public CheckersGame AddGame(CheckersGame game)
    {
        throw new NotImplementedException();
    }
}