using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALList :IDAL
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

    }
}
