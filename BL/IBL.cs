

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
        void SetHostingUnit(HostingUnit hostingUnit);
        List<HostingUnit> getHostingUnitsList();
        List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate);
        #endregion

        #region GuestRequest
        void AddGuestRequest();
        void SetGuestRequest(GuestRequest guestRequest);
        List<GuestRequest> GetGuestRequestsList();
        List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate);
        #endregion

        #region Order
        bool AddOrder();
        void SetOrder();
        void setOrder(Order order);
        List<Order> getOrders(Func<Order, bool> predicate);
        List<BE.Order> OrderExistenceEqualsDays(Int32 days);
        #endregion

        #region change now
        bool RevocationPermission(int bankAccountNumber, BankBranch bankBranchDetails);//??????
        void SendEmail(Order ord);
        //bool CheckAvailable(HostingUnit hostingUnit, DateTime entry, Int32 vactiondays);
        List<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays);
        Int32 NumDays(DateTime start, DateTime end=default(DateTime.Now));
        List<Order> DaysPassedOrders(Int32 days);
        List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic);
        Int32 NumOfInvetations(BE.GuestRequest costumer);
        Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit);
        #endregion

        #region grouping
        IEnumerable<IGrouping<Area, GuestRequest>> GroupGRByArea();
        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers();
        IEnumerable<IGrouping<int, Host>> GroupHostByHostingUnit();
        IEnumerable<IGrouping<Area, HostingUnit>> GroupHostByHostingUnit();
        #endregion
        
    }
}