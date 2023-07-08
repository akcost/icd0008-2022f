using Domain;
using Microsoft.Extensions.Logging;

namespace GameBrain;

public class CheckersBrain
{
    private readonly CheckersState _state = default!;
    private Dictionary<Coordinate, List<Coordinate>> _availableMovesEmpty = null!;
    private Dictionary<Coordinate, List<Coordinate>> _availableMovesJump = null!;


    public CheckersBrain(CheckersOption option, CheckersState? state)
    {
        if (state == null)
        {
            _state = new CheckersState();
            InitializeNewGame(option);
        }
        else
        {
            _state = state;
        }
    }

    public Dictionary<Coordinate, List<Coordinate>> GetAvailableMoves()
    {
        _availableMovesEmpty = new();
        _availableMovesJump = new();
        GenerateAllPossibleMoves();

        return _availableMovesJump.Count > 0 ? _availableMovesJump : _availableMovesEmpty;
    }

    private void InitializeNewGame(CheckersOption option)
    {
        var boardWidth = option.Width;
        var boardHeight = option.Height;


        if (boardWidth < 8 || boardHeight < 8)
        {
            throw new ArgumentException("Board Size too small.");
        }

        if (boardWidth > 26 || boardHeight > 26)
        {
            throw new ArgumentException("Board Size too big.");
        }

        _state.GameBoard = new EGamePiece?[boardWidth][];

        for (int i = 0; i < boardWidth; i++)
        {
            _state.GameBoard[i] = new EGamePiece?[boardHeight];
        }

        for (var y = 0; y < boardHeight; y++)
        {
            for (var x = 0; x < boardWidth; x++)
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

                _state.GameBoard[x][y] = button;
            }
        }
        
