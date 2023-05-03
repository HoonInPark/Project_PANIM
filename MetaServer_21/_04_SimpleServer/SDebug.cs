using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDebug
{
    public static class SDebug
    {
        static bool isDebug { get; set; }

        static SDebug()
        {
            isDebug = true;
        }
        
        public static void WriteLog(string message)
        {
            if(isDebug)
                Console.WriteLine(message);
        }
    }
}
