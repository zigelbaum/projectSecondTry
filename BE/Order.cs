using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{

    [Serializable]
    public class Order
    {
        #region fileds
        Int32 _OrderKey ;
        Int32 _GuestRequestKey;
        Int32 _HostingUnitKey;
        private Enums.OrderStatus orderStatus;
        private DateTime createDate;
        private DateTime orderDate;
        #endregion


        #region properties  
        public Enums.OrderStatus OrderStatus { get => orderStatus; set => orderStatus = value; }
        public DateTime CreateDate { get => createDate; set => createDate = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
        public int OrderKey { get => _OrderKey; set => _OrderKey = value; }
        public int GuestRequestKey { get => _GuestRequestKey; set => _GuestRequestKey = value; }
        public int HostingUnitKey { get => _HostingUnitKey; set => _HostingUnitKey = value; }
        #endregion


        #region functions
        public override string ToString()
        {
            string order;
            order = "OrderKey: " + OrderKey + "@Guest Request Key: " + GuestRequestKey +
                "@Hosting Unit Key: " + HostingUnitKey + "@OrderStatus: " + OrderStatus +
                "@Created At: " + CreateDate + "@Order Date: " + OrderDate;
            order = order.Replace("@", System.Environment.NewLine);
            return order.ToString();
        }
        #endregion

        
    }
}
