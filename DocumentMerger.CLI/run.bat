@echo off
echo Document Merger CLI
echo Usage: run.bat [input] [output] --data <json-file> [--type <DtoType>]
echo.
dotnet run --project "%~dp0DocumentMerger.CLI.csproj" -- %*
