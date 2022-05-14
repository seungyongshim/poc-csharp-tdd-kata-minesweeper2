using System.Security.Cryptography;
using System.Text;
using LanguageExt;
using static LanguageExt.Prelude;
using CellMap = LanguageExt.HashMap<(int, int), Minesweeper.Cell>;

namespace Minesweeper;

public record MineField
{

    public record Setup(int Width, int Height) : MineField;
    public record SetupWithBombs(int Width, int Height, int Bombs) : MineField;
    public record SetupWithBombs1(int Width, int Height, IEnumerable<(int X, int Y)> Bombs) : MineField;
    public record Playing(int Width, int Height, CellMap Cells) : MineField;
    public record Win;
    public record Loose;

    public string ToInnerStr() => this switch
    {
        Playing x => (from b in Enumerable.Range(0, x.Height)
                      from a in Enumerable.Range(0, x.Width)
                      select (a, b))
                     .Fold(new StringBuilder(), (s, e) => s.Append(x.Cells[e].ToInnerChar()))
                     .ToString()
    };

    public string ToStr() => this switch
    {
        Playing x => (from b in Enumerable.Range(0, x.Height)
                      from a in Enumerable.Range(0, x.Width)
                      select (a, b))
                     .Fold(new StringBuilder(), (s, e) => s.Append(x.Cells[e].ToChar()))
                     .ToString()        
    };

    public CellMap? ToCells() => this switch
    {
        Playing x => x.Cells,

        _ => null
    };

    public MineField ClickTo(int xPos, int yPos) => this switch
    {
        Setup x => x.StartTo().ClickTo(xPos, yPos),

        Playing x => x with
        {
            Cells = x.Cells.AddOrUpdate((xPos, yPos), y => y.ClickTo(), new Cell.Empty())
        }
    };

    public MineField StartTo() => this switch
    {
        SetupWithBombs1 x => new Playing(x.Width, x.Height, fun(() =>
        {
            var q = new CellMap(from a in Enumerable.Range(0, x.Width)
                                from b in Enumerable.Range(0, x.Height)
                                select ((a, b), Cell.New()));

            return x.Bombs.Fold(q, (s, e) => s.AddOrUpdate(e, x => x.BombTo(), new Cell.Empty()));
        })()),

        Setup x => new SetupWithBombs(x.Width, x.Height, 0).StartTo(),

        SetupWithBombs x => fun(() =>
        {
            var bombPos = (from i in RandomGenerator(x.Width)
                           from j in RandomGenerator(x.Height)
                           select (i, j)).Distinct().Take(x.Bombs);

            return new SetupWithBombs1(x.Width, x.Height, bombPos);

            static IEnumerable<int> RandomGenerator(int max)
            {
                while (true)
                {
                    yield return RandomNumberGenerator.GetInt32(max);
                }
            }
        })().StartTo(),

        var x => x
    };
}