        _state.NextMoveByBlack = !option.WhiteStarts;
    }

    public EGamePiece?[][] GetBoard()
    {
        var jsonStr = System.Text.Json.JsonSerializer.Serialize(_state.GameBoard);
        return System.Text.Json.JsonSerializer.Deserialize<EGamePiece?[][]>(jsonStr)!;
    }

    public string GetSerializedGameState()
    {
        return System.Text.Json.JsonSerializer.Serialize(_state);
    }

    public CheckersState GetUnserializedGameState()
    {
        return _state;
    }

    public bool NextMoveByBlack() => _state.NextMoveByBlack;

    public void MakeAMoveByAi()
    {
        var moves = GetAvailableMoves();
        if (moves.Count > 0)
        {
            var random = new Random();
            var randomKey = moves.Keys.ElementAt(random.Next(moves.Count));
            var randomMove = moves[randomKey][random.Next(moves[randomKey].Count)];
            MakeAMove(randomKey.X, randomKey.Y, randomMove.X, randomMove.Y);
        }
    }

    public void GenerateAllPossibleMoves()
    {
        EGamePiece myPiece = NextMoveByBlack() ? EGamePiece.Black : EGamePiece.White;
        EGamePiece myKingPiece = NextMoveByBlack() ? EGamePiece.BlackKing : EGamePiece.WhiteKing;
        for (int y = 0; y < _state.GameBoard[0].Length; y++)
        {
            for (int x = 0; x < _state.GameBoard.Length; x++)
            {
                if (_state.GameBoard[x][y] != null &&
                    (_state.GameBoard[x][y] == myPiece || _state.GameBoard[x][y] == myKingPiece))
                {
                    var opCoordinates = IsOpponentNear(x, y);

                    List<Coordinate> jumpingMoves = new List<Coordinate>();
                    if (opCoordinates.Count != 0)
                    {
                        var (canEat, cellX, cellY) = (false, 0, 0);
                        foreach (var opCoordinate in opCoordinates)
                        {
                            (canEat, cellX, cellY) = CanEat(x, y, opCoordinate.X, opCoordinate.Y);

                            if (canEat)
                            {
                                jumpingMoves.Add(new Coordinate { X = cellX, Y = cellY });
                            }
                        }
                    }

                    if (jumpingMoves.Count != 0)
                    {
                        foreach (var jumpingMove in jumpingMoves)
                        {
                            Coordinate coordinate = new Coordinate { X = x, Y = y };
                            if (_availableMovesJump.ContainsKey(coordinate))
                            {
                                _availableMovesJump[coordinate].Add(jumpingMove);
                            }
                            else
                            {
                                var coordinates = new List<Coordinate>();
                                coordinates.Add(jumpingMove);
                                _availableMovesJump.Add(coordinate, coordinates);
                            }
                        }
                    }
                    else
                    {
                        var normalMoves = GetNormalMoves(x, y);
                        foreach (var move in normalMoves)
                        {
                            Coordinate coordinate = new Coordinate();
                            coordinate.X = x;
                            coordinate.Y = y;
                            if (_availableMovesEmpty.ContainsKey(coordinate))
                            {
                                _availableMovesEmpty[coordinate].Add(move);
                            }
                            else
                            {
                                var coordinates = new List<Coordinate>();
                                coordinates.Add(move);
                                _availableMovesEmpty.Add(coordinate, coordinates);
                            }
                        }
                    }
                }
            }
        }
    }

    public List<Coordinate> GetNormalMoves(int posX, int posY)
    {
        List<Coordinate> moveList = new List<Coordinate>();
        bool isKing = IsKing(posX, posY);
        int newX1 = posX + 1;
        int newX2 = posX - 1;
        int step = NextMoveByBlack() ? -1 : 1;
        int newY = posY + step;
        int newY2 = posY - step;

        if (!isKing)
        {
            if (newY > _state.GameBoard[0].Length - 1 || newY < 0)
            {
                return moveList;
            }
        }

        if (newX1 < _state.GameBoard.Length)
        {
            if (newY < _state.GameBoard[0].Length && newY >= 0)
            {
                if (_state.GameBoard[newX1][newY] == null)
                {
                    //No piece on new position, so a move there is possible
                    Coordinate coordinate = new Coordinate();
                    coordinate.X = newX1;
                    coordinate.Y = newY;
                    moveList.Add(coordinate);
                }
            }

            //King move
            if (isKing && newY2 < _state.GameBoard[0].Length && newY2 >= 0)
            {
                if (_state.GameBoard[newX1][newY2] == null)
                {
                    //No piece on new position, so a move there is possible
                    Coordinate coordinate = new Coordinate();
                    coordinate.X = newX1;
                    coordinate.Y = newY2;
                    moveList.Add(coordinate);
                }
            }
        }

        if (newX2 >= 0)
        {
            if (newY < _state.GameBoard[0].Length && newY >= 0)
            {
                if (_state.GameBoard[newX2][newY] == null)
                {
                    //No piece on new position, so a move there is possible
                    Coordinate coordinate = new Coordinate();
                    coordinate.X = newX2;
                    coordinate.Y = newY;
                    moveList.Add(coordinate);
                }
            }

            //King move
            if (newY2 < _state.GameBoard[0].Length && newY2 >= 0)
            {
                if (isKing && _state.GameBoard[newX2][newY2] == null)
                {
                    //No piece on new position, so a move there is possible
                    Coordinate coordinate = new Coordinate();
                    coordinate.X = newX2;
                    coordinate.Y = newY2;
                    moveList.Add(coordinate);
                }
            }
        }

        return moveList;
    }

    public (int, int) GetScore()
    {
        int maxPieces = _state.GameBoard[0].Length * 3 / 8 * (_state.GameBoard.Length / 2);
        int blacks = 0;
        int whites = 0;
        
        for (var y = 0; y < _state.GameBoard[0].Length; y++)
        {
            for (var x = 0; x < _state.GameBoard.Length; x++)
            {
                switch (_state.GameBoard[x][y])
                {
                    case EGamePiece.Black:
                        blacks++;
                        break;
                    case EGamePiece.BlackKing:
                        blacks++;
                        break;
                    case EGamePiece.White:
                        whites++;
                        break;
                    case EGamePiece.WhiteKing:
                        whites++;
                        break;
                }
            }
        }


        return (maxPieces - blacks, maxPieces - whites);
    }

    public List<Coordinate> IsOpponentNear(int posX, int posY)
    {
        List<Coordinate> opCoordinatesList = new List<Coordinate>();
        int opY2 = 0;
        bool isKing = false;
        if (_state.GameBoard[posX][posY] == EGamePiece.WhiteKing ||
            _state.GameBoard[posX][posY] == EGamePiece.BlackKing)
        {
            isKing = true;
            opY2 = posY + (NextMoveByBlack() ? 1 : -1);
        }

        int opY1 = posY + (NextMoveByBlack() ? -1 : 1);
        int opX1 = posX + 1;
        int opX2 = posX - 1;

        if (!isKing)
        {
            if (opY1 > _state.GameBoard.Length - 1 || opY1 < 0)
            {
                return opCoordinatesList;
            }
        }

        EGamePiece opPiece = NextMoveByBlack() ? EGamePiece.White : EGamePiece.Black;
        EGamePiece opKingPiece = NextMoveByBlack() ? EGamePiece.WhiteKing : EGamePiece.BlackKing;

        if (opX1 < _state.GameBoard.Length - 1)
        {
            if (opY1 < _state.GameBoard[0].Length - 1 && opY1 > 0)
            {
                if (_state.GameBoard[opX1][opY1] == opPiece || _state.GameBoard[opX1][opY1] == opKingPiece)
                {
                    opCoordinatesList.Add(new Coordinate { X = opX1, Y = opY1 });
                }
            }

            //King part
            if (opY2 < _state.GameBoard[0].Length - 1 && opY2 > 0)
            {
                if (isKing && (_state.GameBoard[opX1][opY2] == opPiece || _state.GameBoard[opX1][opY2] == opKingPiece))
                {
                    opCoordinatesList.Add(new Coordinate { X = opX1, Y = opY2 });
                }
            }
        }


        if (opX2 > 0)
        {
            if (opY1 < _state.GameBoard[0].Length - 1 && opY1 > 0)
            {
                if (_state.GameBoard[opX2][opY1] == opPiece || _state.GameBoard[opX2][opY1] == opKingPiece)
                {
                    opCoordinatesList.Add(new Coordinate { X = opX2, Y = opY1 });
                }
            }

            //King part
            if (opY2 < _state.GameBoard[0].Length - 1 && opY2 > 0)
            {
                if (isKing && (_state.GameBoard[opX2][opY2] == opPiece || _state.GameBoard[opX2][opY2] == opKingPiece))
                {
                    opCoordinatesList.Add(new Coordinate { X = opX2, Y = opY2 });
                }
            }
        }

        return opCoordinatesList;
    }

    public bool IsKing(int posX, int posY)
    {
        if (_state.GameBoard[posX][posY] == EGamePiece.WhiteKing ||
            _state.GameBoard[posX][posY] == EGamePiece.BlackKing)
        {
            return true;
        }

        return false;
    }

    /*
     * |?| | |
     * | |o| |
     * | | |x|
     * 
     * | | |?|
     * | |o| |
     * |x| | |
     */
    public (bool, int, int) CanEat(int posX, int posY, int opX, int opY)
    {
        int cellX = (opX - posX) + opX;
        int cellY = (opY - posY) + opY;
        if (cellX < 0 || cellX > _state.GameBoard.Length - 1 || cellY < 0 || cellY > _state.GameBoard[0].Length - 1)
        {
            return (false, 0, 0);
        }

        if (_state.GameBoard[cellX][cellY] == null)
        {
            return (true, cellX, cellY);
        }


        return (false, 0, 0);
    }

    public void MakeAMove(int x, int y, int newX, int newY)
    {
        bool isKing = IsKing(x, y);
        EGamePiece myPiece = NextMoveByBlack() ? isKing ? EGamePiece.BlackKing : EGamePiece.Black :
            isKing ? EGamePiece.WhiteKing : EGamePiece.White;
        //Eating move
        if (x - newX == 2 || x - newX == -2)
        {
            int opX = (x + newX) / 2;
            int opY = (y + newY) / 2;
            _state.GameBoard[newX][newY] = myPiece;
            _state.GameBoard[opX][opY] = null;
            _state.GameBoard[x][y] = null;
            
            
            var opsList = IsOpponentNear(newX, newY);
            var canEatAgain = false;
            foreach (var opCoordinate in opsList)
            {
                var (canEat, item2, item3) = CanEat(newX, newY, opCoordinate.X, opCoordinate.Y);
                if (canEat)
                {
                    canEatAgain = true;
                }
            }

            if (myPiece == EGamePiece.White && newY == _state.GameBoard[0].Length - 1)
            {
                _state.GameBoard[newX][newY] = EGamePiece.WhiteKing;
                _state.NextMoveByBlack = !_state.NextMoveByBlack;

            }
            else if (myPiece == EGamePiece.Black && newY == 0)
            {
                _state.GameBoard[newX][newY] = EGamePiece.BlackKing;
                _state.NextMoveByBlack = !_state.NextMoveByBlack;
            } else if (!canEatAgain)
            {
                _state.NextMoveByBlack = !_state.NextMoveByBlack;
            }


            var (whiteScore, blackScore) = GetScore();
            if (whiteScore == 12 || blackScore == 12)
            {
                //Game over;
            }
        }
        //Normal move
        else
        {
            if (!NextMoveByBlack() && newY == _state.GameBoard[0].Length - 1)
            {
                _state.GameBoard[newX][newY] = EGamePiece.WhiteKing;
            }
            else if (NextMoveByBlack() && newY == 0)
            {
                _state.GameBoard[newX][newY] = EGamePiece.BlackKing;
            }
            else
            {
                _state.GameBoard[newX][newY] = myPiece;
            }

            _state.GameBoard[x][y] = null;
            _state.NextMoveByBlack = !_state.NextMoveByBlack;
        }
    }
}