using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit /*: ICloneable*/
    {
        #region fileds
        Int32 hostingUnitKey = Configuration.HostingUnitKey;
        private Host owner;
        private string hostingUnitName;
        private Enums.HostingUnitType hostingUnitType;
        private bool[,] diary = new bool[12, 31];
        #endregion

       

        #region properties
        public Enums.HostingUnitType HostingUnitType { get => hostingUnitType; set => hostingUnitType=value; }
        public string HostingUnitName { get => hostingUnitName; set => hostingUnitName=value; }
        public bool[,] Diary { get => diary; set => diary = value; }
        public Host Owner { get => owner; set => owner = value; }
        public int HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }

        #endregion



        #region functions
        public override string ToString()
        {
            return this.HostingUnitName + " " + HostingUnitType;
        }

        #endregion
    }
}
