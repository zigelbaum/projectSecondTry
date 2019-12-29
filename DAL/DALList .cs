using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    internal class DALList : IDAL
    {
        #region Singleton
        private static readonly DALList instance = new DALList();
        public static DALList Instance
        {
            get { return instance; }
        }

        private DALList() { }
        static DALList() { }

        #endregion

        #region HostingUnit
        List<HostingUnit> getHostingUnitsList()
        {

        }

        public void addHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if(DataSource.hostingUnits.Any(h=>h.HostingUnitKey == hostingUnit.HostingUnitKey))
                    DataSource.hostingUnits.Add(hostingUnit);
                else
                    throw new ExistException("Hosting Unit already exists");
            }
            catch (ExistException c)
            {
                throw c;
            }
        }

        void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if(DataSource.hostingUnits.Any(h=>h.HostingUnitKey == hostingUnit.HostingUnitKey))
                    throw new NotExistException("Hosting Unit not exists");
                else
                    DataSource.hostingUnits.Remove(hostingUnit);
            }
            catch (NotExistException c)
            {
                throw c;
            }
        }

        void SetHostingUnit(HostingUnit hostingUnit)
        {
            //!!!!!!!!!!!!!!!!!
            try
            {
                var my_request = from gu in DataSource.guestRequests
                                 where gu.GuestRequestKey == guest.GuestRequestKey
                                 select gu;
                if(my_request == null)
                    throw new NotExist("The hosting unit is not exist");
                else
                {
                    //לעדכן
                }
            }
            catch (NotExist c)
            {
                throw c;
            }
        }

        public List<HostingUnit> getAllHostingUnits()
        {
           return DataSource.hostingUnits.Select(hu =>(HostingUnit) hu.Clone()).ToList();
           // return DataSource.hostingUnits.ToArray().Clone();
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate = null)
        {
            return DataSource.hostingUnits.Where(predicate).Select(hu => (HostingUnit)hu.Clone()).ToList();
        }
        #endregion

        #region GuestRequest
        void SetGuestRequest(GuestRequest guest)
        {
            //????????
            try
            {
                var my_request = from gu in DataSource.guestRequests
                                 where gu.GuestRequestKey == guest.GuestRequestKey
                                 select gu;
                if(my_request == null)
                    throw new NotExist("The request is not exist");
                else
                {
                    //לעדכן סטטוס
                }
            }
            catch (NotExist c)
            {
                throw c;
            }
        }

        public void addGuestRequest(GuestRequest guest/*string id, string name, int age*/)
        {
            //throw new NotImplementedException();//???
            try
            {
                if(DataSource.guestRequests.Any(g=>g.GuestRequestKey == guest.GuestRequestKey))
                {
                    DataSource.guestRequests.Add(guest);
                }
                else
                    throw new DuplicateObjectException("Request already exists");
            }
            catch (DuplicateObjectException c)
            {
                throw c;
            }
        }

        List<GuestRequest> GetGuestRequests()
        {
            //??????
        }
        #endregion

        #region Order
        public void addOrder(Order order)
        {
            try
            {
                if(DataSource.orders.Any(ord=>ord.orderKey == order.orderKey))
                     DataSource.orders.Add(order);
                else
                    throw new ExistException("This order already exists");
            }
            catch (ExistException c)
            {
                throw c;
            }
        }

        void setOrder()
        {
            //????????
            try
            {
                var my_request = from gu in DataSource.guestRequests
                                 where gu.GuestRequestKey == guest.GuestRequestKey
                                 select gu;
                if(my_request == null)
                    throw new NotExist("The order is not exist");
                else
                {
                    //לעדכן סטטוס
                }
            }
            catch (NotExist c)
            {
                throw c;
            }
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BanckBranch
        List<BankBranch> GetBankBranchesList()
        {
            //????
        }
        #endregion
    }
}
