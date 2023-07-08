using Domain;
using GameBrain;


namespace ConsoleUI;

public static class UserInterface
{
    public static void DrawGameBoard(
        EGamePiece?[][] board, Coordinate selected, Coordinate? moveButton,
        Dictionary<Coordinate, List<Coordinate>> availableMoves)
    {
        var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        var columns = board.GetLength(0);
        var rows = board.GetLength(0);

        var rowSpace = new string(' ', rows.ToString().Length);
        var alphaLine = "  " + rowSpace;

        for (var i = 0; i < columns; i++)
        {
            alphaLine += alpha[i] + "  ";
        }

        Console.WriteLine(selected);

        Console.WriteLine(alphaLine);

        var consoleColor = ConsoleColor.White;

        int moveX = -1;
        int moveY = -1;
        if (moveButton != null)
        {
            moveX = moveButton.Value.X;
            moveY = moveButton.Value.Y;
        }

        for (var y = 0; y < rows; y++)
        {
            Console.ResetColor();
            Console.Write(rows - y + " " + new string(' ', rows.ToString().Length - (rows - y).ToString().Length));
            for (var x = 0; x < columns; x++)
            {
                if (x == selected.X && y == selected.Y)
                {
                    consoleColor = ConsoleColor.Green;
                }
                else if (x == moveX && y == moveY)
                {
                    consoleColor = ConsoleColor.Magenta;
                }
                else
                {
                    if ((x + y) % 2 == 0)
                    {
                        consoleColor = ConsoleColor.Black;
                    }
                    else
                    {
                        consoleColor = ConsoleColor.White;
                    }
                    var coordinates = new Coordinate { X = x, Y = y };

                    if (moveButton != null)
                    {
                        if (availableMoves[moveButton.Value].Contains(coordinates))
                        {
                            consoleColor = ConsoleColor.Yellow;
                        }
                    }
                }
                
                Console.BackgroundColor = consoleColor;
                switch (board[x][y])
                {
                    case EGamePiece.Black:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" ◉ ");
                        break;
                    case EGamePiece.BlackKing:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" ◎ ");
                        break;
                    case EGamePiece.White:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(" ◉ ");
                        break;
                    case EGamePiece.WhiteKing:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(" ◎ ");
                        break;
                    case null:
                        Console.Write("   ");
                        break;
                    default:
                        Console.Write("   ");
                        break;
                }

                Console.ResetColor();
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }

    public static void DrawSavedGames(int selected, List<CheckersGame> checkersGames)
    {
        Console.WriteLine(">>> Select Checkers Game <<<");
        var current = 0;
        foreach (var checkersGame in checkersGames)
        {
            Console.ResetColor();
            if (selected == current)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            current++;
            Console.Write(checkersGame.Id);
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    public static string AskForString(string askedName)
    {
        var gameName = "";

        while (gameName is { Length: < 2 })
        {
            Console.Write($"Write {askedName}: ");
            gameName = Console.ReadLine();
        }

        return gameName ?? $"New {askedName}";
    }

    public static EPlayerType AskForPlayerType()
    {
        var selected = false;
        var current = EPlayerType.Human;
        while (!selected)
        {
            Console.WriteLine(">>> Select Player Type <<<");

            if (current == EPlayerType.Human)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write("Human");
            Console.ResetColor();
            Console.WriteLine();
            if (current == EPlayerType.Ai)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write("AI");
            Console.ResetColor();
            Console.WriteLine();


            var consoleKeyInfo = Console.ReadKey();
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.DownArrow:
                    current = current == EPlayerType.Human ? EPlayerType.Ai : EPlayerType.Human;
                    break;

                case ConsoleKey.UpArrow:
                    current = current == EPlayerType.Human ? EPlayerType.Ai : EPlayerType.Human;
                    break;
                case ConsoleKey.Enter:
                    selected = true;
                    break;
            }
        }

        return current;
    }

    public static bool AskTrueFalse(string askedValueName)
    {
        var selected = false;
        var value = true;
        while (!selected)
        {
            Console.WriteLine($">>> {askedValueName} <<<");

            if (value)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write("true");
            Console.ResetColor();
            Console.WriteLine();
            if (!value)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write("false");
            Console.ResetColor();
            Console.WriteLine();
            var consoleKeyInfo = Console.ReadKey();
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.DownArrow:
                    value = !value;
                    break;

                case ConsoleKey.UpArrow:
                    value = !value;
                    break;
                case ConsoleKey.Enter:
                    selected = true;
                    break;
            }
        }

        return value;
    }
}