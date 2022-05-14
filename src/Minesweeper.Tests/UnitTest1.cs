using Xunit;
using static Minesweeper.Cell;

namespace Minesweeper.Tests;

public class UnitTest1
{
    [Fact]
    public void BombSpec()
    {
        var sut = new Bomb();
        var ret = sut.ToChar();

        Assert.Equal('*', ret);
    }

    [Fact]
    public void NumberSpec()
    {
        var sut = new Number(1);
        var ret = sut.ToChar();

        Assert.Equal('0', ret);
    }
}
