using System;
using System.Collections;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Morin.App.Common.Code
{
    public class HotKey
    {

        int keyId;         //热键编号
        IntPtr handle;     //窗体句柄
        Window window;     //热键所在窗体
        uint controlKey;   //热键控制键
        uint key;          //热键主键

        public delegate void OnHotKeyEventHandler();     //热键事件委托
        public event OnHotKeyEventHandler OnHotKey = null;   //热键事件

        static Hashtable KeyPair = new Hashtable();         //热键哈希表
        private const int WM_HOTKEY = 0x0312;       // 热键消息编号

        public enum KeyFlags    //控制键编码        
        {
            MOD_ALT = 0x1,
            MOD_CONTROL = 0x2,
            MOD_SHIFT = 0x4,
            MOD_WIN = 0x8
        }


        ///<summary>
        /// 构造函数
        ///</summary>
        ///<param name="win">注册窗体</param>
        ///<param name="control">控制键</param>
        ///<param name="key">主键</param>
        public HotKey(Window win, KeyFlags control, Keys key)
        {
            handle = new WindowInteropHelper(win).Handle;
            window = win;
            controlKey = (uint)control;
            this.key = (uint)key;
            keyId = (int)controlKey + (int)this.key * 10;

            if (KeyPair.ContainsKey(keyId))
            {
                //throw new Exception("热键已经被注册!");
            }

            //注册热键
            if (false == RegisterHotKey(handle, keyId, controlKey, this.key))
            {
                // throw new Exception("热键注册失败!");
            }

            //消息挂钩只能连接一次!!
            if (KeyPair.Count == 0)
            {
                if (false == InstallHotKeyHook(this))
                {
                    //throw new Exception("消息挂钩连接失败!");
                }
            }

            //添加这个热键索引
            KeyPair.Add(keyId, this);
        }

        ///<summary>
        /// 构造函数
        ///</summary>
        ///<param name="win">注册窗体</param>
        ///<param name="control">控制键</param>
        ///<param name="key">主键</param>
        public HotKey(Window win, Keys key)
        {
            handle = new WindowInteropHelper(win).Handle;
            window = win;
            controlKey = (uint)0;
            this.key = (uint)key;
            keyId = (int)controlKey + (int)this.key * 10;

            if (KeyPair.ContainsKey(keyId))
            {
                //throw new Exception("热键已经被注册!");
            }

            //注册热键
            if (false == RegisterHotKey(handle, keyId, controlKey, this.key))
            {
                // throw new Exception("热键注册失败!");
            }

            //消息挂钩只能连接一次!!
            if (KeyPair.Count == 0)
            {
                if (false == InstallHotKeyHook(this))
                {
                    //throw new Exception("消息挂钩连接失败!");
                }
            }

            //添加这个热键索引
            KeyPair.Add(keyId, this);
        }

        //析构函数,解除热键
        ~HotKey()
        {
            UnregisterHotKey(handle, keyId);
        }


        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint controlKey, uint virtualKey);

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //安装热键处理挂钩
        static private bool InstallHotKeyHook(HotKey hk)
        {
            if (hk.window == null || hk.handle == IntPtr.Zero)
            {
                return false;
            }

            //获得消息源
            System.Windows.Interop.HwndSource source = HwndSource.FromHwnd(hk.handle);
            if (source == null)
            {
                return false;
            }

            //挂接事件            
            source.AddHook(HotKeyHook);
            return true;
        }

        //热键处理过程
        static private IntPtr HotKeyHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                HotKey hk = (HotKey)KeyPair[(int)wParam];
                if (hk.OnHotKey != null)
                {
                    hk.OnHotKey();
                }
            }
            return IntPtr.Zero;
        }
    }
}
