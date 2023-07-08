using System.Text.Json;
using ConsoleUI;
using DAL;
using DAL.Db;
using Domain;
using GameBrain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;


var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
    // .UseLoggerFactory(Helpers.MyLoggerFactory)
    .UseSqlite("Data Source=/Users/akselcosta/RiderProjects/icd0008-2022f/CheckersGame/DAL.Db/app.db")
    .Options;
const string optionName = "8x8";
using var ctx = new AppDbContext(dbOptions);

var gameOptions = new CheckersOption
{
    Name = optionName,
    Height = 8,
    Width = 8
};

CheckersGame checkersGameCurrent;

IGameOptionsRepository repoDb = new GameOptionsRepositoryDb(ctx);
ISavedGameStateRepository savedGameStateRepositoryDataBase = new SavedGameStateRepositoryDb(ctx);
ISavedCheckersGameRepository savedCheckersGameRepositoryDataBase = new SavedCheckersGameRepositoryDb(ctx);

var gameOptionsRepository = repoDb;
var savedGameStateRepository = savedGameStateRepositoryDataBase;
var savedCheckersGameRepository = savedCheckersGameRepositoryDataBase;


gameOptionsRepository.SaveGameOptions(gameOptions.Name, gameOptions);


var gameOptionsMenu = new Menu(
    EMenuLevel.Second,
    ">>>Checkers Options<<<",
    new List<MenuItem>()
    {
        new MenuItem("1", "Start Game", DoNewGame),
        new MenuItem("C", "Create Options", CreateGameOptions),
        new MenuItem("L", "Load Options", LoadGameOptions),
        new MenuItem("D", "Delete Options", null),
        new MenuItem("E", "Edit Options", null),
        new MenuItem("S", "Save Current Options", null),
    }
);

var optionsMenu = new Menu(
    EMenuLevel.Second,
    ">>>Checkers Options<<<",
    new List<MenuItem>()
    {
        new MenuItem("C", "Create Options", null),
        new MenuItem("L", "Load Options", LoadGameOptions),
        new MenuItem("D", "Delete Options", null),
        new MenuItem("E", "Edit Options", null),
        new MenuItem("S", "Save Current Options", null)
    }
);

var mainMenu = new Menu(
    EMenuLevel.Main,
    ">>>Checkers<<<",
    new List<MenuItem>()
    {
        new MenuItem("1", "New Game", gameOptionsMenu.RunMenu),
        new MenuItem("2", "Load Game", LoadSavedGame),
        new MenuItem("3", "Options", optionsMenu.RunMenu)
    }
);


string DoNewGame()
{
    Console.WriteLine("New Game Method");
    SaveOption(gameOptions.Name, gameOptions);

    var gameName = UserInterface.AskForString("Game Name");
    var player1Name = UserInterface.AskForString("player 1 Name");
    var player1Type = UserInterface.AskForPlayerType();
    var player2Name = UserInterface.AskForString("player 2 Name");
    var player2Type = UserInterface.AskForPlayerType();

    checkersGameCurrent = new CheckersGame
    {
        GameName = gameName,
        Player1Name = player1Name,
        Player1Type = player1Type,
        Player2Name = player2Name,
        Player2Type = player2Type,
        CheckersOption = gameOptions,
    };
    gameOptions.CheckersGames ??= new List<CheckersGame>();
    gameOptions.CheckersGames.Add(checkersGameCurrent);
    SaveGame(checkersGameCurrent,
        CreateNewGameBoard(gameOptions.Height, gameOptions.Width));

    PlayAsPlayerOption(checkersGameCurrent);
    return "";
}


