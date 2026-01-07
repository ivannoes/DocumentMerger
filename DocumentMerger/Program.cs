var data = new UserDto
{
    Id = 1,
    Name = "John Doe",
    Email = "j.doe@fakemail.com"
};
IDocumentCreator creator = new WordDocumentCreator();
DocumentMerger merger = new WordMerger(creator);
merger.MergeDocument("sample.docx", data);