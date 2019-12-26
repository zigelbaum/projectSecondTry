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
        public void addHostingUnit(HostingUnit hostingUnit)
        {
            DataSource.hostingUnits.Add(hostingUnit);
        }

        void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            DataSource.hostingUnits.Remove(hostingUnit);
        }

        void SetHostingUnit(HostingUnit hostingUnit)
        {
            //!!!!!!!!!!!!!!!!!
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
        }

        public void addGuestRequest(string id, string name, int age)
        {
            throw new NotImplementedException();//???
        }

        List<GuestRequest> GetGuestRequests()
        {
            //??????
        }
        #endregion

        #region Order
        public void addOrder(Order order)
        {
            DataSource.orders.Add(order);
        }

        void setOrder()
        {
            //????????
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BanckBranch
        List<BankBranch> GetBankBranches()
        {
            //????
        }
        #endregion
    }
}
