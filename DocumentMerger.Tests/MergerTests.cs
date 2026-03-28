using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentMergerTests;

public class TestDocumentFacade : IDocumentFacade
{
    public string? LoadedPath { get; private set; }
    public string? CreatedPath { get; private set; }
    public bool SaveCalled { get; private set; }
    public List<(string placeholder, string value)> Replacements { get; } = new();

    public void Open(string pathFile)
    {
        LoadedPath = pathFile;
    }

    public void Create(string pathFile)
    {
        CreatedPath = pathFile;
    }

    public void Save()
    {
        SaveCalled = true;
    }

    public void ReplaceText(string placeholder, string value)
    {
        Replacements.Add((placeholder, value));
    }

    public void Reset()
    {
        LoadedPath = null;
        CreatedPath = null;
        SaveCalled = false;
        Replacements.Clear();
    }
}

public class TestDocumentCreator : IDocumentCreator
{
    public TestDocumentFacade Document { get; } = new();

    public IDocumentFacade CreateDocumentObject()
    {
        return Document;
    }
}

public class TestableMerger : DocumentMerger
{
    public TestableMerger(IDocumentCreator creator) : base(creator) { }

    public override void LoadDocument(string pathInputDocument, IDocumentFacade document)
    {
        document.Open(pathInputDocument);
    }

    public override void ReplacePlaceholdersWithDictonary(IDocumentFacade document, Dictionary<string, object> dict)
    {
        foreach (var kvp in dict)
        {
            document?.ReplaceText($"{{{{{kvp.Key}}}}}", $"{kvp.Value}");
        }
        document?.Save();
    }
}

[TestClass]
public class MergerTests
{
    private TestDocumentCreator _creator = null!;
    private TestableMerger _merger = null!;

    [TestInitialize]
    public void Setup()
    {
        _creator = new TestDocumentCreator();
        _merger = new TestableMerger(_creator);
    }

    [TestMethod]
    public void MergeDocument_CallsToDictionary()
    {
        var data = new UserDto
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com"
        };

        _merger.MergeDocument("test.docx", data);

        Assert.IsNotNull(_creator.Document.LoadedPath);
        Assert.AreEqual("test.docx", _creator.Document.LoadedPath);
    }

    [TestMethod]
    public void MergeDocument_ReplacesPlaceholders()
    {
        var data = new UserDto
        {
            Id = 42,
            Name = "John Doe",
            Email = "john@example.com"
        };

        _merger.MergeDocument("test.docx", data);

        Assert.IsTrue(_creator.Document.Replacements.Count > 0);
        Assert.IsTrue(_creator.Document.SaveCalled);
    }

    [TestMethod]
    public void MergeDocument_ReplacesAllUserDtoFields()
    {
        var data = new UserDto
        {
            Id = 1,
            Name = "Alice",
            Email = "alice@example.com"
        };

        _merger.MergeDocument("test.docx", data);

        Assert.AreEqual(3, _creator.Document.Replacements.Count);
        Assert.IsTrue(_creator.Document.Replacements.Any(r => r.placeholder == "{{Id}}" && r.value == "1"));
        Assert.IsTrue(_creator.Document.Replacements.Any(r => r.placeholder == "{{Name}}" && r.value == "Alice"));
        Assert.IsTrue(_creator.Document.Replacements.Any(r => r.placeholder == "{{Email}}" && r.value == "alice@example.com"));
    }

    [TestMethod]
    public void MergeDocument_ReplacesAllAddressDtoFields()
    {
        var data = new AddressDto
        {
            Street = "123 Main St",
            City = "Springfield",
            State = "IL",
            ZipCode = "62701"
        };

        _merger.MergeDocument("test.docx", data);

        Assert.AreEqual(4, _creator.Document.Replacements.Count);
        Assert.IsTrue(_creator.Document.Replacements.Any(r => r.placeholder == "{{Street}}" && r.value == "123 Main St"));
        Assert.IsTrue(_creator.Document.Replacements.Any(r => r.placeholder == "{{City}}" && r.value == "Springfield"));
        Assert.IsTrue(_creator.Document.Replacements.Any(r => r.placeholder == "{{State}}" && r.value == "IL"));
        Assert.IsTrue(_creator.Document.Replacements.Any(r => r.placeholder == "{{ZipCode}}" && r.value == "62701"));
    }

    [TestMethod]
    public void MergeDocument_HandlesNullValues()
    {
        var data = new UserDto
        {
            Id = 1,
            Name = null,
            Email = null
        };

        _merger.MergeDocument("test.docx", data);

        Assert.IsTrue(_creator.Document.Replacements.Count >= 1);
    }

    [TestMethod]
    public void MergeDocument_CallsDocumentOpen()
    {
        var data = new UserDto { Id = 1, Name = "Test", Email = "test@test.com" };

        _merger.MergeDocument("document.docx", data);

        Assert.AreEqual("document.docx", _creator.Document.LoadedPath);
    }

    [TestMethod]
    public void MergeDocument_CallsDocumentSave()
    {
        var data = new UserDto { Id = 1, Name = "Test", Email = "test@test.com" };

        _merger.MergeDocument("document.docx", data);

        Assert.IsTrue(_creator.Document.SaveCalled);
    }

    [TestMethod]
    public void PDFMerger_LoadsDocument()
    {
        var pdfCreator = new TestDocumentCreator();
        var pdfMerger = new PDFMerger(pdfCreator);

        var data = new UserDto { Id = 1, Name = "Test", Email = "test@test.com" };
        pdfMerger.MergeDocument("test.pdf", data);

        Assert.AreEqual("test.pdf", pdfCreator.Document.LoadedPath);
    }
}
