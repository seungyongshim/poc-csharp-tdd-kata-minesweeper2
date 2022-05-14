namespace Minesweeper;

public static class CellExtension
{
    public static char ToChar(this ICell v) => v switch
    {
        ICell.Covered => '.',
        ICell.Bomb => '*',
        ICell.Number x => Convert.ToChar(48 + x.Value),
        _ => throw new NotImplementedException(),
    };
}
