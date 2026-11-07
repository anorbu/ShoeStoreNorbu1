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

namespace ShoeStoreНорбу
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private ShoeStoreНорбуEntities _context = new ShoeStoreНорбуEntities();
        private int _roleId;
        public OrdersWindow(int roleId)
        {
            InitializeComponent();

            _roleId = roleId;


            if (_roleId == 1)
            {
                BtnAddOrder.Visibility = Visibility.Visible;
                BtnDeleteOrder.Visibility = Visibility.Visible;
            }

            LoadOrders();
        }
        private void LoadOrders()
        {
            var orders = _context.Orders
           .Include("PickupPoints")
           .Include("Products")
           .ToList();

            ListOrders.ItemsSource = orders;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            AddEditOrderWindow w = new AddEditOrderWindow(null);
            w.ShowDialog();
            LoadOrders();
        }

        private void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_roleId != 1) return;
            var sel = ListOrders.SelectedItem as Orders;
            if (sel == null) { MessageBox.Show("Выберите заказ!"); return; }
            if (MessageBox.Show($"Удалить заказ №{sel.OrderID}?", "Подтверждение",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            _context.Orders.Remove(sel);
            _context.SaveChanges();
            LoadOrders();
        }
        protected override void OnClosed(EventArgs e)
        {
            _context.Dispose();
            base.OnClosed(e);
        }

        private void ListOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_roleId != 1) return;

            var selected = ListOrders.SelectedItem as Orders;
            if (selected == null) return;

            AddEditOrderWindow w = new AddEditOrderWindow(selected);
            w.ShowDialog();
            LoadOrders();
        }
    }
}
