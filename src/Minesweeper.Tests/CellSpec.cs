using Xunit;
using static Minesweeper.ICell;

namespace Minesweeper.Tests;

public record CellSpec
{
    [Fact]
    public void BombSpec()
    {
        ICell sut = new Bomb();
        var ret = sut.ToChar();

        Assert.Equal('*', ret);
    }

    [Fact]
    public void NumberSpec()
    {
        ICell sut = new Number(1);
        var ret = sut.ToChar();

        Assert.Equal('1', ret);
    }

    [Fact]
    public void CoveredSpec()
    {
        ICell sut = new Covered(new Number(0));
        var ret = sut.ToChar();

        Assert.Equal('.', ret);
    }

    [Fact]
    public void ClickSpec()
    {
        ICell sut = new Covered(new Number(1));
        var ret = sut.Click().ToChar();

        Assert.Equal('1', ret);
    }
}
