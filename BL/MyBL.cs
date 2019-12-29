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
            return host.CollectionClearance1;

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

        public bool ableToChangeOrderStatus(Order order)
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
                return true;
            return false;
        }
        #endregion

       

        #region Dalfunctions
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

        public void addHostingUnit(HostingUnit hostingUnit)
        {
            myDAL.addHostingUnit(hostingUnit);
        }

        public void addOrder(Order order)
        {
           myDAL.addOrder(order);
        }

        void SetHostingUnit(HostingUnit hostingUnit)
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

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            return myDAL.getOrders(predicate);
        }

        #endregion

        #region change now
        public bool RevocationPermission(int bankAccountNumber, BankBranch bankBranchDetails)
        {
            //?????????????????/
        }

        void SendEmail(Order ord)
        {
            MailMessage my_mail = new MailMessage();
            my_mail.To.Add("Client");
            my_mail.From = new MailAddress("zimmers");
            mail.Subject = "your request";
            mail.Body = "items";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("tamarbuterman@gmail.com", "myGmailPassword");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        bool CheckAvailable(HostingUnit hostingUnit, DateTime entry, Int32 vactiondays)
        {
             //לבדוק אם יש תאריכים חופפים-לקחת חלק מהקוד מתרגיל 2
             bool[,] diary=hostingUnit.Diary;
             int i=entry.Month;
             int j=entry.Day; 
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

        List<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays)
        {
            IDAL dal = DAL.FactoryDal.getDal();
            List<BE.HostingUnit> listToReturn;
            //List<BE.HostingUnit> listHostingUnit = dal.getAllHostingUnits();
            IEnumerable<HostingUnit> listHostingUnit = dal.getAllHostingUnits();
            //IEnumerator enumerator = listHostingUnit.GetEnumerator();
            //while (enumerator.MoveNext())
            foreach (HostingUnit hostingUnit in listHostingUnit)
            {
                if(checkAvailable(hostingUnit, entry, vactiondays))
                    listToReturn.Add(hostingUnit);
            }
            return listToReturn;
        }

        Int32 NumDays(DateTime start, DateTime end=default(DateTime.Now))
        {
            return (end-start).Days;
        }

        List<Order> DaysPassedOrders(Int32 days)
        {
            List<Order> listToReturn;
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<Order> listOrders = dal.getOrders();
            foreach(Order ord in listOrders)
            {
                Int32 create = (DateTime.Now - ord.createDate).Days;
                Int32 sent = (DateTime.Now - ord.orderDate).Days;
                if((days < create) || (days < sent))
                    listToReturn.Add(ord);
            }
            return listToReturn;
        }

        List<GuestRequest> RequestMatchToStipulation(Predicate<GuestRequest> predic)
        {
            List<GuestRequest> listToReturn;
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequests();
            foreach(GuestRequest request in listGuestRequests)
            {
                if(predic(request))
                    listToReturn.Add(request);
            }
            return listToReturn;
        }

        Int32 NumOfInvetations(GuestRequest costumer)
        {
            int i=0;            
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<Order> listOrders = dal.getOrders();
            foreach(Order ord in listOrders)
            {
                if(ord._GuestRequestKey == costumer._GuestRequestKey)
                    i++;
            }
            return i;
        }

        Int32 NumOfSuccessfullOrders(BE.HostingUnit hostingunit)
        {
            int i=0;            
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<Order> listOrders = dal.getOrders();
            foreach(Order ord in listOrders)
            {
                if(ord._HostingUnitKey == costumer._HostingUnitKey)
                    i++;
            }
            return i;
        }
        #endregion

        #region grouping
        IEnumerable<IGrouping<Enums.Area , GuestRequest>> GroupGRByArea()
        {
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequests();
            var groupToReturn = from request in listGuestRequests
                                group request by request.Area into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers()
        {
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequests();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Children+request.Adults) into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        IEnumerable<IGrouping<int, Host>> GroupHostByHostingUnit()
        {
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<HostingUnit> listHostingUnits = dal.getAllHostingUnits();
            var groupToReturn = from unit in listHostingUnits
                                group unit by unit.Owner into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        IEnumerable<IGrouping<Area, HostingUnit>> GroupHostByHostingUnit()
        {
            IDAL dal = DAL.FactoryDal.getDal();
            IEnumerable<HostingUnit> listHostingUnits = dal.getAllHostingUnits();
            var groupToReturn = from unit in listHostingUnits
                                group unit by unit.Area into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        #endregion
    }
}
