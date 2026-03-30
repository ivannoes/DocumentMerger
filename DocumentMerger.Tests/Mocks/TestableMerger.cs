namespace DocumentMergerTests.Mocks;

public class TestableMerger : DocumentMerger
{
    public TestableMerger(IDocumentCreator creator) : base(creator) { }

    public override bool LoadDocument(string pathInputDocument, IDocumentFacade document)
    {
        document.Open(pathInputDocument);
        return true;
    }

    public override void ReplacePlaceholdersWithDictonary(IDocumentFacade document, Dictionary<string, object> dict, string pathOutputDocument)
    {
        if (dict == null) return;

        foreach (var kvp in dict)
        {
            document?.ReplaceText($"{{{{{kvp.Key}}}}}", $"{kvp.Value}");
        }
        document?.SaveAs(pathOutputDocument);
    }
}
