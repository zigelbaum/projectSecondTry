using System;
using BE;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel;

namespace DAL
{
    class DAL_xml : IDAL
    {
        #region Singleton
        private static readonly DAL_xml instance = new DAL_xml();
        XElement orderRoot;
        XElement bankBranchRoot;
        XElement configRoot;

        string hostingUnitPath = @"HostingUnitXml.xml";
        string guestRequestPath = @"GuestRequestXml.xml";
        string orderPath = @"OrderXml.xml";
        string bankBranchPath = @"https://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/snifim_dnld_he.xml";
        string configPath = @"ConfigXml.xml";

        public static DAL_xml Instance
        {
            get { return instance; }
        }
        #endregion

        private DAL_xml()
        {
            if (!File.Exists(hostingUnitPath))
            {
                FileStream fileHostingUnit = new FileStream(hostingUnitPath, FileMode.Create);
                fileHostingUnit.Close();
            }
            else
                DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));

            if (!File.Exists(guestRequestPath))
            {
                FileStream fileGuestRequest = new FileStream(guestRequestPath, FileMode.Create);
                fileGuestRequest.Close();
            }
            else
                DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));

            if (!File.Exists(orderPath))
            {
                orderRoot = new XElement("Orders");
                orderRoot.Save(orderPath);
            }
            else
                LoadData(ref orderRoot, orderPath);

            if (!File.Exists(bankBranchPath))
            {
                bankBranchRoot = new XElement("BankBranches");
                orderRoot.Save(bankBranchPath);
            }
            else
                LoadData(ref bankBranchRoot, bankBranchPath);

            if (!File.Exists(configPath))
            {
                configRoot = new XElement("Configure", new XElement("GuestRequestKey", "10000000"), new XElement("HostingUnitKey", "10000000"), new XElement("OrderKey", "10000000"));
                configRoot.Save(configPath);
            }
            else
                LoadData(ref configRoot, configPath);

            saveListToXML<HostingUnit>(DS.DataSource.hostingUnitsCollection, hostingUnitPath);
            saveListToXML<GuestRequest>(DS.DataSource.guestRequestsCollection, guestRequestPath);
        }

        static DAL_xml() { }

        private void LoadData(ref XElement t, string a)
        {
            try
            {
                t = XElement.Load(a);
            }
            catch
            {
                throw new DirectoryNotFoundException("Coudn't load" + a + "file");
            }
        }

        public static List<T> loadListFromXML<T>(string path)
        {
            if (File.Exists(path))
            {
                List<T> list;
                XmlSerializer x = new XmlSerializer(typeof(List<T>));
                FileStream file = new FileStream(path, FileMode.Open);
                list = (List<T>)x.Deserialize(file);
                file.Close();
                return list;
            }
            else return new List<T>();
        }

        public static void saveListToXML<T>(List<T> list, string Path)
        {
            FileStream file = new FileStream(Path, FileMode.Create);
            XmlSerializer x = new XmlSerializer(list.GetType());
            x.Serialize(file, list);
            file.Close();
        }

        XElement ConvertOrder(Order order)
        {
            XElement testElement = new XElement("test");

            foreach (PropertyInfo item in typeof(Order).GetProperties())
            {
                if (item.GetValue(order, null) != null)
                {

                    testElement.Add(new XElement(item.Name, item.GetValue(order, null).ToString()));

                }
                else
                {
                    testElement.Add(new XElement(item.Name, "null"));
                }
            }
            return testElement;
        }

        Order ConvertOrder(XElement element)
        {
            Order order = new Order();

            foreach (PropertyInfo item in typeof(Order).GetProperties())
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(item.PropertyType);
                object convertValue;
                convertValue = typeConverter.ConvertFromString(element.Element(item.Name).Value);
                if (item.CanWrite)
                    item.SetValue(order, convertValue);
            }
            return order;
        }

        BankBranch ConvertBranch(XElement element)
        {
            BankBranch branch = new BankBranch();

            foreach (PropertyInfo item in typeof(BankBranch).GetProperties())
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(item.PropertyType);
                object convertValue;
                convertValue = typeConverter.ConvertFromString(element.Element(item.Name).Value);
                if (item.CanWrite)
                    item.SetValue(branch, convertValue);
            }
            return branch;
        }

        #region Order
        public int addOrder(Order order)
        {
            try
            {
                IDAL dal = DAL.factoryDAL.getDAL("XML");
                LoadData(ref configRoot, configPath);

                int code = Convert.ToInt32(configRoot.Element("OrderKey").Value);

                order.OrderKey = code++;
                if (order.OrderKey > 99999999)
                    throw new Exception("DAL: you cannot add the current order, you passed the limit of 8 digits code ");
                configRoot.Element("OrderKey").Value = code.ToString();
                configRoot.Save(configPath);

                if(dal.OrderExist(order))
                    throw new Exception("DAL: you cannot add the current order,an order with the same key allready exists ");

                orderRoot.Add(ConvertOrder(order));
                orderRoot.Save(orderPath);
                return order.OrderKey;
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        public bool OrderExist(Order order)
        {
            IDAL dal = DAL.factoryDAL.getDAL("XML");
            IEnumerable<Order> listOrders = dal.GetOrdersList();            
            var temp = from ord in listOrders
                       where (ord.OrderKey == order.OrderKey)
                       select order;
            if (temp.Count() == 0)
                return false;
            return true;
        }

        public List<Order> GetOrdersList()
        {
            LoadData(ref orderRoot, orderPath);
            List<Order> orders = null;
            try
            {
                foreach (XElement item in orderRoot.Elements())
                    orders.Add(ConvertOrder(item));
            }
            catch
            {
                orders = null;
            }
            return orders;
        }

        public List<Order> getOrders(Func<Order, bool> predicate)
        {
            LoadData(ref orderRoot, orderPath);
            List<Order> orders = null;
            try
            {
                Order ord;
                foreach (XElement item in orderRoot.Elements())
                {
                    ord=ConvertOrder(item);
                    if (predicate(ord))
                        orders.Add(ord);

                }           
            }
            catch
            {
                orders = null;
            }
            return orders;
        }

        public Order FindOrder(Int32 ordKey)
        {
            LoadData(ref orderRoot, orderPath);
            XElement order = null;
            try
            {
                order = (from item in orderRoot.Elements()
                        where Convert.ToInt32(item.Element("OrderKey").Value) == ordKey
                        select item).FirstOrDefault();

            }
            catch (Exception)
            {
                return null;
            }

            if (order == null)
                return null;

            return ConvertOrder(order);
        }

        public void setOrder(Order order)
        {
            try
            {
                XElement toUpdate = (from item in orderRoot.Elements()
                                     where Convert.ToInt32(item.Element("OrderKey").Value) == order.OrderKey
                                     select item).FirstOrDefault();

                if (toUpdate == null)
                    throw new Exception("the requested order was not found...");
                XElement updatedTest = ConvertOrder(order);

                toUpdate.ReplaceNodes(updatedTest.Elements());

                orderRoot.Save(orderPath);
            }
            catch (DataException c)
            {
                throw c;
            }
        }
        #endregion

        #region HostingUnit
        public int addHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                if (hostingUnit == null)
                    throw new Exception("DAL: there is no unit to add");
                DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));
                LoadData(ref configRoot, configPath);
                int code = Convert.ToInt32(configRoot.Element("HostingUnitKey").Value);
                hostingUnit.HostingUnitKey = code++;
                HostingUnit unit = DS.DataSource.hostingUnitsCollection.FirstOrDefault(t => t.HostingUnitKey == hostingUnit.HostingUnitKey);
                if (unit != null)
                    throw new Exception("DAL: hosting unit with the same key already exists...");
                configRoot.Element("HostingUnitKey").Value = code.ToString();
                configRoot.Save(configPath);
                DS.DataSource.hostingUnitsCollection.Add(hostingUnit);
                saveListToXML(DS.DataSource.hostingUnitsCollection, hostingUnitPath);
                return hostingUnit.HostingUnitKey;
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        public void DeleteHostingUnit(HostingUnit hostingUnit)
        {
            try
            {
                DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));
                int index = DS.DataSource.hostingUnitsCollection.FindIndex(t => t.HostingUnitKey == hostingUnit.HostingUnitKey);
                if (index == -1)
                    throw new Exception("DAL: the requested hosting unit was not found in the system...");
                HostingUnit unit = DS.DataSource.hostingUnitsCollection.FirstOrDefault(t => t.HostingUnitKey == hostingUnit.HostingUnitKey);
                DS.DataSource.hostingUnitsCollection.Remove(unit);
                saveListToXML(DS.DataSource.hostingUnitsCollection, hostingUnitPath);
            }
            catch (Exception a)
            {
                throw a;
            }            
        }

        public List<HostingUnit> getHostingUnitsList()
        {
            DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));
            List<HostingUnit> units = null;
            foreach (HostingUnit item in DS.DataSource.hostingUnitsCollection)
                units.Add(item);
            return units;
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate)
        {
            DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));
            List<HostingUnit> units = null;
            foreach (HostingUnit item in DS.DataSource.hostingUnitsCollection)
                if (predicate(item))
                units.Add(item);
            return units;
        }

        public void SetHostingUnit(HostingUnit hostingUnit)
        {
            DS.DataSource.hostingUnitsCollection= (loadListFromXML<HostingUnit>(hostingUnitPath));
            int index = DS.DataSource.hostingUnitsCollection.FindIndex(t => t.HostingUnitKey == hostingUnit.HostingUnitKey);
            if (index == -1)
                throw new Exception("DAL: the requested hosting unit was not found in the system...");

            DS.DataSource.hostingUnitsCollection.RemoveAt(index);
            DS.DataSource.hostingUnitsCollection.Add(hostingUnit);
            saveListToXML(DS.DataSource.hostingUnitsCollection, hostingUnitPath);
        }

        public bool UnitExist(HostingUnit unit)
        {
            DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));
            int index = DS.DataSource.hostingUnitsCollection.FindIndex(t => t.HostingUnitKey == unit.HostingUnitKey);
            if (index == -1)
                return false;
            return true;
        }
        #endregion

        #region GuestRequest
        public int addGuestRequest(GuestRequest guest)
        {
            try
            {
                if (guest == null)
                    throw new Exception("No request to add");
                DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));
                LoadData(ref configRoot, configPath);
                int code = Convert.ToInt32(configRoot.Element("GuestRequestKey").Value);
                guest.GuestRequestKey = code++;
                if (guest.GuestRequestKey > 99999999)
                    throw new Exception("DAL: you cannot add the current request, you passed the limit of 8 digits code ");
                configRoot.Element("GuestRequestKey").Value = code.ToString();
                configRoot.Save(configPath);
                guest.Status = Enums.GuestRequestStatus.Active;
                guest.RegistrationDate = DateTime.Now;
                DS.DataSource.guestRequestsCollection.Add(guest);
                saveListToXML(DS.DataSource.guestRequestsCollection, guestRequestPath);
                return guest.GuestRequestKey;
            }
            catch (Exception c)
            {
                throw c;
            }
        }

        public List<GuestRequest> GetGuestRequestsList()
        {
            DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));
            List<GuestRequest> requests = null;
            foreach (GuestRequest item in DS.DataSource.guestRequestsCollection)
                requests.Add(item);
            return requests;
        }

        public List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate)
        {
            DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));
            List<GuestRequest> requests = null;
            foreach (GuestRequest item in DS.DataSource.guestRequestsCollection)
                if (predicate(item))
                    requests.Add(item);
            return requests;
        }

        public void SetGuestRequest(GuestRequest guest)
        {
            try
            {               
                DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));
                int index = DS.DataSource.guestRequestsCollection.FindIndex(t => t.GuestRequestKey == guest.GuestRequestKey);
                if (index == -1)
                    throw new Exception("DAL: the requested guest request was not found in the system...");

                DS.DataSource.guestRequestsCollection.RemoveAt(index);
                DS.DataSource.guestRequestsCollection.Add(guest);
                saveListToXML(DS.DataSource.guestRequestsCollection, guestRequestPath);
            }
            catch (DataException c)
            {
                throw c;
            }

        }

        public bool RequestExist(GuestRequest request)
        {

            DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));
            int index = DS.DataSource.guestRequestsCollection.FindIndex(t => t.GuestRequestKey == request.GuestRequestKey);
            if (index == -1)
                return false;
            return true;
        }
        #endregion

        #region BankBranches
        public List<BankBranch> GetBankBranchesList()
        {
            LoadData(ref bankBranchRoot, bankBranchPath);
            List<BankBranch> branches = null;
            try
            {
                foreach (XElement item in bankBranchRoot.Elements())
                    branches.Add(ConvertBranch(item));
            }
            catch
            {
                branches = null;
            }
            return branches;
        }
        #endregion
    }
}


