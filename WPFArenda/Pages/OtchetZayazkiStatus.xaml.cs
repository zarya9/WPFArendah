using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFArenda.Classes;

namespace WPFArenda.Pages
{
    /// <summary>
    /// Логика взаимодействия для OtchetZayazkiStatus.xaml
    /// </summary>
    public partial class OtchetZayazkiStatus : Window
    {
        public ChartValues<int> ObjectCounts { get; set; }
        public List<string> ObjectNames { get; set; }
        public OtchetZayazkiStatus()
        {

            InitializeComponent();
            var popularObjects = ConnectionClass.connect.Zayavka
                .GroupBy(z => z.ID_Object)
                .Select(group => new
                {
                    ObjectName = ConnectionClass.connect.Object.FirstOrDefault(o => o.ID_Object == group.Key).Title,
                    Count = group.Count()
                })
                .OrderByDescending(o => o.Count)
                .Take(10) 
                .ToList();

            ObjectCounts = new ChartValues<int>(popularObjects.Select(o => o.Count));
            ObjectNames = popularObjects.Select(o => o.ObjectName).ToList();

            DataContext = this;
        }
    }
}
