namespace GameBrain;

public struct Coordinate
{

    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return "X = " + X + ", Y = " + Y;
    }
}