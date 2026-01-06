
public abstract class MergerAbstract
{
    private IDocumentProduct? _document;
    public void mergeDocument(string pathInputDocument, DtoGeneric data)
    {
        Console.WriteLine("Merging document...");

        _document = LoadDocument(pathInputDocument);
        _document?.ToString();

        Console.WriteLine($"data is type of {data.GetType()}");
        var dataAsDto = data.CastDto();
        Console.WriteLine($"dtoCreator is type of {dataAsDto.GetType()}");

        // Example of using the dictionary representation of the DTO
        var dict = dataAsDto.ToDictionary();
        foreach(var x in dict)
        {
            Console.WriteLine($"{x.Key} = {x.Value}");
        }
        
        // Additional merging logic would go here

    }

    public abstract IDocumentProduct? LoadDocument(string pathInputDocument);
        
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
    public override IDocumentProduct? LoadDocument(string pathInputDocument)
    {
        Console.WriteLine("Loading Word document.");
        try
        {
            IDocumentCreator creator = new WordDocumentCreatorConcrete();
            var document = creator.CreateDocumentObject();            
            document.Open(pathInputDocument);
            
            return document as WordDocumentConcrete;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error loading Word document: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        return null;
    }
}
