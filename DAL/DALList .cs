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
        public bool UnitExist(HostingUnit unit)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<HostingUnit> listHostingUnits = dal.getHostingUnitsList();
            var temp = from HostingUnit host in listHostingUnits
                       let hostingKey = host.HostingUnitKey
                       where hostingKey == unit.HostingUnitKey                       
                       select host;
            if (temp.Count() == 0)
                return false;
            return true;
        }

        public List<HostingUnit> getHostingUnitsList()
        {
            return DS.DataSource.hostingUnitsCollection.Select(item => (HostingUnit)item.Clone()).ToList();
        }

        public int addHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if (hostingUnit == null)
                    throw new DataException("No hosting unit");

                IDAL dal = DAL.factoryDAL.getDAL("List");
                hostingUnit.HostingUnitKey = Configuration.HostingUnitKey;
                Configuration.HostingUnitKey++;
                DS.DataSource.hostingUnitsCollection.Add(hostingUnit.Clone());
                return hostingUnit.HostingUnitKey;
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
                if (!UnitExist(hostingUnit))
                    throw new DataException("The hosting unit not exist");
                else
                    DS.DataSource.hostingUnitsCollection.RemoveAll(x => x.HostingUnitKey == hostingUnit.HostingUnitKey);
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
                if (!UnitExist(hostingUnit))
                    throw new DataException("The hosting unit is not exist");
                else
                {
                    DS.DataSource.hostingUnitsCollection.RemoveAll(u => u.HostingUnitKey == hostingUnit.HostingUnitKey);
                }
                DS.DataSource.hostingUnitsCollection.Add(hostingUnit.Clone());
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate)
        {
            return DS.DataSource.hostingUnitsCollection.Where(predicate).Select(hu => (HostingUnit)hu.Clone()).ToList();
        }
        #endregion

        #region GuestRequest
        public bool RequestExist(GuestRequest request)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            List<GuestRequest> listGuestRequests = dal.GetGuestRequestsList();
            var temp = from GuestRequest guest in listGuestRequests
                       where request.GuestRequestKey == guest.GuestRequestKey
                       select new { request, t = true };
            if (temp.Count() == 0)
                return false;
            return true;
        }

        public void SetGuestRequest(GuestRequest guest)
        {
            try
            {
                if (!RequestExist(guest))
                    throw new DataException("The request is not exist");
                else
                {
                    DS.DataSource.guestRequestsCollection.RemoveAll(g => g.GuestRequestKey == guest.GuestRequestKey);
                }
                DS.DataSource.guestRequestsCollection.Add(guest.Clone());
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public int addGuestRequest(GuestRequest guest)
        {
            try
            {
                if (guest == null)
                    throw new DataException("No request");
                guest.GuestRequestKey = Configuration.GuestRequestKey;
                Configuration.GuestRequestKey++;
                guest.Status = Enums.GuestRequestStatus.Active;
                guest.RegistrationDate = DateTime.Now;
                DS.DataSource.guestRequestsCollection.Add(guest.Clone());
                return guest.GuestRequestKey;
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
            return DS.DataSource.guestRequestsCollection.Where(predicate).Select(hu => (GuestRequest)hu.Clone()).ToList();
        }
        #endregion

        #region Order
        public bool OrderExist(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("List");
            IEnumerable<Order> listOrders = dal.GetOrdersList();
            var temp = from Order ord in listOrders
                       where ord.OrderKey == order.OrderKey
                       select order;
            if (temp.Count() == 0)
                return false;
            return true;
        }

        public Order FindOrder(Int32 ordKey)
        {
            List<Order> orders = GetOrdersList();          
            var temp = from Order ord in orders
                       where ord.OrderKey == ordKey
                       select ord;
            if (temp.Count() == 0)
                return null;
            return temp.First();
        }

        public int addOrder(Order order)
        {
            try
            {
                if (!OrderExist(order))
                {
                    order.OrderKey = Configuration.OrderKey;
                    Configuration.OrderKey++;
                    DS.DataSource.ordersCollection.Add(order.Clone());
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
                if (!OrderExist(order))
                    throw new DataException("The order is not exist");
                else
                {
                    DS.DataSource.ordersCollection.RemoveAll(o => o.OrderKey == order.OrderKey);
                }
                DS.DataSource.ordersCollection.Add(order.Clone());
            }
            catch (DataException c)
            {
                throw c;
            }
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            return DS.DataSource.ordersCollection.Where(predicate).Select(hu => (Order)hu.Clone()).ToList();
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
