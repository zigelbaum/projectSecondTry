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
        List<HostingUnit> getAllHostingUnits();
        List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate = null);
        #endregion

        #region GuestRequest
        void SetGuestRequest(GuestRequest guest);
        void addGuestRequest(GuestRequest guest/*String id, String name, int age*/);
        List<GuestRequest> GetGuestRequests();
        #endregion

        #region Order
        void addOrder(Order order);
        void setOrder(Order order);
        List<Order> getOrders(Func<Order, bool> predicate);
        #endregion

        #region BanckBranch
        List<BankBranch> GetBankBranches();
        #endregion
    }
}
