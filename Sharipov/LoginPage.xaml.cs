using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public static class UserID
    {
        public static int ID = 0;
    }
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        async void LoginBtn_Sleep()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                LoginBtn.IsEnabled = false;
            }));
            Thread.Sleep(10000);
            this.Dispatcher.Invoke((Action)(() =>
            {
                LoginBtn.IsEnabled = true;
            }));
        }
        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = SharipovEntities.GetContext().Users.ToList();
            currentUser = currentUser.Where(p => p.UserLogin == LoginTB.Text && p.UserPassword == PasvordTB.Text).ToList();
            int userID = 0;
            if (currentUser.Count() == 0)
            {
                MessageBox.Show("Введён не правильный логин или пароль");
                await Task.Run(() => LoginBtn_Sleep());

            }
            else
            {
                foreach (User user in currentUser)
                {
                    userID = user.UserID;
                }
                if (currentUser.Count == 0)
                {

                }
                else if (currentUser.Count == 1)
                {
                    currentUser = SharipovEntities.GetContext().Users.ToList();
                    UserID.ID = currentUser[userID - 1].UserRole;
                    MainClass.MainFrame.Navigate(new ProductPage(userID-1));
                }
            }

        }
        private void VizitBtn_Click(object sender, RoutedEventArgs e)
        {
            UserID.ID = -1;
            LoginTB.Text = "";
            PasvordTB.Text = "";
            MainClass.MainFrame.Navigate(new ProductPage(-1));
        }
    }
}
