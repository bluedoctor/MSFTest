using PWMIS.EnterpriseFramework.Service.Basic;
using PWMIS.EnterpriseFramework.Service.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestClient;
using TestDto;

namespace MSFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("******** PDF.NET MSF 客户端测试程序 *********");
            Console.WriteLine();
            Proxy client = new Proxy();
            client.ErrorMessage += client_ErrorMessage;
            Console.Write("请输入服务器的主机名或者IP地址(默认 127.0.0.1)：");
            string host = Console.ReadLine();
            if (string.IsNullOrEmpty(host))
                host = "127.0.0.1";
            Console.WriteLine("服务地址：{0}",host);

            Console.Write("请输入服务的端口号(默认 8888)：");
            string port = Console.ReadLine();
            if (string.IsNullOrEmpty(port))
                port = "8888";
            Console.WriteLine("服务端口号：{0}", port);

            client.ServiceBaseUri = string.Format("net.tcp://{0}:{1}", host, port);
            Console.WriteLine("当前客户端代理的服务基础地址是：{0}",client.ServiceBaseUri);
            Console.WriteLine();
            Console.WriteLine("MSF 请求-响应 模式调用示例：");

            client.RequestService<string>("Service://Service1/SayHello/System.String=bluedoctor1",
               PWMIS.EnterpriseFramework.Common.DataType.Text,
               s =>
               {
                   Console.WriteLine("1,Server Response:【{0}】", s);
               });

            ServiceRequest request = new ServiceRequest();
            request.ServiceName = "Service1";
            request.MethodName = "SayHello";
            request.Parameters = new object[] { "bluedoctor23" };
            client.RequestService<string>(request,
                PWMIS.EnterpriseFramework.Common.DataType.Text,
                s =>
                {
                    Console.WriteLine("2,Server Response:【{0}】", s);
                });

            string serverMsg= client.RequestServiceAsync<string>(request).Result;
            Console.WriteLine("3,Server Response:【{0}】", serverMsg);

            client.RequestService<MailMessage>("Service://Service1/GetMailMessage/System.String=bluedoctor4",
               PWMIS.EnterpriseFramework.Common.DataType.Json,
               mail =>
               {
                   Console.WriteLine("4,Server Response:【{0}】，\r\n Revovery Time：{1}", mail.Message,mail.RevoveryTime);
               });

            ServiceRequest request2 = new ServiceRequest();
            request2.ServiceName = "Service1";
            request2.MethodName = "GetMailMessage";
            request2.Parameters = new object[] { "bluedoctor567" };

            client.RequestService<MailMessage>(request2,
              PWMIS.EnterpriseFramework.Common.DataType.Json,
              mail =>
              {
                  Console.WriteLine("5,Server Response:【{0}】，\r\n Revovery Time：{1}", mail.Message, mail.RevoveryTime);
              });

            client.RequestService<MailMessage2>(request2,
             PWMIS.EnterpriseFramework.Common.DataType.Json,
             mail =>
             {
                 Console.WriteLine("6,Server Response:【{0}】，\r\n Revovery Time：{1}", mail.Message, mail.RevoveryTime);
             });

            MailMessage mail2 = client.RequestServiceAsync<MailMessage>(request2).Result;
            Console.WriteLine("7,Server Response:【{0}】，\r\n Revovery Time：{1}", mail2.Message, mail2.RevoveryTime);

           

            DateTime serverTime = client.RequestServiceAsync<DateTime>("Service://TestTimeService/ServerTime/", 
                PWMIS.EnterpriseFramework.Common.DataType.DateTime).Result;
            Console.WriteLine("MSF Get Server Time:{0}", serverTime);
        
            Console.WriteLine("按回车键继续");
            Console.ReadLine();


            Console.WriteLine();
            Console.WriteLine("MSF 发布-订阅 模式调用示例：");

            ServiceRequest request3 = new ServiceRequest();
            request3.ServiceName = "TestTimeService";
            request3.MethodName = "ServerTime";
            int count = 0;
            client.Subscribe<DateTime>(request3, 
                PWMIS.EnterpriseFramework.Common.DataType.DateTime, 
                s => 
                {
                    if (s.Succeed)
                    {
                        Console.WriteLine("MSF Server Time:{0}", s.Result);
                      
                    }
                    else
                    {
                        Console.WriteLine("MSF Server Error:{0}", s.ErrorMessage);
                    }
                    count++;
                    if (count > 10)
                    {
                        client.Close();
                        Console.WriteLine("订阅【服务器时钟服务】结束。按回车键继续。");
                    }
                });

            Console.ReadLine();
            DateTime alarmTime;
            string strTime;
            do
            {
                Console.Write("请输入闹铃响铃时间(示例输入格式 {0}) >>", DateTime.Now.ToShortTimeString());
                 strTime = Console.ReadLine();
            }
            while (!DateTime.TryParse(strTime, out alarmTime));
           

            Console.WriteLine("订阅闹钟服务，闹钟将在 {0} 响铃...",strTime);

            ServiceRequest request4 = new ServiceRequest();
            request4.ServiceName = "AlarmClockService";
            request4.MethodName = "SetAlarmTime";
            request4.Parameters = new object[] { alarmTime };

            client.Subscribe<DateTime>(request4,
                  PWMIS.EnterpriseFramework.Common.DataType.DateTime, 
                  s =>
                  {
                      if (s.Succeed)
                      {
                          Console.WriteLine("闹钟响了，现在时间:{0}", s.Result);
                          if (s.Result == new DateTime(1900, 1, 1))
                          {
                              client.Close();
                              Console.WriteLine("闹铃服务结束，按回车键继续。");
                          }
                      }
                      else
                      {
                          Console.WriteLine("MSF Server Error:{0}", s.ErrorMessage);
                          client.Close();
                      }
              });

            Console.ReadLine();


            string repMsg = "你好！";

            client.SubscribeTextMessage("我是客户端", serverMessage => {
                Console.WriteLine();
                Console.WriteLine("[来自服务器的消息]::{0}", serverMessage);
            });

            while (repMsg != "")
            {
                Console.Write("回复服务器(输入为空，则退出)：>>");
                repMsg = Console.ReadLine();
                client.SendTextMessage(repMsg);
            }

            Console.WriteLine("测试完成，退出");

        }

        static void client_ErrorMessage(object sender, MessageSubscriber.MessageEventArgs e)
        {
            Console.WriteLine("---处理服务时错误：{0}",e.MessageText);
        }
    }
}
