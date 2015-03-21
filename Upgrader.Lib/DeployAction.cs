using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeployerLib
{
    public class DeployAction
    {
        public readonly string _sourceDirectory;
        public readonly string _destinationDirectory;
        public readonly string _manifestFileName;
        public readonly List<string> _executeArgs;
        private bool _overWrite;

        public List<string> files { get; set; }

        public List<string> _executables;

        public DeployAction(string sourceDirectory, string destinationDirectory, string manifestFileName, List<string> executeArgs)
        {
            _destinationDirectory = destinationDirectory.Trim();
            _sourceDirectory = sourceDirectory.Trim();
            _manifestFileName = manifestFileName;
            _executeArgs = executeArgs;
        }

        public int Act()
        {
            files = null;
            if (!string.IsNullOrEmpty(_manifestFileName))
            {
                files = ReadManifesList(_sourceDirectory, _manifestFileName);
            }
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_sourceDirectory);
                FileInfo[] fileInfos = dirInfo.GetFiles();
                files = fileInfos.Select(e => e.Name).ToList();
            }

            // unify case for comparison
            files = files.Select(e => e.ToLowerInvariant()).ToList();

            files = ExcludeNoCopyFiles(files);

            List<string> olderFiles = GetOlderFiles(_sourceDirectory, _destinationDirectory, files, _overWrite);
            if (olderFiles.Count == 0)
            {
                /* nothing to update */
                return 0;
            }

            List<string> executables = files.Where(e => e.ToLower().EndsWith(".exe")).ToList();
            List<string> locked = GetLocked(_destinationDirectory, executables);
            if (locked != null && locked.Count > 0)
            {
                string message = string.Format("Another version of the application is running.{0}{0}Locked files are: ", Environment.NewLine);
                for (int i = 0; i < Math.Min(3, locked.Count); i++)
                {
                    message += locked[i] + ", ";
                }

                throw new UiException(message);
            }

            CopyNewerFiles(_sourceDirectory, _destinationDirectory, olderFiles);

            return olderFiles.Count();
        }

        public void ExecuteAfter()
        {
            ExecuteAfter(_destinationDirectory, _executeArgs);
        }

        public string CheckStopFile()
        {
            string stopFilePath = _sourceDirectory + @"\" + ConstantsPR.STOP_FILE_NAME;
            if (System.IO.File.Exists(stopFilePath) == false)
            {
                return null;
            }
            try
            {
                string text = File.ReadAllText(stopFilePath);
                return text;
            }
            catch
            {
                return "";
            }
        }

        private List<string> ReadManifesList(string manifestDirectory, string manifestFile)
        {
            string manifestPath = manifestDirectory + @"\" + manifestFile;
            if (File.Exists(manifestPath) == false)
            {
                throw new UiException("Manifest file is not found, path '" + manifestPath + "'");
            }

            List<string> lines = new List<string>();
            List<string> rawLines = File.ReadAllLines(manifestPath).ToList();
            foreach (string rawLine in rawLines)
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("//"))
                {
                    continue;
                }

                if (line.StartsWith(ConstantsPR.MANIFEST_COMMAND_MARKER))
                {
                    if (line.StartsWith(ConstantsPR.MANIFEST_OVERWRITE_COMMAND))
                    {
                        _overWrite = true;
                    }
                    continue;
                }

                lines.Add(line);
            }

            return lines;
        }

        private List<string> ExcludeNoCopyFiles(List<string> files)
        {
            return files.Except(ConstantsPR.NoCopyFiles).ToList();
        }

        private List<string> GetOlderFiles(string baseDirectory, string targetDirectory, List<string> files, bool overWrite)
        {
            List<string> results = new List<string>();

            foreach (string file in files)
            {
                string basePath = baseDirectory + @"\" + file;
                string targetPath = targetDirectory + @"\" + file;
                FileInfo baseInfo = new FileInfo(basePath);
                if (baseInfo.Exists == false)
                {
                    throw new UiException("Base file '" + basePath + "' was not found.");
                }

                FileInfo targetInfo = new FileInfo(targetPath);
                if (targetInfo.Exists == false)
                {
                    /* file is missing in target */
                    results.Add(file);
                }
                else
                {
                    if (overWrite || baseInfo.LastWriteTime > targetInfo.LastWriteTime)
                    {
                        /* base contains newer file */
                        results.Add(file);
                    }
                }
            }

            return results;
        }

        private List<string> GetLocked(string path, List<string> files)
        {
            List<string> locked = new List<string>();
            foreach (string file in files)
            {
                string exePath = path + @"\" + file;
                FileInfo fileInfo = new FileInfo(exePath);
                if (fileInfo.Exists == false)
                {
                    /* we might be looking for locked files, but they not yet has been copied to the target directory.
                       such 'non-existing' files considered not to be locked. */
                    continue;
                }

                bool isLocked = false;
                try
                {
                    /* read only can cause an exception if read-only file is locked at the same time */
                    FileHelpers.CheckAndRemoveReadOnly(fileInfo);

                    isLocked = FileHelpers.IsFileLocked(exePath);
                }
                catch
                {
                    isLocked = true;
                }

                if (isLocked)
                {
                    /* add file to locked collection */
                    locked.Add(file);
                }
            }

            return locked;
        }

        private void CopyNewerFiles(string baseDirectory, string targetDirectory, List<string> files)
        {
            foreach (string file in files)
            {
                string basePath = baseDirectory + @"\" + file;
                string targetPath = targetDirectory + @"\" + file;

                FileInfo targetInfo = new FileInfo(targetPath);
                if (targetInfo.Exists)
                {
                    FileHelpers.CheckAndRemoveReadOnly(targetInfo);
                }

                File.Copy(basePath, targetPath, true);

                /* remove read only, if source was read only */
                targetInfo = new FileInfo(targetPath);
                FileHelpers.CheckAndRemoveReadOnly(targetInfo);
            }
        }

        private void ExecuteAfter(string executeDirectory, List<string> executeArgs)
        {
            if (executeArgs == null || executeArgs.Count() == 0)
            {
                return;
            }

            //string[] tokens = executeCommand.Split(new[] { ' ' }, 2);
            string file = executeArgs[0].Trim();
            if (file.Length == 0)
            {
                return;
            }

            string arguments = "";
            for (int i = 1; i < executeArgs.Count; i++)
            {
                arguments += executeArgs[i] + " ";
            }

            string commandPath = executeDirectory + @"\" + file;
            if (string.IsNullOrEmpty(commandPath) == false)
            {
                FileHelpers.ExecuteCommand(commandPath, arguments);
            }
        }
    }
}