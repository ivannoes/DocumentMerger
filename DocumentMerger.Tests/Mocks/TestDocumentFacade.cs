namespace DocumentMergerTests.Mocks;

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

    public void SaveAs(string pathFile)
    {
        CreatedPath = pathFile;
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
