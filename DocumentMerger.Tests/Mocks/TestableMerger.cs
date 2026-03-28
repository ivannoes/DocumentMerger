namespace DocumentMergerTests.Mocks;

public class TestableMerger : DocumentMerger
{
    public TestableMerger(IDocumentCreator creator) : base(creator) { }

    public override void LoadDocument(string pathInputDocument, IDocumentFacade document)
    {
        document.Open(pathInputDocument);
    }

    public override void ReplacePlaceholdersWithDictonary(IDocumentFacade document, Dictionary<string, object> dict)
    {
        if (dict == null) return;

        foreach (var kvp in dict)
        {
            document?.ReplaceText($"{{{{{kvp.Key}}}}}", $"{kvp.Value}");
        }
        document?.Save();
    }
}
