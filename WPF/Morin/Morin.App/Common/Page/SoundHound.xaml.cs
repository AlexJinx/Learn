using Oraycn.MCapture;
using Oraycn.MFile;

using System;
using System.Windows;
using System.Windows.Input;

namespace Morin.App.Common.Page
{
    /// <summary>
    /// SoundHound.xaml 的交互逻辑
    /// </summary>
    public partial class SoundHound : Window
    {
        public SoundHound()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (listen.IsWaving == true)
            {
                start.Content = "开始识别";
                listen.IsWaving = false;
            }
            else
            {
                Start();
                start.Content = "停止识别";
                listen.IsWaving = true;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            start.Content = "开始识别";
            listen.IsWaving = false;
            Visibility = Visibility.Hidden;
        }

        private ISoundcardCapturer soundcardCapturer;
        private AudioFileMaker audioFileMaker;

        public void Start()
        {
            try
            {
                int audioSampleRate = 16000;
                int channelCount = 1;
                //int audioSampleRate = 44100;
                //int channelCount = 2;

                //声卡采集器 【目前声卡采集仅支持vista以及以上系统】
                soundcardCapturer = CapturerFactory.CreateSoundcardCapturer();
                //this.soundcardCapturer.CaptureError += capturer_CaptureError;
                audioSampleRate = this.soundcardCapturer.SampleRate;
                channelCount = this.soundcardCapturer.ChannelCount;

                soundcardCapturer.AudioCaptured += audioMixter_AudioMixed;
                soundcardCapturer.Start();

                audioFileMaker = new AudioFileMaker();
                audioFileMaker.Initialize("test.mp3", audioSampleRate, channelCount);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        public void Stop()
        {
            soundcardCapturer.Stop();
            audioFileMaker.Close(true);
        }
        void audioMixter_AudioMixed(byte[] audioData)
        {
            audioFileMaker.AddAudioFrame(audioData);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
