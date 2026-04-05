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

## Contributing

We welcome contributions from the community! Whether you're fixing bugs, adding features, improving documentation, or reporting issues, your help makes DocumentMerger better for everyone.

### Getting Started

1. **Fork and Clone**: Fork the repository and clone it to your local machine.
2. **Set Up**: The project uses **.NET 8**. Ensure you have the latest .NET SDK installed.
3. **Build**: Run `dotnet build` in the project root to compile the solution.
4. **Run Tests**: Execute `dotnet test` to run the test suite and ensure everything works correctly.

### Ways to Contribute

#### 🧪 Running and Improving Tests

- **Run existing tests** to verify your environment is set up correctly: `dotnet test`
- **Expand test coverage**: Add tests in `DocumentMerger.Tests/` for edge cases, new features, or untested code paths
- **Integration tests**: Create new integration tests in the `Tests/Integration/` directory to validate real-world workflows with Word and PDF documents
- **Fix flaky tests**: Identify and stabilize any intermittent test failures

#### 🐛 Reporting and Fixing Bugs

- **Discover bugs**: Test the CLI tool with various document formats, DTO types, and edge cases
  ```bash
  dotnet run --project DocumentMerger.CLI/DocumentMerger.CLI.csproj -- template.docx output.pdf --data data.json
  ```
- **Report issues**: Open an issue on GitHub describing the problem, expected behavior, and steps to reproduce
- **Fix bugs**: Pick an open issue, create a fix, and submit a pull request with a test that covers the bug

#### ✨ Suggesting and Implementing Features

- **Request features**: Open a GitHub issue with a detailed description of your idea
- **Implement features**: Some potential areas for enhancement:
  - Support for additional document formats (PowerPoint, Excel, etc.)
  - New DTO types beyond UserDto and AddressDto
  - Enhanced placeholder replacement strategies
  - Better error handling and validation
  - CLI improvements (new options, better help messages)
  - Performance optimizations

#### 📖 Improving Documentation

- **Update README.md**: Clarify existing instructions or add examples
- **Code comments**: Add documentation to complex logic in the DocumentMerger/ library
- **Usage examples**: Create guides for common workflows or integration patterns
- **Troubleshooting guide**: Document common issues and solutions

#### 💬 Code Review and Feedback

- **Review pull requests**: Test and provide constructive feedback on open PRs
- **Suggest improvements**: Comment on code quality, design patterns, or best practices
- **Discuss design decisions**: Participate in GitHub discussions about the project direction

### Development Workflow

1. Create a branch for your changes: `git checkout -b feature/your-feature-name`
2. Make your changes and ensure all tests pass:
   ```bash
   dotnet test
   ```
3. Commit with clear messages: Use meaningful commit messages that describe your changes
4. Push and create a pull request: Include a description of your changes and any relevant issue numbers
5. Address feedback: Be responsive to code review comments and iterate on your changes

### Testing Guidelines

- Unit tests use MSTest framework
- Mock objects are in `DocumentMerger.Tests/Mocks/`
- Each test class should be focused on a single component:
  - `MergerTests.cs` - Core merge functionality
  - `DocumentFacadeTests.cs` - Word and PDF document operations
  - `AddressDtoTests.cs` / `UserDtoTests.cs` - DTO serialization
  - `*IntegrationTests.cs` - End-to-end workflows
- Write tests that are independent, repeatable, and clear

### Project Structure

```
DocumentMerger/
├── DocumentMerger/           # Core library
├── DocumentMerger.CLI/       # Command-line tool
└── DocumentMerger.Tests/     # Test suite
    ├── Mocks/               # Mock implementations
    ├── Tests/
    │   ├── Dto/            # DTO tests
    │   ├── Document/       # Document facade tests
    │   ├── Merger/         # Core merger tests
    │   └── Integration/    # End-to-end tests
```

### Questions or Need Help?

- Open an issue with the question label
- Check existing issues for similar topics
- Review the CLI usage section in the main README
