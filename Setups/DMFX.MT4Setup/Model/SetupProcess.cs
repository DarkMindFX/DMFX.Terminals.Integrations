using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMFX.MT4Setup.Model
{
    public class SetupProcess : ObservableObject
    {
        private string _licenseText = string.Empty;
        private int _progress = 0;

        public class MT4BrokerInfo
        {
            public string Name
            {
                get;
                set;
            }
            public string InstallFolder
            {
                get;
                set;
            }
            public string RoamingFolder
            {
                get;
                set;
            }
        }

        private Dictionary<string, MT4BrokerInfo> _MT4BrokersInfo = new Dictionary<string, MT4BrokerInfo>();

        private void PopulateMT4BrokersList()
        {

            // searching all files in Roaming under Terminals folder
            var terminalsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.Combine("MetaQuotes", "Terminal"));

            if (Directory.Exists(terminalsFolder))
            {

                foreach (var subDir in Directory.GetDirectories(terminalsFolder))
                {
                    var terminalDir = subDir;
                    var originFile = Path.Combine(terminalDir, "origin.txt");
                    if (File.Exists(originFile))
                    {
                        MT4BrokerInfo mt4BrokerInfo = new MT4BrokerInfo();
                        mt4BrokerInfo.InstallFolder = File.ReadAllText(originFile);
                        mt4BrokerInfo.RoamingFolder = terminalDir;
                        mt4BrokerInfo.Name = mt4BrokerInfo.InstallFolder.Split('\\').Last();

                        if (Directory.Exists(mt4BrokerInfo.InstallFolder))
                        {
                            _MT4BrokersInfo.Add(mt4BrokerInfo.Name, mt4BrokerInfo);
                        }
                    }
                }
            }

            // checking program files folder - for old MT4 installations
            var programFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            if (Directory.Exists(programFilesFolder))
            {
                foreach (var subDir in Directory.GetDirectories(programFilesFolder))
                {
                    var dirs = Directory.GetDirectories(subDir);
                    var mql4Dir = dirs.FirstOrDefault(x => x.Contains("MQL4"));
                    if (mql4Dir != null)
                    {
                        MT4BrokerInfo mt4BrokerInfo = new MT4BrokerInfo();
                        mt4BrokerInfo.InstallFolder = subDir;
                        mt4BrokerInfo.RoamingFolder = subDir;
                        mt4BrokerInfo.Name = mt4BrokerInfo.InstallFolder.Split('\\').Last();

                        if (Directory.Exists(mt4BrokerInfo.InstallFolder) && !_MT4BrokersInfo.ContainsKey(mt4BrokerInfo.Name))
                        {
                            _MT4BrokersInfo.Add(mt4BrokerInfo.Name, mt4BrokerInfo);
                        }
                    }

                }
            }

        }

        public string LicenseText
        {
            get
            {
                if (string.IsNullOrEmpty(_licenseText))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    _licenseText = DMFX.MT4Setup.Properties.Resources.DMFX_MT4_License;
                }
                return _licenseText;
            }
        }

        public Dictionary<string, MT4BrokerInfo> MT4Brokers
        {
            get
            {
                if (_MT4BrokersInfo.Count() == 0)
                {
                    PopulateMT4BrokersList();
                    if (_MT4BrokersInfo.Count > 0)
                    {
                        Terminal = _MT4BrokersInfo.First().Key;
                    }
                }
                return _MT4BrokersInfo;
            }
        }

        public string Terminal
        {
            get;
            set;
        }

        public void InstallIndicators()
        {

            Progress = 0;
            string installDir = _MT4BrokersInfo[Terminal].InstallFolder;
            string roamingDir = _MT4BrokersInfo[Terminal].RoamingFolder;

            string indicatorsFolder = Path.Combine(roamingDir, "MQL4", "Indicators");

            var processes = Process.GetProcessesByName("terminal");
            foreach (var prc in processes)
            {
                if (prc.MainModule.FileName.Contains(installDir))
                {
                    prc.Kill();
                }
            }

            Progress = 20;

            // removing old files
            try
            {
                string dmfxIndicatorsFolder = Path.Combine(indicatorsFolder, "DarkMindFx");
                if (Directory.Exists(dmfxIndicatorsFolder))
                {
                    Directory.Delete(dmfxIndicatorsFolder, true);
                }
            }
            catch
            {
                // catching exception - if for some reason we cannot remove old files - just continue installing
            }

            Progress = 50;

            // extracting zip
            byte[] zipBytes = DMFX.MT4Setup.Properties.Resources.MetaTrader4;
            MemoryStream ms = new MemoryStream(zipBytes);

            using (ZipFile zip = ZipFile.Read(ms))
            {
                zip.ExtractAll(indicatorsFolder, ExtractExistingFileAction.OverwriteSilently);
            }

            Progress = 90;

            // copying indicators
            var fldIndicators = Path.Combine(indicatorsFolder, "MT4Package\\.indicators\\DarkMindFx");

            Directory.Move(fldIndicators, Path.Combine(indicatorsFolder, "DarkMindFx"));

            // copying all .dlls files except
            var fldDlls = Directory.EnumerateFiles(Path.Combine(indicatorsFolder, "MT4Package\\.dlls"));
            foreach (var file in fldDlls)
            {
                string dstFile = Path.Combine(installDir, Path.GetFileName(file));
                if (File.Exists(dstFile))
                {
                    File.Delete(dstFile);
                }
                File.Move(file, dstFile);
            }

            Directory.Delete(Path.Combine(indicatorsFolder, "MT4Package"), true);


            Progress = 100;


        }

        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    RaisePropertyChangedEvent("Progress");
                }
            }
        }


        private void ZipExtractProgress(object sender, ExtractProgressEventArgs args)
        {
            Progress = (int)((double)args.EntriesExtracted / (double)args.EntriesTotal * 100);
        }

    }
}
