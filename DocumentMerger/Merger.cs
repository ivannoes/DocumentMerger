
public abstract class MergerAbstract
{
    private IDocumentProduct? _document;
    public void mergeDocument(string pathInputDocument, DtoGeneric data)
    {
        Console.WriteLine("Merging document...");

        _document = LoadDocument(pathInputDocument);
        var dataAsDto = CastDto(data);

        // Example of using the dictionary representation of the DTO
        var dict = dataAsDto.ToDictionary();
        foreach(var x in dict)
        {
            Console.WriteLine($"{x.Key} = {x.Value}");
        }
        
        // Additional merging logic would go here

    }
    public DtoGeneric CastDto(DtoGeneric data)
    {
        Console.WriteLine($"data is type of {data.GetType()}");
        IDtoCreator dtoCreator = new DtoCreator(data);
        var dataAsDto = dtoCreator.CreateDto(data);
        Console.WriteLine($"dtoCreator is type of {dataAsDto.GetType()}");
        return dataAsDto;
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
