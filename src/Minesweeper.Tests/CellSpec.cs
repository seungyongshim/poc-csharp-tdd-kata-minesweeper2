using Xunit;
using static Minesweeper.Cell;

namespace Minesweeper.Tests;

public record CellSpec
{
    [Fact]
    public void BombSpec()
    {
        Cell sut = new Bomb();
        var ret = sut.ToChar();

        Assert.Equal('*', ret);
    }

    [Fact]
    public void NumberSpec()
    {
        Cell sut = new Number(1);
        var ret = sut.ToChar();

        Assert.Equal('1', ret);
    }

    [Fact]
    public void CoveredSpec()
    {
        Cell sut = new Covered(new Number(0));
        var ret = sut.ToChar();

        Assert.Equal('.', ret);
    }

    [Fact]
    public void ClickSpec()
    {
        Cell sut = new Covered(new Number(1));
        var ret = sut.ClickTo().ToChar();

        Assert.Equal('1', ret);
    }
}
