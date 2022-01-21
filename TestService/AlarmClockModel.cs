using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class AlarmClockModel
    {
        const int MAX_COUNT = 100;
        System.Timers.Timer timer;
        /// <summary>
        /// 设定的闹铃开始响铃的时间
        /// </summary>
       public DateTime AlarmTime { get; set; }
        /// <summary>
        /// 已经响铃的次数
        /// </summary>
        public int AlarmCount { get; set; }
        /// <summary>
        /// 最大响铃次数
        /// </summary>
        public int MaxAlarmCount { get; set; }
        /// <summary>
        /// 响铃事件
        /// </summary>
        public event EventHandler Alarming;
        /// <summary>
        /// 响铃停止事件
        /// </summary>
        public event EventHandler StopAlarm;

        public AlarmClockModel()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += Timer_Elapsed;

            MaxAlarmCount = MAX_COUNT;
        }

        /// <summary>
        /// 设置响铃时间和次数
        /// </summary>
        /// <param name="at"></param>
        /// <param name="count"></param>
        public void SetAlarm(DateTime at,int count)
        {
            this.AlarmTime = at;
            if (count > MAX_COUNT)
                MaxAlarmCount = MAX_COUNT;
            else
                MaxAlarmCount = count;

            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (e.SignalTime >= this.AlarmTime)
            {
                if (Alarming != null)
                    Alarming(this, new EventArgs());

                
                AlarmCount++;
                //Console.WriteLine("AlarmClockService Publish Count:{0}", AlarmCount);
            }
            else
            {
                //Console.WriteLine("Alarm Time:{0},AlarmClock waiting...", this.AlarmTime);
            }
            if (AlarmCount > MaxAlarmCount)
            {
                timer.Stop();
                //推送一个结束标记值：1900-1-1
                //base.CurrentContext.PublishData(new DateTime(1900, 1, 1));
                //Console.WriteLine("[{0}] AlarmClockService Timer Stoped. ", new DateTime(1900, 1, 1));
                //base.CurrentContext.PublishEventSource.DeActive();

                if (StopAlarm != null)
                    StopAlarm(this, new EventArgs());

            }
        }
    }
}
