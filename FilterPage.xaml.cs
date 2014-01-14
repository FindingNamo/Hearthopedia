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
        public FilterPage()
        {
            InitializeComponent();
            ClassPanorama.DataContext = CardClassFilter.Instance;
            QualityPanorama.DataContext = CardQualityFilter.Instance;
            RacePanorama.DataContext = CardRaceFilter.Instance;
            SetPanorama.DataContext = CardSetFilter.Instance;
            TypePanorama.DataContext = CardTypeFilter.Instance;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            DataAccess.ApplyFilterOnDisplayedSearch();
        }
    }
}