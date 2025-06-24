using KegiFin.Core.Common.Extensions;

namespace KegiFin.Tests.Unit.Core.Common.Extensions;

public class DateTimeExtensionTests
{
    [Theory]
    [InlineData(2023, 6, 18, 2023, 6, 1)]
    [InlineData(2024, 2, 15, 2024, 2, 1)]
    [InlineData(2020, 12, 31, 2020, 12, 1)]
    public void GetFirstDay_ShouldReturnFirstDayOfMonth(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
    {
        var date = new DateTime(year, month, day);

        var firstDay = date.GetFirstDay();

        Assert.Equal(expectedYear, firstDay.Year);
        Assert.Equal(expectedMonth, firstDay.Month);
        Assert.Equal(expectedDay, firstDay.Day);
    }
    
    [Theory]
    [InlineData(2023, 6, 18, 2023, 6, 30)]
    [InlineData(2024, 2, 15, 2024, 2, 29)]  // Leap year
    [InlineData(2023, 2, 15, 2023, 2, 28)]  // Non-leap year
    public void GetLastDay_ShouldReturnLastDayOfMonth(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
    {
        var date = new DateTime(year, month, day);

        var lastDay = date.GetLastDay();

        Assert.Equal(expectedYear, lastDay.Year);
        Assert.Equal(expectedMonth, lastDay.Month);
        Assert.Equal(expectedDay, lastDay.Day);
    }
    
    [Fact]
    public void GetFirstDay_WithYearAndMonthParameters_ShouldUseParameters()
    {
        var date = new DateTime(2023, 6, 18);

        var firstDay = date.GetFirstDay(2020, 12);

        Assert.Equal(2020, firstDay.Year);
        Assert.Equal(12, firstDay.Month);
        Assert.Equal(1, firstDay.Day);
    }

    [Fact]
    public void GetLastDay_WithYearAndMonthParameters_ShouldUseParameters()
    {
        var date = new DateTime(2023, 6, 18);

        var lastDay = date.GetLastDay(2020, 2);

        Assert.Equal(2020, lastDay.Year);
        Assert.Equal(2, lastDay.Month);
        Assert.Equal(29, lastDay.Day); // Leap year
    }
    
    [Theory]
    [InlineData(2025, 0, "GetFirstDay")]
    [InlineData(2025, 13, "GetFirstDay")]
    [InlineData(2025, 0, "GetLastDay")]
    [InlineData(2025, 13, "GetLastDay")]
    public void DateTimeExtension_Methods_ShouldFailForInvalidMonth(int year, int month, string methodName)
    {
        var date = DateTime.Now;

        Action act = methodName switch
        {
            "GetFirstDay" => () => date.GetFirstDay(year, month),
            "GetLastDay" => () => date.GetLastDay(year, month),
            _ => throw new ArgumentException("Invalid method name for test", nameof(methodName))
        };

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }
}