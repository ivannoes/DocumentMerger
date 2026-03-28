
public abstract class DocumentMerger
{
    private readonly IDocumentCreator _documentCreator;

    public DocumentMerger(IDocumentCreator documentCreator)
    {
        _documentCreator = documentCreator;
    }

    public void MergeDocument(string pathInputDocument, DtoGeneric data)
    {
        Console.WriteLine("Merging document...");
        IDocumentFacade document = _documentCreator.CreateDocumentObject();
        
        if (!LoadDocument(pathInputDocument, document))
        {
            return;
        }

        Console.WriteLine($"data is type of {data.GetType()}");
        
        // Example of using the dictionary representation of the DTO
        var dict = data.ToDictionary();

        // Additional merging logic would go here
        ReplacePlaceholdersWithDictonary(document, dict);
    }

    public abstract bool LoadDocument(string pathInputDocument, IDocumentFacade document);
    public abstract void ReplacePlaceholdersWithDictonary(IDocumentFacade document, Dictionary<string, object> dict);


}

public class PDFMerger : DocumentMerger
{
    public PDFMerger(IDocumentCreator documentCreator) : base(documentCreator)
    {
    }

    public override bool LoadDocument(string pathInputDocument, IDocumentFacade document)
    {            
        Console.WriteLine("Loading PDF document.");
        document.Open(pathInputDocument);
        return true;
    }
    public override void ReplacePlaceholdersWithDictonary(IDocumentFacade document, Dictionary<string, object> dict)
    {
        Console.WriteLine($"Replacing placeholder using a dictonary...");
    }
}

public class WordMerger : DocumentMerger
{
    public WordMerger(IDocumentCreator documentCreator) : base(documentCreator)
    {
    }

    public override bool LoadDocument(string pathInputDocument, IDocumentFacade document)
    {
        Console.WriteLine("Loading Word document.");
        document.Open(pathInputDocument);
        return true;
    }
    public override void ReplacePlaceholdersWithDictonary(IDocumentFacade document, Dictionary<string, object> dict)
    {
        Console.WriteLine($"Replacing placeholder using a dictonary...");
        foreach (var x in dict)
        {
            Console.WriteLine($"{x.Key} = {x.Value}");
            document?.ReplaceText($"{{{{{x.Key}}}}}", $"{x.Value}");
        }
        document?.Save();
    }
}
