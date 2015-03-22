using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployerLib
{
    public class ConstantsPR
    {
        public static string ApplicationName { get; set; } = "Upgrader";

        public const string MANIFEST_COMMAND_MARKER = "##";
        public const string MANIFEST_OVERWRITE_COMMAND = "##overwrite";

        public const string STOP_FILE_NAME = "_Stop.txt";


        public static List<string> NoCopyFiles = new List<string>() {
            "Upgrader.exe".ToLowerInvariant(),
            /*"Upgrader.Lib.dll".ToLowerInvariant(),*/
            STOP_FILE_NAME.ToLowerInvariant(),
            /*"Upgrader.exe.config", */
        };
    }
}
