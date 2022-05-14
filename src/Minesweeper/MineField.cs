using System.Security.Cryptography;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using CellMap = LanguageExt.HashMap<(int, int), Minesweeper.Cell>;

namespace Minesweeper;

public record MineField
{

    public record Setup(int Width, int Height) : MineField;
    public record SetupWithBombs(int Width, int Height, int Bombs) : MineField;
    public record SetupWithBombsPos(int Width, int Height, IEnumerable<(int X, int Y)> Bombs) : MineField;
    public record Playing(int Width, int Height, CellMap Cells) : MineField;
    public record Win : MineField;
    public record Loose : MineField;

    public string ToInnerStr() => this switch
    {
        Playing x => (from b in Enumerable.Range(0, x.Height)
                      from a in Enumerable.Range(0, x.Width)
                      select (a, b))
                     .OrderBy(x => x)
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

        SetupWithBombsPos x => x.StartTo().ClickTo(xPos, yPos),

        SetupWithBombs x => x.StartTo().ClickTo(xPos, yPos),

        Playing x => x.Cells.ContainsKey((xPos, yPos)) ? x.Cells[(xPos, yPos)] is Cell.Covered ? Id(
            from _1 in Id(x with
            {
                Cells = x.Cells.AddOrUpdate((xPos, yPos), y => y.ClickTo(), new Cell.Empty())
            })
            let _2 = _1.Cells[(xPos, yPos)] switch
            {
                Cell.Bomb => new Loose(),
                Cell.Number { Value: 0 } => _1.ClickTo(xPos - 1, yPos - 1)
                                              .ClickTo(xPos, yPos - 1)
                                              .ClickTo(xPos + 1, yPos - 1)
                                              .ClickTo(xPos - 1, yPos)
                                              .ClickTo(xPos + 1, yPos)
                                              .ClickTo(xPos - 1, yPos + 1)
                                              .ClickTo(xPos, yPos + 1)
                                              .ClickTo(xPos + 1, yPos + 1),
                _ => _1
            } 
            select _2).Value.Map(a => a.ToCells()?
                                       .Filter((k, v) => v is Cell.Covered)
                                       .Exists((k, v) => v.IsBomb() is not true) ?? true ? a : new Win()).Value
            : x : x,

        var x => x
    };

    public MineField StartTo() => this switch
    {
        SetupWithBombsPos x => new Playing(x.Width, x.Height, Id(() =>
        {
            var q = new CellMap(from b in Enumerable.Range(0, x.Height)
                                from a in Enumerable.Range(0, x.Width)
                                select ((a, b), Cell.New()));

            return x.Bombs.Fold(q, (s, e) => s.AddOrUpdate(e, x => x.BombTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X - 1, e.Y - 1), x => x.AddNumberTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X , e.Y - 1), x => x.AddNumberTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X + 1, e.Y - 1), x => x.AddNumberTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X - 1, e.Y), x => x.AddNumberTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X + 1, e.Y), x => x.AddNumberTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X - 1, e.Y + 1), x => x.AddNumberTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X , e.Y + 1), x => x.AddNumberTo(), new Cell.Empty())
                                              .AddOrUpdate((e.X + 1, e.Y + 1), x => x.AddNumberTo(), new Cell.Empty()));
        }).Value()),

        Setup x => new SetupWithBombs(x.Width, x.Height, 0).StartTo(),

        SetupWithBombs x => Id(() =>
        {
            var bombPos = (from j in RandomGenerator(x.Height)
                           from i in RandomGenerator(x.Width)
                           select (i, j)).Distinct().Take(x.Bombs);

            return new SetupWithBombsPos(x.Width, x.Height, bombPos);

            static IEnumerable<int> RandomGenerator(int max)
            {
                while (true)
                {
                    yield return RandomNumberGenerator.GetInt32(max);
                }
            }
        }).Value().StartTo(),

        var x => x
    };
}

