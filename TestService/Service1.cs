using PWMIS.EnterpriseFramework.Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDto;

namespace TestService
{
    public class Service1 : IService
    {
        public string SayHello(string who)
        {
            return string.Format("Hello {0} ,I am  MSF Server.", who);
        }

        public MailMessage GetMailMessage(string who)
        {
            MailMessage mail = new MailMessage();
            mail.Reply = who;
            mail.Sender = "MSF Server";
            mail.Message = string.Format("Hello {0} ,I am  MSF Server.", who);
            mail.RevoveryTime = DateTime.Now;
            return mail;
        }

        public void CompleteRequest(IServiceContext context)
        {
            //throw new NotImplementedException();
        }

        public bool IsUnSubscribe
        {
            //get { throw new NotImplementedException(); }
            get
            {
                //返回True ，表示当前服务不执行系统后续的服务方法的订阅处理过程，而是由用户自己输出结果数据
                return false;
            }
        }

        public bool ProcessRequest(IServiceContext context)
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}
