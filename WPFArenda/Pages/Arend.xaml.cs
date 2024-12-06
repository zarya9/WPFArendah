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
    /// Логика взаимодействия для Arend.xaml
    /// </summary>
    public partial class Arend : Page
    {
        DBModel.Object obj;
        DBModel.User u;
        DBModel.Zayavka z = new Zayavka();
        DBModel.Contract contract = new Contract();
        DBModel.Status_Zayavka sz;
        double totalPrice;
        public Arend(DBModel.Object _obj, DBModel.User _u)
        {
            InitializeComponent();
            obj = _obj;
            u = _u;
            this.DataContext = obj;
            StartDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddDays(-1)));
            EndDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddDays(-1)));

        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage(u));
        }

        private void BtnQR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnObjects_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы хотите выйти из аккаунта?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new Authorization());
            }
        }

        private void TxtMessageObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            string message = TxtMessageObject.Text;
            TxtCountSymbol.Text = $"Количество символов: {message.Length}";
        }

        private void BtnArend_Click(object sender, RoutedEventArgs e)
        {
            var status = ConnectionClass.connect.Status_Zayavka
                .FirstOrDefault(s => s.Name == "Ожидает");  

            if (status != null)
            {
                // Присваиваем ID найденного статуса
                z.Status_Zayavka = status;
            }
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите даты начала и окончания аренды.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var startDate = StartDatePicker.SelectedDate.Value;
            var endDate = EndDatePicker.SelectedDate.Value;
            z.ID_Object = obj.ID_Object;
            z.ID_User = u.ID_User;
            z.DateEnd = endDate;
            z.DateStart = startDate;
            z.CreatedDate = DateTime.Now;
            z.Message = TxtMessageObject.Text;
            ConnectionClass.connect.Zayavka.Add(z);
            contract.DateEnd = endDate;
            contract.DateStart = startDate;
            contract.ID_Zayavka = z.ID_Zayavka;
            contract.TotalPrice = Convert.ToInt32(totalPrice);
            contract.PaymentStatus = 2;
            ConnectionClass.connect.Contract.Add(contract);
            ConnectionClass.connect.SaveChanges();
            MessageBox.Show("Заявка создана успешно, ожидайте ответ от владельца","Заявка создана", MessageBoxButton.OK, MessageBoxImage.None);
            NavigationService.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                var startDate = StartDatePicker.SelectedDate.Value;
                var endDate = EndDatePicker.SelectedDate.Value;
                    
                            

                if (startDate > endDate)
                {
                    MessageBox.Show("Дата окончания не может быть раньше даты начала!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    TxtPriceObject.Text = "0 ₽";
                    return;
                }

                
                int days = (endDate - startDate).Days + 1;

                
                if (double.TryParse(TxtPriceObject.Text, out double dailyPrice))
                {
                    totalPrice = days * dailyPrice;
                    TxtPriceObject.Text = $"{totalPrice:C}"; // Форматирование в рубли
                }
                else
                {
                    TxtPriceObject.Text = "0 ₽";
                }
            }
        }
    }
}