void StartGame(CheckersGame checkersGame, int playerNo)
{
    var gameOver = false;
    var selected = new Coordinate { X = 0, Y = 0 };
    Coordinate? moveButton = null;
    while (!gameOver)
    {
        checkersGame.CheckersGameStates = savedGameStateRepository!.GetSavedGameStateListByCheckersGame(checkersGame);
        var checkersBrain = new CheckersBrain(gameOptions, GetLatestGameState(checkersGame));
        var unSerializedGameState = new CheckersState
        {
            GameBoard = checkersBrain.GetBoard(),
            NextMoveByBlack = checkersBrain.NextMoveByBlack()
        };
        if ((checkersGame.Player1Type == EPlayerType.Ai && !unSerializedGameState.NextMoveByBlack) ||
            (checkersGame.Player2Type == EPlayerType.Ai && unSerializedGameState.NextMoveByBlack))
        {
            checkersBrain.MakeAMoveByAi();
            unSerializedGameState = checkersBrain.GetUnserializedGameState();
            SaveGame(checkersGame, unSerializedGameState);
        }

        var maxCoordinate = unSerializedGameState.GameBoard.Length - 1;


        var availableMoves = checkersBrain.GetAvailableMoves();
        if (availableMoves.Count < 1)
        {
            gameOver = true;
            if (checkersGame.GameWonByPlayer == null || checkersGame.GameOverAt == null)
            {
                checkersGame.GameWonByPlayer = unSerializedGameState.NextMoveByBlack
                    ? checkersGame.Player1Name
                    : checkersGame.Player2Name;
                checkersGame.GameOverAt = DateTime.Now;
                SaveGame(checkersGame, unSerializedGameState);
            }

            Console.WriteLine($"Game Over at {checkersGame.GameOverAt}! Winner: {checkersGame.GameWonByPlayer}");
        }


        UserInterface.DrawGameBoard(unSerializedGameState.GameBoard, selected, moveButton, availableMoves);
        var consoleKeyInfo = Console.ReadKey();
        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                if (selected.Y - 1 < 0)
                {
                    selected.Y = maxCoordinate;
                }
                else
                {
                    selected.Y -= 1;
                }

                break;
            case ConsoleKey.DownArrow:
                if (selected.Y + 1 > maxCoordinate)
                {
                    selected.Y = 0;
                }
                else
                {
                    selected.Y += 1;
                }

                break;
            case ConsoleKey.LeftArrow:
                if (selected.X - 1 < 0)
                {
                    selected.X = maxCoordinate;
                }
                else
                {
                    selected.X -= 1;
                }

                break;
            case ConsoleKey.RightArrow:
                if (selected.X + 1 > maxCoordinate)
                {
                    selected.X = 0;
                }
                else
                {
                    selected.X += 1;
                }

                break;
            case ConsoleKey.Enter:

                
                if (IsPlayerMove(checkersBrain, playerNo))
                {
                    if (moveButton != null)
                    {
                        //unselect
                        if (moveButton.Value.X == selected.X && moveButton.Value.Y == selected.Y)
                        {
                            moveButton = null;
                        }
                        else if (availableMoves[moveButton.Value].Contains(selected))
                        {
                            checkersBrain.MakeAMove(moveButton.Value.X, moveButton.Value.Y, selected.X, selected.Y);
                            unSerializedGameState = checkersBrain.GetUnserializedGameState();
                            SaveGame(checkersGame, unSerializedGameState);
                            moveButton = null;
                        }
                    }
                    else
                    {
                        if (availableMoves.ContainsKey(selected))
                        {
                            moveButton = selected;
                        }
                    }
                }

                break;
            case ConsoleKey.X:
                SaveGame(checkersGame, unSerializedGameState);
                gameOver = true;
                break;
        }
    }
}

CheckersState CreateNewGameBoard(int boardWidth, int boardHeight)
{
    if (boardWidth < 8 || boardHeight < 8)
    {
        throw new ArgumentException("Board Size too small.");
    }

    if (boardWidth > 26 || boardHeight > 26)
    {
        throw new ArgumentException("Board Size too big.");
    }

    var state = new CheckersState
    {
        GameBoard = new EGamePiece?[boardWidth][]
    };

    for (int i = 0; i < boardWidth; i++)
    {
        state.GameBoard[i] = new EGamePiece?[boardHeight];
    }

    for (var y = 0; y < boardWidth; y++)
    {
        for (var x = 0; x < boardHeight; x++)
        {
            EGamePiece? button;
            if ((y + x) % 2 == 0)
            {
                button = null;
            }
            else
            {
                var buttonsPlacement = boardHeight * 3 / 8;

                if (y < buttonsPlacement)
                {
                    button = EGamePiece.White;
                }
                else if (y >= boardHeight - buttonsPlacement)
                {
                    button = EGamePiece.Black;
                }
                else
                {
                    button = null;
                }
            }


            state.GameBoard[x][y] = button;
        }
    }

    return state;
}

