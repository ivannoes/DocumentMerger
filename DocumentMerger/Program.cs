var data = new UserDto
{
    Id = 1,
    Name = "John Doe",
    Email = "j.doe@fakemail.com"
};
MergerAbstract merger = new PDFMerger();
merger.mergeDocument("sample.pdf", data);