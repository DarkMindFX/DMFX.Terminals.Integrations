using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DMFX.cTraderSetup.Model
{
    public class SetupProcess : ObservableObject
    {
        private string _licenseText = string.Empty;
        private int _progress = 0;

        private string _cAlgoPath = string.Empty;
        private string _cAlgoIndicatorsPath = string.Empty;

        public SetupProcess()
        {
            FindCAlgo();
        }

        private void FindCAlgo()
        {
            _cAlgoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "cAlgo");
            if (Directory.Exists(_cAlgoPath))
            {
                _cAlgoIndicatorsPath = Path.Combine(_cAlgoPath, "Sources", "Indicators");
            }
            else
            {
                _cAlgoPath = string.Empty;
            }
        }

        public string AlgoIndicatorsPath
        {
            get
            {
                return _cAlgoIndicatorsPath;
            }
        }

        public string LicenseText
        {
            get
            {
                if (string.IsNullOrEmpty(_licenseText))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    _licenseText = DMFX.cTraderSetup.Properties.Resources.DMFX_cTrader_License;
                }
                return _licenseText;
            }
        }

        public void InstallIndicators()
        {
            Progress = 0;
  
            string indicatorsFolder = _cAlgoIndicatorsPath;

            // removing old files
            string[] indFiles = Directory.GetFiles(indicatorsFolder);
            foreach (var indFile in indFiles)
            {
                try
                {
                    if (indFile.Contains("DMFX."))
                    {
                        File.Delete(indFile);
                    }
                }//try
                catch
                {
                    // TODO: need to log if any errors occur
                }//catch
            }

            // extracting zip
            byte[] zipBytes = DMFX.cTraderSetup.Properties.Resources.cTrader_274570A67AD55CE71EB0627146FC8CC772E5EDC6;
            MemoryStream ms = new MemoryStream(zipBytes);

            using (ZipFile zip = ZipFile.Read(ms))
            {
                zip.ExtractAll(indicatorsFolder, ExtractExistingFileAction.OverwriteSilently);
                zip.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(ZipExtractProgress);
            }

            Progress = 99;

            // copying all dll files 
            var enumFiles = Directory.EnumerateFiles(indicatorsFolder);
            foreach (var file in enumFiles)
            {
                if (Path.GetExtension(file) == ".dll")
                {
                    string dstFile = Path.Combine(_cAlgoPath, Path.GetFileName(file));
                    if (File.Exists(dstFile))
                    {
                        File.Delete(dstFile);
                    }
                    File.Move(file, dstFile);

                }
            }

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
