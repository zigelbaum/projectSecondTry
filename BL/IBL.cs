

using BE;
using System.Collections.Generic;

namespace BL
{
    public interface IBL
    {
        #region HostingUnit
        void AddHostingUnit();
        void DeleteHostingUnit();
        void SetHostingUnit();
        List<HostingUnit> getAllHostingUnits();
        List<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays);
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

        Int32 NumDays(DateTime start, DateTime end);
        Int32 NumDays(DateTime start);
        
        Int32 NumOfInvetations(BE.GuestRequest costumer);//??????????????
        Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit);
    }
}