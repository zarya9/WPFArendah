using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для EditObject.xaml
    /// </summary>
    public partial class EditObject : Page
    {
        DBModel.Object ob;
        public EditObject(DBModel.Object _ob)
        {
            InitializeComponent();
            ob = _ob;
            this.DataContext = ob;
            LoadCategories();
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                ob.Image = File.ReadAllBytes(openFileDialog.FileName);
                MemoryStream byteStream = new MemoryStream(ob.Image);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = byteStream;
                image.EndInit();
                IPicture.Source = image;
            }
        }

        private void TxtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var O = ConnectionClass.connect.Object.Where(z => z.ID_Object == ob.ID_Object).FirstOrDefault();
            O.Title = TxtObjectName.Text;
            O.Price = Convert.ToInt32(TxtPrice.Text);
            O.Description = TxtDescription.Text;
            O.ID_Category = CategoryComboBox.SelectedIndex;
            ConnectionClass.connect.Object.Add(O);
            MessageBox.Show("Изменения сохранены", "Изменение записи", MessageBoxButton.OK, MessageBoxImage.Information);
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
