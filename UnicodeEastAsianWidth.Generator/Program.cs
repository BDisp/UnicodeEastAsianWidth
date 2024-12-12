using UnicodeEastAsianWidth.Generator;

public static class Program
{
    private const string FileUrl = $"https://www.unicode.org/Public/UCD/latest/ucd/{InputFileName}";
    private const string InputFileName = "EastAsianWidth.txt";
    private const string OutputFileName = "EastAsianWidthData.cs";

    public static async Task Main(string[] args)
    {
        var projectDirectory = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName!;
        var inputFilePath = Path.Combine(projectDirectory, InputFileName);

        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("Downloading file...");
            await DownloadFileAsync(FileUrl, inputFilePath);
        }

        Console.WriteLine($"Processing file: {inputFilePath}");

        // Parse the file
        var entries = UnicodeEastAsianWidthParser.ParseFile(inputFilePath);

        Console.WriteLine($"Parsed {entries.Count} entries.");

        // Generate the code
        var code = UnicodeEastAsianWidthCodeGenerator.GenerateCode(entries);

        var outputPath = Path.Combine(projectDirectory, OutputFileName);

        // Write the generated code to the file
        await File.WriteAllTextAsync(outputPath, code);
        Console.WriteLine($"C# code generated at {outputPath}");
    }

    private static async Task DownloadFileAsync(string url, string filePath)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
        await response.Content.CopyToAsync(fileStream);
    }
}