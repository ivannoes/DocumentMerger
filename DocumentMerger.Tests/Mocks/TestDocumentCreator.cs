namespace DocumentMergerTests.Mocks;

public class TestDocumentCreator : IDocumentCreator
{
    public TestDocumentFacade Document { get; } = new();

    public IDocumentFacade CreateDocumentObject()
    {
        return Document;
    }
}
