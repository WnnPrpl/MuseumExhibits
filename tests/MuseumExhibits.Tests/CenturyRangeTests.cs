namespace MuseumExhibits.Tests;

public class CenturyRangeTests
{
    private static (int Min, int Max) GetRange(int century)
    {
        int min = century > 0 ? (century - 1) * 100 + 1 : century * 100;
        int max = century > 0 ? century * 100             : (century + 1) * 100 - 1;
        return (min, max);
    }

    [Theory]
    [InlineData(1,   1,   100)]
    [InlineData(5,   401, 500)]
    [InlineData(20,  1901, 2000)]
    public void PositiveCentury_CorrectRange(int century, int expectedMin, int expectedMax)
    {
        var (min, max) = GetRange(century);
        Assert.Equal(expectedMin, min);
        Assert.Equal(expectedMax, max);
    }

    [Theory]
    [InlineData(-1,  -100, -1)]
    [InlineData(-5,  -500, -401)]
    [InlineData(-10, -1000, -901)]
    public void NegativeCentury_CorrectRange(int century, int expectedMin, int expectedMax)
    {
        var (min, max) = GetRange(century);
        Assert.Equal(expectedMin, min);
        Assert.Equal(expectedMax, max);
    }

    [Theory]
    [InlineData(1,  50,  true)]
    [InlineData(1,  101, false)]
    [InlineData(-5, -450, true)]
    [InlineData(-5, -600, false)]
    public void YearFallsInCentury(int century, int year, bool expected)
    {
        var (min, max) = GetRange(century);
        Assert.Equal(expected, year >= min && year <= max);
    }
}
