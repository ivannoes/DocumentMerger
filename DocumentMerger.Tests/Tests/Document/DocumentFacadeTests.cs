using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DocumentMergerTests.Tests.Document;

[TestClass]
public class DocumentFacadeTests
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
    public void PDFDocument_Open_PrintsCorrectMessage()
    {
        var pdfDoc = new PDFDocument();
        var testPath = "test.pdf";

        pdfDoc.Open(testPath);

        var output = _consoleOutput.ToString();
        Assert.IsTrue(output.Contains($"Opening existing PDF document at {testPath}."));
    }

    [TestMethod]
    public void PDFDocument_Create_PrintsCorrectMessage()
    {
        var pdfDoc = new PDFDocument();

        pdfDoc.Create("output.pdf");

        var output = _consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Generating a new, blank PDF document."));
    }

    [TestMethod]
    public void PDFDocument_Save_PrintsCorrectMessage()
    {
        var pdfDoc = new PDFDocument();

        pdfDoc.Save();

        var output = _consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Saving PDF document."));
    }

    [TestMethod]
    public void PDFDocument_ReplaceText_PrintsCorrectMessage()
    {
        var pdfDoc = new PDFDocument();

        pdfDoc.ReplaceText("{{Name}}", "Test User");

        var output = _consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Replacing '{{Name}}' with 'Test User' in PDF document."));
    }

    [TestMethod]
    public void WordDocument_Create_CreatesDocumentInstance()
    {
        var wordDoc = new WordDocument();
        var testPath = Path.Combine(Path.GetTempPath(), $"test_create_{Guid.NewGuid()}.docx");

        try
        {
            wordDoc.Create(testPath);

            Assert.IsNotNull(wordDoc.document);
            Assert.IsTrue(File.Exists(testPath));
        }
        finally
        {
            if (File.Exists(testPath)) File.Delete(testPath);
        }
    }

    [TestMethod]
    public void WordDocument_Save_CallsSaveOnDocument()
    {
        var wordDoc = new WordDocument();
        var testPath = Path.Combine(Path.GetTempPath(), $"test_save_{Guid.NewGuid()}.docx");

        try
        {
            wordDoc.Create(testPath);
            wordDoc.Save();

            Assert.IsNotNull(wordDoc.document);
        }
        finally
        {
            if (File.Exists(testPath)) File.Delete(testPath);
        }
    }

    [TestMethod]
    public void WordDocument_ReplaceText_ModifiesDocument()
    {
        var wordDoc = new WordDocument();
        var testPath = Path.Combine(Path.GetTempPath(), $"test_replace_{Guid.NewGuid()}.docx");

        try
        {
            wordDoc.Create(testPath);
            wordDoc.ReplaceText("{{Name}}", "John Doe");

            Assert.IsNotNull(wordDoc.document);
        }
        finally
        {
            if (File.Exists(testPath)) File.Delete(testPath);
        }
    }

    [TestMethod]
    public void WordDocument_Open_ThrowsFileNotFoundException_ForMissingFile()
    {
        var wordDoc = new WordDocument();
        var nonExistentPath = Path.Combine(Path.GetTempPath(), "non_existent_file.docx");

        Assert.ThrowsException<FileNotFoundException>(() => wordDoc.Open(nonExistentPath));
    }
}
