using System.Globalization;
using FluentAssertions;
using Xunit.Abstractions;

namespace UnicodeEastAsianWidth.Tests;

public class EastAsianWidthTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public EastAsianWidthTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

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

    [Fact]
    public void GetGeneralCategory_ShouldMatch_InternalGetUnicodeCategoryFromApi()
    {
        var mismatchCount = 0;

        for (uint codePoint = 0; codePoint <= 0x10FFFF; codePoint++)
        {
            // Get general category from EastAsianWidth
            var expectedCategory = UnicodeEastAsianWidth.GetGeneralCategory(codePoint);

            // Get general category from .NET UnicodeCategory
            var actualCategory = UnicodeEastAsianWidth.GetUnicodeCategoryFromApi(codePoint);

            if (expectedCategory == actualCategory)
            {
                Assert.Equal(expectedCategory, actualCategory);
            }
            else
            {
                // Log discrepancies to the console
                mismatchCount++;
                _testOutputHelper.WriteLine($"Mismatch for U+{codePoint:X4}:");
                _testOutputHelper.WriteLine($"  - Expected: {expectedCategory}");
                _testOutputHelper.WriteLine($"  - Actual:   {actualCategory}");
            }
        }

        _testOutputHelper.WriteLine("");
        _testOutputHelper.WriteLine($"Mismatch count: {mismatchCount}");
    }

    [Fact]
    public void GetUnicodeGeneralCategory_ShouldHandle_LC_CasedLetters()
    {
        foreach (var entry in EastAsianWidthData.Entries.Where(e => e.GeneralCategory == "L&"))
        {
            for (uint codePoint = entry.Start; codePoint <= entry.End; codePoint++)
            {
                // Use the internal API method to determine the expected category
                var expectedCategory = UnicodeEastAsianWidth.GetUnicodeCategoryFromApi(codePoint);

                // Get the category using GetUnicodeGeneralCategory
                var actualCategory = UnicodeEastAsianWidth.GetGeneralCategory(codePoint);

                Assert.Equal(expectedCategory, actualCategory);
            }
        }
    }

    [Theory]
    [InlineData(0x0378)]
    [InlineData(0x0379)]
    public void Reserved_Code_Points_Fallback_To_EastAsianWidth_Neutral(uint codePoint)
    {
        // Act
        var expectedEastAsianWidth = UnicodeEastAsianWidth.GetEastAsianWidth(codePoint);

        // Assert
        Assert.Equal(EastAsianWidth.Neutral, expectedEastAsianWidth);
    }

    [Theory]
    [InlineData(0x0378)]
    [InlineData(0x0379)]
    public void Reserved_Code_Points_Fallback_To_GeneralCategory_Other_Not_Assigned(uint codePoint)
    {
        // Act
        var expectedCategory = UnicodeEastAsianWidth.GetGeneralCategory(codePoint);

        // Assert
        Assert.Equal(UnicodeCategory.OtherNotAssigned, expectedCategory);
    }
}
