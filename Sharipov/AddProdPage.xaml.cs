using Microsoft.Win32;
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
    /// <summary>
    /// Логика взаимодействия для AddProdPage.xaml
    /// </summary>
    public partial class AddProdPage : Page
    {
        private Product currentProd = new Product();
        private Product cProd = new Product();
        bool f = true;
        public AddProdPage(Product prod)
        {
            cProd = prod;
            InitializeComponent();
            if (prod != null)
            {
                currentProd = prod;
            }
            DataContext = currentProd;
        }

        private void AddProd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentProd.ProductArticleNumber))
                errors.AppendLine("Укажите артикаль");
            if (string.IsNullOrWhiteSpace(currentProd.ProductName))
                errors.AppendLine("Укажите название");
            if (string.IsNullOrWhiteSpace(currentProd.ProductUnit))
                errors.AppendLine("Укажите еденицу измерения");
            if (currentProd.ProductCost == 0)
                errors.AppendLine("Укажите стоимость");
            if (currentProd.ProductDiscountMax < 0)
                errors.AppendLine("Укажите положительную максимальную скидку");
            if (string.IsNullOrWhiteSpace(currentProd.ProductManufacturer))
                errors.AppendLine("Укажите производителя");
            if (string.IsNullOrWhiteSpace(currentProd.ProductProvider))
                errors.AppendLine("Укажите поставщика");
            if (string.IsNullOrWhiteSpace(currentProd.ProductCategory))
                errors.AppendLine("Укажите категорию");
            if (currentProd.ProductDiscountAmount < 0)
                errors.AppendLine("Укажите положительную скидку");
            if (currentProd.ProductQuantityInStock < 0)
                errors.AppendLine("Укажите положительное кол-во на складе");           
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (currentProd.ProductArticleNumber == "")
                SharipovEntities.GetContext().Products.Add(currentProd);
            try
            {
                SharipovEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            if (cProd == null)
            {
                var allProd = SharipovEntities.GetContext().Products.ToList();
                allProd = allProd.Where(p => p.ProductArticleNumber == currentProd.ProductArticleNumber).ToList();

                if (allProd.Count == 0)
                {
                    SharipovEntities.GetContext().Products.Add(currentProd);
                    try
                    {
                        SharipovEntities.GetContext().SaveChanges();
                        MessageBox.Show("информация сохранена");
                        MainClass.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Данный агент уже существует");
                }
            }
            else
            {
                SharipovEntities.GetContext().Products.Add(currentProd);
                try
                {
                    SharipovEntities.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    MainClass.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                currentProd.ProductPhoto = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }
       
    }
}
