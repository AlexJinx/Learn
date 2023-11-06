using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;

namespace VLCDemo2;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Timer _timerPlay, _timerON, _timerOFF;
    private readonly TimeOnly _timeON, _timeOFF;

    private string[] _videos;

    private readonly LibVLC _libVlc;
    private readonly MediaPlayer _player;
    private Media _media, _mediaDefault;

    private int _idx = 0;
    private bool _isDefault = false;

    public MainWindow()
    {
        InitializeComponent();

        // 模拟开/关屏时间
        _timeON = TimeOnly.Parse("10:29");
        _timeOFF = TimeOnly.Parse("10:30");

        #region 初始化计时器
        // 开屏计时器
        _timerON = new() { Interval = 1000 };
        _timerON.Elapsed += TimerON_Elapsed;

        // 关屏计时器
        _timerOFF = new() { Interval = 1000 };
        _timerOFF.Elapsed += TimerOFF_Elapsed;

        // 素材播放计时器
        _timerPlay = new() { Interval = 10 * 1000 };
        _timerPlay.Elapsed += (sender, e) =>
        {
            (sender as Timer).Stop();   // 停止播放计时
            _player.Play(_media);
        };
        #endregion

        #region 初始化并加载播放器
        // 初始化播放器
        _libVlc = new();
        _player = new(_libVlc);
        _player.Opening += (sender, e) => _media?.Dispose();   // 释放当前播放素材
        _player.Playing += Player_Playing;

        // 加载播放器
        Wrapper.Loaded += (sender, e) =>
        {
            (sender as VideoView).MediaPlayer = _player;
            _timerON.Start();   // 开启开屏计时器
        };
        #endregion

        #region 获取素材
        // 获取全部待播放的素材
        Loaded += (sender, e) =>
        {
            _videos = Directory.GetFiles(@"E:\LEDTestMedia");
            _mediaDefault = new(_libVlc, _videos.Last());
        };
        #endregion
    }

    #region 开屏计时器事件
    private void TimerON_Elapsed(object sender, ElapsedEventArgs e)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        if (now < _timeON)   // 未到开屏时间
        {
            if (!_player.IsPlaying)   // 播放默认素材
            {
                _isDefault = true;
                _player.Play(_mediaDefault);
            }

            return;
        }

        _timerON.Dispose();   // 释放开屏计时器

        if (now > _timeOFF)   // 超过关屏时间
        {
            // 播放默认素材
            _isDefault = true;
            _player.Play(_mediaDefault);

            _timerOFF.Dispose();
            return;
        }

        _isDefault = false;
        _timerOFF.Start();   // 开启关屏计时器

        // 开始播放
        _media = new(_libVlc, _videos[_idx++]);
        _player.Play(_media);
    }
    #endregion

    #region 关屏计时器事件
    private void TimerOFF_Elapsed(object sender, ElapsedEventArgs e)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        if (now < _timeOFF)
        {
            return;
        }

        _timerOFF.Dispose();   // 释放关屏计时器

        #region 方案一：播放默认素材
        _isDefault = true;
        _player.Play(_mediaDefault);
        #endregion

        #region 方案二：停止播放
        //_timerPlay.Stop();   // 停止播放计时

        //// 停止播放，并释放播放器内的素材
        //_player.Media.Dispose();
        //_player.Stop();
        #endregion
    }
    #endregion

    #region 播放器事件
    private void Player_Playing(object sender, EventArgs e)
    {
        if (_isDefault)
        {
            return;
        }

        _timerPlay.Start();   // 开启播放计时

        if (_idx == _videos.Length)
        {
            _idx = 0;
        }

        _media = new(_libVlc, _videos[_idx++]);   // 准备下一个素材
    }
    #endregion

    protected override void OnClosed(EventArgs e)
    {
        _media?.Dispose();   // 释放最后一个播放的素材
        _mediaDefault?.Dispose();   // 释放默认素材

        _libVlc.Dispose();
        _player.Dispose();
        _timerPlay.Dispose();

        base.OnClosed(e);
    }
}