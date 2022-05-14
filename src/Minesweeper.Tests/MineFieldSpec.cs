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
        var ret = sut.StartTo();

        Assert.Equal(ret.ToCells().Case switch
        {
            CellMap x => x.Length
        }, 9);
    }

    [Fact]
    public void GenerateWorldWithBombs()
    {
        IMineField sut = new IMineField.SetupWithBombs(3, 3, 3);
        var ret = sut.StartTo();
    }
}
