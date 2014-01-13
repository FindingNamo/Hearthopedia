using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Hearthopedia.Filters;

namespace Hearthopedia
{
    public partial class FilterList : PhoneApplicationPage
    {
        public FilterList()
        {
            InitializeComponent();
            ActiveFilters.ItemsSource = FilterManager.Instance.GetActiveFilters();
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewFilter.xaml", UriKind.Relative));
        }
    }
}