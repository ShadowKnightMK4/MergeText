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
        #region properties to do
        public int PathCount
        {
            get
            {
                return Paths.Count;
            }
        }

        #endregion
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

        internal void AddDireRangeCommon(string path, out IList<string> Rejects, out IList<DirectoryInfo> Added)
        {
            string path_sel;
            if (path.Contains(";"))
            {
                path_sel = ";";
            }
            else if (path.Contains(","))
            {
                path_sel = ",";
            }
            else
            {
                path_sel = null;
            }
            Rejects = new List<string>();
            Added = new List<DirectoryInfo>();
  


            if (path_sel != null)
            {
                if (path.StartsWith("\""))
                    path = path.Substring(1);
                if (path.EndsWith("\""))
                    path = path.Substring(0, path.Length - 1);

                var spl = path.Split(path_sel, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in spl)
                {
                    try
                    {
                        AddDir(item);
                        Added.Add(new DirectoryInfo(item));
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        Rejects.Add(item);
                    }
                }
                return;

            }
            else
            {

                try
                {
                    AddDir(path);
                    Added.Add(new DirectoryInfo(path));
                }
                catch (DirectoryNotFoundException e)
                {
                    Rejects.Add(path);
                }
            }
        }
        public void AddDirRange(string path, out IList<string> Rejects)
        {
            this.AddDireRangeCommon(path, out Rejects, out _);
        }
        public void AddDirRange(string path)
        {
            this.AddDireRangeCommon(path, out _, out _);

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
