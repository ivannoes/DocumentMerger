using System.Reflection;
using Newtonsoft.Json.Linq;

namespace DocumentMergerCli;

class Program
{
    static int Main(string[] args)
    {
        string? inputPath = null;
        string? outputPath = null;
        string? dataPath = null;
        string? explicitType = null;

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (arg == "--input")
            {
                if (inputPath != null)
                {
                    Error("Input specified twice. Use either --input or positional argument, not both.");
                    return 1;
                }
                if (i + 1 >= args.Length)
                {
                    Error("Error: --input requires a value.");
                    return 1;
                }
                inputPath = args[++i];
            }
            else if (arg == "--output")
            {
                if (outputPath != null)
                {
                    Error("Output specified twice. Use either --output or positional argument, not both.");
                    return 1;
                }
                if (i + 1 >= args.Length)
                {
                    Error("Error: --output requires a value.");
                    return 1;
                }
                outputPath = args[++i];
            }
            else if (arg == "--data")
            {
                if (dataPath != null)
                {
                    Error("Data file specified twice.");
                    return 1;
                }
                if (i + 1 >= args.Length)
                {
                    Error("Error: --data requires a value.");
                    return 1;
                }
                dataPath = args[++i];
            }
            else if (arg == "--type")
            {
                if (explicitType != null)
                {
                    Error("DTO type specified twice.");
                    return 1;
                }
                if (i + 1 >= args.Length)
                {
                    Error("Error: --type requires a value.");
                    return 1;
                }
                explicitType = args[++i];
            }
            else if (!arg.StartsWith("-"))
            {
                if (inputPath == null)
                    inputPath = arg;
                else if (outputPath == null)
                    outputPath = arg;
                else
                {
                    Error($"Unexpected argument: {arg}");
                    return 1;
                }
            }
            else
            {
                Error($"Unknown flag: {arg}");
                return 1;
            }
        }

        if (inputPath == null)
        {
            Error("Input template file is required. Use --input or provide as first argument.\n" +
                  "Usage: mergedoc <input> [output] --data <json-file> [--type <DtoType>]");
            return 1;
        }

        if (dataPath == null)
        {
            Error("Data file is required. Use --data.\n" +
                  "Usage: mergedoc <input> [output] --data <json-file> [--type <DtoType>]");
            return 1;
        }

        if (!File.Exists(inputPath))
        {
            Error($"File not found: {inputPath}");
            return 1;
        }

        if (!File.Exists(dataPath))
        {
            Error($"Data file not found: {dataPath}");
            return 1;
        }

        string jsonContent;
        try
        {
            jsonContent = File.ReadAllText(dataPath);
        }
        catch (Exception ex)
        {
            Error($"Could not read data file: {ex.Message}");
            return 1;
        }

        JObject? jsonData;
        try
        {
            jsonData = JObject.Parse(jsonContent);
        }
        catch (Exception ex)
        {
            Error($"Invalid JSON in file '{dataPath}': {ex.Message}");
            return 1;
        }

        var extension = Path.GetExtension(inputPath).ToLowerInvariant();
        bool isWord = extension == ".docx";
        bool isPdf = extension == ".pdf";

        if (!isWord && !isPdf)
        {
            Error($"Unsupported file format: {extension}. Use .docx or .pdf");
            return 1;
        }

        var dtoType = DetermineDtoType(jsonData, explicitType);
        if (dtoType == null)
            return 1;

        outputPath ??= "output-document.pdf";

        Console.WriteLine($"Input: {inputPath}");
        Console.WriteLine($"Output: {outputPath}");
        Console.WriteLine($"Format: {(isWord ? "Word" : "PDF")}");
        Console.WriteLine($"DTO Type: {dtoType.Name}");

        try
        {
            var dto = Activator.CreateInstance(dtoType);
            if (dto == null)
            {
                Error("Failed to create DTO instance.");
                return 1;
            }

            foreach (var prop in dtoType.GetProperties())
            {
                var jsonValue = jsonData[prop.Name];
                if (jsonValue != null)
                {
                    var value = jsonValue.ToObject(prop.PropertyType);
                    prop.SetValue(dto, value);
                }
            }

            IDocumentCreator creator = isWord
                ? new WordDocumentCreator()
                : new PDFDocumentCreator();

            DocumentMerger merger = isWord
                ? new WordMerger(creator)
                : new PDFMerger(creator);

            merger.MergeDocument(inputPath, outputPath, (DtoGeneric)dto);

            Console.WriteLine("Document merged successfully!");
            return 0;
        }
        catch (Exception ex)
        {
            Error($"Error during merge: {ex.Message}");
            return 1;
        }
    }

    static Type? DetermineDtoType(JObject jsonData, string? explicitType)
    {
        var dtoTypes = Assembly.GetAssembly(typeof(DtoGeneric))!
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(DtoGeneric)) && !t.IsAbstract)
            .ToList();

        if (explicitType != null)
        {
            var matched = dtoTypes.FirstOrDefault(t =>
                t.Name.Equals(explicitType, StringComparison.OrdinalIgnoreCase));
            if (matched == null)
            {
                var available = string.Join(", ", dtoTypes.Select(t => t.Name));
                Error($"Unknown DTO type: {explicitType}. Available: {available}");
                return null;
            }
            return matched;
        }

        var matchingTypes = dtoTypes.Where(dtoType =>
        {
            var props = dtoType.GetProperties();
            return props.All(prop => jsonData.ContainsKey(prop.Name));
        }).ToList();

        if (matchingTypes.Count == 0)
        {
            Error("Could not determine DTO type from JSON data. Please specify with --type.\n" +
                  $"Available types: {string.Join(", ", dtoTypes.Select(t => t.Name))}");
            return null;
        }

        if (matchingTypes.Count > 1)
        {
            Error("Ambiguous DTO type. Multiple types match. Please specify with --type.\n" +
                  $"Matching types: {string.Join(", ", matchingTypes.Select(t => t.Name))}");
            return null;
        }

        return matchingTypes[0];
    }

    static void Error(string message)
    {
        Console.Error.WriteLine(message);
    }
}
