using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_DBManager
{
    public class MemberVo
    {
        public string MEMBER_ID { get; set; }
        public string MEMBER_PASS { get; set; }
        public string MEMBER_NAME { get; set; }
        public string MEMBER_AGE { get; set; }
        public string MEMBER_JOB { get; set; }
        public string MEMBER_PHONE { get; set; }
        public string GAMEROOM_ID { get; set; }

        public override string ToString()
        {
            return $"member_id={MEMBER_ID}, " +
                    $"member_pass={MEMBER_PASS}, " +
                    $"member_name={MEMBER_NAME}, " +
                    $"member_age={MEMBER_AGE}, " +
                    $"member_job={MEMBER_JOB}, " +
                    $"member_phone={MEMBER_PHONE}, " +
                    $"gameroom_id={GAMEROOM_ID}";
        }
    }
}
