using System.Collections.Generic;
using System.Linq;

namespace UnicodeEastAsianWidth;

/// <summary>
/// Provides methods to retrieve and analyze Unicode East Asian Width properties.
/// </summary>
/// <remarks>
/// The Unicode East Asian Width property is used to determine the width of characters in certain contexts,
/// such as terminal emulators or text rendering. This class includes methods to:
/// <list type="bullet">
/// <item><description>Retrieve the East Asian Width for a specific code point.</description></item>
/// <item><description>Filter entries by their East Asian Width category.</description></item>
/// <item><description>Get detailed information about a range of code points.</description></item>
/// </list>
/// Data is based on the Unicode Consortium's EastAsianWidth.txt file.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// var width = UnicodeEastAsianWidth.GetEastAsianWidth(0x4E00); // Retrieves the width of '一'
/// var entries = UnicodeEastAsianWidth.GetEntriesByWidth(EastAsianWidth.W); // Retrieves all wide characters
/// var range = UnicodeEastAsianWidth.GetRange(0xFF5E); // Retrieves the entry for FULLWIDTH TILDE
/// </code>
/// </example>
public static class UnicodeEastAsianWidth
{
    /// <summary>
    /// Retrieves the East Asian Width category for the specified code point.
    /// </summary>
    /// <param name="codePoint">The Unicode code point to search for.</param>
    /// <returns>
    /// The <see cref="EastAsianWidth"/> category for the code point,
    /// or <c>null</c> if the code point is not found in any range.
    /// </returns>
    public static EastAsianWidth? GetEastAsianWidth(uint codePoint)
    {
        return EastAsianWidthData.Entries
            .FirstOrDefault(entry => codePoint >= entry.Start && codePoint <= entry.End)
            ?.Width;
    }

    /// <summary>
    /// Retrieves all entries matching the specified East Asian Width category.
    /// </summary>
    /// <param name="width">The East Asian Width category to filter by.</param>
    /// <returns>
    /// A collection of <see cref="EastAsianWidthData.Entry"/> instances
    /// representing the matching ranges.
    /// </returns>
    /// <example>
    /// <code>
    /// var entries = UnicodeEastAsianWidth.GetEntriesByWidth(EastAsianWidth.N);
    /// foreach (var entry in entries)
    /// {
    ///     Console.WriteLine($"Start: {entry.Start}, End: {entry.End}, Width: {entry.Width}");
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<EastAsianWidthData.Entry> GetEntriesByWidth(EastAsianWidth width)
    {
        return EastAsianWidthData.Entries
            .Where(entry => entry.Width == width);
    }

    /// <summary>
    /// Retrieves the entry range that includes the specified code point.
    /// </summary>
    /// <param name="codePoint">The Unicode code point to search for.</param>
    /// <returns>
    /// The <see cref="EastAsianWidthData.Entry"/> representing the range that includes the code point,
    /// or <c>null</c> if no matching range is found.
    /// </returns>
    /// <example>
    /// <code>
    /// var entry = UnicodeEastAsianWidth.GetRange(0x4E00);
    /// if (entry != null)
    /// {
    ///     Console.WriteLine($"Start: {entry.Start}, End: {entry.End}, Width: {entry.Width}");
    /// }
    /// </code>
    /// </example>
    public static EastAsianWidthData.Entry? GetRange(uint codePoint)
    {
        return EastAsianWidthData.Entries
            .FirstOrDefault(entry => codePoint >= entry.Start && codePoint <= entry.End);
    }
}
