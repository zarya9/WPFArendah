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
using WPFArenda.DBModel;
using WPFArenda.Classes;
using Microsoft.Win32;
using System.IO;
using System.Xml.Linq;

namespace WPFArenda.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddObject.xaml
    /// </summary>
    public partial class AddObject : Page
    {
        DBModel.Object obj = new DBModel.Object();
        DBModel.User us;

        public AddObject(DBModel.User _us)
        {
            InitializeComponent();
            us = _us;
            this.DataContext = us;
            LoadCategories();

        }

        private void TxtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void BtnAddObject_Click(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                if (string.IsNullOrEmpty(TxtObjectName.Text))
                {
                    MessageBox.Show("Введите название объекта!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                if (string.IsNullOrEmpty(TxtDescription.Text))
                {
                    MessageBox.Show("Введите описание!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                if (string.IsNullOrEmpty(TxtLocation.Text))
                {
                    MessageBox.Show("Введите место получения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                if (string.IsNullOrEmpty(TxtPrice.Text))
                {
                    MessageBox.Show("Введите цену!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
                

                obj.Title = TxtObjectName.Text;
                obj.Description = TxtDescription.Text;
                obj.Location = TxtLocation.Text;
                obj.Price = Convert.ToInt32(TxtPrice.Text);
                obj.ID_Owner = us.ID_User;
                obj.Dostupnost = true;
                obj.CreatedDate = DateTime.Now;
                obj.ID_Category = CategoryComboBox.SelectedIndex;
                // Проверка изображения
                if (obj.Image == null || obj.Image.Length == 0)
                {
                    MessageBox.Show("Изображение не выбрано!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Добавление объекта в базу данных
                try
                {
                    ConnectionClass.connect.Object.Add(obj);
                    ConnectionClass.connect.SaveChanges();
                    MessageBox.Show("Объект успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении объекта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
                break;
            }
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                obj.Image = File.ReadAllBytes(openFileDialog.FileName);
                MemoryStream byteStream = new MemoryStream(obj.Image);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = byteStream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                IPicture.Source = image;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = CategoryComboBox.SelectedItem as Category;
        }

        private void LoadCategories()
        {
            var categories = ConnectionClass.connect.Category.ToList();
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "ID_Category"; 
            CategoryComboBox.SelectedIndex = 0;
        }
    }
}
