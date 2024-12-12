# UnicodeEastAsianWidth

[![NuGet Version](https://img.shields.io/nuget/v/UnicodeEastAsianWidth)](https://www.nuget.org/packages/UnicodeEastAsianWidth)
[![License](https://img.shields.io/github/license/bdisp/UnicodeEastAsianWidth)](LICENSE)

A .NET library for determining East Asian Width properties of Unicode code points, based on the [Unicode East Asian Width](https://www.unicode.org/reports/tr11/) specification. This library is lightweight and efficient, suitable for text rendering and processing applications.

---

## Features

- Provides accurate East Asian Width categorization for Unicode code points.
- Includes detailed metadata for Unicode ranges, such as general categories and character names.
- Fully documented with XML comments for all public APIs.
- Lightweight and dependency-free.

---

## Installation

Install the package via NuGet:

```bash
dotnet add package UnicodeEastAsianWidth
```

## Usage

Here's a quick example of how to use the library:

```csharp
using UnicodeEastAsianWidth;

// Get the East Asian Width of a code point
var width = UnicodeEastAsianWidth.GetEastAsianWidth(0x4E00); // Example: 'CJK UNIFIED IDEOGRAPH-4E00'

// Get all entries for a specific width category
var fullWidthEntries = UnicodeEastAsianWidth.GetEntriesByWidth(EastAsianWidth.FullWidth);
```

## API Overview

- `GetEastAsianWidth(uint codePoint)`: Retrieves the East Asian Width category for a specific Unicode code point.
- `GetEntriesByWidth(EastAsianWidth width)`: Returns all Unicode ranges for a given East Asian Width category.

For more details, refer to the documentation.

## Development

#Prerequisites

- [.NET 6 or later](https://dotnet.microsoft.com/download/dotnet)
- Visual Studio 2022 or another IDE with .NET support.

#Building the Repository

1. Clone the repository:

```bash
git clone https://github.com/bdisp/UnicodeEastAsianWidth.git
cd UnicodeEastAsianWidth
```

2. Build the solution:

```bash
dotnet build
```

3. Run tests:

```bash
dotnet run --project UnicodeEastAsianWidth.Generator
```

## Generating EastAsianWidthData

To regenerate the EastAsianWidthData.cs file, ensure you have the latest Unicode data file (EastAsianWidth.txt) and then execute the generator project:

```bash
dotnet run --project UnicodeEastAsianWidth.Generator
```

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes. See the CONTRIBUTING.md file for guidelines.

## License

This project is licensed under the [MIT License](LICENSE.txt).

## Acknowledgments

This project uses data from the [Unicode Consortium}(https://www.unicode.org/).

# Unicode Category Discrepancy Analysis

This project compares the Unicode General Category values from two sources:

1. The .NET `UnicodeCategory` enumeration via `CharUnicodeInfo`.
2. The `EastAsianWidth.txt` file provided by the Unicode Consortium.

## Key Findings

- **Total mismatches:** 5,805 code points.
- These mismatches may arise due to:
  - Differences in Unicode standard versions.
  - Default values assigned by .NET for unrecognized or unassigned code points.
  - Updated classification in the latest Unicode standards reflected in `EastAsianWidth.txt`.

### Common Categories of Discrepancies

| **Category**         | **Count** | **Observation**                                                                 |
|-----------------------|-----------|---------------------------------------------------------------------------------|
| `OtherNotAssigned`    | High      | Many code points default to this in .NET but are assigned categories in Unicode.|
| `Mark, Nonspacing`    | Medium    | Seen for certain combining marks not fully recognized by .NET.                  |
| `PrivateUse`          | Low       | Variations in how private-use code points are treated.                          |

## Practical Implications

Developers using this library should be aware that `UnicodeCategory` values returned by .NET may not always align with the latest Unicode standard. For applications requiring strict adherence to Unicode classifications, this library provides accurate mappings derived from `EastAsianWidth.txt`.

## Mismatch Summary

### Patterns Observed

1. **`OtherNotAssigned` in .NET vs Specific Categories**:
   - Many combining marks (`Mn`) and control characters (`Cf`) are defaulted to `OtherNotAssigned` in .NET.

2. **Unassigned Code Points**:
   - .NET might classify unassigned code points incorrectly, whereas `EastAsianWidth.txt` includes future allocations.

3. **Private Use Area**:
   - .NET treats private use characters as `UnicodeCategory.PrivateUse`, but specific allocations might exist in `EastAsianWidth.txt`.

### Examples of Specific Mismatches

| **Code Point** | **Character**                       | **Expected**        | **Actual**               |
|----------------|-------------------------------------|---------------------|--------------------------|
| U+0897         | ARABIC PEPET                       | Mark, Nonspacing (Mn)| OtherNotAssigned         |
| U+1B4E         | BALINESE INVERTED CARIK SIKI       | Mark, Nonspacing (Mn)| OtherNotAssigned         |

## How to Analyze Further

To analyze mismatches in detail, the project includes a utility function that logs discrepancies to a file:

### Script Overview

The `UnicodeDiscrepancyAnalyzer.LogDiscrepancies` function iterates through all Unicode code points and compares their categories. Discrepancies are logged to a [CSV file](UnicodeEastAsianWidth.Tests/discrepancies.csv) for detailed analysis.

