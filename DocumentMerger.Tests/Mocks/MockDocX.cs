using Xceed.Words.NET;
using Xceed.Document.NET;

namespace DocumentMergerTests.Mocks;

public class MockDocX
{
    public DocX? Instance { get; private set; }
    public string? LoadedPath { get; private set; }
    public string? CreatedPath { get; private set; }
    public bool SaveCalled { get; private set; }
    public bool LoadFailed { get; private set; }
    public Exception? LoadException { get; private set; }
    public List<(string searchValue, string newValue)> ReplaceTextCalls { get; } = new();

    public static MockDocX CreateNew(string path)
    {
        var mock = new MockDocX();
        mock.CreatedPath = path;
        mock.Instance = DocX.Create(path);
        return mock;
    }

    public static MockDocX LoadFrom(string path)
    {
        var mock = new MockDocX();
        mock.LoadedPath = path;
        try
        {
            mock.Instance = DocX.Load(path);
        }
        catch (FileNotFoundException ex)
        {
            mock.LoadFailed = true;
            mock.LoadException = ex;
        }
        return mock;
    }

    public void Save()
    {
        SaveCalled = true;
        Instance?.Save();
    }

    public void ReplaceText(string placeholder, string value)
    {
        ReplaceTextCalls.Add((placeholder, value));

        var options = new StringReplaceTextOptions
        {
            SearchValue = placeholder,
            NewValue = value
        };
        Instance?.ReplaceText(options);
    }

    public bool WasReplaced(string placeholder, string expectedValue)
    {
        return ReplaceTextCalls.Any(c => c.searchValue == placeholder && c.newValue == expectedValue);
    }

    public int ReplacementCount => ReplaceTextCalls.Count;
}
