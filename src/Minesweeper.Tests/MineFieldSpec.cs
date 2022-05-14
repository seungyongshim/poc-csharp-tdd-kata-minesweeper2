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
        var sut = new MineField.SetupWithBombsPos(3, 3, new[] { (0, 0), (0, 2) });
        var ret = sut.StartTo();

        Assert.Equal("*2*121000", ret.ToInnerStr());
    }

    [Fact]
    public void GenerateWorldAndClickToLoose()
    {
        var sut = new MineField.SetupWithBombsPos(3, 3, new[] { (0, 0) });
        var ret = sut.ClickTo(0, 0);

        Assert.IsType<MineField.Loose>(ret);
    }

    [Fact]
    public void GenerateWorldAndClickToWin()
    {
        var sut = new MineField.SetupWithBombsPos(3, 3, new[] { (2, 2) });
        var ret = sut.ClickTo(0, 0);

        Assert.IsType<MineField.Win>(ret);
    }

    [Fact]
    public void GenerateWorldAndClick()
    {
        var sut = new MineField.SetupWithBombsPos(3, 3, new[] { (1, 2), (2, 1) });
        var ret = sut.ClickTo(0, 0);

        Assert.Equal("01.12....", ret.ToStr());
    }

}
