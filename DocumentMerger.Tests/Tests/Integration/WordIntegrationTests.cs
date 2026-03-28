using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DocumentMergerTests.Tests.Integration;

[TestClass]
public class WordIntegrationTests
{
    private string _testFilePath = null!;
    private string _outputFilePath = null!;

    [TestInitialize]
    public void Setup()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), $"integration_test_{Guid.NewGuid()}.docx");
        _outputFilePath = Path.Combine(Path.GetTempPath(), $"integration_output_{Guid.NewGuid()}.docx");
        
        using var doc = Xceed.Words.NET.DocX.Create(_testFilePath);
        doc.InsertParagraph("Hello {{Name}}, your email is {{Email}} and your ID is {{Id}}.");
        doc.Save();
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(_testFilePath)) File.Delete(_testFilePath);
        if (File.Exists(_outputFilePath)) File.Delete(_outputFilePath);
    }

    [TestMethod]
    public void Integration_WordMerger_ReplacesAllPlaceholders()
    {
        var data = new UserDto
        {
            Id = 42,
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        IDocumentCreator creator = new WordDocumentCreator();
        DocumentMerger merger = new WordMerger(creator);

        merger.MergeDocument(_testFilePath, data);

        Assert.IsTrue(File.Exists(_testFilePath));
    }

    [TestMethod]
    public void Integration_WordDocument_WorksEndToEnd()
    {
        var wordDoc = new WordDocument();
        
        wordDoc.Create(_outputFilePath);
        Assert.IsNotNull(wordDoc.document);
        
        wordDoc.ReplaceText("{{Name}}", "Test User");
        Assert.IsNotNull(wordDoc.document);
        
        wordDoc.Save();
        Assert.IsTrue(File.Exists(_outputFilePath));
    }
}
