using PWMIS.EnterpriseFramework.Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    public class TimeService:ServiceBase
    {
        Random rand = new Random();//.Next(1000, 60000); 
        public DateTime ServerTime()
        {
            return DateTime.Now;
        }

        public ServiceEventSource TestTask()
        {
            Console.WriteLine("----Test Task-----");
            return new ServiceEventSource(this, 2, () => {
                var task1 = this.StartTask(1);
                var task2 = this.StartTask(2);
                var task3 = this.StartTask(3);
                var task4 = this.StartTask(4);
                var task5 = this.StartTask(5);
                Task.WaitAll(task1, task2, task3, task4, task5);
                Console.WriteLine("ALL ok.");
                this.CurrentContext.PublishData("ALL ok.");
              
                base.CurrentContext.PublishEventSource.DeActive();
            });
        }

        Task StartTask(int id)
        {
            var task1 = Task.Factory.StartNew<int>(() => {
                int time = rand.Next(1000, 6000);
                string msg = string.Format("---Task {0},Begin Sleep {1} ms----", id, time);
                Console.WriteLine(msg);
               this.CurrentContext.PublishData(msg);
                Console.WriteLine("---Task {0} Sned msg1 OK.",id);

                //System.Threading.Thread.Sleep(time);
                Task.Delay(time).Wait();
                msg = string.Format("---Task {0},End Sleep {1} ms----", id, time);
                Console.WriteLine(msg);
               this.CurrentContext.PublishData(msg);
                Console.WriteLine("---Task {0} Sned msg2 OK.----------------\r\n", id);
                return time;
            }, TaskCreationOptions.AttachedToParent );
            return task1;
        }
    }
}
