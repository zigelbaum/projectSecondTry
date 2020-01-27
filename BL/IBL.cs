

using BE;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Net.Mail;

namespace BL
{
    public interface IBL
    {
        #region Dalfunctions
        #region HostingUnit
        int addHostingUnit(HostingUnit hostingUnit);//Add a hosting unit
        void DeleteHostingUnit(HostingUnit hostingUnit);//Removing a hosting unit
        void SetHostingUnit(HostingUnit hostingUnit);//Hosting unit update
        List<HostingUnit> getHostingUnitsList();//Returns a list of all accommodation units
        List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate);//Returns a list of accommodation units that meet a specific condition
        #endregion

        #region GuestRequest
        bool validDate(GuestRequest guest);//Checks that the vacation start date has not passed
        int addGuestRequest(GuestRequest guestRequest);//Add a customer requirement
        void SetGuestRequest(GuestRequest guestRequest);//Customer Requirement Status Update
        List<GuestRequest> GetGuestRequestsList();//Returns a list of all existing client requirements in the system
        List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate);//Returns a list of customer requirements that meet a certain condition
        #endregion

        #region Order
        int AddOrder(Order order);//Add an invitation
        void setOrder(Order order);//Update Order Status
        List<Order> GetOrdersList();//Returns a list of existing orders in the system
        List<Order> getOrders(Func<Order, bool> predicate);//Returns a list of orders that fulfill a particular condition
        bool OrderExist(Order order);//Checks if the order exists in the system
        Order FindOrder(Int32 ordKey);//Returns this order from the system
        #endregion
        #endregion

        #region enforcements
        bool OverNightVacation(GuestRequest guestRequest);//Checks if the holiday duration is at least one day
        bool BankAccountDebitAuthorization(Host host);//Checks whether a collection certificate is available from the host
        bool HostingUnitAvability(Order order);//Checks whether the unit is available on the order dates
        void UpdateDiary(Order order);//Updates dates in the matrix as the order closes
        void UpdateInfoAfterOrderClosed(Order order);//Updates customer status when order is closed
        bool AbleToChangeOrderStatus(Order order);//Checks whether order status can be changed
        double TotalFee(Order order);//Calculates commission fee based on number of days off
        bool TheHostingUnitHasAnOpenOrder(HostingUnit hostingUnit);//Checks whether the unit has an open invitation
        bool RevocationPermission(Host host);//Checks whether account debit authorization can be revoked
        void SendEmail(Order ord);//Sends a customer to the customer with the order details
        bool IsValidEmail(string email);//Checks whether the email address is valid
        #endregion

        #region functions
        bool CheckAvailable(HostingUnit hostingUnit, DateTime entry, DateTime release);//Checks whether the requested vacation time in a particular unit is free
        IEnumerable<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays);//Returns the list of all available accommodation units on this date
        Int32 NumDays(DateTime start, DateTime end);//Returns the number of days passed from the first date to the second
        Int32 NumDays(DateTime start);//Returns the number of days that have passed from the given date to the present
        List<Order> DaysPassedOrders(Int32 days);//Returns all orders that have elapsed since they were created / since sent emails to a customer greater than or equal to the number of days the function received
        List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic, HostingUnit hosting);//Returns all customer requirements that fit a particular condition
        Int32 NumOfInvetations(BE.GuestRequest costumer);//Returns the number of orders sent to a customer
        Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit);//Returns the number of orders sent / number of successfully closed orders for a unit through the site
        Predicate<GuestRequest> BuildPredicate(HostingUnit hosting);//Builds a predicate that filters the hosting units according to the client's requirements
        Order NewOrder(int hostingUnitkey, int guestRequestKey);//Creating order  
        GuestRequest FindGuestRequest(Int32 requestKey);//find the request with this key
        HostingUnit FindUnit(Int32 unitKey);//find the unit with this key
        double Aggregate_fee();//calculates the commission from all order
        List<GuestRequest> DaysPassFromMail(Int32 days); ////returns all gr that were sent a email/ created "numOfDays" ago
        #endregion

        #region grouping
        IEnumerable<IGrouping<Enums.Area, GuestRequest>> GroupGRByArea();//Creates a group of customer requirements by area
        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers();//Creates a group of customer requirements by vacationers
        IEnumerable<IGrouping<Enums.HostingUnitType, GuestRequest>> GroupGRByType();//Creates a group of customer requirements by vocation type
        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByStars();//Creates a group of customer requirements by number of stars
        IEnumerable<IGrouping<Enums.GuestRequestStatus, GuestRequest>> GroupGRByStatus();//Creates a group of customer requirements by status
        IEnumerable<IGrouping<Host, HostingUnit>> GroupHostByHostingUnit();//Creates a group of hosting units by host
        IEnumerable<IGrouping<Enums.HostingUnitType, HostingUnit>> GroupHostingUnitByType();//Creates a group of hosting units by unit type
        IEnumerable<IGrouping<Enums.Area, HostingUnit>> GroupHostingUnitByArea();//Creates a group of hosting units by area
        #endregion

    }
}