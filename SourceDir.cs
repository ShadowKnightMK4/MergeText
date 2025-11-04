using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MergeText
{
    public class SourceDir
    {
        List<DirectoryInfo> Paths = new();
        /// <summary>
        /// Adds dir if it exists
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public void AddDir(string path)
        {
            if (Directory.Exists(path))
            {
                Paths.Add(new DirectoryInfo(path));
            }
            else
            {
                throw new DirectoryNotFoundException(path);
            }
        }

        public void AddDirRange(string path)
        {
            if (path.Contains(";"))
            {
                if (path.StartsWith("\""))
                    path = path.Substring(1);
                if (path.EndsWith("\""))
                    path = path.Substring(0, path.Length - 1);

                var spl = path.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in spl)
                {
                    AddDir(item);
                }
                return;
            }
            AddDir(path);

        }

        public void ClearDir()
        {
            Paths.Clear();
        }
        public IReadOnlyCollection<DirectoryInfo> GetDirs()
        {
            return Paths.AsReadOnly();
        }

        /// <summary>
        /// fetches list of matches from added dirs, openial toplevel only. False means any subfolders.
        /// </summary>
        /// <param name="Wildcard"></param>
        /// <param name="TopLevelOnly"></param>
        /// <returns></returns>
        public List<FileInfo> GetTargets(string Wildcard, bool TopLevelOnly)
        {
            var Options = SearchOption.AllDirectories;
            if (TopLevelOnly)
            {
                Options = SearchOption.TopDirectoryOnly;
            }
            List<FileInfo> x = new();
            foreach (DirectoryInfo dir in Paths)
            {
                string[] files = Directory.GetFiles(dir.FullName, Wildcard, Options);
                foreach (string file in files)
                {
                    x.Add(new FileInfo(file));
                }
                
            }
            return x;
        }
    }
}
