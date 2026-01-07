using Xceed.Words.NET;
public interface IDocumentProduct
{
    void Open(string pathFile);
    void Create(string pathFile);
    void Save();
}

class PDFDocumentConcrete : IDocumentProduct
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
class WordDocumentConcrete : IDocumentProduct
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
}

interface IDocumentCreator
{
    public IDocumentProduct CreateDocumentObject();
}

class PDFDocumentCreatorConcrete : IDocumentCreator
{
    public IDocumentProduct CreateDocumentObject()
    {
        return new PDFDocumentConcrete();
    }
}

class WordDocumentCreatorConcrete : IDocumentCreator
{
    public IDocumentProduct CreateDocumentObject()
    {
        return new WordDocumentConcrete();
    }
}
