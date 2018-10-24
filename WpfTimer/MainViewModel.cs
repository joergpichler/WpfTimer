using System;
using System.Windows.Input;
using WpfTimer.Mvvm;

namespace WpfTimer
{
    public class MainViewModel : NotifyPropertyChangedBase
    {
        private bool _isRunning;

        public MainViewModel()
        {
            StartCommand = new RelayCommand(StartCommandExecute, StartCommandCanExecute);
            StopCommand = new RelayCommand(StopCommandExecute, StopCommandCanExecute);
        }

        public string Hours { get; set; }

        public string Minutes { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public DateTime? TargetDateTime { get; private set; }

        private void StartCommandExecute(object obj)
        {
            TargetDateTime = DateTime.Now.Add(TimeSpan.FromHours(double.Parse(Hours)))
                .Add(TimeSpan.FromMinutes(double.Parse(Minutes)));
            IsRunning = true;
        }

        private bool StartCommandCanExecute(object obj) => !IsRunning;

        private void StopCommandExecute(object obj)
        {
            TargetDateTime = null;
            IsRunning = false;
        }

        private bool StopCommandCanExecute(object obj) => IsRunning;
    }
}