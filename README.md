# Document Merger
A small, focused .NET 8 library for merging DTOs into document templates (PDF, Word, etc.). The project uses a factory pattern to produce document objects and a template-method `MergeDocument` to drive the merge workflow so new formats can be added without changing the high-level logic.
