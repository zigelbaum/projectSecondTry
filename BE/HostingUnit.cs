using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit
    {
        private Host host;
        private string hostingUnitName;

        public HostingUnit()
        {
        }

        public HostingUnit(Host host, string hostingUnitName)
        {
            this.host = host;
            this.hostingUnitName = hostingUnitName;
        }
    }
}
