# Document Merger
A small, focused .NET 8 library for merging DTOs into document templates (PDF, Word, etc.). The project uses a factory pattern to produce document objects and a template-method `MergeDocument` to drive the merge workflow so new formats can be added without changing the high-level logic.

## CLI Usage

The CLI tool merges data from a JSON file into a document template.

### Usage

```bash
dotnet run --project DocumentMerger.CLI/DocumentMerger.CLI.csproj -- [options]
```

Or use the provided `run.bat` helper:

```bash
DocumentMerger.CLI\run.bat [options]
```

### Options

| Option | Description | Required |
|--------|-------------|----------|
| `<input>` | Input template file (.docx or .pdf) | Yes |
| `<output>` | Output file path | No (defaults to `output-document.pdf`) |
| `--data <file>` | JSON file containing merge data | Yes |
| `--type <DtoType>` | Explicit DTO type (UserDto, AddressDto) | No (auto-detected) |
| `--input <file>` | Input template file (alternative syntax) | Yes |
| `--output <file>` | Output file path (alternative syntax) | No |

### Examples

**Basic usage with auto-detected DTO:**
```bash
run.bat template.docx output.docx --data user.json
```

**Using explicit DTO type:**
```bash
run.bat template.docx output.docx --data user.json --type UserDto
```

**Using named flags:**
```bash
run.bat --input template.docx --output output.docx --data user.json
```

**PDF output:**
```bash
run.bat template.docx output.pdf --data user.json
```

**JSON data format:**
```json
{
  "Id": 1,
  "Name": "John Doe",
  "Email": "john@example.com"
}
```

Available DTO types: `UserDto`, `AddressDto`
