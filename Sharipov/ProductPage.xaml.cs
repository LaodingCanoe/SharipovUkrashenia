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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sharipov
{
    public partial class ProductPage : Page
    {
        List<Product> add_product = new List<Product>();
        string FIO;

        public int vsecurrentProductCount = SharipovEntities.GetContext().Products.Count();
        public ProductPage(int userID)
        {
            InitializeComponent();
            var currentUser = SharipovEntities.GetContext().Users.ToList();
            if (userID == 1)
            {
                AddProdBT.Visibility = Visibility.Visible;

            }
            if (userID >= 0)
            {
                var currentRole = SharipovEntities.GetContext().Roles.ToList();
                userRoleTB.Text = currentRole[UserID.ID - 1].RoleName.ToString();
                FIO = "Вы авторизированы как: " + currentUser[userID].UserSurname.ToString() + " " + currentUser[userID].UserName.ToString() + " " + currentUser[userID].UserPatronymic.ToString();
                userTB.Text = FIO;
            }
            else
            {
                FIO = "";
                //userTB.Visibility = Visibility.Collapsed;
                userRoleTB.Text = "Гостевой режим";
            }
            
            ProdList.ItemsSource = SharipovEntities.GetContext().Products.ToList();
            SortCB.SelectedIndex = 0;
            FiterCB.SelectedIndex = 0;
            Update();
        }
        public void Update()
        {
            var currentProduct = SharipovEntities.GetContext().Products.ToList();
            if (FiterCB.SelectedIndex == 1)
            {
                currentProduct = currentProduct.Where(p => p.ProductDiscountAmount < 10).ToList();
            }
            if (FiterCB.SelectedIndex == 2)
            {
                currentProduct = currentProduct.Where(p => p.ProductDiscountAmount > 10 && p.ProductDiscountAmount < 15).ToList();
            }
            if (FiterCB.SelectedIndex == 3)
            {
                currentProduct = currentProduct.Where(p => p.ProductDiscountAmount > 15).ToList();
            }
            if (SortCB.SelectedIndex == 1)
            {
                currentProduct = currentProduct.OrderBy(p => p.ProductCost).ToList();
            }
            if (SortCB.SelectedIndex == 2)
            {
                currentProduct = currentProduct.OrderByDescending(p => p.ProductCost).ToList();
            }
            currentProduct = currentProduct.Where(p => p.ProductName.ToLower().Contains(SearchTB.Text.ToLower())).ToList();
            ProdList.ItemsSource = currentProduct;
            int currentProductCount = currentProduct.Count();
            CountTB.Text = (currentProductCount).ToString() + " из " + (vsecurrentProductCount).ToString();
        }
        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void FiterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void EditWP_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ProdList.SelectedIndex >= 0)
            {
                var prod = ProdList.SelectedItem as Product;
                add_product.Add(ProdList.SelectedItem as Product);
                //var newOrderProduct = new OrderProducts();
                //newOrderProduct.ProductArticleNumber = prod.ProductArticleNumber;

            }
            if (add_product.Count > 0)
            {
                OrderBT.Visibility = Visibility.Visible;
            }
            Update();
        }

        private void OrderBT_Click(object sender, RoutedEventArgs e)
        {
            //OrderWindow OrderWin = new OrderWindow(add_product, FIO);
            //OrderWin.Show();        
        }

        private void EditProdBT_Click(object sender, RoutedEventArgs e)
        {
            MainClass.MainFrame.Navigate(new AddProdPage((sender as Button).DataContext as Product));
            Update();
        }

        private void DeleteProdBT_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button).DataContext as Product;

            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    SharipovEntities.GetContext().Products.Remove(currentProduct);
                    SharipovEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            Update();
        }

        private void AddProdBT_Click(object sender, RoutedEventArgs e)
        {
            MainClass.MainFrame.Navigate(new AddProdPage(null));
            Update();
        }

        private void ProdList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Update();
        }

        private void ProdList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }
    }
}
