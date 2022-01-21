using PWMIS.EnterpriseFramework.Service.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDto;

namespace TestService
{
    public class AlarmClockService:ServiceBase
    {
        AlarmClockModel AlarmClock;

        public AlarmClockService()
        {
            AlarmClock = new AlarmClockModel();
            AlarmClock.Alarming += AlarmClock_Alarming;
            AlarmClock.StopAlarm += AlarmClock_StopAlarm;
        }

        private void AlarmClock_StopAlarm(object sender, EventArgs e)
        {
            //推送一个结束标记值：1900-1-1
            // PublishDistributeEvent(data);
            base.PublishDistributeEvent(new DateTime(1900, 1, 1));
            Console.WriteLine("[{0}] AlarmClock Service Timer Stoped. ", DateTime.Now);
            base.CurrentContext.PublishEventSource.DeActive();
        }

        private void AlarmClock_Alarming(object sender, EventArgs e)
        {
            //向客户端推送正在响铃的事件
            // PublishDistributeEvent(data);
            base.PublishDistributeEvent(DateTime.Now);
            Console.WriteLine("AlarmClock AlarmCount:{0}", AlarmClock.AlarmCount);
        }

       


        public ServiceEventSource SetAlarmTime(AlarmClockParameter para)
        {
            return new ServiceEventSource(AlarmClock, 2, () =>
            {
                //要初始化执行的代码或者方法
                AlarmClock.SetAlarm(para.AlarmTime, para.AlarmCount);

                //如果上面的代码是一个执行时间比较长的方法，但又不知道何时执行完成，
                //并且不想等待超时回收服务对象，而是在执行完成后立即回收服务对象，可以调用下面的代码：
                //CurrentContext.PublishEventSource.DeActive();
                //注意：调用DeActive 方法后将会停止事件推送，所以请注意此方法调用的时机。

                //下面代码仅做测试，查看服务事件源对象的活动生命周期
                //在 ActiveLife 时间之后，一直没有事件推送，则事件源对象被视为非活动状态，发布工作线程会被回收。
                //在本例中，ActiveLife 为ServiceEventSource 构造函数的第二个参数，值为 2分钟，可以通过下面一行代码证实：
                int life = base.CurrentContext.PublishEventSource.ActiveLife;

                //如果上面执行的是一个执行时间比较长的方法，并且有返回值，想将返回值也推送给订阅端，可以再次执行CurrentContext.PublishData
                //CurrentContext.PublishData(DateTime.Now);

                //如果事件推送结束，需要设置事件源为非活动状态，否则，需要等待 ActiveLife 时间之后自然过期成为非活动状态。
                //如果你无法确定事件推送何时结束，请不要调用下面的方法
                //CurrentContext.PublishEventSource.DeActive();
            });
        }
    }
}
