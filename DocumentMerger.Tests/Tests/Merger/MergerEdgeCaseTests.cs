using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DocumentMergerTests.Tests.Merger;

[TestClass]
public class MergerEdgeCaseTests
{
    private StringWriter _consoleOutput = null!;
    private TextWriter _originalOutput = null!;

    [TestInitialize]
    public void Setup()
    {
        _originalOutput = Console.Out;
        _consoleOutput = new StringWriter();
        Console.SetOut(_consoleOutput);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Console.SetOut(_originalOutput);
    }

    [TestMethod]
    public void WordMerger_LoadDocument_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        var creator = new WordDocumentCreator();
        var merger = new WordMerger(creator);
        var nonExistentPath = Path.Combine(Path.GetTempPath(), "non_existent.docx");
        var data = new UserDto { Id = 1, Name = "Test", Email = "test@test.com" };

        Assert.ThrowsException<FileNotFoundException>(() => merger.MergeDocument(nonExistentPath, "output.docx", data));
    }

    [TestMethod]
    public void PDFMerger_ReplacePlaceholdersWithDictonary_ExecutesSuccessfully()
    {
        var creator = new PDFDocumentCreator();
        var pdfMerger = new PDFMerger(creator);
        var data = new UserDto { Id = 1, Name = "Test", Email = "test@test.com" };
        var dict = data.ToDictionary();

        pdfMerger.ReplacePlaceholdersWithDictonary(creator.CreateDocumentObject(), dict!, "output.pdf");

        var output = _consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Replacing placeholder using a dictonary"));
    }

    [TestMethod]
    public void MergeDocument_WithEmptyDictionary_ExecutesSuccessfully()
    {
        var testDoc = new Mocks.TestDocumentFacade();
        var testCreator = new Mocks.TestDocumentCreator();
        var merger = new Mocks.TestableMerger(testCreator);
        var data = new UserDto { Id = 0, Name = null, Email = null };

        merger.MergeDocument("test.docx", "output.docx", data);

        Assert.IsNotNull(testCreator.Document.LoadedPath);
    }

    [TestMethod]
    public void MergeDocument_WithNullProperties_ExecutesSuccessfully()
    {
        var testDoc = new Mocks.TestDocumentFacade();
        var testCreator = new Mocks.TestDocumentCreator();
        var merger = new Mocks.TestableMerger(testCreator);
        var data = new AddressDto
        {
            Street = "123 Main St",
            City = null,
            State = null,
            ZipCode = null
        };

        merger.MergeDocument("test.docx", "output.docx", data);

        Assert.IsTrue(testCreator.Document.Replacements.Count >= 1);
        Assert.IsTrue(testCreator.Document.SaveCalled);
    }

    [TestMethod]
    public void MergeDocument_WithSpecialCharactersInValues_HandlesGracefully()
    {
        var testCreator = new Mocks.TestDocumentCreator();
        var merger = new Mocks.TestableMerger(testCreator);
        var data = new UserDto
        {
            Id = 1,
            Name = "John \"The Boss\" Doe",
            Email = "john@example.com"
        };

        merger.MergeDocument("test.docx", "output.docx", data);

        Assert.IsTrue(testCreator.Document.Replacements.Count >= 3);
    }

    [TestMethod]
    public void MergeDocument_WithPlaceholderBraces_ReplacesCorrectly()
    {
        var testCreator = new Mocks.TestDocumentCreator();
        var merger = new Mocks.TestableMerger(testCreator);
        var data = new UserDto
        {
            Id = 1,
            Name = "Test User",
            Email = "test@test.com"
        };

        merger.MergeDocument("test.docx", "output.docx", data);

        Assert.IsTrue(testCreator.Document.Replacements.Any(r => 
            r.placeholder.Contains("{{") && r.placeholder.Contains("}}")));
    }
}
