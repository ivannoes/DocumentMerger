using Xceed.Words.NET;
using Xceed.Document.NET;

public interface IDocumentFacade
{
    void Open(string pathFile);
    void Create(string pathFile);
    void Save();
    void ReplaceText(string placeholder, string value);
}

class PDFDocument : IDocumentFacade
{
    public void Open(string pathFile)
    {
        Console.WriteLine($"Opening existing PDF document at {pathFile}.");
    }
    public void Create(string pathFile)
    {
        Console.WriteLine("Generating a new, blank PDF document.");
    }
    public void Save()
    {
        Console.WriteLine("Saving PDF document.");
    }
    public void ReplaceText(string placeholder, string value)
    {
        Console.WriteLine($"Replacing '{placeholder}' with '{value}' in PDF document.");
    }
}
class WordDocument : IDocumentFacade
{
    public DocX? document { get; set; }
    
    public void Open(string pathFile)
    {
        Console.WriteLine($"Opening Word document at {pathFile}.");
        document = DocX.Load(pathFile); 
    }
    public void Create(string pathFile)
    {
        Console.WriteLine("Generating a new, blank Word document.");
        document = DocX.Create(pathFile);
        document?.Save();
    }
    public void Save()
    {        
        document?.Save();
    }
    public void ReplaceText(string placeholder, string value)
    {
        var options = new StringReplaceTextOptions
        {
            SearchValue = placeholder,
            NewValue = value
        };
        document?.ReplaceText(options);        
    }
}

public interface IDocumentCreator
{
    public IDocumentFacade CreateDocumentObject();
}

public class PDFDocumentCreator : IDocumentCreator
{
    public IDocumentFacade CreateDocumentObject()
    {
        return new PDFDocument();
    }
}

public class WordDocumentCreator : IDocumentCreator
{
    public IDocumentFacade CreateDocumentObject()
    {
        return new WordDocument();
    }
}
