

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
        bool OverNightVacation(GuestRequest guestRequest);
        bool BankAccountDebitAuthorization(Host host);
        bool HostingUnitAvability(Order order);
        void UpdateDiary(Order order);
        void UpdateInfoAfterOrderClosed(Order order);
        bool AbleToChangeOrderStatus(Order order);
        double TotalFee(Order order);
        bool TheHostingUnitHasAnOpenOrder(HostingUnit hostingUnit);
        bool RevocationPermission(Host host);//Checks whether account debit authorization can be revoked
        void SendEmail(Order ord);//Sends a customer to the customer with the order details\
        bool IsValidEmail(string email);
        #endregion

        #region functions
        bool CheckAvailable(HostingUnit hostingUnit, DateTime entry, DateTime release);//Checks whether the requested vacation time in a particular unit is free
        IEnumerable<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays);//Returns the list of all available accommodation units on this date
        Int32 NumDays(DateTime start, DateTime end);//Returns the number of days passed from the first date to the second
        Int32 NumDays(DateTime start);//Returns the number of days that have passed from the given date to the present
        List<Order> DaysPassedOrders(Int32 days);//Returns all orders that have elapsed since they were created / since sent emails to a customer greater than or equal to the number of days the function received
        List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic);//Returns all customer requirements that fit a particular condition
        Int32 NumOfInvetations(BE.GuestRequest costumer);//Returns the number of orders sent to a customer
        Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit);//Returns the number of orders sent / number of successfully closed orders for a unit through the site
        Predicate<GuestRequest> BuildPredicate(HostingUnit hosting);//Builds a predicate that filters the hosting units according to the client's requirements
        Order NewOrder(int hostingUnitkey, int guestRequestKey);//Creating order  
        GuestRequest FindGuestRequest(Int32 requestKey);//find the request with this key
        HostingUnit FindUnit(Int32 unitKey);//find the unit with this key
        // FindOrder(Int32 orderKey);//find the unit with this key
        string GetCostumerImagePath(int requestKey);
        void AddCostumerImage(int key, string newImagePath);
        void ChangeCostuerImage(int requestKey, string newImagePath);
        #endregion

        #region grouping
        IEnumerable<IGrouping<Enums.Area, GuestRequest>> GroupGRByArea();
        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers();
        IEnumerable<IGrouping<Enums.HostingUnitType, GuestRequest>> GroupGRByType();
        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByStars();
        IEnumerable<IGrouping<Enums.GuestRequestStatus, GuestRequest>> GroupGRByStatus();
        IEnumerable<IGrouping<Host, HostingUnit>> GroupHostByHostingUnit();
        IEnumerable<IGrouping<Enums.HostingUnitType, HostingUnit>> GroupHostingUnitByType();
        IEnumerable<IGrouping<Enums.Area, HostingUnit>> GroupHostingUnitByArea();
        #endregion

    }
}