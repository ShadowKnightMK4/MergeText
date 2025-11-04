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
        static FileStream Output=null;
        static void Main(string[] args)
        {
            try
            {


                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].StartsWith("-dir:"))
                    {
                        string pos = args[i].Substring("-dir:".Length);
                        SearchThese.AddDirRange(pos);
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
        }
    }
}