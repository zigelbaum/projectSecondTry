using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class FactoryBL
    {
        public static IBL getDAL(string typeBL)
        {
            switch (typeBL)
            {
                case "List": return MyBL.Instance;
                //  case "XML": return DAL_XML.Instance;
                default: return null;
            }
        }
    }
}
