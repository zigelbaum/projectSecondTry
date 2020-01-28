using System;
using BE;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Reflection;
using System.ComponentModel;
using System.Net;

namespace DAL
{
    class DAL_xml : IDAL
    {
        #region Singleton
        private static readonly DAL_xml instance = new DAL_xml();
        XElement orderRoot;
        //XElement bankBranchRoot;
        XElement configRoot;

        string hostingUnitPath = @"HostingUnitXml.xml";
        string guestRequestPath = @"GuestRequestXml.xml";
        string orderPath = @"OrderXml.xml";
        string configPath = @"ConfigXml.xml";
       
        public static volatile bool bankDownloaded = false;
        BackgroundWorker worker;

        public static DAL_xml Instance
        {
            get { return instance; }
        }
        #endregion

        private DAL_xml()
        {
            try//bank download
            {
                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerAsync();

            }
            catch (Exception e)
            {
                throw e;
            }

            if (!File.Exists(configPath))
            {
                configRoot = new XElement("Configure", new XElement("GuestRequestKey", "10000000"), new XElement("HostingUnitKey", "10000000"), new XElement("OrderKey", "10000000"),new XElement("Fee", "10"));
                configRoot.Save(configPath);
            }
            else
                LoadData(ref configRoot, configPath);

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

            saveListToXML<HostingUnit>(DS.DataSource.hostingUnitsCollection, hostingUnitPath);
            saveListToXML<GuestRequest>(DS.DataSource.guestRequestsCollection, guestRequestPath);
            saveListToXML <Order>(DS.DataSource.ordersCollection, orderPath);
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
            XElement orderElement = new XElement("order");

            foreach (PropertyInfo item in typeof(Order).GetProperties())
            {
                if (item.GetValue(order, null) != null)
                {

                    orderElement.Add(new XElement(item.Name, item.GetValue(order, null).ToString()));

                }
                else
                {
                    orderElement.Add(new XElement(item.Name, "null"));
                }
            }
            return orderElement;
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
                    throw new DataException("DAL: you cannot add the current order, you passed the limit of 8 digits code ");
                configRoot.Element("OrderKey").Value = code.ToString();
                configRoot.Save(configPath);

                if (dal.OrderExist(order))
                    throw new DataException("DAL: you cannot add the current order,an order with the same key allready exists ");

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
                var v = from item in orderRoot.Elements()
                        let c = ConvertOrder(item)
                        select c;
                orders = v.Select(ord => ord.Clone()).ToList();
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
                var v= from item in orderRoot.Elements()
                           let c = ConvertOrder(item)
                           where predicate(c)
                           select c;
                orders = v.Select(ord => ord.Clone()).ToList();
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
                    throw new DataException("the requested order was not found...");
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
                    throw new DataException("DAL: there is no unit to add");
                DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));
                LoadData(ref configRoot, configPath);
                int code = Convert.ToInt32(configRoot.Element("HostingUnitKey").Value);
                hostingUnit.HostingUnitKey = code++;
                HostingUnit unit = DS.DataSource.hostingUnitsCollection.FirstOrDefault(t => t.HostingUnitKey == hostingUnit.HostingUnitKey);
                if (unit != null)
                    throw new DataException("DAL: hosting unit with the same key already exists...");
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
                    throw new DataException("DAL: the requested hosting unit was not found in the system...");
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
            return DS.DataSource.hostingUnitsCollection;
        }

        public List<HostingUnit> getHostingUnits(Func<HostingUnit, bool> predicate)
        {
            DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));                
            DS.DataSource.hostingUnitsCollection.RemoveAll(hu=>!predicate(hu));
            return DS.DataSource.hostingUnitsCollection;
        }

        public void SetHostingUnit(HostingUnit hostingUnit)
        {
            DS.DataSource.hostingUnitsCollection = (loadListFromXML<HostingUnit>(hostingUnitPath));
            int index = DS.DataSource.hostingUnitsCollection.FindIndex(t => t.HostingUnitKey == hostingUnit.HostingUnitKey);
            if (index == -1)
                throw new DataException("DAL: the requested hosting unit was not found in the system...");

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
                    throw new DataException("No request to add");
                DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));
                LoadData(ref configRoot, configPath);
                int code = Convert.ToInt32(configRoot.Element("GuestRequestKey").Value);
                guest.GuestRequestKey = code++;
                if (guest.GuestRequestKey > 99999999)
                    throw new DataException("DAL: you cannot add the current request, you passed the limit of 8 digits code ");
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
            return DS.DataSource.guestRequestsCollection;
        }

        public List<GuestRequest> getGuestRequests(Func<GuestRequest, bool> predicate)
        {
            DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));                       
            DS.DataSource.guestRequestsCollection.RemoveAll(gr=>!predicate(gr));
            return DS.DataSource.guestRequestsCollection;
        }

        public void SetGuestRequest(GuestRequest guest)
        {
            try
            {
                DS.DataSource.guestRequestsCollection = (loadListFromXML<GuestRequest>(guestRequestPath));
                int index = DS.DataSource.guestRequestsCollection.FindIndex(t => t.GuestRequestKey == guest.GuestRequestKey);
                if (index == -1)
                    throw new DataException("DAL: the requested guest request was not found in the system...");

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

        private List<BankBranch> banks = null;

        public List<BankBranch> GetBankBranchesList()
        {
            if (bankDownloaded)
            {
                if (banks == null)
                {
                    banks = new List<BankBranch>();
                    XmlDocument doc = new XmlDocument();
                    doc.Load(@"atm.xml");
                    XmlNode rootNode = doc.DocumentElement;
                    XmlNodeList children = rootNode.ChildNodes;
                    foreach (XmlNode child in children)
                    {
                        BankBranch b = GetBranchByXmlNode(child);
                        if (b != null)
                        {
                            banks.Add(b);
                        }
                    }
                }
                return banks;
            }
            else
                throw new DataException("bank didn't download");
        }

        private static BankBranch GetBranchByXmlNode(XmlNode node)
        {
            if (node.Name != "BRANCH") return null;
            BankBranch branch = new BankBranch();
            branch.BankNumber = -1;

            XmlNodeList children = node.ChildNodes;

            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "Bank_Code":
                        branch.BankNumber = int.Parse(child.InnerText);
                        break;
                    case "Bank_Name":
                        branch.BankName = child.InnerText;
                        break;
                    case "Branch_Code":
                        branch.BranchNumber = int.Parse(child.InnerText);
                        break;
                    case "Branch_Address":
                        branch.BranchCity = child.InnerText;
                        break;
                    case "City":
                        branch.BranchAddress = child.InnerText;                       
                        break;

                }

            }

            if (branch.BranchNumber > 0)
                return branch;

            return null;

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            object ob = e.Argument;
            while (bankDownloaded == false)
            {
                try
                {
                    DownloadBank();
                    Thread.Sleep(2000);
                }
                catch
                { }
            }
            GetBankBranchesList();
        }

        private void DownloadBank()
        {
            #region downloadBank
            string xmlLocalPath = @"atm.xml";
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath =
               @"https://www.boi.org.il/en/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/snifim_en.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
                bankDownloaded = true;
            }
            catch
            {

                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
                bankDownloaded = true;

            }
            finally
            {
                wc.Dispose();
            }
            #endregion
        }

        #endregion
    }
}

