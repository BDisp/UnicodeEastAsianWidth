using FluentAssertions;

namespace UnicodeEastAsianWidth.Tests;

public class EastAsianWidthTests
{
    [Fact]
    public void GetWidth_ShouldReturnCorrectWidth()
    {
        // Arrange
        uint codePoint = 0xFF5E; // FULLWIDTH TILDE

        // Act
        var width = UnicodeEastAsianWidth.GetEastAsianWidth(codePoint);

        // Assert
        Assert.Equal(EastAsianWidth.FullWidth, width);
    }

    [Fact]
    public void GetRange_ShouldReturnCorrectRange()
    {
        // Arrange
        uint codePoint = 0x0C92; // KANNADA LETTER O

        // Act
        var range = UnicodeEastAsianWidth.GetRange(codePoint);

        // Assert
        range?.Start.Should().Be(0x0C92);
        range?.End.Should().Be(0x0CA8);
        range?.Width.Should().Be(EastAsianWidth.Neutral);
        range?.GeneralCategory.Should().Be("Lo");
        range?.RangeLength.Should().Be(23);
        range?.StartName.Should().Be("KANNADA LETTER O");
        range?.EndName.Should().Be("KANNADA LETTER NA");
    }

    [Theory]
    [InlineData(0x0C85, EastAsianWidth.Neutral)] // KANNADA LETTER A
    [InlineData(0xFF5E, EastAsianWidth.FullWidth)] // FULLWIDTH TILDE
    [InlineData(0x3000, EastAsianWidth.FullWidth)] // IDEOGRAPHIC SPACE
    [InlineData(0x3001, EastAsianWidth.Wide)] // IDEOGRAPHIC COMMA
    public void GetWidth_ShouldHandleMultipleCodePointsCorrectly(uint codePoint, EastAsianWidth expectedWidth)
    {
        // Act
        var width = UnicodeEastAsianWidth.GetEastAsianWidth(codePoint);

        // Assert
        Assert.Equal(expectedWidth, width);
    }

    [Fact]
    public void GetEntriesByWidth_ShouldReturnMatchingEntries()
    {
        // Arrange
        var targetWidth = EastAsianWidth.Neutral; // Example width category
        var expectedEntries = EastAsianWidthData.Entries
            .Where(entry => entry.Width == targetWidth)
            .ToList();

        // Act
        var result = UnicodeEastAsianWidth.GetEntriesByWidth(targetWidth).ToList();

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(expectedEntries.Count, result.Count);

        for (int i = 0; i < expectedEntries.Count; i++)
        {
            Assert.Equal(expectedEntries[i].Start, result[i].Start);
            Assert.Equal(expectedEntries[i].End, result[i].End);
            Assert.Equal(expectedEntries[i].Width, result[i].Width);
        }
    }

    [Fact]
    public void GetEntriesByWidth_ShouldReturnEmptyForNonexistentWidth()
    {
        // Arrange
        var nonexistentWidth = (EastAsianWidth)(-1); // Invalid width category

        // Act
        var result = UnicodeEastAsianWidth.GetEntriesByWidth(nonexistentWidth);

        // Assert
        Assert.Empty(result);
    }
}
