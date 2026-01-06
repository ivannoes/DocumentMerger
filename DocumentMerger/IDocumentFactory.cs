public interface IDocumentProduct
{
    void Open(string pathFile);
    void Create(string pathFile);
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
}
class WordDocumentConcrete : IDocumentProduct
{
    public void Open(string pathFile)
    {
        Console.WriteLine($"Opening existing Word document at {pathFile}.");
    }
    public void Create(string pathFile)
    {
        Console.WriteLine("Generating a new, blank Word document.");
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
