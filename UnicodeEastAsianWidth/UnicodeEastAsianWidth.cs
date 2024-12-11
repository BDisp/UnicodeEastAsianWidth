using System.Collections.Generic;
using System.Linq;

namespace UnicodeEastAsianWidth;

public static class UnicodeEastAsianWidth
{
        /// <summary>
        /// Retrieves the East Asian Width for the specified code point.
        /// </summary>
        /// <param name="codePoint">The Unicode code point.</param>
        /// <returns>The East Asian Width and metadata, or null if not found.</returns>
        public static EastAsianWidthData.Entry? GetEastAsianWidth(uint codePoint)
        {
            return EastAsianWidthData.Entries
                .FirstOrDefault(entry => codePoint >= entry.Start && codePoint <= entry.End);
        }

        /// <summary>
        /// Retrieves all entries for a specific East Asian Width category.
        /// </summary>
        /// <param name="width">The East Asian Width category.</param>
        /// <returns>A list of matching entries.</returns>
        public static IEnumerable<EastAsianWidthData.Entry> GetEntriesByWidth(EastAsianWidth width)
        {
            return EastAsianWidthData.Entries
                .Where(entry => entry.Width == width);
        }
    }