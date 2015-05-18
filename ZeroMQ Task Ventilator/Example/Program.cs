using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ_Task_Ventilator;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskSinkThread = new Thread(RunTaskSink);
            TaskVentThread = new Thread(RunTaskVent);
            TaskSinkThread.Start();
            TaskVentThread.Start();
            Thread.Sleep(1000);
            for (int i = 0; i < workerscount; i++)
            {
                Thread worker = new Thread(RunTaskWorker);
                workers.Add(worker);
                worker.Start();
            }
            do
            {
                Thread.Sleep(100);
            } while (true);
        }

        static List<Thread> workers = new List<Thread>();
        static int workerscount = 25;

        static void RunTaskSink()
        {
            TaskSink ts = new TaskSink();
            Console.WriteLine("TaskSinkEnded");
        }

        static void RunTaskVent()
        {
            TaskVent tv = new TaskVent();
            Console.WriteLine("TaskVentEnded");
        }

        static int i = 0;
        static void RunTaskWorker()
        {
            i++;
            TaskWorker tw = new TaskWorker(i.ToString());
            Console.WriteLine("TaskWorkerEnded");
        }

        static Thread TaskSinkThread;
        static Thread TaskVentThread;
    }
}
