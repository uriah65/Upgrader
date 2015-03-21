using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeployerLib
{
    public static class FileHelpers
    {
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);
                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }

        public static void CheckAndRemoveReadOnly(FileInfo fileInfo)
        {
            if (IsFileReadonly(fileInfo))
            {
                SetFileNotReadonly(fileInfo);
            }
        }

        public static bool IsFileReadonly(FileInfo fileInfo)
        {
            return fileInfo.IsReadOnly;
        }

        public static void SetFileReadonly(FileInfo fileInfo)
        {
            fileInfo.IsReadOnly = true;
        }

        public static void SetFileNotReadonly(FileInfo fileInfo)
        {
            fileInfo.IsReadOnly = false;
        }

        public static Process ExecuteCommand(string command, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(command);
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = true;
            Process process = Process.Start(startInfo);
            return process;
        }

    }
}
