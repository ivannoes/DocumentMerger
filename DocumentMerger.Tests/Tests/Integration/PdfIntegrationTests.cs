using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DocumentMergerTests.Tests.Integration;

[TestClass]
public class PdfIntegrationTests
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
    public void Integration_PDFMerger_ExecutesFullWorkflow()
    {
        var data = new UserDto
        {
            Id = 123,
            Name = "Jane Smith",
            Email = "jane.smith@example.com"
        };
        var addressData = new AddressDto
        {
            Street = "456 Oak Avenue",
            City = "Portland",
            State = "OR",
            ZipCode = "97201"
        };
        IDocumentCreator creator = new PDFDocumentCreator();
        DocumentMerger merger = new PDFMerger(creator);

        merger.MergeDocument("template.pdf", "output.pdf", data);

        var output = _consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Merging document"));
        Assert.IsTrue(output.Contains("Loading PDF document"));
        Assert.IsTrue(output.Contains("data is type of"));
    }

    [TestMethod]
    public void Integration_PDFDocument_ExecutesAllOperations()
    {
        var pdfDoc = new PDFDocument();

        pdfDoc.Create("output.pdf");
        Assert.IsTrue(_consoleOutput.ToString().Contains("Generating a new, blank PDF document"));

        pdfDoc.Open("template.pdf");
        Assert.IsTrue(_consoleOutput.ToString().Contains("Opening existing PDF document"));

        pdfDoc.ReplaceText("{{Name}}", "Test");
        Assert.IsTrue(_consoleOutput.ToString().Contains("Replacing"));

        pdfDoc.Save();
        Assert.IsTrue(_consoleOutput.ToString().Contains("Saving PDF document"));
    }

    [TestMethod]
    public void Integration_PDFMerger_WithAddressDto_ExecutesSuccessfully()
    {
        var data = new AddressDto
        {
            Street = "789 Pine Street",
            City = "Seattle",
            State = "WA",
            ZipCode = "98101"
        };
        IDocumentCreator creator = new PDFDocumentCreator();
        DocumentMerger merger = new PDFMerger(creator);

        merger.MergeDocument("address_template.pdf", "output.pdf", data);

        var output = _consoleOutput.ToString();
        Assert.IsTrue(output.Contains("Merging document"));
        Assert.IsTrue(output.Contains("data is type of"));
        Assert.IsTrue(output.Contains("AddressDto"));
    }
}
