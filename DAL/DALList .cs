using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    internal class DALList :IDAL
    {
        #region Singleton
        //private static readonly DALList instance = new DALList();
        //public static DALList Instance
        //{
        //    get { return instance; }
        //}

        //private DALList() { }
        //static DALList() { }
        private static DALList instance;

        public static DALList Instance
        {
            get
            {
                if (instance == null)
                    instance = new DALList();
                return instance;
            }
        }

        private DALList() { }

        #endregion

        #region HostingUnit
        public List<HostingUnit> getHostingUnitsList()
        {
            return DS.DataSource.hostingUnitsCollection.Select(item => (HostingUnit)item.Clone()).ToList();
        }

        public void addHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if (DataSource.hostingUnitsCollection.Any(h => h.HostingUnitKey == hostingUnit.HostingUnitKey))
                {
                    hostingUnit.HostingUnitKey = Configuration.HostingUnitKey;
                    Configuration.HostingUnitKey++;
                    DataSource.hostingUnitsCollection.Add(hostingUnit);
                }
                else
                    throw new ExistException("Hosting Unit already exists");
            }
            catch (ExistException c)
            {
                throw c;
            }
        }

        public void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if(DataSource.hostingUnitsCollection.Any(h=>h.HostingUnitKey == hostingUnit.HostingUnitKey))
                    throw new NotExistException("Hosting Unit not exists");
                else
                    DataSource.hostingUnitsCollection.Remove(hostingUnit);
            }
            catch (NotExistException c)
            {
                throw c;
            }
        }

        public void SetHostingUnit(HostingUnit hostingUnit)
        {
            //!!!!!!!!!!!!!!!!!
            try
            {
                HostingUnit hosting=null;
                IDAL dal = DAL.factoryDAL.getDAL("List");
                IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
                foreach (HostingUnit host in listHostingUnit)
                {
                    if(hostingUnit.hostingUnitKey == host.hostingUnitKey)
                        hosting=host;

                }
                if(hosting == null)
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

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate )
        {
            return DataSource.hostingUnitsCollection.Where(predicate).Select(hu => (HostingUnit)hu.Clone()).ToList();
        }
        #endregion

        #region GuestRequest
        public void SetGuestRequest(GuestRequest guest)
        {
            //????????
            try
            {
                GuestRequest  my_request = null;
                IDAL dal = DAL.factoryDAL.getDAL("List");
                IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequests();
                foreach(GuestRequest request in listGuestRequests)
                {
                    if(request._GuestRequestKey == guest._GuestRequestKey)
                        my_request = guest;
                }
                if(my_request == null)
                    throw new NotExist("The request is not exist");
                else
                {
                    //לעדכן סטטוס
                    if(my_request._Status == Active)
                        my_request._Status = Enums.GuestRequestStatus[1];
                    if(my_request._Status ==  ClosedOnTheWeb)
                        my_request._Status = Enums.GuestRequestStatus[2];
                }
            }
            catch (NotExist c)
            {
                throw c;
            }
        }

        public void addGuestRequest(GuestRequest guest)
        {
            //throw new NotImplementedException();//???
            try
            {
                if(DataSource.guestRequestsCollection.Any(g=>g.GuestRequestKey == guest.GuestRequestKey))
                {
                    guest.GuestRequestKey = Configuration.GuestRequestKey;
                    Configuration.GuestRequestKey++;
                    DataSource.guestRequestsCollection.Add(guest);
                }
                else
                    throw new DuplicateObjectException("Request already exists");
            }
            catch (DuplicateObjectException c)
            {
                throw c;
            }
        }

        public List<GuestRequest> GetGuestRequestsList()
        { 
                return DS.DataSource.guestRequestsCollection.Select(item => (GuestRequest)item.Clone()).ToList();
        }

        public List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate)
        {
            return DataSource.guestRequestsCollection.Where(predicate).Select(hu => (GuestRequest)hu.Clone()).ToList();
        }
        #endregion

        #region Order
        public void addOrder(Order order)
        {
            try
            {
                if (DataSource.ordersCollection.Any(ord => ord.OrderKey == order.OrderKey))
                {
                    order.OrderKey = Configuration.OrderKey;
                    Configuration.OrderKey++;
                    DataSource.ordersCollection.Add(order);
                }
                else
                    throw new ExistException("This order already exists");
            }
            catch (ExistException c)
            {
                throw c;
            }
        }

        public void setOrder(Order order)
        {
            //????????
            try
            {
                Order my_order = null;
                IDAL dal = DAL.factoryDAL.getDAL("List");
                IEnumerable<Order> listOrders = dal.GetOrdersList();
                foreach(Order ord in listOrders)
                {
                    if(ord.OrderKey == order.OrderKey)
                        my_order = ord;
                }
                if(my_order == null)
                    throw new NotExist("The order does not exist");
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
           return DataSource.ordersCollection.Where(predicate).Select(hu => (Order)hu.Clone()).ToList();
        }

        public List<Order> GetOrdersList()
        {
            return DS.DataSource.ordersCollection.Select(item => (Order)item.Clone()).ToList();
        }
        #endregion

        #region BanckBranch
        public List<BankBranch> GetBankBranchesList()
        {
            return DS.DataSource.BankBranchesCollection.Select(item => (BankBranch)item.Clone()).ToList();
        }
        #endregion
    }
}
