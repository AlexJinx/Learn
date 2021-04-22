using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Serializable
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("In Main");

            var result = ConsoleTest();
            //Console.WriteLine(result.Result);

            Console.WriteLine("Exit Main");

            Console.Read();



            //Console.WriteLine("主线程开始");
            //Task<string> task = Task<string>.Run(() =>
            //{
            //    Thread.Sleep(2000);
            //    return Thread.CurrentThread.ManagedThreadId.ToString();
            //});

            ////会等到任务执行完之后执行
            //task.GetAwaiter().OnCompleted(() =>
            //{
            //    Console.WriteLine(task.Result);
            //});

            //Console.WriteLine("主线程结束");
            //Console.Read();


            //// A thread will be created to run
            //// function1 parallely
            //Thread obj1 = new Thread(Function1);
            //obj1.IsBackground = false;
            //obj1.Start();

            //// the control will come here and exit the main function
            //Console.WriteLine("The main application has exited");


            //return;

            //ReadTxt();
            //return;
            //Person person = new Person
            //{
            //    No = "007",
            //    Age = 18,
            //    Name = "Wnm",
            //    Sex = "man"
            //};


            //IFormatter formatter = new BinaryFormatter();
            //try
            //{
            //    Stream stream = new FileStream("D:/personInfo.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            //    formatter.Serialize(stream, person);
            //    stream.Close();

            //    var c = new { Name = "wnm", Age = 18 };

            //    var dates = new[]
            //    {
            //        DateTime.UtcNow.AddHours(-1),
            //        DateTime.UtcNow,
            //        DateTime.UtcNow.AddHours(1),
            //    };

            //    foreach (var tuple in
            //                dates.Select(
            //                    date => new Tuple<string, long>($"{date:MMM dd, yyyy hh:mm zzz}", date.Ticks)))
            //    {
            //        Console.WriteLine($"Ticks: {tuple.Item2}, formatted: {tuple.Item1}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }


        private static void ReadTxt()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("D:/personInfo.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            var person = (Person)formatter.Deserialize(stream);
            Console.WriteLine(person.No);
        }


        private static void Function1()
        {
            Console.WriteLine("Function 1 entered");
            // wait here until the user put any input
            Console.ReadLine();
            Console.WriteLine("Function 1 is exited");
        }


        private static Task<string> ConsoleTest()
        {
            var task = Task.Run(() =>
            {
                Console.WriteLine("Exec ConsoleTest Before");
                Thread.Sleep(5000);
                Console.WriteLine("Exec ConsoleTest After");
                return "Success";
            });

            return task;
        }

    }
}