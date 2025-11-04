using MergeText;

namespace UnitTests
{
    [TestClass]
    public   class SourceDir_Tests
    {
        [TestMethod]
        public void AddDir_SingleDir_CountShouldBe1()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDir(folder);

            Assert.AreEqual(1, sd.PathCount);

        }

        [TestMethod]
        public void AddDir_SingleDir_ContentsShouldMatch()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDir(folder);


            var contents = sd.GetDirs();
            foreach (DirectoryInfo d in contents)
            {
                Assert.AreEqual(d.FullName, folder);
            }
        }


        [TestMethod]
        public void AddDirRange_SingleDir_ShouldBeCount1()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDirRange(folder);

            Assert.AreEqual(1, sd.PathCount);
        }

        [TestMethod]
        public void AddDirRange_TwoDir_ShouldBeCount2_UseCommaSep()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fodler2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDirRange(folder + "," + fodler2);

            Assert.AreEqual(2, sd.PathCount);
        }

        [TestMethod]
        public void AddDirRange_TwoDir_ShouldBeCount2_UseSemiSep()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fodler2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDirRange(folder + ";" + fodler2);

            Assert.AreEqual(2, sd.PathCount);
        }


        [TestMethod]
        public void AddDirRange_TwoDir_ContentsShouldMatch_UseCommaSep()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fodler2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDirRange(folder + "," + fodler2);

            Assert.AreEqual(2, sd.PathCount);
            foreach (DirectoryInfo d in sd.GetDirs())
            {
                if (string.Compare(d.FullName, folder) == 0)
                {
                    continue;
                }
                else
                {
                    if (string.Compare(d.FullName, fodler2) == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Assert.Fail("Assert Fail: Contents did not match what was expected of first or second folder");
                    }
                }
            }
        }

        [TestMethod]
        public void AddDir_NonExisting_ShouldSetRejectPath_()
        {
            var folder = "ZZ\\Test";
            var sd = new SourceDir();
            IList<string> reject = new List<string>();
            sd.AddDirRange(folder, out reject);

            Assert.AreEqual(0, sd.PathCount);
            Assert.AreEqual(1, reject.Count);
        }


        [TestMethod]
        public void AddDir_NonExisting_ShouldSetRejectPath_AndPassItUnchanged()
        {
            var folder = "ZZ\\Test";
            var sd = new SourceDir();
            IList<string> reject = new List<string>();
            sd.AddDirRange(folder, out reject);

            Assert.AreEqual(0, sd.PathCount);
            Assert.AreEqual(1, reject.Count);
            Assert.Contains(folder, reject);
        }
        [TestMethod]
        public void AddDirRange_TwoDir_ContentsShouldMatch_UseSemiSep()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fodler2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDirRange(folder + ";" + fodler2);

            Assert.AreEqual(2, sd.PathCount);
            foreach (DirectoryInfo d in sd.GetDirs())
            {
                if (string.Compare(d.FullName, folder) == 0)
                {
                    continue;
                }
                else
                {
                    if (string.Compare(d.FullName, fodler2) == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Assert.Fail("Assert Fail: Contents did not match what was expected of first or second folder");
                    }
                }
            }
        }

        [TestMethod]
        public void ClearDir_ShouldResetToZero()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var fodler2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var sd = new SourceDir();
            Assert.AreEqual(0, sd.PathCount);
            sd.AddDirRange(folder + ";" + fodler2);

            Assert.AreEqual(2, sd.PathCount);
            sd.ClearDir();
            Assert.AreEqual(0, sd.PathCount);
        }
    }
}
