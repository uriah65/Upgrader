using System;
using DeployerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Infrastructure;

namespace Tests
{
    [TestClass]
    public class T10_TestingInfrastructure
    {
        private Pawns _dummies;

        [TestInitialize()]
        public void Initialize()
        {
            // filling dummy files collection
            // fill A
            // fill B
            // delete C
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
        public void TestMoveMethods()
        {

            _dummies.MoveToSource("ao", "bn");
            _dummies.MoveToTarget("an", "en");
            _dummies.TargetIs("an", "dn");

            // empty directory get filled

            // single file get overridden (with read only  flag)

            // new file stays

            // exception on the file that absent in source

            // exception on the file that is locked in destination
        }




    }
}
