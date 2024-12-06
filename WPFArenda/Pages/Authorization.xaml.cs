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
using WPFArenda.Classes;
using WPFArenda.Pages;

namespace WPFArenda.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }
        public static class CurrentUser
        {
            public static int UserId { get; set; }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;
            

            var user = ConnectionClass.connect.User.FirstOrDefault(log => log.Username == login && log.Password == password);
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден!");
                MessageBoxResult result = MessageBox.Show("Желаете зарегистрироваться?", "Регистрация", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    RegistWindow rw = new RegistWindow();
                    rw.Show();
                    
                }
                return;
            }
            else
            {
                CurrentUser.UserId = user.ID_User;
                MessageBox.Show($"Вы авторизовались как {user.FName} {user.Name}!");

                NavigationService.Navigate( new HomePage(user));
            }
        }
    }
}
