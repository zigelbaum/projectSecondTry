using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HostingUnit:IComparable
    {
        #region fileds
        Int32 hostingUnitKey;
        private Host owner;
        private string hostingUnitName;
        private Enums.HostingUnitType hostingUnitType;
        private bool[,] diary = new bool[12, 31];
        private Enums.Area area;
        private string subArea;
        private bool pool;
        private int adults;
        private int kids;
        private bool jaccuzi;
        private bool garden;
        private bool childrenAttraction;
        private int stars;
        private bool meals;

        #endregion

        #region properties
        public Enums.HostingUnitType HostingUnitType { get => hostingUnitType; set => hostingUnitType=value; }
        public string HostingUnitName { get => hostingUnitName; set => hostingUnitName=value; }
        public bool[,] Diary { get => diary; set => diary = value; }
        public Host Owner { get => owner; set => owner = value; }
        public int HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public Enums.Area Area { get => area; set => area = value; }
        public string SubArea { get => subArea; set => subArea = value; }
        public bool Pool { get => pool; set => pool = value; }
        public int Adults { get => adults; set => adults = value; }
        public int Kids { get => kids; set => kids = value; }
        public bool Jaccuzi { get => jaccuzi; set => jaccuzi = value; }
        public bool Garden { get => garden; set => garden = value; }
        public bool ChildrenAttraction { get => childrenAttraction; set => childrenAttraction = value; }
        public int Stars { get => stars; set => stars = value; }
        public bool Meals { get => meals; set => meals = value; }
        #endregion

        #region functions

        public override string ToString()
        {
            string unit;
            unit = "Unit ID:" + HostingUnitKey + "@Name of the unit: " + HostingUnitName + "@Type of Vacation:" + HostingUnitType + "@Area:" +
               Area+ "@Sub Area: " + SubArea + "@Host ID: " + Owner.HostKey;
            unit = unit.Replace("@", System.Environment.NewLine);
            return unit.ToString();
        }

        public Int32 CompareTo(object obj)
        {
            Int32 my_owner = this.Owner.CompareTo((obj as HostingUnit).Owner);
            Int32 my_name = this.HostingUnitName.CompareTo((obj as HostingUnit).HostingUnitName);
            int my_type = this.HostingUnitType.CompareTo((obj as HostingUnit).HostingUnitType);
            if (my_owner == 0 && my_name == 0 && my_type == 0)
                return 0;
            return 1;
        }

        #endregion
    }
}
