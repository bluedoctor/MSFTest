using PWMIS.EnterpriseFramework.Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestService
{
    public class TimeService:ServiceBase
    {
        public DateTime ServerTime()
        {
            return DateTime.Now;
        }
    }
}
