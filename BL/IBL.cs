

using BE;
using System.Collections.Generic;
using System;

namespace BL
{
    public interface IBL
    {
        #region HostingUnit
        void AddHostingUnit();
        void DeleteHostingUnit();
        void SetHostingUnit();
        List<HostingUnit> getAllHostingUnits();
        #endregion

        #region GuestRequest
        void AddGuestRequest();
        void SetGuestRequest();
        #endregion

        #region Order
        bool AddOrder();
        void SetOrder();
        List<BE.Order> OrderExistenceEqualsDays(Int32 days);
        #endregion

        #region change now
        bool RevocationPermission(int bankAccountNumber, BankBranch bankBranchDetails);//??????
        void SendEmail(Order ord);
        List<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays);
        Int32 NumDays(DateTime start, DateTime end=default(DateTime.Now));
        List<Order> DaysPassedOrders(Int32 days);
        List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic);
        Int32 NumOfInvetations(BE.GuestRequest costumer);
        Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit);
        #endregion
        //Int32 NumDays(DateTime start);
    }
}