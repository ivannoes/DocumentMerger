var data = new UserDto
{
    Id = 1,
    Name = "John Doe",
    Email = "j.doe@fakemail.com"
};
MergerAbstract merger = new WordMerger();
merger.mergeDocument("sample.docx", data);