using System.Linq;
using Xunit;
using CellMap = LanguageExt.HashMap<(int, int), Minesweeper.Cell>;

namespace Minesweeper.Tests;

public record MineFieldSpec
{
    [Fact]
    public void GenerateWorld()
    {
        MineField sut = new MineField.Setup(3, 3);
        var ret = sut.StartTo();

        Assert.Equal(9, ret.ToCells()?.Count);
    }

    [Fact]
    public void GenerateWorldWithBombs()
    {
        MineField sut = new MineField.SetupWithBombs(3, 3, 3);
        var ret = sut.StartTo();

        Assert.Equal(3, ret.ToCells()?.Where(x => x.Value.IsBomb()).Count());
    }
}
