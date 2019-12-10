using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class MyBL : IBL
    {
        #region Singleton
        private static readonly MyBL instance = new MyBL();

        public static MyBL Instance
        {
            get { return instance; }
        }
        #endregion

        static IDAL myDAL;

        static MyBL()
        {
            string TypeDAL = ConfigurationSettings.AppSettings.Get("TypeDS");
            //string TypeDAL = "List";
            myDAL = factoryDAL.getDAL(TypeDAL);
        }

        public void addHostingUnit(HostingUnit hostingUnit)
        {
            myDAL.addHostingUnit(hostingUnit);
        }
    }
}
