using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Mail;
using System.IO;
using System.Net.Mail;

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
            //string TypeDAL = Configuration.TypeDAL;
            // string TypeDAL = "List";
            myDAL = factoryDAL.getDAL("XML");
        }
        private MyBL() { }
        #endregion

        #region enforcements
        public bool OverNightVacation(GuestRequest guestRequest)
        {
            TimeSpan vacationDays = guestRequest.ReleaseDate - guestRequest.EnteryDate;
            if (vacationDays.Days < 1)
                throw new ExceptionBL("The vacation is less than one day");
            return true;
        }

        public bool BankAccountDebitAuthorization(Host host)
        {
            return host.CollectionClearance;

        }

        public bool HostingUnitAvability(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            List<HostingUnit> hostingUnits = dal.getHostingUnits(x => x.HostingUnitKey == order.HostingUnitKey);
            HostingUnit unit = hostingUnits.Find(x => order.HostingUnitKey == x.HostingUnitKey);
            List<GuestRequest> guestRequests = dal.getGuestRequests(x => order.GuestRequestKey == x.GuestRequestKey);
            GuestRequest guest = guestRequests.Find(x => order.GuestRequestKey == x.GuestRequestKey);
            if (!CheckAvailable(unit, guest.EnteryDate, guest.ReleaseDate))
                throw new ExceptionBL("the unit is not available for these dates");
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
            GuestRequest request = guestRequest.Find(x => x.GuestRequestKey == order.GuestRequestKey);
            request.Status = Enums.GuestRequestStatus.ClosedOnTheWeb;
            SetGuestRequest(request);
            List<Order> ordersForTheRequest = getOrders(x => x.GuestRequestKey == order.GuestRequestKey);
            foreach (var order1 in ordersForTheRequest)
                if (order1.OrderStatus != Enums.OrderStatus.Closed)
                {
                    order1.OrderStatus = Enums.OrderStatus.NotRelevent;
                    setOrder(order1);
                }
        }

        public bool AbleToChangeOrderStatus(Order order)
        {
            if (order.OrderStatus == Enums.OrderStatus.Closed || order.OrderStatus == Enums.OrderStatus.NotRelevent)
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
            List<Order> openOrders = getOrders(x => x.OrderStatus != Enums.OrderStatus.Closed || x.OrderStatus != Enums.OrderStatus.NotRelevent);
            if(openOrders.Count!=0)
                    return false;
            return true;
        }

        public void SendEmail(Order ord)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            GuestRequest gr = dal.getGuestRequests(x => x.GuestRequestKey == ord.GuestRequestKey).Find(x => x.GuestRequestKey == ord.GuestRequestKey);
            Host h = dal.getHostingUnits(hu => hu.HostingUnitKey == ord.HostingUnitKey).Find(hut => hut.HostingUnitKey == ord.HostingUnitKey).Owner;
            try
            {
                IsValidEmail(gr.MailAddress);
                IsValidEmail(h.MailAddress);
            }
            catch (InvalidOperationException a)
            {
                throw a;
            }
            new Thread(() =>
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(gr.MailAddress);
                mail.From = new MailAddress("zimmersProCT@gmail.com");
                mail.Subject = "vacation offer";
                mail.Body = "Hello, I am a Host at 'Zimmers'.I have a proposition that suits your request perfectly.if you are interested in coninuing the process please contact me at " + h.MailAddress;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("zimmersProCT@gmail.com", "Prozimmers");
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }).Start();
        }

        public bool validDate(GuestRequest guest)
        {
            if (DateTime.Today > guest.EnteryDate)
                throw new ExceptionBL("The entry date pass");
            return true;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                throw new InvalidOperationException("invalid email address");
            }
        }
        #endregion

        #region Dalfunctions

        #region HostingUnit
        public int addHostingUnit(HostingUnit hostingUnit)//!!!!!!!
        {
            try
            {
                IDAL dal = DAL.factoryDAL.getDAL("XML");
                return dal.addHostingUnit(hostingUnit.Clone());
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
        }

        public void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            try
            {
                List<HostingUnit> my_unit = dal.getHostingUnits(u => u.HostingUnitKey == hostingUnit.HostingUnitKey);
                HostingUnit hosting = my_unit.Find(u => u.HostingUnitKey == hostingUnit.HostingUnitKey);
                if (hosting.Owner.CollectionClearance == true && hostingUnit.Owner.CollectionClearance == false)
                    if (!RevocationPermission(hostingUnit.Owner))
                        dal.SetHostingUnit(hostingUnit);
                    else
                        throw new ExceptionBL("you have order open");//change text
                else
                    dal.SetHostingUnit(hostingUnit.Clone());
            }
            catch (Exception e)
            {
                throw new ExceptionBL(e.Message);
            }
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.getHostingUnits(predicate);
        }

        public List<HostingUnit> getHostingUnitsList()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.getHostingUnitsList();
        }
        #endregion

        #region GuestRequest
        public void SetGuestRequest(GuestRequest guestRequest)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.GetGuestRequestsList();
        }

        public List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate)//!!!!
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.getGuestRequests(predicate);
        }

        public int addGuestRequest(GuestRequest guestRequest)
        {
            try
            {
                IDAL dal = DAL.factoryDAL.getDAL("XML");
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            try
            {
                List<Order> orders = getOrders(x => x.OrderKey == order.OrderKey);
                Order ord = orders.Find(x => x.OrderKey == order.OrderKey);
                if (AbleToChangeOrderStatus(ord) == true)
                {
                    if (order.OrderStatus == Enums.OrderStatus.Closed)
                    {
                        order.Fee = TotalFee(order);
                        dal.setOrder(order);
                        UpdateDiary(order);                        
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.GetOrdersList();
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.getOrders(predicate);
        }

        public bool OrderExist(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.OrderExist(order);
        }

        public Order FindOrder(Int32 ordKey)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            return dal.FindOrder(ordKey);
        }
        #endregion

        #region BankBranches
        public IEnumerable<IGrouping<int, BankBranch>> GetBankBranchesGroup()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            try
            {
                List<BankBranch> my_bank = dal.GetBankBranchesList();
                var my_group = from branches in my_bank
                       group branches by branches.BankNumber into allBanks
                       select allBanks;
                return my_group;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #endregion

        #region functions       

        public bool CheckAvailable(HostingUnit hostingUnit, DateTime entry, DateTime realese)
        {
            int d = (realese - entry).Days;
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

        public IEnumerable<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<HostingUnit> listHostingUnit = dal.getHostingUnitsList();
            var my_unit_list = from HostingUnit hostingUnit in listHostingUnit
                               where CheckAvailable(hostingUnit, entry, entry.AddDays(vactiondays))
                               select hostingUnit;
            return my_unit_list;
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
 
            var result = from Order ord in listOrders
                         let tr = (days < (DateTime.Now - ord.CreateDate).Days) || (days < (DateTime.Now - ord.OrderDate).Days)
                         where tr
                         select ord;
            return result.ToList();
        }

        public List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic, HostingUnit hosting)
        {
            List<GuestRequest> listToReturn = new List<GuestRequest>();
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            List<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            List<Order> myOrders = dal.getOrders(o => o.HostingUnitKey == hosting.HostingUnitKey);
            bool temp = true;
            foreach (GuestRequest request in listGuestRequests)
            {
                if (request.Status == Enums.GuestRequestStatus.Active && myOrders.Find(o=>o.GuestRequestKey == request.GuestRequestKey) == null) 
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

            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            int i = 0;
            var temp = from Order ord in listOrders
                       where ord.GuestRequestKey == costumer.GuestRequestKey
                       select new { ord, couner = ++i };                     
            return temp.Last().couner;
        }

        public Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit)
        {
            int i = 0;
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            var temp = from Order ord in listOrders
                       where ord.HostingUnitKey == hostingunit.HostingUnitKey && ord.OrderStatus == Enums.OrderStatus.Closed
                       select new {ord, couner= ++i};
            return temp.Last().couner;
        }

        public Predicate<GuestRequest> BuildPredicate(HostingUnit hosting)//Builds a predicate that filters the hosting units according to the client's requirements
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<GuestRequest> guestRequests = dal.GetGuestRequestsList();
            Predicate<GuestRequest> pred = default(Predicate<GuestRequest>);
            bool RGPool(GuestRequest request) { return request.Pool == true || request.Pool == null; }
            bool RGNoPool(GuestRequest request) { return request.Pool == false || request.Pool == null; }
            bool RGJacuzzi(GuestRequest request) { return request.Jacuzzi == true || request.Jacuzzi == null; }
            bool RGNoJacuzzi(GuestRequest request) { return request.Jacuzzi == false || request.Jacuzzi == null; }
            bool RGGarden(GuestRequest request) { return request.Garden == true || request.Garden == null; }
            bool RGNoGarden(GuestRequest request) { return request.Garden == false || request.Garden == null; }
            bool RGChildrenAttraction(GuestRequest request) { return request.ChildrenAttraction == true || request.ChildrenAttraction == null; }
            bool RGNoChildrenAttraction(GuestRequest request) { return request.ChildrenAttraction == false || request.ChildrenAttraction == null; }
            bool RGMeals(GuestRequest request) { return request.Meals == true || request.Meals == null; }
            bool RGNoMeals(GuestRequest request) { return request.Meals == false || request.Meals == null; }
            bool RGSynagogue(GuestRequest request) { return request.Synagogue == true || request.Synagogue == null; }
            bool RGNoSynagogue(GuestRequest request) { return request.Synagogue == null || request.Synagogue == null; }
            bool RGStars(GuestRequest request) { return request.Stars <= hosting.Stars; }
            bool RGArea(GuestRequest request) { return request.Area == hosting.Area || request.Area == Enums.Area.All; }
            bool RGSubArea(GuestRequest request) { return ((request.SubArea == hosting.SubArea) || (request.SubArea == null) || hosting.SubArea == null); }
            bool RGType(GuestRequest request) { return request.Type == hosting.HostingUnitType; }
            bool RGAdultes(GuestRequest request) { return request.Adults <= hosting.Adults; }
            bool RGKids(GuestRequest request) { return request.Children <= hosting.Kids; }

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
            if (hosting.Synagogue)
                pred += RGSynagogue;
            else
                pred += RGNoSynagogue;
            pred += RGArea;
            pred += RGSubArea;
            pred += RGType;
            pred += RGStars;
            pred += RGAdultes;
            pred += RGKids;
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
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            List<GuestRequest> guests = dal.GetGuestRequestsList();
            foreach (GuestRequest item in guests)
            {
                if (item.GuestRequestKey == requestKey)
                    return item;
            }
            return null;
        }

        public HostingUnit FindUnit(Int32 unitKey)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            List<HostingUnit> units = dal.getHostingUnitsList();
            foreach (HostingUnit item in units)
            {
                if (item.HostingUnitKey == unitKey)
                    return item;
            }
            return null;
        }

        public double Aggregate_fee()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            double all_fee = 0;
            List<Order> orders = dal.GetOrdersList();
            foreach (Order ord in orders)
            {
                all_fee += ord.Fee;
            }
            return all_fee;
        }

        public List<GuestRequest> DaysPassFromMail(Int32 days)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            List<GuestRequest> requests = dal.GetGuestRequestsList();
            var requestsList = from GuestRequest request in requests
                               where (request.RegistrationDate.AddDays(days) < DateTime.Today && request.Status == Enums.GuestRequestStatus.Active)
                               select request;
            return requestsList.ToList();
        }
        #endregion

        #region grouping
        public IEnumerable<IGrouping<Enums.Area, GuestRequest>> GroupGRByArea()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by request.Area;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Enums.GuestRequestStatus, GuestRequest>> GroupGRByStatus()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by request.Status;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Enums.HostingUnitType, GuestRequest>> GroupGRByType()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Type);
            return groupToReturn;
        }

        public IEnumerable<IGrouping<int, GuestRequest>> GroupGRByStars()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Stars);
            return groupToReturn;
        }

        public IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Children + request.Adults);
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Host, HostingUnit>> GroupHostByHostingUnit()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            IEnumerable<IGrouping<Host, HostingUnit>> groupToReturn = from unit in listHostingUnits
                                                                      group unit by unit.Owner;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Enums.HostingUnitType, HostingUnit>> GroupHostingUnitByType()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            IEnumerable<IGrouping<Enums.HostingUnitType, HostingUnit>> groupToReturn = from unit in listHostingUnits
                                                                                       group unit by unit.HostingUnitType;
            return groupToReturn;
        }

        public IEnumerable<IGrouping<Enums.Area, HostingUnit>> GroupHostingUnitByArea()
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            var groupToReturn = from unit in listHostingUnits
                                group unit by unit.Area;
            return groupToReturn;
        }
        #endregion
    }
}
