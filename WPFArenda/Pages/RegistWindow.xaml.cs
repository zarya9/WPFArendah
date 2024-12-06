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
using WPFArenda.Classes;

namespace WPFArenda.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistWindow.xaml
    /// </summary>
    public partial class RegistWindow : Window
    {
        DBModel.User u = new  DBModel.User();
        private bool _isPasswordVisible = false;
        public RegistWindow()
        {
            InitializeComponent();
            int Role;
            LoadRole();
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            while (true) 
            {
                if (string.IsNullOrEmpty(FNameTextBox.Text))
                {
                    FName.Visibility = Visibility.Visible;
                    break;
                }
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {

                   Name.Visibility = Visibility.Visible;
                    break;
                }
                if (string.IsNullOrEmpty(PatronumicTextBox.Text))
                {
                    Patronumic.Visibility = Visibility.Visible;
                    break;
                }
                if (string.IsNullOrEmpty(PasswordBox.Password))
                {
                    Password.Visibility = Visibility.Visible;
                    break;
                }
                if (string.IsNullOrEmpty(ConfirmPasswordBox.Text))
                {
                    SecondPassword.Visibility = Visibility.Visible;
                    break;
                }
                if (string.IsNullOrEmpty(PhoneNumberTextBox.Text))
                {
                    Phone.Visibility = Visibility.Visible;
                    break;
                }
                if (PasswordBox.Password != ConfirmPasswordBox.Password)
                {
                    MessageBox.Show("Пароли не совпадают!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                if(RoleCombobox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите роль!", "Ошибка регистрации", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                u.Username = UsernameTextBox.Text;
                u.Phone = PhoneNumberTextBox.Text;
                u.Password = PasswordBox.Password;
                u.Name = NameTextBox.Text;
                u.FName = FNameTextBox.Text;
                u.Patronumic = PatronumicTextBox.Text;
                u.CreatedDate = DateTime.Now;
                u.ID_Role = (int)RoleCombobox.SelectedValue;
                ConnectionClass.connect.User.Add(u);
                ConnectionClass.connect.SaveChanges();
                MessageBox.Show("Добро пожаловать!", "Добавлен новый пользователь", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                break;
                
            }
        }

        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void RoleCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoleCombobox.SelectedValue != null)
            {
                int selectedRoleId = (int)RoleCombobox.SelectedValue;
            }
        }
        private void LoadRole()
        {
            var statuses = ConnectionClass.connect.Role.Where(r => r.ID_Role != 3).ToList();
            RoleCombobox.ItemsSource = statuses;
            RoleCombobox.DisplayMemberPath = "Name";
            RoleCombobox.SelectedValuePath = "ID_Role";
            RoleCombobox.SelectedIndex = 0;
        }
    }
}
