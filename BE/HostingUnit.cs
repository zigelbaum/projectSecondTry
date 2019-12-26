using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit : ICloneable
    {
        #region fileds
        readonly Int32 hostingUnitKey = Configuration.HostingUnitKey;
        private Host owner;
        private string hostingUnitName;
        private Enums.HostingUnitType hostingUnitType;
        private bool[,] diary = new bool[12, 31];
        #endregion

       /* public HostingUnit(Host host, string hostingUnitName, Enums.HostingUnitType hostingUnitType)
        {
            this.host = host;
            this.hostingUnitName = hostingUnitName;
            this.hostingUnitType = hostingUnitType;
        }*/

        #region properties
        public Enums.HostingUnitType HostingUnitType { get => hostingUnitType; }
        public string HostingUnitName { get => hostingUnitName;  }
        #endregion
        
       /* public object Clone()
        {
            return new HostingUnit(this.host,this.HostingUnitName,this.HostingUnitType);
        }*/

        #region functions
        public override string ToString()
        {
            return this.HostingUnitName + " " + HostingUnitType ;
        }
        #endregion
    }
}
