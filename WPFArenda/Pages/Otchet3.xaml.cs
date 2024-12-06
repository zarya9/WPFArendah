﻿using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static WPFArenda.Pages.Authorization;
using WPFArenda.Classes;
using WPFArenda.DBModel;

namespace WPFArenda.Pages
{
    /// <summary>
    /// Логика взаимодействия для Otchet3.xaml
    /// </summary>
    public partial class Otchet3 : Window
    {
        DBModel.User user;
        public SeriesCollection DealsCounts { get; set; }
        public SeriesCollection AveragePrices { get; set; }
        public List<string> Days { get; set; }

        public Otchet3(DBModel.User _user)
        {
            InitializeComponent();
            user = _user;
            this.DataContext = user;
            var data = ConnectionClass.connect.Zayavka
                .Where(z => /*z.Object.ID_Owner == user.ID_User &&*/ z.CreatedDate.HasValue)
                .AsEnumerable()
                .GroupBy(z => z.CreatedDate.Value.ToString("yyyy-MM-dd")) 
                .Select(g => new
                {
                    Day = g.Key,
                    DealsCount = g.Count(),
                    AveragePrice = g.Average(z => z.Object.Price)
                })
                .OrderBy(d => d.Day)
                .ToList();


            Days = data.Select(d => d.Day).ToList();
            DealsCounts = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Количество сделок",
                    Values = new ChartValues<int>(data.Select(d => d.DealsCount))
                }
            };
            AveragePrices = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Средняя цена аренды",
                    Values = new ChartValues<double>(data.Select(d => d.AveragePrice ?? 0)) 
                }
            };

            DataContext = this;
        }
    }
}
