using iText.Kernel.Pdf;
using System;
using System.IO;
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
using iText.Layout.Font;
using static WPFArenda.Pages.Authorization;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using Paragraph = iText.Layout.Element.Paragraph;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace WPFArenda.Pages
{
    /// <summary>
    /// Логика взаимодействия для Zayavki.xaml
    /// </summary>
    public partial class Zayavki : Page
    {
       
        DBModel.User u;
        DBModel.Zayavka z;
        DBModel.Object o;
        public Zayavki(DBModel.Zayavka _z, DBModel.User _u, DBModel.Object _o)
        {
            InitializeComponent();
            z = _z;
            this.u = _u;
            this.o = _o;
            LoadStatusFilter();
            if (u.ID_Role == 1)
            {
                ZayavkiListView.ItemsSource = ConnectionClass.connect.Zayavka.Where(z => z.Object.ID_Owner == u.ID_User).ToList();
            }
            if (u.ID_Role == 2)
            {
                ZayavkiListView.ItemsSource = ConnectionClass.connect.Zayavka.Where(z => z.ID_User == u.ID_User).ToList();
            }
            if (u.ID_Role == 3)
            {
                ZayavkiListView.ItemsSource = ConnectionClass.connect.Zayavka.ToList();
                BtnDogovor.Visibility = Visibility.Collapsed;
                StatusFilterCombo.Visibility = Visibility.Collapsed;
                TxtFilter.Visibility = Visibility.Collapsed;
            }
            Refresh();
        }

        public void Refresh()
        {

            if (u.ID_Role == 1)
            {
                ZayavkiListView.ItemsSource = null;
                ZayavkiListView.ItemsSource = ConnectionClass.connect.Zayavka.Where(z => z.Object.ID_Owner == u.ID_User).ToList();

                BtnYes.Visibility = Visibility.Visible;
                BtnNo.Visibility = Visibility.Visible;
                StatusFilterCombo.Visibility = Visibility.Visible;
                TxtFilter.Visibility = Visibility.Visible;
                BtnReview.Visibility = Visibility.Collapsed;
            }
            if (u.ID_Role == 2)
            {
                ZayavkiListView.ItemsSource = null;
                ZayavkiListView.ItemsSource = ConnectionClass.connect.Zayavka.Where(z => z.ID_User == u.ID_User).ToList();
                BtnYes.Visibility = Visibility.Collapsed;
                BtnNo.Visibility = Visibility.Collapsed;
            }
            if (u.ID_Role == 3)
            {
                ZayavkiListView.ItemsSource = null;
                ZayavkiListView.ItemsSource = ConnectionClass.connect.Zayavka.ToList();
                BtnYes.Visibility = Visibility.Collapsed;
                BtnNo.Visibility = Visibility.Collapsed;
                BtnReview.Visibility = Visibility.Collapsed;
                BtnDogovor.Visibility = Visibility.Collapsed;
                StatusFilterCombo.Visibility = Visibility.Collapsed;
                TxtFilter.Visibility = Visibility.Collapsed;
            }
        }

        private void StatusFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedStatusId = (int)StatusFilterCombo.SelectedValue;
            IQueryable<DBModel.Zayavka> filteredZayavkiQuery = null;

            if (u.ID_Role == 1) 
            {
                filteredZayavkiQuery = ConnectionClass.connect.Zayavka
                    .Where(z => z.Object.ID_Owner == u.ID_User && z.ID_Status == selectedStatusId);
            }
            else if (u.ID_Role == 2) 
            {
                filteredZayavkiQuery = ConnectionClass.connect.Zayavka
                    .Where(z => z.ID_User == u.ID_User && z.ID_Status == selectedStatusId);
            }
            else if (u.ID_Role == 3)
            {
                filteredZayavkiQuery = ConnectionClass.connect.Zayavka
                    .Where(z => z.ID_Status == selectedStatusId);
            }
            
            ZayavkiListView.ItemsSource = filteredZayavkiQuery
                
                .ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void LoadZayavki(string statusFilter = null)
        {
            IQueryable<DBModel.Zayavka> zayavkiQuery = null;
            if (u.ID_Role == 1) 
            {
                zayavkiQuery = ConnectionClass.connect.Zayavka.Where(z => z.Object.ID_Owner == u.ID_User); 
            }
            else if (u.ID_Role == 2)
            {
                zayavkiQuery = ConnectionClass.connect.Zayavka.Where(z => z.ID_User == u.ID_User);
            }
            else if (u.ID_Role == 3)
            {
                zayavkiQuery = ConnectionClass.connect.Zayavka;
            }
            if (!string.IsNullOrEmpty(statusFilter))
            {
                zayavkiQuery = zayavkiQuery.Where(z => z.Status_Zayavka.Name == statusFilter);
            }

            ZayavkiListView.ItemsSource = zayavkiQuery
                .Select(z => new
                {
                    Title = z.Object.Title ?? "Объект не найден",
                    Message = z.Message ?? "Сообщение отсутствует",
                    Status = z.Status_Zayavka.Name ?? "Статус не указан"
                })
                .ToList();
        }

        private void LoadStatusFilter()
        {
            var statuses = ConnectionClass.connect.Status_Zayavka.ToList();
            StatusFilterCombo.ItemsSource = statuses;
            StatusFilterCombo.DisplayMemberPath = "Name";
            StatusFilterCombo.SelectedValuePath = "ID_Status";
            StatusFilterCombo.SelectedIndex = 0;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            var selectedZayavka = ZayavkiListView.SelectedItem as DBModel.Zayavka;
            if (selectedZayavka.ID_Status == 3)
            {
                MessageBox.Show("Уже отклонено. Дальнейшее принятие заявки нереально", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
            if (selectedZayavka != null)
            {
                var a = ConnectionClass.connect.Zayavka.FirstOrDefault(o => o.ID_Zayavka == selectedZayavka.ID_Zayavka);

                if (a != null)
                {
                    a.ID_Status = 2;
                    ConnectionClass.connect.SaveChanges();
                    Refresh();
                }
            }
        
            else
            {
                MessageBox.Show("Че ты там одобрить решил", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            var selectedZayavka = ZayavkiListView.SelectedItem as DBModel.Zayavka;
            if (selectedZayavka != null)
            {
                var a = ConnectionClass.connect.Zayavka.FirstOrDefault(o => o.ID_Zayavka == selectedZayavka.ID_Zayavka);

                if (a != null)
                {
                    a.ID_Status = 3;
                    var contract = ConnectionClass.connect.Contract.FirstOrDefault(c => c.ID_Zayavka == selectedZayavka.ID_Zayavka);
                    if (contract == null)
                    {
                        MessageBox.Show("Уже поздно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        ConnectionClass.connect.Contract.Remove(contract);
                        ConnectionClass.connect.SaveChanges();
                        Refresh();
                    }
                    
                }
            }

            else
            {
                MessageBox.Show("Че ты там отклонить решил", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
            
        private void BtnReview_Click(object sender, RoutedEventArgs e)
        {
            var selectedZayavka = ZayavkiListView.SelectedItem as DBModel.Zayavka;

            if (selectedZayavka == null)
            {
                MessageBox.Show("Пожалуйста, выберите заявку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (selectedZayavka.ID_Status != 2)
            {
                MessageBox.Show("Статус заявки не соответствует требуемому. Только заявки с статусом 'Принято' могут быть обработаны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                NavigationService.Navigate(new Review(selectedZayavka, u));
            }

        }

        private void BtnDogovor_Click(object sender, RoutedEventArgs e)
        {
            var selectedZayavka = ZayavkiListView.SelectedItem as DBModel.Zayavka;
            if (selectedZayavka == null)
            {
                MessageBox.Show("Пожалуйста, выберите заявку для генерации договора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedZayavka.ID_Status == 2)
            {
                GenerateContract(selectedZayavka);
            }
            else
            {
                MessageBox.Show("Вы не можете составить договор с непринятой заявкой на объект аренды. Ожидайте ответ собственника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
        }

        public void GenerateContract(DBModel.Zayavka zayavka)
        {
            var user = ConnectionClass.connect.User.FirstOrDefault(u => u.ID_User == zayavka.ID_User);
            var obj = ConnectionClass.connect.Object.FirstOrDefault(o => o.ID_Object == zayavka.ID_Object);
            var owner = ConnectionClass.connect.User.FirstOrDefault(u => u.ID_User == obj.ID_Owner);

            if (user == null || obj == null || owner == null)
            {
                MessageBox.Show("Не удалось найти данные для договора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string userShortFIO = $"{user.FName} {user.Name[0]}. {user.Patronumic?[0] ?? '-'}.";
            string ownerShortFIO = $"{owner.FName} {owner.Name[0]}. {owner.Patronumic?[0] ?? '-'}.";

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = System.IO.Path.Combine(desktopPath, $"Договор_№{zayavka.ID_Zayavka}.txt");

            string contractText = $@"
ДОГОВОР АРЕНДЫ ОБЪЕКТА № {zayavka.ID_Zayavka}

г. Казань, {obj.Location ?? "Не указан"}                                       {DateTime.Now:dd.MM.yyyy}

Гражданин {userShortFIO} (далее - ""Арендатор""), с одной стороны,
и {ownerShortFIO} (далее - ""Арендодатель""), с другой стороны,
заключили настоящий договор о нижеследующем:

1. Предмет договора
    1.1. Арендодатель предоставляет, а Арендатор принимает в аренду объект ""{obj.Title}"" по адресу: {obj.Location ?? "Адрес не указан"}.

2. Срок аренды
    2.1. Настоящий договор действует с {zayavka.DateStart:dd.MM.yyyy} по {zayavka.DateEnd:dd.MM.yyyy}.

3. Порядок расчетов
    3.1. Стоимость аренды составляет {obj.Price} рублей в день.

Подписи сторон:

Арендатор: _______________________ {userShortFIO}
Арендодатель: ____________________ {ownerShortFIO}";

            File.WriteAllText(filePath, contractText);

            MessageBox.Show($"Договор успешно создан и сохранён в файл:\n{filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
