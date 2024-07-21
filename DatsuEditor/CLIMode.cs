using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZanLibrary;
using ZanLibrary.Dat;

namespace DatsuEditor
{
    internal static class CLIMode
    {
        public static void BatchRepackFolders(string path)
        {
            string[] DatFolders = Directory.GetDirectories(path);
            foreach (var datfolder in DatFolders)
            {
                RepackFileFromFolder(datfolder);
            }
        }
        public static void BatchUnpackFolder(string path)
        {
            string[] DatFiles = Directory.GetFiles(path);
            foreach (var datPath in DatFiles)
            {
                byte[] magic = new byte[4];
                File.OpenRead(datPath).Read(magic, 0, 4);
                if (FormatUtils.DetectFileFormat(magic) == MGRFileFormat.DAT)
                    ExtractFileIntoFolder(datPath);
            }
        }
        public static void RepackFileFromFolder(string Input, string Output = null)
        {
            string OutputFilename = "";

            if (Input.Substring(Input.Length-4, 1) == "_")
            {
                OutputFilename = Input.Substring(0, Input.Length-4) + "." + Input.Substring(Input.Length-3,3);
            }
            else
                OutputFilename = Input + ".dat";

            if (Output != null)
                OutputFilename = Output;

            // MessageBox.Show(OutputFilename);

            bool useDatabase = true;
            List<string> fileDatabase = new List<string>();
            var dbLines = File.Exists(Input + "\\.datsudb") ? File.ReadAllLines(Input + "\\.datsudb") : new string[1];
            if (dbLines[0] != "@DatsuEditorFileDatabase")
                useDatabase = false;
            // MessageBox.Show(useDatabase.ToString());
            fileDatabase = dbLines.Skip(1).ToList();
            List<DatFileEntry> files = new List<DatFileEntry>();
            string[] DirFiles = Directory.GetFiles(Input);
            List<string> unlistedFiles = new List<string>();
            if (useDatabase)
            {
                foreach (var dbFile in fileDatabase)
                {
                    // MessageBox.Show(dbFile);
                    if (File.Exists(Input + "\\" + dbFile))
                        files.Add(new DatFileEntry(dbFile, File.ReadAllBytes(Input + "\\" + dbFile)));    
                }
                unlistedFiles = DirFiles.Where((x) => !fileDatabase.Contains(Path.GetFileName(x)) && !x.EndsWith(".datsudb")).ToList();
            }
            else
                unlistedFiles = DirFiles.Where((x) => !x.EndsWith(".datsudb")).ToList();
            foreach (var unlistedFile in unlistedFiles)
            {
                files.Add(new DatFileEntry(Path.GetFileName(unlistedFile), File.ReadAllBytes(unlistedFile)));
            }
            File.WriteAllBytes(OutputFilename, DatFile.Save(files.ToArray()));
        }
        public static void RepackFileInPlace(string Filepath)
        {
            var datEntries = DatFile.Load(File.ReadAllBytes(Filepath));
            File.WriteAllBytes(Filepath, DatFile.Save(datEntries));
        }
        public static void ExtractFileIntoFolder(string Input, string Output = null)
        {
            StringBuilder nameBuilder = new StringBuilder(Input);
            nameBuilder[Input.LastIndexOf('.')] = '_';
            string OutputDirectory = nameBuilder.ToString();
            var datEntries = DatFile.Load(File.ReadAllBytes(Input));
            if (Output != null)
                OutputDirectory = Output;

            StringBuilder fileBaseBuilder = new StringBuilder();
            fileBaseBuilder.AppendLine("@DatsuEditorFileDatabase");
            if (!Directory.Exists(OutputDirectory))
                Directory.CreateDirectory(OutputDirectory);
            foreach (var entry in datEntries)
            {
                fileBaseBuilder.AppendLine(entry.Name);
                File.WriteAllBytes(OutputDirectory + "\\" + entry.Name, entry.Data);
            }
            File.WriteAllText(OutputDirectory + "\\" + ".datsudb", fileBaseBuilder.ToString());
        }
    }
}
