using System.Threading;
using DeployerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Tests.Infrastructure;

namespace Tests
{
    [TestClass]
    public class T20_FileOperations
    {
        private Pawns _dummies;

        [TestInitialize()]
        public void Initialize()
        {
            // filling dummy files collection
            _dummies = new Pawns();
            _dummies.ReadDirectory(ConstantsPR.SourceOldPath, false);
            _dummies.ReadDirectory(ConstantsPR.SourceNewPath, true);

            // empty both test folders
            _dummies.EmptyDirectory(ConstantsPR.TestSourcePath);
            _dummies.EmptyDirectory(ConstantsPR.TestTargetPath);
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void File_Locked_Unlocked()
        {
            _dummies.MoveToTarget("fn");
            string filePath = "";

            /* non existing file is not locked */
            filePath = ConstantsPR.TestTargetPath + @"\" + "nonexistinf.file";
            Assert.AreEqual(false, FileHelpers.IsFileLocked(filePath));
            
            /* non running file is not locked */
            filePath = ConstantsPR.TestTargetPath + @"\" + "f.exe";
            FileHelpers.SetFileNotReadonly(new FileInfo(filePath));
            Assert.AreEqual(false, FileHelpers.IsFileLocked(filePath));

            /* running file is locked */
            Process process  = FileHelpers.ExecuteCommand(filePath, "");
            Assert.AreEqual(true, FileHelpers.IsFileLocked(filePath));

            /* closing running process */
            Thread.Sleep(1000);
            process.CloseMainWindow();
            process.Close();

            /* non running file is not locked */
            Thread.Sleep(1000);
            Assert.AreEqual(false, FileHelpers.IsFileLocked(filePath));
        }

 
        [TestMethod]
        public void ReadOnly_Set_Remove()
        {
            /* prepare a file */
            _dummies.MoveToSource("ao");
            string filePath = ConstantsPR.TestSourcePath + @"\a.txt";

            /* set RON and verify */
            FileHelpers.SetFileReadonly(new FileInfo(filePath));
            Assert.AreEqual(true, FileHelpers.IsFileReadonly(new FileInfo(filePath)));

            /* remove RON and verify */
            FileHelpers.SetFileNotReadonly(new FileInfo(filePath));
            Assert.AreEqual(false, FileHelpers.IsFileReadonly(new FileInfo(filePath)));
        }
    }
}