using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DatsuEditor
{
    internal static class Program
    {
        [DllImport("kernel32")]
        public static extern bool AttachConsole(int dwProcessId);
        [DllImport("kernel32")]
        public static extern bool FreeConsole();

        const string CliModeTitle = "DatsuEditor - CLI Mode Help";
        private static void DatsuCLI(string[] args)
        {
            if (args.Length == 2 && args[1] == "-h")
            {
                StringBuilder HelpString = new StringBuilder();

                HelpString.AppendLine("$ DatsuEditor -c (-u/-r) [InputPath] [(optional) OutputPath]");

                HelpString.AppendLine("\nExample of unpacking:");
                HelpString.AppendLine(" * unpack pl1010.dat into pl1010_dat:\n * * $ DatsuEditor -c -u pl1010.dat");
                HelpString.AppendLine(" * unpack pl1010.dat into raiden:\n * * $ DatsuEditor -c -u pl1010.dat raiden");
                HelpString.AppendLine(" * unpack all detected .dat files in pl:\n * * $ DatsuEditor -c -u pl");

                HelpString.AppendLine("\nExample of repacking:");
                HelpString.AppendLine(" * repack pl1010.dat into pl1010.dat (repack without any changes):\n * * $ DatsuEditor -c -r pl1010.dat");
                HelpString.AppendLine(" * repack pl1010.dat into raiden.dat:\n * * $ DatsuEditor -c -u pl1010.dat raiden.dat");
                HelpString.AppendLine(" * repack pl1010_dat into pl1010.dat:\n * * $ DatsuEditor -c -u pl1010_dat");
                HelpString.AppendLine(" * repack raiden into pl1010.dat:\n * * $ DatsuEditor -c -u raiden pl1010.dat");

                MessageBox.Show(HelpString.ToString(), CliModeTitle);
            }
            if (args.Length < 2)
            {
                MessageBox.Show("CLI Mode called with no arguments\nPass '-c -h' to see help.", CliModeTitle);
                return;
            }
            if (args.Length >= 2)
            {
                switch (args[1])
                {
                    case "-u":
                        if (!File.Exists(args[2]) && !Directory.Exists(args[2]))
                        {
                            MessageBox.Show($"No file \"{args[2]}\" found.");
                            Application.Exit();
                        }
                        FileAttributes attr = File.GetAttributes(args[2]);
                        if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                        {
                            CLIMode.ExtractFileIntoFolder(args[2], args.Length >= 4 ? args[3] : null);
                        }
                        else
                        {
                            CLIMode.BatchUnpackFolder(args[2]);
                        }
                        break;
                    case "-r":
                        if (!File.Exists(args[2]) && !Directory.Exists(args[2]))
                        {
                            MessageBox.Show($"No file/directory \"{args[2]}\" found.");
                            Application.Exit();
                        }
                        if (args.Length >= 4 && args[3] == "-b")
                        {
                            CLIMode.BatchRepackFolders(args[2]);
                        }
                        else
                        {
                            FileAttributes rattr = File.GetAttributes(args[2]);
                            if ((rattr & FileAttributes.Directory) != FileAttributes.Directory)
                            {
                                CLIMode.RepackFileInPlace(args[2]);
                            }
                            else
                            {
                                CLIMode.RepackFileFromFolder(args[2], args.Length >= 4 ? args[3] : null);
                            }
                        }
                        break;
                }
            } 
        }

        [STAThread]
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major < 1)
            {
                MessageBox.Show($"DatsuEditor is not compatible with any Windows operating system below Vista (NT 6.0)\nYou are currently running {Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}", "Unsupported OS");
                Environment.Exit(0);
            }

            bool CLIMode = false;
            if (args.Length > 0) { 
                if (args[0] == "-c")
                {
                    CLIMode = true;
                    DatsuCLI(args);
                    Application.Exit();
                }
            }

            if (!CLIMode) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += HandleDatsuException;
                Application.Run(new MainForm(args.Length == 1 ? args[0] : null));
            }
        }

        private static void HandleDatsuException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var now = DateTime.Now;
            var reportFileName = $"DatsuCrash_{now.Day:D2}{now.Month:D2}{now.Year:D2}_{now.Hour:D2}{now.Minute:D2}{now.Second:D2}.txt";
            StringBuilder reportBuilder = new StringBuilder();
            reportBuilder.AppendLine(e.Exception.Message);
            reportBuilder.AppendLine("\nStack Trace:");
            reportBuilder.AppendLine(e.Exception.StackTrace);

            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + reportFileName, reportBuilder.ToString());
            MessageBox.Show(e.Exception.Message + $"\n\nMore info saved into \"{reportFileName}\" in DatsuEditor's folder.", "Unhandled Exception");
        }
    }
}