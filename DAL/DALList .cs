using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using static DS.DataSource;
namespace DAL
{
    internal class DALList :IDAL
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
        public bool UnitExist(HostingUnit unit)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            foreach (HostingUnit host in listHostingUnits)
            {
                if(unit.HostingUnitKey == host.HostingUnitKey)
                    return true;
            }
            return false;
        }

        public List<HostingUnit> getHostingUnitsList()
        {
            return DS.DataSource.hostingUnitsCollection.Select(item => (HostingUnit)item.Clone()).ToList();
        }

        public void addHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if (hostingUnit == null)
                    throw new DataException("No hosting unit");

                IDAL dal = DAL.factoryDAL.getDAL("List");
                IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
                foreach (HostingUnit host in listHostingUnits)
                {
                    if (hostingUnit.CompareTo(host) == 0)
                        throw  new DataException("This host already exists"); ;
                }
                hostingUnit.HostingUnitKey = Configuration.HostingUnitKey;
                Configuration.HostingUnitKey++;
                hostingUnitsCollection.Add(hostingUnit);  
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if(!UnitExist(hostingUnit))
                    throw new DataException("The hosting unit not exist");
                else
                    hostingUnitsCollection.Remove(hostingUnit);
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public void SetHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if(!UnitExist(hostingUnit))
                    throw new DataException("The hosting unit is not exist");
                else
                {
                    HostingUnit unit = hostingUnit.Clone();
                    hostingUnitsCollection.Remove(unit);
                }
                hostingUnitsCollection.Add(hostingUnit);
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate )
        {
            return hostingUnitsCollection.Where(predicate).Select(hu => (HostingUnit)hu.Clone()).ToList();
        }
        #endregion

        #region GuestRequest
        public bool RequestExist(GuestRequest request)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            List<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            foreach(GuestRequest guest in listGuestRequests)
            {
                if(request.GuestRequestKey == guest.GuestRequestKey)
                    return true;
            }
            return false;
        }

        public void SetGuestRequest(GuestRequest guest)
        {
            try
            {
                if(!RequestExist(guest))
                    throw new DataException("The request is not exist");
                else
                {
                    GuestRequest request = guest.Clone();
                    guestRequestsCollection.Remove(request);
                }
                guestRequestsCollection.Add(guest);
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public void addGuestRequest(GuestRequest guest)
        {
            try
            {
                if (guest == null)
                    throw new DataException("No request");
                guest.GuestRequestKey = Configuration.GuestRequestKey;
                Configuration.GuestRequestKey++;
                guest.Status = Enums.GuestRequestStatus.Active;
                guest.RegistrationDate = DateTime.Now;
                guestRequestsCollection.Add(guest);
            }
            catch (DataException c)
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
            return guestRequestsCollection.Where(predicate).Select(hu => (GuestRequest)hu.Clone()).ToList();
        }
        #endregion

        #region Order
        public bool OrderExist(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            foreach(Order ord in listOrders)
            {
                if(ord.OrderKey == order.OrderKey)
                    return true;
            }
            return false;
        }

        public int addOrder(Order order)
        {
            try
            {
                if (!OrderExist(order))
                {
                    order.OrderKey = Configuration.OrderKey;
                    Configuration.OrderKey++;
                    ordersCollection.Add(order);
                    return order.OrderKey;
                }
                else
                    throw new DataException("This order already exist");
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public void setOrder(Order order)
        {
            try
            {
                if(!OrderExist(order))
                    throw new DataException("The order is not exist");
                else
                {
                    Order ord = order.Clone();
                    ordersCollection.Remove(ord);
                }
                ordersCollection.Add(order);
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
           return ordersCollection.Where(predicate).Select(hu => (Order)hu.Clone()).ToList();
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
