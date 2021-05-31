using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadAndTask
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //AllocConsole();
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Test");
            Console.WriteLine("111 Thread ID ： " + Thread.CurrentThread.ManagedThreadId);
            var ResultTask = AsyncMethod();
            Console.WriteLine(ResultTask.Result);
            Console.WriteLine("222 Thread ID ： :" + Thread.CurrentThread.ManagedThreadId);
        }


        private async Task<string> AsyncMethod()
        {
            var ResultFromTimeConsumingMethod = TimeConsumingMethod();
            string Result = await ResultFromTimeConsumingMethod + " + AsyncMethod. My Thread ID is :" + Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(Result);
            return Result;
        }


        //这个函数就是一个耗时函数，可能是IO操作，也可能是cpu密集型工作。
        private Task<string> TimeConsumingMethod()
        {
            var task = Task.Run(() =>
            {
                Console.WriteLine("Helo I am TimeConsumingMethod. My Thread ID is :" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(5000);
                Console.WriteLine("Helo I am TimeConsumingMethod after Sleep(5000). My Thread ID is :" + Thread.CurrentThread.ManagedThreadId);
                return "Hello I am TimeConsumingMethod";
            });

            return task;
        }
    }
}
