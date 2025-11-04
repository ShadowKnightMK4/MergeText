using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeText
{
    internal class WildcardChecking
    {
            List<string> Wildcards = new();
            /// <summary>
            /// Adds dir if it exists
            /// </summary>
            /// <param name="path"></param>
            public void AddWildcard(string pattern)
            {
                Wildcards.
                Add(pattern);
            }

            public void AddWildcardRange(string pattern)
            {
                if (pattern.Contains(";"))
                {
                    if (pattern.StartsWith("\""))
                      pattern = pattern.Substring(1);
                    if (pattern.EndsWith("\""))
                    pattern = pattern.Substring(0, pattern.Length - 1);

                    var spl = pattern.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in spl)
                    {
                        AddWildcard(item);
                    }
                    return;
                }
              AddWildcard(pattern);

            }

            public void Clear()
            {
            Wildcards.Clear();
            }
            public IReadOnlyCollection<string> GetWildcards()
            {
                return Wildcards.AsReadOnly();
            }

            /// <summary>
            /// fetches list of matches from added dirs, openial toplevel only. False means any subfolders.
            /// </summary>
            /// <param name="Wildcard"></param>
            /// <param name="TopLevelOnly"></param>
            /// <returns></returns>
            public List<FileInfo> GetTargets(SourceDir SearchMe, bool TopLevelOnly)
            {
            List<FileInfo> x = new();
            foreach (var item in Wildcards)
            {
                x.AddRange(SearchMe.GetTargets(item, TopLevelOnly));
            }
            return x;
            }
        
    }

}
