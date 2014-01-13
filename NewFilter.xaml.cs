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
    public partial class NewFilter : PhoneApplicationPage
    {
        public ICardFilter CurrentFilter;

        public NewFilter()
        {
            InitializeComponent();

            // Setup the property listbox.
            FilterTypes.DataContext = EnumUtilities.GetEnumNames<FilterType>();
        }

        private void SetCurrentFilter(FilterType filter)
        {
            if (CurrentFilter != null)
                CurrentFilter.Dispose();

            CurrentFilter = FilterFactory.GetFilter(filter);
            this.DataContext = CurrentFilter;
            ValuePlaceholder.Children.Clear();
            ValuePlaceholder.Children.Add(CurrentFilter.GetFilterValueXamlElement());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            FilterManager.Instance.AddFilter(CurrentFilter);
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void FilterTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListPicker listPicker = (ListPicker)sender;
            SetCurrentFilter(EnumUtilities.GetEnumValueFromEnumName<FilterType>((string)listPicker.SelectedItem));
        }

        private void ListPicker_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ListPicker listPicker = (ListPicker) sender;
            CurrentFilter.SetOperationIndex(listPicker.SelectedIndex);
        }
    }
}