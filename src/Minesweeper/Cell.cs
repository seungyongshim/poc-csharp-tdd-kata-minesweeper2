namespace Minesweeper;

public interface ICell
{
    public record Covered(ICell Inner) : ICell;
    public record Bomb : ICell;
    public record Number(int Value) : ICell;
    public record Empty : ICell;

    public ICell Click() => this switch
    {
        Covered x => x.Inner,
        var x => x
    };

    public char ToChar() => this switch
    {
        Covered => '.',
        Bomb => '*',
        Number x => Convert.ToChar(48 + x.Value),
        _ => throw new NotImplementedException(),
    };
}
