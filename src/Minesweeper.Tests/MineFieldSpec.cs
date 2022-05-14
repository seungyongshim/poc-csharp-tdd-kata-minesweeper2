using System.Linq;
using Xunit;

namespace Minesweeper.Tests;

public record MineFieldSpec
{
    [Fact]
    public void GenerateWorld()
    {
        var sut = new MineField.Setup(3, 3);
        var ret = sut.StartTo();

        Assert.Equal(9, ret.ToCells()?.Count);
    }

    [Fact]
    public void GenerateWorldWithBombs()
    {
        var sut = new MineField.SetupWithBombs(3, 3, 3);
        var ret = sut.StartTo();

        Assert.Equal(3, ret.ToCells()?.Where(x => x.Value.IsBomb()).Count());
    }

    [Fact]
    public void GenerateWorldWithBombPos()
    {
        var sut = new MineField.SetupWithBombs1(3, 3, new[] { (0, 0), (0, 2) });
        var ret = sut.StartTo();

        Assert.Equal("*2*121000", ret.ToInnerStr());
    }
}
