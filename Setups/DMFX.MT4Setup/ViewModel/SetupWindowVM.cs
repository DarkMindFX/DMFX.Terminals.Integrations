using DMFX.MT4Setup.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace DMFX.MT4Setup.ViewModel
{
    public enum EScreens
    {
        Welcome,
        License,
        Brokers,
        Progress,
        Final
    }

    public class SetupWindowVM : ObservableObject
    {
        

        private EScreens _currentScreen = EScreens.Final;
        private bool _isLicenseAccepted = false;
        private int _progress = 0;
        private string _title = string.Empty;
        private List<Exception> _errors = new List<Exception>();
        private Window _window;

        private SetupProcess _setupProcess = new SetupProcess();

        public SetupWindowVM()
        {
            this.CmdFinish = new DelegateCommand<Window>(new Action<Window>(this.CmdFinishImpl));
            this.CmdCancel = new DelegateCommand<Window>(new Action<Window>(this.CmdCancelImpl));
            this.CmdBack = new DelegateCommand(this.CmdBackImpl);
            this.CmdNext = new DelegateCommand<Window>(new Action<Window>(this.CmdNextImpl));
            this.CmdDonate = new DelegateCommand(this.CmdDonateImpl);
           
            if (Brokers != null && Brokers.Count() > 0)
            {
                CurrentBroker = Brokers.First().Name;
            }
            CurrentScreen = EScreens.Welcome;
            _setupProcess.PropertyChanged += ModelPropertyChanged;
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Progress")
            {
                this.Progress = _setupProcess.Progress;
            }
        }

        public Visibility RequestDonate
        {
            get;
            set;
        }

        public EScreens CurrentScreen
        {
            get
            {
                return _currentScreen;
            }
            set
            {
                try
                {
                    if (_currentScreen != value)
                    {
                        _currentScreen = value;
                        RaisePropertyChangedEvent("CurrentScreen");

                        switch (_currentScreen)
                        {
                            case EScreens.Welcome:
                                Title = DMFX.MT4Setup.Properties.Resources.WelcomeTitle;
                                break;
                            case EScreens.License:
                                Title = DMFX.MT4Setup.Properties.Resources.LicenseAggrTitle;
                                if (Brokers == null || Brokers.Count() == 0)
                                {
                                    MessageBox.Show(_window,
                                        "No MetaTrader terminals were found on this machine. Setup will exit.",
                                        "DarkMindFX Indicators Package Setup", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                                    _errors.Add(new Exception("No MetaTrader terminals were found on this machine."));
                                    CurrentScreen = EScreens.Final;
                                    
                                }
                                break;
                            case EScreens.Brokers:
                                Title = DMFX.MT4Setup.Properties.Resources.BrokersTitle;
                                break;
                            case EScreens.Progress:
                                Title = DMFX.MT4Setup.Properties.Resources.ProgressTitle;
                                _setupProcess.InstallIndicators();
                                RequestDonate = Visibility.Visible;
                                break;
                            case EScreens.Final:
                                Title = DMFX.MT4Setup.Properties.Resources.DoneTitle;
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    RequestDonate = Visibility.Collapsed;
                    _errors.Add(ex);
                    ShowError(ex);                    
                    Progress = 100;
                }

                RaisePropertyChangedEvent("FinalText");
            }
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
                    if (_progress == 100)
                    {
                        CurrentScreen = EScreens.Final;
                    }
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChangedEvent("Title");
                }
            }
        }


        public string LicenseText
        {
            get
            {
                return _setupProcess.LicenseText;
            }
        }

        public string FinalText
        {
            get
            {
                return _errors.Count == 0 ? DMFX.MT4Setup.Properties.Resources.InstallOKTxt : DMFX.MT4Setup.Properties.Resources.InstallationFailTxt;
            }
        }

        

        public string VersionText
        {
            get
            {
                return string.Format(DMFX.MT4Setup.Properties.Resources.PackageVersionTxt, DMFX.MT4Setup.Properties.Resources.Version);
            }
        }

        public bool IsLicenseAccepted
        {
            get
            {
                return _isLicenseAccepted;
            }
            set
            {
                if (_isLicenseAccepted != value)
                {
                    _isLicenseAccepted = value;
                    RaisePropertyChangedEvent("IsLicenseAccepted");
                }
            }
        }

        public string CurrentBroker
        {
            get
            {
                return _setupProcess.Terminal;
            }

            set
            {
                _setupProcess.Terminal = value;
            }
        }

        public IEnumerable<SetupProcess.MT4BrokerInfo> Brokers
        {
            get
            {
                return _setupProcess.MT4Brokers.Values;
            }
        }

        public ICommand CmdNext
        {
            get;
            set;
        }

        public ICommand CmdBack
        {
            get;
            set;
        }

        public ICommand CmdCancel
        {
            get;
            set;
        }

        public ICommand CmdFinish
        {
            get;
            set;
        }

        public ICommand CmdDonate
        {
            get;
            set;
        }

        private void CmdDonateImpl()
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RA3ADYNALNCG6");
        }

        private void CmdNextImpl(Window window)
        {
            _window = window;
            int currentScreen = (int)CurrentScreen;
            ++currentScreen;
            CurrentScreen = (EScreens)currentScreen;
            
        }

        private void CmdBackImpl()
        {
            int currentScreen = (int)CurrentScreen;
            --currentScreen;
            CurrentScreen = (EScreens)currentScreen;
        }

        private void CmdCancelImpl(Window window)
        {
            if(MessageBox.Show(window, "Do you really want to cancel installation?", "DarkMindFX Indicators Package Setup", MessageBoxButton.YesNo, MessageBoxImage.Exclamation ) == MessageBoxResult.Yes)
            {
                window.Close();
            }
        }

        private void CmdFinishImpl(Window window)
        {
            window.Close();
        }

        private void ShowError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }

    }
}
