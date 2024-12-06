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
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        DBModel.User u;
        
        Zayavka z;
        DBModel.Object obj;
        
        public HomePage(DBModel.User _u)
        {
            InitializeComponent();
            u = _u;
            
            this.DataContext = u;
            if (u.ID_Role == 3)
            {
                RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.ToList();
                TxtRecomend.Text = "Все объекты аренды";
                BtnArend.Visibility = Visibility.Collapsed;
                BtnAccount.Visibility = Visibility.Collapsed;
                BtnOtchet2.Visibility = Visibility.Collapsed;

            }
            if (u.ID_Role == 2)
            {
                BtnOtchet.Visibility = Visibility.Collapsed;
                BtnOtchet2.Visibility = Visibility.Collapsed;
                BtnOtchet3.Visibility = Visibility.Collapsed;
                BtnAccount.Visibility = Visibility.Collapsed;
            }
            if(u.ID_Role == 1)
            {
                BtnOtchet3.Visibility = Visibility.Collapsed;
            }
            RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(obj => obj.ID_Owner != u.ID_User).ToList();
            Refresh();
        }


        public void Refresh()
        {
            RentalObjectsListView.ItemsSource = null;
                RentalObjectsListView.ItemsSource = RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(obj => obj.ID_Owner != u.ID_User).ToList();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            foreach (var child in ((Grid)Content).Children)
            {
                if (child is Expander expander && expander != sender)
                {
                    expander.IsExpanded = false;
                }
            }
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new YourObjects(u,obj));
        }

        private void BtnObjects_Click(object sender, RoutedEventArgs e)
        {

            
        }

        private void BtnOtchet_Click(object sender, RoutedEventArgs e)
        {
            OtchetZayazkiStatus otchet = new OtchetZayazkiStatus();
            otchet.ShowDialog();
        }

        private void PriceFilterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int maxPrice = Convert.ToInt32(PriceFilterSlider.Value);
            PriceFilterValue.Text = $"Цена: до {maxPrice:C0}";
            RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(obj => obj.Price <= maxPrice && obj.ID_Owner != u.ID_User) .ToList();
        }

        private void SortVozrUp_Click(object sender, RoutedEventArgs e)
        {
            RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(obj => obj.ID_Owner != u.ID_User).OrderBy(z => z.Price).ToList();


        }

        private void SortVozrDown_Click(object sender, RoutedEventArgs e)
        {
            RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(obj => obj.ID_Owner != u.ID_User).OrderByDescending(z => z.Price).ToList();
        }

        private void BtnQR_Click(object sender, RoutedEventArgs e)
        {
            QR qr = new QR();
            qr.ShowDialog();
        }

        private void BtnArend_Click(object sender, RoutedEventArgs e)
        {
            var i = RentalObjectsListView.SelectedItem as DBModel.Object;
            if (i != null)
            {
                NavigationService.Navigate(new Arend(i,u));
            }
            else
            {
                MessageBox.Show("Нужно выбрать объект", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnZayavka_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Zayavki(z, u, obj));
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Хотите выйти из своего аккаунта?","Выход",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new Authorization());
            }
        }

        private void BtnReviewObject_Click(object sender, RoutedEventArgs e)
        {
            var i = RentalObjectsListView.SelectedItem as DBModel.Object;
            if (i != null)
            {
                NavigationService.Navigate(new PageReviews(i, u));
            }
            else
            {
                MessageBox.Show("Нужно выбрать объект", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtNameObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            RentalObjectsListView.ItemsSource = ConnectionClass.connect.Object.Where(z => z.Title.ToLower().Contains(TxtNameObject.Text)).ToList();
        }

        private void BtnOtchet2_Click(object sender, RoutedEventArgs e)
        {
            
            OtchetPizdec op = new OtchetPizdec(u);
            op.ShowDialog();
        }

        private void BtnOtchet3_Click(object sender, RoutedEventArgs e)
        {
            Otchet3 op = new Otchet3(u);
            op.ShowDialog();
        }
    }
}
