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
using WPFArenda.DBModel;

namespace WPFArenda.Pages
{
    /// <summary>
    /// Логика взаимодействия для YourObjects.xaml
    /// </summary>
    public partial class YourObjects : Page
    {
        DBModel.User us;
        DBModel.Object obj;
        public YourObjects(DBModel.User _us, DBModel.Object obj)
        {
            InitializeComponent();
            us = _us;
            this.DataContext = us;
            this.obj = obj;
            //исправить на текущего пользователя
            RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(z => z.ID_Owner == us.ID_User).ToList();
            TxtFname.Text = us.FName;
            TxtName.Text = us.Name;
            TxtPatronumic.Text = us.Patronumic;
            TxtEmail.Text = us.Email;
            TxtPhone.Text = us.Phone;
            if (us.ID_Role == 2)
            {
                BtnAddObjects.Visibility = Visibility.Collapsed;
                BtnEditObject.Visibility = Visibility.Collapsed;
                BtnDeleteObject.Visibility = Visibility.Collapsed;
            }
            Refresh();
            
        }

        public void Refresh()
        {
            
            if (RentalObjectsListView.ItemsSource == null)
            {
                TxtObjectNone.Visibility = Visibility.Visible;
                RentalObjectsListView.Visibility = Visibility.Hidden;
            }
            else
            {
                RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(z => z.ID_Owner == us.ID_User).ToList();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnAddObjects_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(new AddObject(us));
        }

        private void BtnEditObject_Click(object sender, RoutedEventArgs e)
        {
            var i = RentalObjectsListView.SelectedItem as DBModel.Object;
            if (i != null)
            {
                NavigationService.Navigate(new EditObject(i));
            }
            else
            {
                MessageBox.Show("Нужно выбрать объект", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteObject_Click(object sender, RoutedEventArgs e)
        {
            var i = RentalObjectsListView.SelectedItem as DBModel.Object;
            ConnectionClass.connect.Object.Remove(i);
            ConnectionClass.connect.SaveChanges();
            MessageBox.Show("Объект удален", "Удаление", MessageBoxButton.OK, MessageBoxImage.None);
            Refresh();
        }

        private void RentalObjectsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RentalObjectsListView.SelectedItem is DBModel.Object selectedObject)
            {


                NavigationService.Navigate(new PageReviews(obj, us));
            }
        }
    }
}
