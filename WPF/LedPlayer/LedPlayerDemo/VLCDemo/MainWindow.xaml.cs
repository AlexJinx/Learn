using LibVLCSharp.Shared;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

using SystemTimer = System.Timers.Timer;

namespace VLCDemo;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private int _idx = 0;
    private readonly SystemTimer _playTimer;
    private readonly SystemTimer _checkTimer;
    private readonly string[] _videos;

    private bool _isPlayer1 = false;
    private bool _player1IsLoaded = false;
    private bool _player2IsLoaded = false;

    private Media _media1;
    private LibVLC _libVlc1;
    private MediaPlayer _player1;

    private Media _media2;
    private LibVLC _libVlc2;
    private MediaPlayer _player2;

    public MainWindow()
    {
        InitializeComponent();
        AllowsTransparency = true;
        WindowStyle = WindowStyle.None;
        WindowStartupLocation = WindowStartupLocation.Manual;

        _videos = Directory.GetFiles(@"D:\Project\BackEnd\coding\led.client\Led.Client\Led.Client\bin\Debug\net7.0-windows\material");

        _playTimer = new()
        {
            AutoReset = false
        };
        _playTimer.Elapsed += (s, e) => Play();

        _isPlayer1 = true;
        _libVlc1 = new();
        _player1 = new(_libVlc1);
        _player1.Playing += Player1_Playing;
        Wrapper1.MediaPlayer = _player1;

        _libVlc2 = new();
        _player2 = new(_libVlc2);
        _player2.Playing += Player2_Playing;
        Wrapper2.MediaPlayer = _player2;

        _checkTimer = new()
        {
            AutoReset = true,
            Interval = 100
        };
        _checkTimer.Elapsed += (s, e) => CheckPlayer();
    }

    private void CheckPlayer()
    {
        if (!_isPlayer1)
        {
            Debug.WriteLine($"Player1的位置：{_player1.Position}");
            if (_player1.Position > 0)
            {
                Player1_Playing();
                _checkTimer.Stop();
            }
        }
        else
        {
            Debug.WriteLine($"Player2的位置：{_player2.Position}");
            if (_player2.Position > 0)
            {
                Player2_Playing();
                _checkTimer.Stop();
            }
        }
    }

    private void Player1_Playing()
    {
        Dispatcher.Invoke(() =>
        {
            Wrapper1.Visibility = Visibility.Visible;
            Wrapper2.Visibility = Visibility.Hidden;
        });
    }

    private void Player2_Playing()
    {
        Dispatcher.Invoke(() =>
        {
            Wrapper2.Visibility = Visibility.Visible;
            Wrapper1.Visibility = Visibility.Hidden;
        });
    }

    private void Player1_Playing(object sender, EventArgs e)
    {
        _checkTimer.Start();
    }

    private void Player2_Playing(object sender, EventArgs e)
    {
        _checkTimer.Start();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        StartPlay();
    }

    public void StartPlay()
    {
        _isPlayer1 = true;
        _media1 = new(_libVlc1, _videos.FirstOrDefault());
        _player1.Media = _media1;
        _idx = 0;
        _playTimer.Start();
    }

    public void Play()
    {
        try
        {
            Dispatcher.Invoke(() =>
            {
                Wrapper1.MediaPlayer ??= _player1;
                Wrapper2.MediaPlayer ??= _player2;
            });

            _playTimer.Interval = 15 * 1000;
            _playTimer.Start();

            if (_isPlayer1)
            {
                _player1.Play();
                Debug.WriteLine($"Player1开始播放");
            }
            else
            {
                _player2.Play();
                Debug.WriteLine($"Player2开始播放");
            }

            if (++_idx >= _videos.Length)
            {
                _idx = 0;
            }
        }
        finally
        {
            _media1?.Dispose();
            _media2?.Dispose();
            ReadyNext();
        }
    }

    public void ReadyNext()
    {
        if (_isPlayer1)
        {
            _media2 = new(_libVlc2, _videos[_idx]);
            _player2.Media = _media2;
            _isPlayer1 = false;
        }
        else
        {
            _media1 = new(_libVlc1, _videos[_idx]);
            _player1.Media = _media1;
            _isPlayer1 = true;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _media1 = new(_libVlc1, _videos[_idx]);

        _player1.Pause();
        _player1.Play();
        _player1.Position = 0;
    }

    //private void Button_Click_1(object sender, RoutedEventArgs e)
    //{
    //    _player1.Play();
    //}
}