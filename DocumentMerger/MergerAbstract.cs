
public abstract class MergerAbstract
{
    private IDocumentProduct? _document;
    public void mergeDocument(string pathInputDocument, object data)
    {
        Console.WriteLine("Merging document...");

        _document = LoadDocument(pathInputDocument);
        // Additional merging logic would go here

    }

    public abstract IDocumentProduct LoadDocument(string pathInputDocument);
        
}

public class PDFMerger : MergerAbstract
{
    public override IDocumentProduct LoadDocument(string pathInputDocument)
    {            
        Console.WriteLine("Loading PDF document.");
        IDocumentCreator creator = new PDFDocumentCreatorConcrete();
        var document = creator.CreateDocumentObject();
        document.Open(pathInputDocument);
        return document;
    }
}

public class WordMerger : MergerAbstract
{
    public override IDocumentProduct LoadDocument(string pathInputDocument)
    {
        Console.WriteLine("Loading Word document.");
        IDocumentCreator creator = new WordDocumentCreatorConcrete();
        var document = creator.CreateDocumentObject();
        document.Open(pathInputDocument);
        return document;
    }
}
