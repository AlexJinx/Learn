using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadAndTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            var result = Task.Run(new Func<string>(DoForLoop));
            txtOne.Text = result.Result;
            txtMain.Text = "主线程结束One";
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            var result = DoForLoopAsync();
            txtTwo.Text = result.Result;
            txtMain.Text = "主线程结束Two";
        }

        private async void btnThree_Click(object sender, EventArgs e)
        {
            var result = await DoForLoopAsync();

            txtMain.Text = "主线程结束Three";

        }

        //普通方法
        private string DoForLoop()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(i);
            }
            Thread.Sleep(2000);
            return "完成";
        }

        //异步方法
        private Task<string> DoForLoopAsync()
        {
            var task = Task.Run<string>(() =>
           {
               for (int i = 0; i < 1000; i++)
               {
                   Console.WriteLine(i);
               }
               Thread.Sleep(2000);
               return "完成";
           });

            task.GetAwaiter().OnCompleted(() =>
            {
                txtThree.Text = task.Result;
            });
            return task;
        }
    }
}
