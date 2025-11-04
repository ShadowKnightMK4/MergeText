using MergeText;
using System;
using System.Text;

namespace MergeText
{
    internal class Program
    {
        static long  BytesWrote = 0;
        static void writestring(FileStream fs, string s) 
        {
            var b = Encoding.UTF8.GetBytes(s);
            fs.Write(b, 0, b.Length);
            BytesWrote += s.Length;
        }

        static void writestring(Stream fs, byte[] s)
        {
            BytesWrote += s.Length;
            fs.Write(s, 0, s.Length);
        }
        static int chunk_size = 1024 * 5;
        static void chunk_write(Stream fs, FileStream source)
         {
            long lastpos = 0;
            var myread = chunk_size;
            var c = new byte[chunk_size];
            try
            {
                while (true)
                {
                    lastpos = source.Position;
                    source.ReadExactly(c, 0, chunk_size);
                    
                    writestring(fs, c);
                }
            }
            catch (EndOfStreamException)
            {
                
                var lastc = new byte[source.Position - lastpos];
                source.Position = lastpos;
                source.ReadExactly(lastc, 0, lastc.Length);
                writestring(fs, lastc);
                
            }
            return;
        }
        static MergeText.WildcardChecking Patterns = new();
        static SourceDir SearchThese = new();
        static IList<string> RejectedPaths = new List<string>();
        static FileStream Output=null;

        static void NoArgs()
        {
            const string quick_readme = $@"MergeText [arguments]

Parameters

| Argument | Description |
|-----------|-------------|
| `-dir:` | One or more directories to search. If using multiple directories, quote it and use comma or semi-colon.  |
| `-match:` | One or more wildcard patterns for matching files (e.g. `*.txt`, `*.cs`). |
| `-output:` | Optional. Path to the merged output file. If omitted, output is written to the console (stdout). |


IMPORTANT if folders have comma in name *use semi-colon* when passing them with -dir otherwise it's likely parsing will break.

## Examples
1. Merge all .txt files from a folder into one file
MergeText -dir:C:\Notes -match:*.txt -output:merged_notes.txt

2. Merge all .cs files from multiple directories to console
MergeText -dir:""src;tests"" -match:*.cs

3. Mix multiple match patterns
MergeText -dir:Docs -match:""""*.md,*.txt"" -output:all_docs.txt

## Output

Important! This utility does *not* care of how the input file is encoded. It's copied as is.
Displays list of folders passed it that it did not have access at the top.
Each file in the merged output is prefixed with a comment line before it's contents:

/*  Sourced from C:\Path\To\File.txt */
";
            Console.WriteLine(quick_readme);
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                NoArgs();
                return;
            }
            try
            {


                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].StartsWith("-dir:"))
                    {
                        string pos = args[i].Substring("-dir:".Length);
                        SearchThese.AddDirRange(pos, out RejectedPaths);
                        continue;
                    }
                    if (args[i].StartsWith("-match:"))
                    {
                        string pos = args[i].Substring("-match:".Length);
                        Patterns.AddWildcardRange(pos);
                        continue;
                    }

                    if (args[i].StartsWith("-output:"))
                    {
                        string pos = args[i].Substring("-output:".Length);
                        Output = File.OpenWrite(pos);
                    }

                }

                if (RejectedPaths.Count != 0)
                {
                    for (int i =0; i < RejectedPaths.Count; i++) 
                    {
                        Console.WriteLine($"Warning: {RejectedPaths[i]} doesn't exist or isn't accessable for MergeText. ");
                    }
                }
                var results = Patterns.GetTargets(SearchThese, false);
                for (int i = 0; i < results.Count; i++)
                {
                    if (Output != null)
                    {
                        writestring(Output, "/*  Sourced from " + results[i].FullName + "*/\r\n");
                        using (FileStream source = File.OpenRead(results[i].FullName))
                        {
                            //writestring(Output, File.ReadAllText(results[i].FullName));
                            chunk_write(Output, source);
                        }
                    }
                    else
                    {
                        Console.WriteLine("/*  Sourced from " + results[i].FullName + "*/\r\n");
                        //Console.Write(File.ReadAllText(results[i].FullName));
                        using (FileStream source = File.OpenRead(results[i].FullName))
                        {

                            using (Stream target = Console.OpenStandardOutput())
                            {
                                chunk_write(target, source);
                            }
                        }
                    }

                }
            }
            finally
            {
                Output?.Dispose();
            }
            Console.Write($"{BytesWrote} bytes or {BytesWrote / 1024} KB were written to ");
            
            if (Output is null)
            {
                Console.WriteLine("Console Window (STDOUT)");
            }
            else
            {
                Console.WriteLine(Output.Name);
            }
            if (RejectedPaths.Count != 0)
            {
                Console.WriteLine($"{RejectedPaths.Count} paths set as input were not accessible to MergeText;");
            }
        }
    }
}