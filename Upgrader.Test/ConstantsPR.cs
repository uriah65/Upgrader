namespace Tests
{
    internal static class ConstantsPR
    {
        private const string _base = @"..\..\..\Upgrader.Test\";
   

        public const string SourceOldPath = _base + @"Infrastructure\SourceOld";
        public const string SourceNewPath = _base + @"Infrastructure\SourceNew";
        public const string TestSourcePath = _base + @"TestGround\TestBase";
        public const string TestTargetPath = _base + @"TestGround\TestTarget";

        public const string LockFile = _base + @"Infrastructure\FileLocking\Test.bat";
    }
}