using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface IDAL
    {
        #region HostingUnit        
        void addHostingUnit(HostingUnit hostingUnit);
        void DeleteHostingUnit(HostingUnit hostingUnit);
        void SetHostingUnit(HostingUnit hostingUnit);
        bool UnitExist(HostingUnit unit);
        List<HostingUnit> getHostingUnitsList();
        List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate = null);
        #endregion

        #region GuestRequest
        void SetGuestRequest(GuestRequest guest);
        void addGuestRequest(GuestRequest guest);
        List<GuestRequest> GetGuestRequestsList();
        bool RequestExist(GuestRequest request);
        #endregion

        #region Order
        void addOrder(Order order);
        void setOrder(Order order);
        List<Order> getOrders(Func<Order, bool> predicate);
        bool OrderExist(Order order);
        #endregion

        #region BanckBranch
        List<BankBranch> GetBankBranchesList();
        #endregion

        #region HostingUnits
        List<HostingUnit> GetHostingUnitsList();
        #endregion


    }
}
