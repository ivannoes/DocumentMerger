var data = new UserDto
{
    Id = 1,
    Name = "John Doe",
    Email = ""
};
MergerAbstract merger = new PDFMerger();
merger.mergeDocument("sample.pdf", data);