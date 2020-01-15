using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;

namespace BL
{
    public class MyBL : IBL
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
                throw new ArgumentException("The vacation is less than one day");
            return true;
        }

        public bool BankAccountDebitAuthorization(Host host)
        {
            return host.CollectionClearance;

        }

        public bool HostingUnitAvability(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            List<HostingUnit> hostingUnits = dal.getHostingUnits(x => x.HostingUnitKey == order.HostingUnitKey);
            HostingUnit unit = hostingUnits.Find(x => order.HostingUnitKey == x.HostingUnitKey);
            List<GuestRequest> guestRequests = dal.getGuestRequests(x => order.GuestRequestKey == x.GuestRequestKey);
            GuestRequest guest = guestRequests.Find(x => order.GuestRequestKey == x.GuestRequestKey);
            if (!CheckAvailable(unit, guest.EnteryDate, guest.ReleaseDate))
                throw new ArgumentException("the unit is not available for these dates");
            return true;
           
        }

        public void UpdateDiary(Order order)
        {
            List<HostingUnit> hostingUnits = getHostingUnits(x => order.HostingUnitKey == x.HostingUnitKey);
            HostingUnit unit = hostingUnits.Find(x => order.HostingUnitKey == x.HostingUnitKey);
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
                throw new ExceptionBL("The order is already closed");
            return true;
        }

        public double TotalFee(Order order)
        {
            List<GuestRequest> guestRequest = getGuestRequests(x => x.GuestRequestKey == order.GuestRequestKey);
            GuestRequest request = guestRequest.Find(x => x.GuestRequestKey == order.GuestRequestKey);
            return Configuration.Fee * (double)NumDays(request.EnteryDate, request.ReleaseDate);
        }

        public bool TheHostingUnitHasAnOpenOrder(HostingUnit hostingUnit)
        {
            List<Order> openOrders = getOrders(x => x.HostingUnitKey == hostingUnit.HostingUnitKey && x.OrderStatus == Enums.OrderStatus.Active);
            if (openOrders.Count != 0)
                throw new ExceptionBL("the Hosting unit has open orders");
            return false; 
        }

        public bool RevocationPermission(Host host)
        {
            List<Order> openOrders = getOrders(x => x.OrderStatus == Enums.OrderStatus.Active);
            List<HostingUnit> hostingUnits = null;
            foreach (var order in openOrders)
                hostingUnits.Add(FindHostingUnit(order.HostingUnitKey));
            foreach (var unit in hostingUnits)
                if (unit.Owner.HostKey == host.HostKey)

                    return false;
            return true;
        }

        public void SendEmail(Order ord)
        {
            Console.WriteLine("email was sent");
        }

        public bool validDate(GuestRequest guest)
        {
            //TimeSpan vacationDays = DateTime.Today - guest.EnteryDate;
            if (DateTime.Today > guest.EnteryDate)
                throw new ArgumentException("The entry date pass");
            return true;
        }
        #endregion

        #region Dalfunctions

        #region HostingUnit
        public int addHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                IDAL dal = DAL.factoryDAL.getDAL("List");
                return dal.addHostingUnit(hostingUnit);
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
        }

        public void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            try
            {
                if (TheHostingUnitHasAnOpenOrder(hostingUnit) == false)
                    dal.DeleteHostingUnit(hostingUnit);
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
        }

        public void SetHostingUnit(HostingUnit hostingUnit)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            try
            {
                dal.SetHostingUnit(hostingUnit);
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            return dal.getHostingUnits(predicate);
        }

        public List<HostingUnit> getHostingUnitsList()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            return dal.getHostingUnitsList();
        }
        #endregion

        #region GuestRequest
        public void SetGuestRequest(GuestRequest guestRequest)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            try
            {
                dal.SetGuestRequest(guestRequest);
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
        }

        public List<GuestRequest> GetGuestRequestsList()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            return dal.GetGuestRequestsList();
        }

        public List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            return dal.getGuestRequests(predicate);
        }

        public int addGuestRequest(GuestRequest guestRequest)
        {
            try
            {
                IDAL dal = DAL.factoryDAL.getDAL("List");
                if ((OverNightVacation(guestRequest)) && (validDate(guestRequest)))
                    return dal.addGuestRequest(guestRequest);
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
            return 0;
        }
        #endregion

        #region Order

        public void setOrder(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            try
            {
                List<Order> orders = getOrders(x => x.OrderKey == order.OrderKey);
                Order ord = orders.Find(x => x.OrderKey == order.OrderKey);
                if (AbleToChangeOrderStatus(ord) == true)
                {
                    if (order.OrderStatus == Enums.OrderStatus.Closed)
                    {
                        dal.setOrder(order);
                        UpdateDiary(order);
                        TotalFee(order);//what to do with returned value?
                        UpdateInfoAfterOrderClosed(order);
                    }
                    else
                    {

                        if (order.OrderStatus == Enums.OrderStatus.SentEmail)
                        {
                            List<HostingUnit> hostingUnits = getHostingUnits(x => order.HostingUnitKey == x.HostingUnitKey);
                            HostingUnit unit = hostingUnits.Find(x => order.HostingUnitKey == x.HostingUnitKey);
                            if (BankAccountDebitAuthorization(unit.Owner) == true)
                            {
                                dal.setOrder(order);
                                SendEmail(order);
                            }
                            else
                                throw new ExceptionBL("Has not banck account permission");
                        }
                        else
                            dal.setOrder(order);
                    }

                }
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }

        }

        public int AddOrder(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            try
            {
                if (HostingUnitAvability(order))
                    return dal.addOrder(order);
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
            return 0;
        }

        public List<Order> GetOrdersList()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            return dal.GetOrdersList();
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            return dal.getOrders(predicate);
        }

        public bool OrderExist(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            return dal.OrderExist(order);
        }

        public Order FindOrder(Int32 ordKey)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");         
            return dal.FindOrder(ordKey);
        }
        #endregion

        #endregion

        #region functions       

        public bool CheckAvailable(HostingUnit hostingUnit, DateTime entry, DateTime realese)
        {
            int d =(realese - entry).Days;
            bool[,] diary = hostingUnit.Diary;
            int i = entry.Month - 1;
            int j = entry.Day - 1;
            for (int k = 0; k < d; k++)
            {
                if (diary[i, j])
                    return false;
                if (j == 30)
                {
                    j = 0;
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
                if (CheckAvailable(hostingUnit, entry, entry.AddDays(vactiondays)))
                    listToReturn.Add(hostingUnit.Clone());
            }
            return listToReturn;
        }

        public Int32 NumDays(DateTime start, DateTime end)
        {
            return (end - start).Days;
        }

        public Int32 NumDays(DateTime start)
        {
            DateTime end = DateTime.Now;
            return (end - start).Days;
        }

        public HostingUnit FindHostingUnit(int unitKey)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<HostingUnit> listHostingUnit = dal.getHostingUnitsList();
            foreach (HostingUnit unit in listHostingUnit)
            {
                if (unit.HostingUnitKey == unitKey)
                    return unit;               
            }
            return null;
        }

        public List<Order> DaysPassedOrders(Int32 days)
        {
            List<Order> listToReturn = null;
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            foreach (Order ord in listOrders)
            {
                Int32 create = (DateTime.Now - ord.CreateDate).Days;
                Int32 sent = (DateTime.Now - ord.OrderDate).Days;
                if ((days < create) || (days < sent))
                    listToReturn.Add(ord);
            }
            return listToReturn;
        }

        public List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic)
        {

            List<GuestRequest> listToReturn = new List<GuestRequest>();
            IDAL dal = DAL.factoryDAL.getDAL("List");
            List<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            bool temp = true;
            foreach (GuestRequest request in listGuestRequests)
            {
                if (request.Status == Enums.GuestRequestStatus.Active)
                {
                    foreach (Predicate<GuestRequest> item in predic.GetInvocationList())
                    {
                        if (!item(request))
                            temp = false;
                    }
                    if (temp)
                        listToReturn.Add(request);
                    temp = true;
                }
            }
            return listToReturn;
        }

        public Int32 NumOfInvetations(GuestRequest costumer)
        {
            List<Order> temp = null; ;
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            foreach (Order ord in listOrders)
            {
                if (ord.GuestRequestKey == costumer.GuestRequestKey)
                    temp.Add(ord.Clone());
            }
            return temp.Count;
        }

        public Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit)
        {
            int i = 0;
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            foreach (Order ord in listOrders)
            {
                if (ord.HostingUnitKey == hostingunit.HostingUnitKey)
                    i++;
            }
            return i;
        }

        public Predicate<GuestRequest> BuildPredicate(HostingUnit hosting)//Builds a predicate that filters the hosting units according to the client's requirements
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> guestRequests = dal.GetGuestRequestsList();
            Predicate<GuestRequest> pred = default(Predicate<GuestRequest>);
            bool RGPool(GuestRequest request) { return request.Pool == Enums.intrested.Necessary || request.Pool == Enums.intrested.Possible; }
            bool RGNoPool(GuestRequest request) { return request.Pool == Enums.intrested.NoThanks || request.Pool == Enums.intrested.Possible; }
            bool RGJacuzzi(GuestRequest request) { return request.Jacuzzi == Enums.intrested.Necessary || request.Jacuzzi == Enums.intrested.Possible; }
            bool RGNoJacuzzi(GuestRequest request) { return request.Jacuzzi == Enums.intrested.NoThanks || request.Jacuzzi == Enums.intrested.Possible; }
            bool RGGarden(GuestRequest request) { return request.Garden == Enums.intrested.Necessary || request.Garden == Enums.intrested.Possible; }
            bool RGNoGarden(GuestRequest request) { return request.Garden == Enums.intrested.NoThanks || request.Garden == Enums.intrested.Possible; }
            bool RGChildrenAttraction(GuestRequest request) { return request.ChildrenAttraction == Enums.intrested.Necessary || request.ChildrenAttraction == Enums.intrested.Possible; }
            bool RGNoChildrenAttraction(GuestRequest request) { return request.ChildrenAttraction == Enums.intrested.NoThanks || request.ChildrenAttraction == Enums.intrested.Possible; }
            bool RGMeals(GuestRequest request) { return request.Meals == Enums.intrested.Necessary || request.Meals == Enums.intrested.Possible; }
            bool RGNoMeals(GuestRequest request) { return request.Meals == Enums.intrested.NoThanks || request.Meals == Enums.intrested.Possible; }
            bool RGStars(GuestRequest request) { return request.Stars <= hosting.Stars; }
            bool RGArea(GuestRequest request) { return request.Area == hosting.Area; }
            bool RGSubArea(GuestRequest request) { return request.SubArea == hosting.SubArea; }
            bool RGType(GuestRequest request) { return request.Type == hosting.HostingUnitType; }

            if (hosting.Pool)
                pred += RGPool;
            else pred += RGNoPool;
            if (hosting.Jaccuzi)
                pred += RGJacuzzi;
            else pred += RGNoJacuzzi;
            if (hosting.Garden)
                pred += RGGarden;
            else pred += RGNoGarden;
            if (hosting.ChildrenAttraction)
                pred += RGChildrenAttraction;
            else pred += RGNoChildrenAttraction;
            if (hosting.Meals)
                pred += RGMeals;
            else pred += RGNoMeals;
            pred += RGArea;
            pred += RGSubArea;
            pred += RGType;
            pred += RGStars;
            return pred;
        }

        public Order NewOrder(int hostingUnitkey, int guestRequestKey)
        {
            Order ord = new Order
            { GuestRequestKey = guestRequestKey, HostingUnitKey = hostingUnitkey, OrderStatus = Enums.OrderStatus.Active, CreateDate = DateTime.Now };
            return ord;
        }  

        public GuestRequest FindGuestRequest(Int32 requestKey)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            List<GuestRequest> guests = dal.GetGuestRequestsList();
            foreach(GuestRequest item in guests)
            {
                if(item.GuestRequestKey == requestKey)
                    return item;
            }
            return null;
        }

        public HostingUnit FindUnit(Int32 unitKey)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            List<HostingUnit> units = dal.getHostingUnitsList();
            foreach (HostingUnit item in units)
            {
                if (item.HostingUnitKey == unitKey)
                    return item;
            }
            return null;
        }

        //public Order FindOrder(Int32 orderKey)
        //{
        //    IDAL dal = DAL.factoryDAL.getDAL("List");
        //    List<Order> orders = dal.GetOrdersList();
        //    foreach (Order item in orders)
        //    {
        //        if (item.HostingUnitKey == orderKey)
        //            return item;
        //    }
        //    return null;
        //}

        public string GetCostumerImagePath(int requestKey)
        {
            string tempSource = System.IO.Path.GetTempFileName();
            File.Copy(@"..\..\..\CostumersImages\" + requestKey + @".jpg", tempSource, true);
            return tempSource;
        }

        public void AddCostumerImage(int key, string newImagePath)
        {
            string photoPath = @"..\..\..\TestersImages\" + key + @".jpg";
            (File.Create(photoPath)).Close();
            System.IO.File.Copy(newImagePath, photoPath, true);
        }

        public void ChangeCostuerImage(int requestKey, string newImagePath)
        {
            string destination = @"..\..\..\TestersImages\" + requestKey + @".jpg";
            try
            {
                File.Delete(destination);
                (File.Create(destination)).Close();
                System.IO.File.Copy(newImagePath, destination, true);
            }
            catch { throw; }
        }
        #endregion

        #region grouping
        public IEnumerable<IGrouping<Enums.Area, GuestRequest>> GroupGRByArea()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by request.Area;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Enums.GuestRequestStatus, GuestRequest>> GroupGRByStatus()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by request.Status;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Enums.HostingUnitType, GuestRequest>> GroupGRByType()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Type);
            return groupToReturn;
        }

        public IEnumerable<IGrouping<int, GuestRequest>> GroupGRByStars()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Stars);
            return groupToReturn;
        }

        public IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Children + request.Adults);
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Host, HostingUnit>> GroupHostByHostingUnit()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            IEnumerable<IGrouping<Host, HostingUnit>> groupToReturn = from unit in listHostingUnits
                                                                      group unit by unit.Owner;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Enums.Area, HostingUnit>> GroupHostingUnitByArea()
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            var groupToReturn = from unit in listHostingUnits
                                group unit by unit.Area;
            return groupToReturn;
        }
        #endregion
    }
}
