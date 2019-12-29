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
        bool UnitExist(HostingUnit unit)
        {
            IDAL dal = DAL.factoryDal.getDal("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getAllHostingUnits();
            foreach (HostingUnit host in listHostingUnit)
            {
                if(unit.HostingUnitKey == host.HostingUnitKey)
                    return true;
            }
            return false;
        }

        List<HostingUnit> getHostingUnitsList()
        {

        }

        public void addHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if(!UnitExist(hostingUnit))
                    DataSource.hostingUnitsCollection.Add(hostingUnit);
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
                if(!UnitExist(hostingUnit))
                    throw new NotExistException("Hosting Unit not exists");
                else
                    DataSource.hostingUnitsCollection.Remove(hostingUnit);
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
                if(!UnitExist(hostingUnit))
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
        bool RequestExist(GuestRequest request)
        {
            IDAL dal = DAL.factoryDal.getDal("List");
            IEnumerable<GuestRequest> listGuestRequests = dal.GetGuestRequests();
            foreach(GuestRequest guest in listGuestRequests)
            {
                if(request._GuestRequestKey == guest._GuestRequestKey)
                    return true;
            }
            return false;
        }

        void SetGuestRequest(GuestRequest guest)
        {
            //????????
            try
            {
                if(!RequestExist(guest))
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

        public void addGuestRequest(GuestRequest guest/*string id, string name, int age*/)
        {
            //throw new NotImplementedException();//???
            try
            {
                if(!RequestExist(guest))
                {
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

        List<GuestRequest> GetGuestRequests()
        {
            //??????
        }
        #endregion

        #region Order
        bool OrderExist(Order order)
        {
            IDAL dal = DAL.factoryDal.getDal("List");
            IEnumerable<Order> listOrders = dal.getOrders();
            foreach(Order ord in listOrders)
            {
                if(ord._orderKey == order._orderKey)
                    return true;
            }
            return false;
        }

        public void addOrder(Order order)
        {
            try
            {
                if(!OrderExist(order))
                     DataSource.orders.Add(order);
                else
                    throw new ExistException("This order already exists");
            }
            catch (ExistException c)
            {
                throw c;
            }
        }

        void setOrder(Order order)
        {
            //????????
            try
            {
                
                if(!OrderExist(order))
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
