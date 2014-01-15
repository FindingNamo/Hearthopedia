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
    public partial class FilterPage : PhoneApplicationPage
    {
        private string _searchString ="";

        public FilterPage()
        {
            InitializeComponent();
            ClassPanorama.DataContext = CardClassFilter.Instance;
            QualityPanorama.DataContext = CardQualityFilter.Instance;
            RacePanorama.DataContext = CardRaceFilter.Instance;
            SetPanorama.DataContext = CardSetFilter.Instance;
            TypePanorama.DataContext = CardTypeFilter.Instance;
            NumberPanorama.DataContext = NumberFilter.Instance;
            MechanicPanorama.DataContext = CardMechanicFilter.Instance;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationContext.QueryString.TryGetValue("search", out _searchString);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            DataAccess.SearchCards(_searchString);
        }

        private void UncheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            PanoramaItem currentItem = (PanoramaItem)(EntirePanorama.SelectedItem);   
            ICardFilter filter = (ICardFilter)(currentItem).DataContext;
            filter.SetUncheckedAll();
        }

        private void CheckAllButton_Click_1(object sender, RoutedEventArgs e)
        {
            PanoramaItem currentItem = (PanoramaItem)(EntirePanorama.SelectedItem);
            ICardFilter filter = (ICardFilter)(currentItem).DataContext;
            filter.SetCheckedAll();
        }

    }
}