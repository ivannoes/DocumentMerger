using System;
using System.IO;
using System.Collections.Generic;
using MiniSoftware;

public abstract class DocumentMerger
{
    private readonly IDocumentCreator _documentCreator;

    public DocumentMerger(IDocumentCreator documentCreator)
    {
        _documentCreator = documentCreator;
    }

    public void MergeDocument(string pathInputDocument, string pathOutputDocument, DtoGeneric data)
    {
        Console.WriteLine("Merging document...");
        IDocumentFacade document = _documentCreator.CreateDocumentObject();
        
        if (!LoadDocument(pathInputDocument, document))
        {
            return;
        }

        Console.WriteLine($"data is type of {data.GetType()}");
        
        var dict = data.ToDictionary();

        ReplacePlaceholdersWithDictonary(document, dict);
        SaveOutput(document, pathOutputDocument);
    }

    public abstract bool LoadDocument(string pathInputDocument, IDocumentFacade document);
    public abstract void ReplacePlaceholdersWithDictonary(IDocumentFacade document, Dictionary<string, object> dict);
    public abstract void SaveOutput(IDocumentFacade document, string pathOutputDocument);
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
    public override void SaveOutput(IDocumentFacade document, string pathOutputDocument)
    {
        document.SaveAs(pathOutputDocument);
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
    }
    public override void SaveOutput(IDocumentFacade document, string pathOutputDocument)
    {
        string outputExtension = Path.GetExtension(pathOutputDocument).ToLowerInvariant();
        
        if (outputExtension == ".pdf")
        {
            string tempDocxPath = Path.GetTempFileName() + ".docx";
            document?.SaveAs(tempDocxPath);
            Console.WriteLine($"Converting Word to PDF...");
            MiniPdf.ConvertToPdf(tempDocxPath, pathOutputDocument);
            File.Delete(tempDocxPath);
            Console.WriteLine($"PDF saved to {pathOutputDocument}");
        }
        else
        {
            document?.SaveAs(pathOutputDocument);
        }
    }
}
