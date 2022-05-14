using LanguageExt;
using static Minesweeper.IMineField;
using CellMap = LanguageExt.HashMap<(int, int), Minesweeper.ICell>;
using static LanguageExt.Prelude;

namespace Minesweeper;

public interface IMineField
{
    public record Setup(int Width, int Height) : IMineField;
    public record Playing(int Width, int Height, CellMap Cells) : IMineField;
}

public static class MineFieldExtension
{
    public static Option<CellMap> ToCells(this IMineField @this) => @this switch
    {
        Playing x => x.Cells,
        _ => None
    };

    public static IMineField ClickTo(this IMineField @this, int xPos, int yPos) => @this switch
    {
        Setup x => ClickTo(StartTo(x), xPos, yPos),
        Playing x => x with
        {
            Cells = x.Cells.AddOrUpdate((xPos, yPos), y => y.Click(), new ICell.Empty())
        }
    };

    public static IMineField StartTo(this IMineField @this) => @this switch
    {
        Setup l => new Playing(l.Width, l.Height, new CellMap(from a in Enumerable.Range(0, l.Width)
                                                              from b in Enumerable.Range(0, l.Height)
                                                              select ((a, b), new ICell.Covered(new ICell.Number(0)) as ICell)))
    };
}
