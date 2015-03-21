using DeployerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ConstantsPR.ApplicationName = Properties.Settings.Default.ApplicationName;
                Main_Inner(args);
            }
            catch (UiException ex)
            {
                string message = string.Format("Cannot execute your command.{0}{0}{1}", Environment.NewLine, ex.Message);
                MessageBox.Show(message, ConstantsPR.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string message = string.Format("We are sorry, an error has occurred.{0}{0}{1}{0}{0}{2}", Environment.NewLine, ex.Message, ex.StackTrace);
                MessageBox.Show(message, ConstantsPR.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Main_Inner(string[] args)
        {
            // getting target path
            string targetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            targetPath = Path.GetDirectoryName(targetPath);

            // reading configuration and arguments
            List<string> argsList = args.ToList();
            string sourcePath = Properties.Settings.Default.SourceDirectory;
            string manifestFile = Properties.Settings.Default.ManifestFile;
            string startExe = Properties.Settings.Default.StartupArguments;
            if (string.IsNullOrWhiteSpace(startExe) == false)
            {
                argsList.Insert(0, startExe);
            }

            //throw new ApplicationException("test");

            // performing upgrade action
            DeployAction action = new DeployAction(sourcePath, targetPath, manifestFile, argsList);
            int ix = action.Act();
            if (ix > 0)
            {
                if (Properties.Settings.Default.ShowUpgradeMessage)
                {
                    Application.Run(new UpdatePopup());
                }
            }

            // check for stop file
            string message = action.CheckStopFile();
            if (message != null)
            {
                if (message == "")
                {
                    message = "Access to the application '" + ConstantsPR.ApplicationName + "' has been temporarily suspended.";
                }
                MessageBox.Show(message, ConstantsPR.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // running postupgrade commands
            action.ExecuteAfter();
        }
    }
}
