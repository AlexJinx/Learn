using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MediaElementInstanceDemo;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    readonly Timer _timer;
    readonly string[] _videos;

    int _idx;
    MediaElement _player1, _player2;

    public MainWindow()
    {
        InitializeComponent();
        AllowsTransparency = true;
        WindowStyle = WindowStyle.None;
        WindowStartupLocation = WindowStartupLocation.Manual;

        _idx = 0;
        _videos = Directory.GetFiles(@"D:\Project\BackEnd\coding\led.client\Led.Client\Led.Client\bin\Debug\net7.0-windows\material");
        _videos = new string[] { @"D:\Project\BackEnd\coding\led.client\Led.Client\Led.Client\bin\Debug\net7.0-windows\material\e10fc9744acadcd7f8ba15eb0abaa628.mp4" };
        _timer = new()
        {
            Interval = 10000
        };

        _timer.Elapsed += Timer_Elapsed;

        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _player1 = InitPlayer();
        _player1.Source = new(_videos[_idx], UriKind.Absolute);

        Wrapper.Children.Add(_player1);

        _player2 = PreparePlayer();

        _timer.Start();
        _player1.Play();
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        Dispatcher.Invoke(_player2.Play);
    }

    private void Player_MediaOpened(object sender, RoutedEventArgs e)
    {
        if (sender == _player2)
        {
            Wrapper.Children.Add(_player2);
        }
    }

    private MediaElement InitPlayer()
    {
        var player = new MediaElement
        {
            LoadedBehavior = MediaState.Manual,
            UnloadedBehavior = MediaState.Manual,
            //Stretch = Stretch.UniformToFill
        };

        player.MediaOpened += Player_MediaOpened;
        return player;
    }

    private MediaElement PreparePlayer()
    {
        var player = InitPlayer();
        player.Loaded += Player_Loaded;

        if (++_idx == _videos.Length)
        {
            _idx = 0;
        }

        player.Source = new(_videos[_idx], UriKind.Absolute);
        return player;
    }

    private void Player_Loaded(object sender, RoutedEventArgs e)
    {
        _player1.Stop();
        _player1.Close();
        Wrapper.Children.Remove(_player1);

        _player1 = _player2;
        _player2 = PreparePlayer();
    }

    protected override void OnClosed(EventArgs e)
    {
        _player1.Stop();
        _player1.Close();

        _player2.Stop();
        _player2.Close();

        _timer.Dispose();

        base.OnClosed(e);
    }
}