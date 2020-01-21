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
        Int32 _GuestRequestKey;
        string _PrivateName;
        string _FamilyName;
        string _MailAddress;
        Enums.GuestRequestStatus _Status;
        DateTime _RegistrationDate;
        DateTime _EnteryDate;
        DateTime _ReleaseDate;
        Enums.Area _Area;
        string _SubArea;
        Enums.HostingUnitType _Type;
        int _Adults;
        int _Children;
        bool? _pool;
        bool _Jacuzzi;
        bool _Garden;
        bool _ChildrenAttraction;
        int _Stars;
        bool meals;
        #endregion

        #region properties
       
        public int GuestRequestKey { get => _GuestRequestKey; set => _GuestRequestKey=value; }
        public string PrivateName { get => _PrivateName; set => _PrivateName = value; }
        public string FamilyName { get => _FamilyName; set => _FamilyName = value; }
        public string MailAddress { get => _MailAddress; set => _MailAddress = value; }
        public Enums.GuestRequestStatus Status { get => _Status; set => _Status = value; }
        public DateTime RegistrationDate { get => _RegistrationDate; set => _RegistrationDate = value; }
        public DateTime EnteryDate { get => _EnteryDate; set => _EnteryDate = value; }
        public DateTime ReleaseDate { get => _ReleaseDate; set => _ReleaseDate = value; }
        public Enums.Area Area { get => _Area; set => _Area = value; }
        public string SubArea { get => _SubArea; set => _SubArea = value; }
        public Enums.HostingUnitType Type { get => _Type; set => _Type = value; }
        public int Adults { get => _Adults; set => _Adults = value; }
        public int Children { get => _Children; set => _Children = value; }
        public bool? Pool { get => _pool; set => _pool = value; }
        public bool Jacuzzi { get => _Jacuzzi; set => _Jacuzzi = value; }
        public bool Garden { get => _Garden; set => _Garden = value; }
        public bool ChildrenAttraction { get => _ChildrenAttraction; set => _ChildrenAttraction = value; }      
        public bool Meals { get => meals; set => meals = value; }
        public int Stars { get => _Stars; set => _Stars = value; }


        #endregion

        #region functions
        public override string ToString()
        {
            string request;
            request = "Request ID: " + GuestRequestKey + "@costumer Details: Name: " + PrivateName + " " + FamilyName +
                " Mail:" + MailAddress + "@Vacation Details: Date: " + EnteryDate + "-" + ReleaseDate +
                "Place: " + Area + " " + SubArea + "Number Of Travelers: " + (Adults + Children) +
                "@Registration Date: " + RegistrationDate + "@Request's status: " + Status;
            request = request.Replace("@", System.Environment.NewLine);
            return request.ToString();
        }      
        #endregion

    }
}
