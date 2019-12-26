using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration
    {
        public static string TypeDAL = ConfigurationSettings.AppSettings.Get("TypeDS");
        public static Int32 GuestRequestKey = 10000000;
        public static Int32 HostingUnitKey = 10000000;
        public static Int32 OrderKey = 10000000;
    }
}