CheckersState GetLatestGameState(CheckersGame checkersGame)
{
    var latestStart = checkersGame.CheckersGameStates!.Max(gs => gs.CreatedAt);
    var gameState = checkersGame.CheckersGameStates!.Last(gs => gs.CreatedAt == latestStart);
    // CheckersGameState checkersGameState = checkersGame.CheckersGameStates!.LastOrDefault()!;


    CheckersState cState = JsonSerializer.Deserialize<CheckersState>(gameState.SerializedGameState)!;

    return cState;
}

string LoadGameOptions()
{
    Console.Write("Options Name:");
    var optionsName = Console.ReadLine();
    if (optionsName == null) return ".";
    gameOptions = gameOptionsRepository.GetGameOptions(optionsName);
    return ".";
}

string CreateGameOptions()
{
    Console.Write("Board Width and Height (between 8 and 26):");
    var width = int.Parse(Console.ReadLine() ?? "8");

    var whiteStarts = UserInterface.AskTrueFalse("White Starts");

    gameOptions = new CheckersOption()
    {
        Height = width,
        Width = width,
        Name = $"{width}x{width}",
        WhiteStarts = whiteStarts
    };
    Console.WriteLine($"\nBoard selected for {gameOptions}.");
    SaveOption($"{width}x{width}", gameOptions);
    return ";";
}

void SaveOption(string name, CheckersOption gameOption)
{
    gameOptionsRepository.SaveGameOptions(name, gameOption);
}


void SaveGame(CheckersGame checkersGame, CheckersState checkersUnSerializedState)
{
    var checkersGameState = new CheckersGameState()
    {
        CheckersGame = checkersGame,
        SerializedGameState = JsonSerializer.Serialize(checkersUnSerializedState)
    };
    var id = savedGameStateRepository.SaveGameState(0, checkersGameState);

    checkersGame.CheckersGameStates ??= new List<CheckersGameState>();
    checkersGame.CheckersGameStates.Add(checkersGameState);
    checkersGame.CheckersGameStateIds ??= new List<int>();
    checkersGame.CheckersGameStateIds.Add(id);
    savedCheckersGameRepository!.SaveCheckersGame(checkersGame);
}

string LoadSavedGame()
{
    var selected = 0;
    var checkersGames = savedCheckersGameRepository.GetSavedCheckersGameList();
    var loadGame = false;
    while (!loadGame)
    {
        UserInterface.DrawSavedGames(selected, checkersGames);
        var consoleKeyInfo = Console.ReadKey();
        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.DownArrow:
                if (selected + 1 < checkersGames.Count)
                {
                    selected++;
                }
                else
                {
                    selected = 0;
                }

                break;
            case ConsoleKey.UpArrow:
                if (selected > 0)
                {
                    selected--;
                }
                else
                {
                    selected = checkersGames.Count - 1;
                }

                break;

            case ConsoleKey.Enter:
                loadGame = true;
                PlayAsPlayerOption(checkersGames[selected]);
                break;
        }
    }

    return "Game Loaded";
}

void PlayAsPlayerOption(CheckersGame checkersGame)
{
    var selected = 0;
    var loadGame = false;

    var options = new List<string>()
    {
        $"Player 1 - {checkersGame.Player1Name}",
        $"Player 2 - {checkersGame.Player2Name}",
        "Both"
    };
    while (!loadGame)
    {
        Console.WriteLine("<<< Select Player >>>");
        foreach (var player in options)
        {
            if (options[selected] == player)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write(player);
            Console.ResetColor();
            Console.WriteLine();
        }

        var consoleKeyInfo = Console.ReadKey();
        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.DownArrow:
                if (selected < 2)
                {
                    selected++;
                }
                else
                {
                    selected = 0;
                }

                break;
            case ConsoleKey.UpArrow:
                if (selected > 0)
                {
                    selected--;
                }
                else
                {
                    selected = 2;
                }

                break;
            case ConsoleKey.Enter:
                loadGame = true;
                Console.WriteLine("Selected: " + selected);
                StartGame(checkersGame, selected);
                break;
        }
    }
}


bool IsPlayerMove(CheckersBrain checkersBrain, int playerNo)
{
    if (playerNo == 2)
    {
        return true;
    }

    if (checkersBrain.NextMoveByBlack() && playerNo == 1)
    {
        return true;
    }

    if (!checkersBrain.NextMoveByBlack() && playerNo == 0)
    {
        return true;
    }

    return false;
}

mainMenu.RunMenu();