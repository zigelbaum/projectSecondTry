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
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        IBL myBl = BL.FactoryBL.getBL("XML");
        HostingUnit host; // לעשות שהוא יבין לבד לאיזו יחידה לבדוק התאמה
        public List<Order> listOrders;
        public OrderWindow()
        {          
            listOrders = myBl.getOrders(u => u.HostingUnitKey == host.HostingUnitKey);
            InitializeComponent();
        }
        private void UploadOrderButton_Click(object sender, RoutedEventArgs e)
        {
            UploadOrderWindow upload_ord_Window = new UploadOrderWindow();
            upload_ord_Window.ShowDialog();
        }
        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderWindow new_ord_Window = new CreateOrderWindow();
            new_ord_Window.ShowDialog();
        }
    }
}
