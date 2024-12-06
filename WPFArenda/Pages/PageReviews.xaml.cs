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
    /// Логика взаимодействия для PageReviews.xaml
    /// </summary>
    public partial class PageReviews : Page
    {
        DBModel.Object obj;
        DBModel.User user;
        public PageReviews(DBModel.Object _selectedObject, User _user)
        {
            InitializeComponent();
            if (_selectedObject == null)
            {
                MessageBox.Show("Данные не переданы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService.GoBack();
                return;
            }
            obj = _selectedObject;
            
            this.DataContext = obj;
            ReviewsListView.ItemsSource = ConnectionClass.connect.Reviews.Where(r => r.ID_Object == obj.ID_Object).ToList();
            this.user = _user;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
