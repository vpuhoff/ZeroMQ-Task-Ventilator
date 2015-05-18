using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace ZeroMQ_Task_Ventilator
{
    public class TaskWorker
    {
        public TaskWorker(string workerid="*",string SenderIP = "127.0.0.1", int SenderPort = 5557, string sinkIP = "127.0.0.1", int sinkPort=5558)
        {
            //
            // Task worker
            // Connects PULL socket to tcp://localhost:5557
            // Collects workloads from ventilator via that socket
            // Connects PUSH socket to tcp://localhost:5558
            // Sends results to sink via that socket
            //
            // Author: metadings
            //

            // Socket to receive messages on and
            // Socket to send messages to
            using (var context = new ZContext())
            using (var receiver = new ZSocket(context, ZSocketType.PULL))
            using (var sink = new ZSocket(context, ZSocketType.PUSH))
            {
                receiver.Connect(String.Format ("tcp://{0}:{1}",SenderIP,SenderPort));
                sink.Connect(string.Format("tcp://{0}:{1}",sinkIP,sinkPort ));
                Console.WriteLine("Worker " + workerid + " ready.");
                // Process tasks forever
                while (true)
                {
                    var replyBytes = new byte[4];
                    receiver.ReceiveBytes(replyBytes, 0, replyBytes.Length);
                    int workload = BitConverter.ToInt32(replyBytes, 0);
                    Console.WriteLine("{0}.", workload);    // Show progress

                    Thread.Sleep(workload);    // Do the work

                    sink.Send(new byte[0], 0, 0);    // Send results to sink
                }
            }
        }
    }
}
