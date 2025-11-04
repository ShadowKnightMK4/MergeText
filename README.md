# MergeText
MergeText

MergeText is a lightweight C# command-line tool that merges the contents of multiple text files into a single output stream or file.
It supports directory scanning, wildcard-based file matching, and efficient chunked reading/writing to handle large files.


The original objective of the tool was I wanted to merge a multi file Visual Studio Solution to a single file, 
labled for easy LLM evaluation.

Important! This utility does *not* care of how the input file is encoded. It's copied as is.

🧩 Features

Merge multiple files into one.

Use wildcards (e.g. *.cs, *.txt) to filter files.

Scan multiple directories recursively.

Output to a file or to standard output (console).

Efficient chunk-based streaming (5 KB chunks).

⚙️ Usage
MergeText [arguments]

Parameters

| Argument | Description |
|-----------|-------------|
| `-dir:` | One or more directories to search. If using multiple directories, quote it and use comma or semi-colon |
| `-match:` | One or more wildcard patterns for matching files (e.g. `*.txt`, `*.cs`). |
| `-output:` | Optional. Path to the merged output file. If omitted, output is written to the console (stdout). |



🧰 Examples
1. Merge all .txt files from a folder into one file
MergeText -dir:C:\Notes -match:*.txt -output:merged_notes.txt

2. Merge all .cs files from multiple directories to console
MergeText -dir:"src;tests" -match:*.cs

3. Mix multiple match patterns
MergeText -dir:Docs -match:""*.md,*.txt" -output:all_docs.txt

📦 Output

Each file in the merged output is prefixed with a comment line:

/*  Sourced from C:\Path\To\File.txt */

🧮 Notes

Uses 5 KB chunks to efficiently read and write large files.

Displays total bytes and kilobytes written after completion.

Important! This utility does *not* care of how the input file is encoded. It's copied as is.