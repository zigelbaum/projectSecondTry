using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest
    {
        #region fileds
        readonly Int32 _GuestRequestKey = Configuration.GuestRequestKey;
        string _PrivateName;
        string _FamilyName;
        string _MailAddress;
        Enums.GuestRequestStatus _Status;
        DateTime _RegistrationDate;
        DateTime _EnteryDate;
        DateTime _ReleaseDate;
        Enums.Area _Area;
        string _SubArea;
        Enums.Type _Type;
        int _Adults;
        int _Children;
        Enums.intrested _pool;
        Enums.intrested _Jacuzzi;
        Enums.intrested _Garden;
        Enums.intrested _ChildrenAttraction;
        #endregion

        #region properties
        //has to checked if set exsitence requested
        public int GuestRequestKey { get => _GuestRequestKey; }
        public string PrivateName { get => _PrivateName; set => _PrivateName = value; }
        public string FamilyName { get => _FamilyName; set => _FamilyName = value; }
        public string MailAddress { get => _MailAddress; set => _MailAddress = value; }
        public Enums.GuestRequestStatus Status { get => _Status; set => _Status = value; }
        public DateTime RegistrationDate { get => _RegistrationDate; set => _RegistrationDate = value; }
        public DateTime EnteryDate { get => _EnteryDate; set => _EnteryDate = value; }
        public DateTime ReleaseDate { get => _ReleaseDate; set => _ReleaseDate = value; }
        public Enums.Area Area { get => _Area; set => _Area = value; }
        public string SubArea { get => _SubArea; set => _SubArea = value; }
        public Enums.Type Type { get => _Type; set => _Type = value; }
        public int Adults { get => _Adults; set => _Adults = value; }
        public int Children { get => _Children; set => _Children = value; }
        public Enums.intrested Pool { get => _pool; set => _pool = value; }
        public Enums.intrested Jacuzzi { get => _Jacuzzi; set => _Jacuzzi = value; }
        public Enums.intrested Garden { get => _Garden; set => _Garden = value; }
        public Enums.intrested ChildrenAttraction { get => _ChildrenAttraction; set => _ChildrenAttraction = value; }
        
        #endregion

        #region functions
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

    }
}
