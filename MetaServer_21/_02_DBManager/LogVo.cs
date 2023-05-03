using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_DBManager
{
    public class LogVo
    {
        public string LOG_SEQ { get; set; }
        public string MEMBER_ID { get; set; }
        public string LOG_TIME { get; set; }
        public string LOG_IP { get; set; }
        public string LOG_PORT { get; set; }
        public string LOG_INFO { get; set; }

        public override string ToString()
        {
            return $"log_seq={LOG_SEQ}, " +
                $"member_id={MEMBER_ID}, " +
                $"log_time={LOG_TIME}, " +
                $"log_ip={LOG_IP}, " +
                $"log_port={LOG_PORT}, " +
                $"log_info={LOG_INFO}";
        }
    }
}
