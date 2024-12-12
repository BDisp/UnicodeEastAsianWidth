namespace UnicodeEastAsianWidth.Tests;

public static class UnicodeDiscrepancyAnalyzer
{
    private static void LogDiscrepancies(string filePath)
    {
        using var writer = new StreamWriter(filePath);
        writer.WriteLine("CodePoint,ExpectedCategory,ActualCategory");

        int mismatchCount = 0;

        foreach (uint codePoint in Enumerable.Range(0, 0x10FFFF).Select(i => (uint)i))
        {
            var expectedCategory = UnicodeEastAsianWidth.GetGeneralCategory(codePoint);
            var actualCategory = UnicodeEastAsianWidth.GetUnicodeCategoryFromApi(codePoint);

            if (expectedCategory != actualCategory)
            {
                writer.WriteLine($"U+{codePoint:X4},{expectedCategory},{actualCategory}");
                mismatchCount++;
            }
        }

        writer.WriteLine($"Total mismatches: {mismatchCount}");
    }

    [Fact]
    public static void Run_LogDiscrepancies()
    {
        // Arrange
        var projectDirectory = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName!;
        var outputFile = Path.Combine(projectDirectory, "discrepancies.csv");

        // Act
        LogDiscrepancies(outputFile);

        // Assert
        Assert.True(File.Exists(outputFile));
    }
}
