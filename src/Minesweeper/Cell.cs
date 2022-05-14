namespace Minesweeper;

public record Cell
{
    public record Covered(Cell Inner) : Cell;
    public record Bomb : Cell;
    public record Number(int Value) : Cell;
    public record Empty : Cell;

    public Cell ClickTo() => this switch
    {
        Empty x => x,
        Covered x => x.Inner.ClickTo(),
        var x => x
    };

    public Cell BombTo() => this switch
    {
        Empty x => x,
        Covered x => x with
        {
            Inner = new Bomb()
        },
        _ => new Bomb()
    };

    public bool IsBomb() => this switch
    {
        Bomb => true,
        Covered x => x.Inner.IsBomb(),
        _ => false
    };

    public char ToChar() => this switch
    {
        Covered => '.',
        Bomb => '*',
        Number x => Convert.ToChar(48 + x.Value),
        _ => throw new NotImplementedException(),
    };

    public static Cell New() => new Covered(new Number(0));

    public char ToInnerChar() => this switch
    {
        Covered x => x.Inner.ToChar(),
        var x => x.ToChar()
    };
}
