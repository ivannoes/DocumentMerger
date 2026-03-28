using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentMergerTests.Tests.Document;

[TestClass]
public class DocumentFactoryTests
{
    [TestMethod]
    public void PDFDocumentCreator_CreateDocumentObject_ReturnsPDFDocument()
    {
        var creator = new PDFDocumentCreator();

        var result = creator.CreateDocumentObject();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(PDFDocument));
    }

    [TestMethod]
    public void WordDocumentCreator_CreateDocumentObject_ReturnsWordDocument()
    {
        var creator = new WordDocumentCreator();

        var result = creator.CreateDocumentObject();

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(WordDocument));
    }

    [TestMethod]
    public void PDFDocumentCreator_ReturnsNewInstanceEachCall()
    {
        var creator = new PDFDocumentCreator();

        var doc1 = creator.CreateDocumentObject();
        var doc2 = creator.CreateDocumentObject();

        Assert.AreNotSame(doc1, doc2);
    }

    [TestMethod]
    public void WordDocumentCreator_ReturnsNewInstanceEachCall()
    {
        var creator = new WordDocumentCreator();

        var doc1 = creator.CreateDocumentObject();
        var doc2 = creator.CreateDocumentObject();

        Assert.AreNotSame(doc1, doc2);
    }
}
