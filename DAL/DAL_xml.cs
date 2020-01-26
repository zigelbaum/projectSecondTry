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
        //XElement hostingUnitRoot;
        //XElement guestRequestRoot;
        XElement orderRoot;
        XElement bankBranchRoot;
        XElement configRoot;

        string hostingUnitPath = @"HostingUnitXml.xml";
        string guestRequestPath = @"GuestRequestXml.xml";
        string orderPath = @"OrderXml.xml";
        string bankBranchPath = @"https://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/snifim_dnld_he.xml";
        string configPath = @"ConfigXml.xml";
        string filesWithWrongStruct = "";

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
                configRoot = new XElement("Configure",new XElement("GuestRequestKey", "10000000"),new XElement("HostingUnitKey", "10000000"), new XElement("OrderKey", "10000000"));
                configRoot.Save(configPath);
            }
            else
                LoadData(ref configRoot, configPath);

            saveListToXML<HostingUnit>(DS.DataSource.hostingUnitsCollection, hostingUnitPath);
            saveListToXML<GuestRequest>(DS.DataSource.guestRequestsCollection,guestRequestPath);
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

        XElement ConvertTest(Order order)
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

        Order ConvertTest(XElement element)
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
    }
}

        
