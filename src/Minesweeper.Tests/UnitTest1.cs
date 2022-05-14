using Xunit;

namespace Minesweeper.Tests;

public class UnitTest1
{
    [Fact]
    public void BombSpec()
    {
        var sut = new Cell.Bomb();
        var ret = sut.ToChar();

        Assert.Equal('*', ret);
    }
}
