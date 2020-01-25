using System;
using BE;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Threading;

namespace DAL
{
    class DAL_xml:IDAL
    {
        #region Singleton
        private static readonly DAL_xml instance = new DAL_xml();

        XElement hostingUnitRoot;
        XElement guestRequestRoot;
        XElement orderRoot;
        string hostingUnitPath = @"HostingUnitXml.xml";
        string guestRequestPath = @"GuestRequestXml.xml";
        string orderPath = @"OrderXml.xml";
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
                hostingUnitRoot = new XElement("HostingUnits");
                hostingUnitRoot.Save(hostingUnitPath);
            }
            else
                LoadData(ref hostingUnitRoot, hostingUnitPath);
            if(!File.Exists(guestRequestPath))
            {
                guestRequestRoot = new XElement("GuestRequests");
                guestRequestRoot.Save(guestRequestPath);
            }
            else
                LoadData(ref guestRequestRoot, guestRequestPath);
            if (!File.Exists(orderPath))
            {
                orderRoot = new XElement("Orders");
                orderRoot.Save(orderPath);
            }
            else
                LoadData(ref orderRoot, orderPath);
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

        public void AddStudent(Student student)
        {
            XElement id = new XElement("id", student.Id);
            XElement firstName = new XElement("firstName", student.FirstName);
            XElement lastName = new XElement("lastName", student.LastName);
            XElement name = new XElement("name", firstName, lastName);
            studentRoot.Add(new XElement("student", id, name));
            studentRoot.Save(studentPath);
        }
    }
}
