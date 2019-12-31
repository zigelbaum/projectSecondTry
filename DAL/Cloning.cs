using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public static class Cloning
    {
        #region BankBranch
        public static BankBranch Clone(this BankBranch original)
        {
            BankBranch target = new BankBranch();
            target.BankName = original.BankName;
            target.BankNumber = original.BankNumber;
            target.BranchAddress = original.BranchAddress;
            target.BranchCity = original.BranchCity;
            target.BranchNumber = original.BranchNumber;
            return target;

        }
        #endregion

        #region GuestRequest
        public static GuestRequest Clone(this GuestRequest original)
        {
            GuestRequest target = new GuestRequest();
            target.Adults = original.Adults;
            target.Area = original.Area;
            target.Children = original.Children;
            target.ChildrenAttraction = original.ChildrenAttraction;
            target.EnteryDate = original.EnteryDate;
            target.FamilyName = original.FamilyName;
            target.Garden = original.Garden;
            target.GuestRequestKey = original.GuestRequestKey;
            target.Jacuzzi = original.Jacuzzi;
            target.MailAddress = original.MailAddress;
            target.Pool = original.Pool;
            target.PrivateName = original.PrivateName;
            target.RegistrationDate = original.RegistrationDate;
            target.ReleaseDate = original.ReleaseDate;
            target.Status = original.Status;
            target.SubArea = original.SubArea;
            target.Type = original.Type;
            return target;
        }
        #endregion

        #region Host
        public static Host Clone(this Host original)
        {
            Host target = new Host();
            target.BankAccountNumber = original.BankAccountNumber;
            target.BankBranchDetails = original.BankBranchDetails;
            target.CollectionClearance= original.CollectionClearance;
            return target;
        }
        #endregion

        #region HostingUnit
        public static HostingUnit Clone(this HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            target.Diary = original.Diary;
            target.HostingUnitKey = original.HostingUnitKey;
            target.HostingUnitName = original.HostingUnitName;
            target.HostingUnitType = original.HostingUnitType;
            target.Owner = original.Owner;
            target.Adults = original.Adults;
            target.Area = original.Area;
            target.ChildrenAttraction = original.ChildrenAttraction;
            target.Garden = original.Garden;
            target.Jaccuzi = original.Jaccuzi;
            target.Kids = original.Kids;
            target.Meals = original.Meals;
            target.Pool = original.Pool;
            target.Stars = original.Stars;
            target.SubArea = original.SubArea;   
            return target;
        }
        #endregion

        #region Order
        public static Order Clone(this Order original)
        {
            Order target = new Order();
            target.GuestRequestKey = original.GuestRequestKey;
            target.HostingUnitKey = original.HostingUnitKey;
            target.OrderDate = original.OrderDate;
            target.OrderKey = original.OrderKey;
            target.OrderStatus = original.OrderStatus;
            target.CreateDate = original.CreateDate;
            return target;
        }
        #endregion
    }
}
