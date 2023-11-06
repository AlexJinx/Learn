using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;

namespace VLCDemoPri;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Timer _timerPlay, _timerON, _timerOFF, _timerJumpBegin, _timerJumpEnd, _timer;
    private readonly TimeOnly _timeON, _timeOFF, _timeJumpBegin, _timeJumpEnd;

    private string[] _videos, _jumpVideos;

    private readonly LibVLC _libVlc;
    private readonly MediaPlayer _player;
    private Media _media, _mediaDefault;

    private int _idx = 0, _idxJump = 0;
    private bool _isDefault = false, _isJump = false;

    public MainWindow()
    {
        InitializeComponent();

        // 模拟开/关屏时间
        _timeON = TimeOnly.Parse("13:45");
        _timeOFF = TimeOnly.Parse("14:30");

        // 插播开始/结束
        _timeJumpBegin = TimeOnly.Parse("14:27:15");
        _timeJumpEnd = TimeOnly.Parse("14:27:30");

        _timer = new() { Interval = 1000 };
        _timer.Elapsed += (sender, e) => Dispatcher.Invoke(() => LblTimer.Content = DateTime.Now.ToString("HH:mm:ss"));
        _timer.Start();

        #region 初始化计时器
        // 开屏计时器
        _timerON = new() { Interval = 1000 };
        _timerON.Elapsed += TimerON_Elapsed;

        // 关屏计时器
        _timerOFF = new() { Interval = 1000 };
        _timerOFF.Elapsed += TimerOFF_Elapsed;

        // 插播计时器
        _timerJumpBegin = new() { Interval = 1000 };
        _timerJumpBegin.Elapsed += TimerJumpBegin_Elapsed;
        _timerJumpEnd = new() { Interval = 1000 };
        _timerJumpEnd.Elapsed += TimerJumpEnd_Elapsed;

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
            _timerJumpBegin.Start();   //  开启插播计时器
        };
        #endregion

        #region 获取素材
        // 获取全部待播放的素材
        Loaded += (sender, e) =>
        {
            _videos = Directory.GetFiles("E:\\LEDTestMedia");
            _jumpVideos = Directory.GetFiles("E:\\Test2");
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
        if (now < _timeOFF || _isJump)   // 时间没到或插播没完
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

    #region 插播计时器事件
    private void TimerJumpBegin_Elapsed(object sender, ElapsedEventArgs e)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        if (now < _timeJumpBegin)   // 未到插播开始时间
        {
            return;
        }

        _isJump = true;
        _timerJumpBegin.Dispose();   // 释放插播开始计时器

        if (now > _timeJumpEnd)   // 超过插播结束时间
        {
            _isJump = false;
            _timerJumpEnd.Dispose();

            return;
        }

        _timerPlay.Stop();
        _timerJumpEnd.Start();   // 开启插播结束计时器

        _media = new(_libVlc, _jumpVideos[_idxJump++]);
        _player.Play(_media);
    }

    private void TimerJumpEnd_Elapsed(object sender, ElapsedEventArgs e)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        if (now < _timeJumpEnd)   // 未到插播结束时间
        {
            return;
        }

        _isJump = false;
        _timerJumpEnd.Dispose();

        // 继续播放原始列表
        _timerPlay.Stop();

        if (_idx == _videos.Length)
        {
            _idx = 0;
        }

        _media = new(_libVlc, _videos[_idx]);
        _player.Play(_media);
    }
    #endregion

    #region 播放器事件
    private void Player_Playing(object sender, EventArgs e)
    {
        Dispatcher.Invoke(() => Lbl.Content = _player.Media.Mrl);

        if (_isDefault)
        {
            return;
        }

        _timerPlay.Start();   // 开启播放计时

        if (_isJump)
        {
            if (_idxJump == _jumpVideos.Length)
            {
                _idxJump = 0;
            }
        }
        else
        {
            if (_idx == _videos.Length)
            {
                _idx = 0;
            }
        }

        _media = new(_libVlc, _isJump ? _jumpVideos[_idxJump++] : _videos[_idx++]);   // 准备下一个素材
    }
    #endregion

    protected override void OnClosed(EventArgs e)
    {
        _media?.Dispose();   // 释放最后一个播放的素材
        _mediaDefault?.Dispose();   // 释放默认素材

        _libVlc.Dispose();
        _player.Dispose();
        _timerPlay.Dispose();

        _timer.Dispose();

        base.OnClosed(e);
    }
}