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
        readonly Int32 _OrderKey = Configuration.OrderKey;
        readonly Int32 _GuestRequestKey = Configuration.GuestRequestKey;
        readonly Int32 _HostingUnitKey = Configuration.HostingUnitKey;
        private Enums.OrderStatus orderStatus;
        private DateTime createDate;
        private DateTime orderDate;
        #endregion


        #region properties
        public int OrderKey => _OrderKey;
        public int GuestRequestKey => _GuestRequestKey;
        public int HostingUnitKey => _HostingUnitKey;
        public Enums.OrderStatus OrderStatus { get => orderStatus; set => orderStatus = value; }
        public DateTime CreateDate { get => createDate; set => createDate = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
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
