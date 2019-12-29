﻿using BE;
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

        public void addHostingUnit(HostingUnit hostingUnit)
        {
            myDAL.addHostingUnit(hostingUnit);
        }

        public void addOrder(Order order)
        {
           myDAL.addOrder(order);
        }

        public List<HostingUnit> getAllHostingUnits()
        {
           return myDAL.getAllHostingUnits();
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> p)
        {
            return myDAL.getHostingUnits(p);
        }

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
        List<BE.HostingUnit> AvailableHostingUnits(DateTime entry, Int32 vactiondays)
        {
            IDAL dal = DAL.factoryDal.getDal("List");
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
            IDAL dal = DAL.factoryDal.getDal("List");
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
            IDAL dal = DAL.factoryDal.getDal("List");
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
            IDAL dal = DAL.factoryDal.getDal("List");
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
            IDAL dal = DAL.factoryDal.getDal("List");
            IEnumerable<Order> listOrders = dal.getOrders();
            foreach(Order ord in listOrders)
            {
                if(ord._HostingUnitKey == hostingunit._HostingUnitKey)
                    i++;
            }
            return i;
        }
        #endregion

        #region grouping
        IEnumerable<IGrouping<Area, GuestRequest>> GroupGRByArea()
        {
            IDAL dal = DAL.factoryDal.getDal("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequests();
            var groupToReturn = from request in listGuestRequests
                                group request by request.Area into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        IEnumerable<IGrouping<int, GuestRequest>> GroupGRByVacationers()
        {
            IDAL dal = DAL.factoryDal.getDal("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequests();
            var groupToReturn = from request in listGuestRequests
                                group request by (request.Children+request.Adults) into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        IEnumerable<IGrouping<int, Host>> GroupHostByHostingUnit()
        {
            IDAL dal = DAL.factoryDal.getDal("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getAllHostingUnits();
            var groupToReturn = from unit in listHostingUnits
                                group unit by unit.Owner into newGroup
                                //orderby newGroup.Key
                                select newGroup;
            return groupToReturn;
        }
        IEnumerable<IGrouping<Area, HostingUnit>> GroupHostByHostingUnit()
        {
            IDAL dal = DAL.factoryDal.getDal("List");
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
