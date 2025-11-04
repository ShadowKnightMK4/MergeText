using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeText;
using Microsoft.VisualBasic;
namespace UnitTests
{
    [TestClass]
    public class WildcardMatch_Tests
    {
        [TestMethod]
        public void AddWildcard_CountShouldBe1()
        {
            var wd = new WildcardChecking();
            Assert.AreEqual(0, wd.WildcardCount);
            wd.AddWildcard("*.*");

            Assert.AreEqual(1, wd.WildcardCount);
        }

        [TestMethod]
        public void ClearWildcard_CountShouldBe0()
        {
            var wd = new WildcardChecking();
            Assert.AreEqual(0, wd.WildcardCount);
            wd.AddWildcard("*.*");
            Assert.AreEqual(1, wd.WildcardCount);

            wd.Clear();
            Assert.AreEqual(0, wd.WildcardCount);
        }

        [TestMethod]
        public void AddWildcardRange_CountBe2()
        {
            var wd = new WildcardChecking();
            Assert.AreEqual(0, wd.WildcardCount);
            wd.AddWildcardRange("*.sll;*.dll");
            Assert.AreEqual(2, wd.WildcardCount);
        }


        [TestMethod]
        public void AddWildcardRange_IsQuoted_ShouldMatch()
        {
            var wd = new WildcardChecking();
            Assert.AreEqual(0, wd.WildcardCount);
            wd.AddWildcardRange("\"*.sll;*.dll\"");
            Assert.AreEqual(2, wd.WildcardCount);
            foreach (string s in wd.GetWildcards())
            {
                if (string.Compare(s,"*.sll") == 0)
                {
                    continue;
                }
                else
                {
                    if (string.Compare(s,"*.dll") == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Assert.Fail("Unexpected wildcard in the list");
                    }
                }
            }
        }



        [TestMethod]
        public void AddWildcardRange_IsNOTQuoted_ShouldMatch()
        {
            var wd = new WildcardChecking();
            Assert.AreEqual(0, wd.WildcardCount);
            wd.AddWildcardRange("*.sll;*.dll");
            Assert.AreEqual(2, wd.WildcardCount);
            foreach (string s in wd.GetWildcards())
            {
                if (string.Compare(s, "*.sll") == 0)
                {
                    continue;
                }
                else
                {
                    if (string.Compare(s, "*.dll") == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Assert.Fail("Unexpected wildcard in the list");
                    }
                }
            }
        }
    }
}
