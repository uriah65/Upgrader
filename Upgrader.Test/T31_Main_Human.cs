using DeployerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tests.Infrastructure;

namespace Tests
{
    [TestClass]
    public class T31_Main_Human
    {
        private Pawns _dummies;
        private DeployAction _action;

        [TestInitialize()]
        public void Initialize()
        {
            _dummies = new Pawns();
            _dummies.ReadDirectory(ConstantsPR.SourceOldPath, false);
            _dummies.ReadDirectory(ConstantsPR.SourceNewPath, true);

            // empty both test folders
            _dummies.EmptyDirectory(ConstantsPR.TestSourcePath);
            _dummies.EmptyDirectory(ConstantsPR.TestTargetPath);

            _action = new DeployAction(ConstantsPR.TestSourcePath, ConstantsPR.TestTargetPath, null, null);
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void PostExecute_Arguments_PassThrough()
        {
            return;
            _dummies.MoveToSource("fn");

            _action = new DeployAction(ConstantsPR.TestSourcePath, ConstantsPR.TestTargetPath, null, new List<string>() { "f.exe", "a1", "a2", "a3" });
            _action.Act();

            /* we expect to see f.exe window with arguments a1, a2 and a3 */
            _action.ExecuteAfter();
        }

    }
}