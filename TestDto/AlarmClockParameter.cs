using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDto
{
    public class AlarmClockParameter
    {
        /// <summary>
        /// 响铃时间
        /// </summary>
        public DateTime AlarmTime { get; set; }
        /// <summary>
        /// 响铃次数
        /// </summary>
        public int AlarmCount { get; set; }
    }
}
