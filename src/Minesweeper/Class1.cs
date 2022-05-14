namespace Minesweeper;

public record Cell
{
    public record Covered(Cell Inner) : Cell;
    public record Bomb : Cell;
    public record Number(int Value) : Cell;
}


public static class CellExtension
{
    public static char ToChar(this Cell v) => v switch
    {
        Cell.Covered => '.',
        Cell.Bomb => '*',
        Cell.Number x => Convert.ToChar(x.Value),
        _ => throw new NotImplementedException(),
    };
}
