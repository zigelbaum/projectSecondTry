﻿using BE;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for UploadOrderWindow.xaml
    /// </summary>
    public partial class UploadOrderWindow : Window
    {
        #region variable
        IBL myBl = BL.FactoryBL.getBL("XML");
        HostingUnit host;
        List<Order> listOrders;
        Order myorder;
        #endregion

        public UploadOrderWindow()
        {
            InitializeComponent();
            ConButton.Visibility = Visibility.Visible;
            OrderstList.Visibility = Visibility.Hidden;
            StatusOrder.Visibility = Visibility.Hidden;
            StatusOrderString.Visibility = Visibility.Hidden;
            CreateDate.Visibility = Visibility.Hidden;
            CreateDateString.Visibility = Visibility.Hidden;
            OrderDate.Visibility = Visibility.Hidden;
            OrderDateString.Visibility = Visibility.Hidden;
        }

        private void ContinueButton_Click(object sender, SelectionChangedEventArgs e)
        {
            string unitName = hostingUnitName.Text;
            List<HostingUnit> hostList = myBl.getHostingUnits(h => h.HostingUnitName == unitName);
            ConButton.Visibility = Visibility.Hidden;
            host = hostList[0];
            //מראה את ההזמנות המתאימות
            OrderstList.Visibility = Visibility.Visible;
        }

        private void cbOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //למלאות את הפרטים כאשר לוחצים על הזמנה מסוימת
            StatusOrder.Visibility = Visibility;
            StatusOrderString.Visibility = Visibility;

            CreateDate.Visibility = Visibility;
            CreateDateString.Visibility = Visibility;

            Int32 index = OrderstList.SelectedIndex;
            myorder = listOrders[index];

            if (myorder.OrderDate != null)
            {
                OrderDate.Visibility = Visibility;
                OrderDateString.Visibility = Visibility;
            }
        }

        private void cbOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //לעדכן סטטוס כשבוחרים סטטוס חדש
            Int32 index = StatusOrder.SelectedIndex;
            Enums.OrderStatus myStatus = (Enums.OrderStatus)index;
            Int32 orderKey = myorder.OrderKey;
            Order ord = myBl.getOrders(o => o.OrderKey == orderKey)[0];
            ord.OrderStatus = myStatus;
            myBl.setOrder(ord);
            //יראה את ההזמנה המעודדכנת
            myorder = myBl.FindOrder(myorder.OrderKey);
        }
    }
}