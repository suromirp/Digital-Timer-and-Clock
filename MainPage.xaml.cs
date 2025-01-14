using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Digital_Timer_Clock
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer clock;
        private DispatcherTimer timer;
        private TimeSpan remainingTime;
        private bool isPaused;
        private bool timerStarted = false;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeClock();
            LoadAlarmSound();
        }

        // Initializing clock with DispatchTimer();
        private void InitializeClock()
        {
            clock = new DispatcherTimer();
            clock.Interval = TimeSpan.FromSeconds(1);
            clock.Tick += SetClockTime;
            clock.Start();
        }

        // Send time to string
        private void SetClockTime(object sender, object e)
        {
            Clock.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void TimerClock_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Add your logic here
        }
        
        // Update timer with remainingTime and substracting seconds
        private void TimerUpdate(object sender, object e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                TimerClock.Text = remainingTime.ToString(@"hh\:mm\:ss");
            }
            else
            {
                timer.Stop();

                // Alarm sound when timer is over
                if (timerStarted && remainingTime.TotalSeconds == 0)
                {
                    TimerAlarm();
                    timerStarted = false;
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimePicker.SelectedTime.HasValue)
            {
                remainingTime = TimePicker.SelectedTime.Value;
                TimerClock.Text = remainingTime.ToString(@"hh\:mm\:ss");

                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += TimerUpdate;
                }
                timer.Start();
                isPaused = false;
                timerStarted = true;
            }
            else
            {
                TimerClock.Text = "No timer selected";
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null && timer.IsEnabled)
            {
                timer.Stop();
                isPaused = true;
                timerStarted = true;
            }
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null && isPaused)
            {
                timer.Start();
                isPaused = false;
                timerStarted = true;
                AlarmSound.Stop();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                remainingTime = TimeSpan.Zero;
                TimerClock.Text = "00:00:00";
                isPaused = false;
                timerStarted = false;
                AlarmSound.Stop();
            }
        }

        private void TimePicker_Hour_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Add logic here
        }

        private void TimePicker_Min_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Add logic here
        }

    private void TimerAlarm()
        {
            if (timerStarted)
            {
                AlarmSound.Play();
                TimerClock.Text = "Time's up!";
            }
        }

        private void LoadAlarmSound()
        {
            var uri = new Uri("ms-appx:///Assets/lofi-alarm-clock.mp3");
            AlarmSound.Source = uri;
            AlarmSound.AutoPlay = false;
        }
    }
}

