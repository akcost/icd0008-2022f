namespace MenuSystem;

public class Menu
{
    private readonly EMenuLevel _menuLevel;
    private const string ShortcutExit = "X";
    private const string ShortcutGoBack = "B";
    private const string ShortcutGoMain = "M";

    private string Title { get; set; }

    private readonly Dictionary<string, MenuItem> _menuItems = new();
    private readonly MenuItem _menuItemExit = new(ShortcutExit, "Exit", null);
    private readonly MenuItem _menuItemGoBack = new(ShortcutGoBack, "Go Back", null);
    private readonly MenuItem _menuItemGoMain = new(ShortcutGoMain, "Back to Main Menu", null);


    public Menu(EMenuLevel menuLevel, string title, List<MenuItem> menuItems)
    {
        _menuLevel = menuLevel;
        Title = title;
        foreach (var menuItem in menuItems) 
        {
            _menuItems.Add(menuItem.Shortcut, menuItem);
            
        }
        if (_menuLevel != EMenuLevel.Main)
        {
            _menuItems.Add(ShortcutGoBack, _menuItemGoBack);
            _menuItems.Add(ShortcutGoMain, _menuItemGoMain);
        }
        _menuItems.Add(ShortcutExit, _menuItemExit);

        {
            
        }
    }

    public string RunMenu()
    {
        var currentNumber = 0;
        var menuKeys = _menuItems.Keys.ToList();
        MenuItem currentItem = _menuItems[menuKeys[currentNumber]];
        string userChoice = currentItem.Shortcut;
        var menuDone = false;
        do
        {
            Console.WriteLine(Title);
            Console.WriteLine("=================");

            var max = _menuItems.Count - 1;
            foreach (var menuItem in _menuItems.Values)
            {
                currentItem = _menuItems[menuKeys[currentNumber]];
                if (menuItem == currentItem)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write(menuItem);
                Console.ResetColor();
                Console.WriteLine();

            }
            Console.WriteLine("-----------------");

            
            // userChoice = Console.ReadLine()?.ToUpper().Trim() ?? "";
            
            var consoleKeyInfo = Console.ReadKey();
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.DownArrow:
                    currentNumber++;
                    if (currentNumber > max)
                    {
                        currentNumber = 0;
                    }
                    // currentItem = _menuItems[menuKeys[currentNumber]];
                    break;
                case ConsoleKey.UpArrow:
                    currentNumber--;
                    if (currentNumber < 0)
                    {
                        currentNumber = max;
                    }
                    // currentItem = _menuItems[menuKeys[currentNumber]];
                    break;
                case ConsoleKey.Enter:
                    // currentItem = _menuItems[menuKeys[currentNumber]];
                    userChoice = currentItem.Shortcut;
                    if (_menuItems.ContainsKey(userChoice))
                    {
                        string? methodReturnValue = null;
                        if (_menuItems[userChoice].MethodToRun != null)
                        {
                            methodReturnValue = _menuItems[userChoice].MethodToRun!();
                        }
                        if (_menuItems[userChoice].MethodToRun == null && userChoice == "10x10")
                        {
                            menuDone = true;
                        }
                        if (userChoice == ShortcutGoBack)
                        {
                            menuDone = true;
                        }

                        if (userChoice == ShortcutExit || methodReturnValue == ShortcutExit)
                        {
                            userChoice = methodReturnValue ?? userChoice;
                            menuDone = true;
                        }

                        if ((userChoice != ShortcutGoMain && methodReturnValue != ShortcutGoMain) ||
                            _menuLevel == EMenuLevel.Main) continue;
                        userChoice = methodReturnValue ?? userChoice;
                    }
                    else
                    {
                        Console.WriteLine("Wrong choice! :(");
                        Console.WriteLine();
                    }
                    menuDone = true;
                    break;
            }

        } while (menuDone == false);
        
        return userChoice;
        
    }
}