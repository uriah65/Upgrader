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
    public class T40_StopFile
    {
        private Pawns _dummies;
        private DeployAction _action;

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

            _action = new DeployAction(ConstantsPR.TestSourcePath, ConstantsPR.TestTargetPath, null, null, false);
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void StopFile_Exist()
        {
            _dummies.MoveToSource("_stopn");

            string message = _action.CheckStopFile();
            Assert.AreEqual("App suspended.", message);
        }

        [TestMethod]
        public void StopFile_NotFound()
        {
            string message = _action.CheckStopFile();
            Assert.AreEqual(null, null);
        }
    }
}
