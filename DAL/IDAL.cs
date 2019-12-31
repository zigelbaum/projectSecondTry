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
        int addHostingUnit(HostingUnit hostingUnit);//Add a hosting unit
        void DeleteHostingUnit(HostingUnit hostingUnit);//Removes hosting unit
        void SetHostingUnit(HostingUnit hostingUnit);//Updating hosting unit
        bool UnitExist(HostingUnit unit);//Checks if the hosting unit exists
        List<HostingUnit> getHostingUnitsList();//Returns a list of all accommodation units
        List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate);//Returns a list of all accommodation units that meet certain conditions
        #endregion

        #region GuestRequest
        void SetGuestRequest(GuestRequest guest);//Updating customer status
        int addGuestRequest(GuestRequest guest);//Add a customer requirement
        List<GuestRequest> GetGuestRequestsList();//Returns a list of all customer requirements
        List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate);//Returns a list of customer requirements that meet a specific condition
        bool RequestExist(GuestRequest request);//Checks whether the customer requirement exists in the system
        #endregion

        #region Order
        int addOrder(Order order);//Add an invitation
        void setOrder(Order order);//Update order status
        List<Order> GetOrdersList();//Returns a list of all orders
        List<Order> getOrders(Func<Order, bool> predicate);//Returns a list of all orders that fulfill a particular condition
        bool OrderExist(Order order);//Checks if the order exists in the system
        #endregion

        #region BanckBranch
        List<BankBranch> GetBankBranchesList();//Returns a list of all bank branches
        #endregion

    }
}
