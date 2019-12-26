using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        #region fileds
        readonly Int32 _orderKey = Configuration.orderKey;
        readonly Int32 _GuestRequestKey = Configuration.GuestRequestKey;
        readonly Int32 _HostingUnitKey = Configuration.HostingUnitKey;
        private Enums.OrderStatus orderStatus;
        private DateTime createDate;
        private DateTime orderDate;
        #endregion

        #region functions
        public override string ToString()
        {
            return base.ToString();
        }
        #endregion

        /*public Order(Enums.OrderStatus orderStatus, DateTime createDate)
        {
            this.orderStatus = orderStatus;
            this.createDate = createDate;
        }*/
    }
}
