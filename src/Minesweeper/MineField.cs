using LanguageExt;
using static Minesweeper.MineField;
using CellMap = LanguageExt.HashMap<(int, int), Minesweeper.Cell>;
using static LanguageExt.Prelude;
using System.Security.Cryptography;

namespace Minesweeper;

public record MineField
{
    public record Setup(int Width, int Height) : MineField;
    public record SetupWithBombs(int Width, int Height, int Bombs) : MineField;
    public record Playing(int Width, int Height, CellMap Cells) : MineField;

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
        Setup x => new SetupWithBombs(x.Width, x.Height, 0).StartTo(),

        SetupWithBombs x => new Playing(x.Width, x.Height, fun(() =>
        {
            var bombPos = (from i in RandomGenerator(x.Width)
                           from j in RandomGenerator(x.Height)
                           select (i, j)).Distinct().Take(x.Bombs);

            var q = new CellMap(from a in Enumerable.Range(0, x.Width)
                                from b in Enumerable.Range(0, x.Height)
                                select ((a, b), new Cell.Covered(new Cell.Number(0)) as Cell));

            return bombPos.Fold(q, (s, e) => s.AddOrUpdate(e, x => x.ToHasBomb(), () => null));

            static IEnumerable<int> RandomGenerator(int max)
            {
                while (true)
                {
                    yield return RandomNumberGenerator.GetInt32(max);
                }
            }
        })())
    };

    
}

