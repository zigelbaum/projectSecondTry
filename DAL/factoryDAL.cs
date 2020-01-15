using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class factoryDAL
    {
        public static IDAL getDAL(string typeDAL)
        {
            switch (typeDAL)
            {
                case "List": return DALList.Instance;
                case "XML": return DALList.Instance;
                default: return null;
            }
        }

    }
}
