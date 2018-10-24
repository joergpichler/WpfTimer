using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += DispatcherTimerOnTick;

            _viewModel = (MainViewModel) DataContext;
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void DispatcherTimerOnTick(object sender, EventArgs e)
        {
            var remainingTime = GetRemainingTime();

            if (remainingTime.TotalMilliseconds >= 0)
            {
                _viewModel.StopCommand.Execute(null);
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("Timer finished!", "WpfTimer");
            }
            else
            {
                TextBlockRemainingTime.Text = FormatTimeSpan(remainingTime);
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.IsRunning))
            {
                if (_viewModel.IsRunning)
                {
                    _dispatcherTimer.Start();
                }
                else
                {
                    _dispatcherTimer.Stop();
                    TextBlockRemainingTime.Text = null;
                }
            }
        }

        private TimeSpan GetRemainingTime()
        {
            return DateTime.Now - _viewModel.TargetDateTime.Value;
        }

        private string FormatTimeSpan(TimeSpan remainingTime)
        {
            return remainingTime.ToString(@"dd\.hh\:mm\:ss");
        }
    }
}
