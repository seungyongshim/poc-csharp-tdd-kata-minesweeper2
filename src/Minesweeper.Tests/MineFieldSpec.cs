using System;
using Xunit;
using CellMap = LanguageExt.HashMap<(int, int), Minesweeper.ICell>;

namespace Minesweeper.Tests;

public record MineFieldSpec
{
    [Fact]
    public void GenerateWorld()
    {
        IMineField sut = new IMineField.Setup(3, 3);
        var ret = sut.ClickTo(0, 0);

        Assert.Equal(ret.ToCells().Case switch
        {
            null => throw new Exception(),
            CellMap x => x.Length
        }, 9);
    }
}
