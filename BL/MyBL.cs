using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace BL
{
    public class MyBL: IBL
    {
        static IDAL myDAL;

        #region Singleton
        private static readonly MyBL instance = new MyBL();

        public static MyBL Instance
        {
            get { return instance; }
        }

        static MyBL()
        {
            // string TypeDAL = ConfigurationSettings.AppSettings.Get("TypeDS");
            string TypeDAL = Configuration.TypeDAL;
            // string TypeDAL = "List";
            myDAL = factoryDAL.getDAL(TypeDAL);
        }
        private MyBL() { }
        #endregion

        #region enforcements
        public bool OverNightVacation(GuestRequest guestRequest)
        {
            TimeSpan vacationDays = guestRequest.ReleaseDate - guestRequest.EnteryDate;
            if (vacationDays.Days < 1)
                return false;
            return true;
        }

        public bool BankAccountDebitAuthorization(Host host)
        {
            return host;

        }

        public bool HostingUnitAvability(Order order)
        {
            List<HostingUnit> hostingUnits = getHostingUnits(x => order.HostingUnitKey == x.HostingUnitKey);
            bool[,] diary = hostingUnits.Find(x => order.HostingUnitKey == x.HostingUnitKey).Diary;
            List<GuestRequest> guestRequests = getGuestRequests(x => order.GuestRequestKey == x.GuestRequestKey);
            GuestRequest guest = guestRequests.Find(x => order.GuestRequestKey == x.GuestRequestKey);
            TimeSpan d = guest.ReleaseDate - guest.EnteryDate;
            Int32 i = guest.EnteryDate.Month - 1;
            Int32 j = guest.EnteryDate.Day - 1;
            for (int k = 0; k < d.Days; k++)
            {
                if (j > 30)
                {
                    j = 0;
                    i++;
                }
                if (diary[i, j] == true)
                { 
                    return false;
                }
                j++;
            }
            return true;

        }

        public void UpdateDiary(Order order)
        {
            List<HostingUnit> hostingUnits = getHostingUnits(x => order.HostingUnitKey == x.HostingUnitKey);
            HostingUnit unit=hostingUnits.Find(x => order.HostingUnitKey == x.HostingUnitKey);
            bool[,] diary = unit.Diary;
            List<GuestRequest> guestRequests = getGuestRequests(x => order.GuestRequestKey == x.GuestRequestKey);
            GuestRequest guest = guestRequests.Find(x => order.GuestRequestKey == x.GuestRequestKey);
            TimeSpan d = guest.ReleaseDate - guest.EnteryDate;
            Int32 i = guest.EnteryDate.Month - 1;
            Int32 j = guest.EnteryDate.Day - 1;

            for (int k = 0; k < d.Days; k++)
            {
                if (j > 30)
                {
                    j = 0;
                    i++;
                }
                diary[i, j] = true;
                j++;
            }
            unit.Diary = diary;
            SetHostingUnit(unit);
        }

        public void UpdateInfoAfterOrderClosed(Order order)
        {
            List<GuestRequest> guestRequest = getGuestRequests(x => x.GuestRequestKey == order.GuestRequestKey);
            guestRequest.Find(x => x.GuestRequestKey == order.GuestRequestKey).Status = Enums.GuestRequestStatus.ClosedOnTheWeb;
            List<Order> ordersForTheRequest = getOrders(x => x.GuestRequestKey == order.GuestRequestKey);
            foreach (var order1 in ordersForTheRequest)
                if (order1.OrderStatus != Enums.OrderStatus.Closed)
                    order1.OrderStatus = Enums.OrderStatus.NotRelevent;
        }

        public bool AbleToChangeOrderStatus(Order order)
        {
            if (order.OrderStatus == Enums.OrderStatus.Closed) 
            return false;
                return true;
        }

        public double TotalFee(Order order)
        {
            List<GuestRequest> guestRequest = getGuestRequests(x => x.GuestRequestKey == order.GuestRequestKey);
            GuestRequest request= guestRequest.Find(x => x.GuestRequestKey == order.GuestRequestKey);
            return Configuration.Fee * (double)NumDays(request.EnteryDate, request.ReleaseDate);
        }

        public bool TheHostingUnitHasAnOpenOrder(HostingUnit hostingUnit)
        {
            List<Order> openOrders = getOrders(x => x.HostingUnitKey == hostingUnit.HostingUnitKey && x.OrderStatus == Enums.OrderStatus.Active);
            if (openOrders.Count == 0)
                return false;
            return true;
        }
        #endregion

        #region Dalfunctions

        #region HostingUnit
        public void addHostingUnit(HostingUnit hostingUnit)
        {
            myDAL.addHostingUnit(hostingUnit);
        }

        public void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            if (TheHostingUnitHasAnOpenOrder (hostingUnit)==false)
            myDAL.DeleteHostingUnit(hostingUnit);
        }

        public void SetHostingUnit(HostingUnit hostingUnit)
        {
            myDAL.SetHostingUnit(hostingUnit);
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate)
        {
            return myDAL.getHostingUnits(predicate);
        }

        public List<HostingUnit> getHostingUnitsList()
        {
            return myDAL.getHostingUnitsList();
        }
        #endregion

        #region GuestRequest
        public void SetGuestRequest(GuestRequest guestRequest)
        {
            myDAL.SetGuestRequest(guestRequest);
        }

        public List<GuestRequest> GetGuestRequestsList()
        {
            return myDAL.GetGuestRequestsList();
        }

        public List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate)
        {
            return myDAL.getGuestRequests(predicate);
        }

        public void addGuestRequest(GuestRequest guestRequest)
        {
            if (OverNightVacation(guestRequest)==true)
            myDAL.addGuestRequest(guestRequest);
            else 
                //throw "not over night vacation"
        }
        #endregion

        #region Order
        
        public void setOrder(Order order)
        {
            if (order.OrderStatus == Enums.OrderStatus.Closed)
            {
                myDAL.setOrder(order);
                UpdateDiary(order);
                TotalFee(order);//what to do with returned value?
                UpdateInfoAfterOrderClosed(order);
            }
            else
            {
                List<Order> orders = getOrders(x => x.OrderKey == order.OrderKey);
                Order ord = orders.Find(x => x.OrderKey == order.OrderKey);
                if (AbleToChangeOrderStatus(ord) == true)
                {
                    if (order.OrderStatus == Enums.OrderStatus.SentEmail)
                    {
                        List<HostingUnit> hostingUnits = getHostingUnits(x => order.HostingUnitKey == x.HostingUnitKey);
                        HostingUnit unit = hostingUnits.Find(x => order.HostingUnitKey == x.HostingUnitKey);
                        if (BankAccountDebitAuthorization(unit.Owner) == true)
                            myDAL.setOrder(order);
                    }
                    else
                        myDAL.setOrder(order);
                }

            }
            
            
        }
        
        public void AddOrder(Order order)
        {
            if (HostingUnitAvability(order)==true)
             myDAL.addOrder(order);
        }

        public List<Order> GetOrdersList()
        {
           return myDAL.GetOrdersList();
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            return myDAL.getOrders(predicate);
        }
        #endregion

        #endregion

        #region change now
        public bool RevocationPermission(int bankAccountNumber, BankBranch bankBranchDetails)
        {
            //?????????????????
        }

        public void SendEmail(Order ord)
        {
            Console.WriteLine("email was sent");
        }

        public bool CheckAvailable(HostingUnit hostingUnit, DateTime entry, Int32 vactiondays)
        {
             bool[,] diary=hostingUnit.Diary;
             int i=entry.Month-1;
             int j=entry.Day-1; 
             for(int k=0; k<vactiondays; k++)
             {
                if(diary[i,j])
                    return false;
                if(j==30)
                {
                    j=0;
                    i++;
                }
             }
             return true;
        }

        public List<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            List<BE.HostingUnit> listToReturn = null;
            IEnumerable<HostingUnit> listHostingUnit = dal.getHostingUnitsList();
            foreach (HostingUnit hostingUnit in listHostingUnit)
            {
                if (CheckAvailable(hostingUnit, entry, vactiondays))
                    listToReturn.Add(hostingUnit.Clone());
            }
            return listToReturn;
        }

        public Int32 NumDays(DateTime start, DateTime end)
        {
            return (end-start).Days;
        }

        public Int32 NumDays(DateTime start)
        {
            DateTime end = DateTime.Now;
            return (end - start).Days;
        }

        public List<Order> DaysPassedOrders(Int32 days)
        {
            List<Order> listToReturn = null;
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            foreach(Order ord in listOrders)
            {
                Int32 create = (DateTime.Now - ord.CreateDate).Days;
                Int32 sent = (DateTime.Now - ord.OrderDate).Days;
                if((days < create) || (days < sent))
                    listToReturn.Add(ord);
            }
            return listToReturn;
        }

        public List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic)
        {
            List<GuestRequest> listToReturn = null;
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            foreach(GuestRequest request in listGuestRequests)
            {
                if(predic(request))
                    listToReturn.Add(request);
            }
            return listToReturn;
        }

        public Int32 NumOfInvetations(GuestRequest costumer)
        {
            List<Order> temp = null; ;            
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            foreach(Order ord in listOrders)
            {
                if (ord.GuestRequestKey == costumer.GuestRequestKey)
                    temp.Add(ord.Clone());
            }
            return temp.Count;
        }

        public Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit)
        {
            int i=0;            
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            foreach(Order ord in listOrders)
            {
                if(ord.HostingUnitKey == hostingunit.HostingUnitKey)
                    i++;
            }
            return i;
        }
        #endregion

        #region grouping
        IEnumerable<IGrouping<Enums.Area , GuestRequest>> GroupGRByArea()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by request.Area into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }

        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Children+request.Adults) into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Host, HostingUnit>> GroupHostByHostingUnit()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            var groupToReturn = from unit in listHostingUnits
                                group unit by unit.Owner into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }

        IEnumerable<IGrouping<Enums.Area, HostingUnit>> GroupHostingUnitByArea()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            var groupToReturn = from unit in listHostingUnits
                                group unit by unit.Area into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        #endregion
    }
}
