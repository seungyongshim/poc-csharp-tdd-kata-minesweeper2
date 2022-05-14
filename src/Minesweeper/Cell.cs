namespace Minesweeper;

public interface ICell
{
    public record Covered(ICell Inner) : ICell;
    public record Bomb : ICell;
    public record Number(int Value) : ICell;
}
