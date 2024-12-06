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
    /// Логика взаимодействия для Review.xaml
    /// </summary>
    public partial class Review : Page
    {
        DBModel.User u;
        int SelectedRating;
        DBModel.Object obj;
        DBModel.Reviews rev = new DBModel.Reviews();
        DBModel.Zayavka z;
        public Review(DBModel.Zayavka _z, DBModel.User _u)
        {
            InitializeComponent();
            z = _z;
            u = _u;
            obj = ConnectionClass.connect.Object.FirstOrDefault(o => o.ID_Object == z.ID_Object);
            TxtObjectTitle.Text = obj.Title;
        }

        private void BtnQR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы хотите выйти из аккаунта?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new Authorization());
            }
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage(u));
        }

        private void Star_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton && int.TryParse(clickedButton.Tag.ToString(), out int rating))
            {
                for (int i = 1; i <= 5; i++)
                {
                    var starButton = (Button)FindName($"Star{i}");
                    if (starButton != null)
                    {
                        starButton.Foreground = i <= rating ? Brushes.Gold : Brushes.Gray;
                    }
                }

                SelectedRating = rating;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnSubmitReview_Click(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                if (SelectedRating == 0)
                {
                    MessageBox.Show("Оставьте оценку!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                rev.ID_Object = obj.ID_Object;
                rev.CreatedDate = DateTime.Now;
                rev.ID_User = u.ID_User;
                rev.Comment = TxtReview.Text;
                rev.Rating = SelectedRating;
                ConnectionClass.connect.Reviews.Add(rev);
                ConnectionClass.connect.SaveChanges();
                MessageBox.Show("Отзыв успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
                break;
            }
            
        }

        private void TxtReview_TextChanged(object sender, TextChangedEventArgs e)
        {
            string message = TxtReview.Text;
            TxtCountSymbol.Text = $"Количество символов: {message.Length}";
        }
    }
}
