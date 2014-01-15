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
            NumberPanorama.DataContext = NumberFilter.Instance;
            MechanicPanorama.DataContext = CardMechanicFilter.Instance;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            DataAccess.SearchCards();
        }

        private void UncheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            PanoramaItem currentItem = (PanoramaItem)(EntirePanorama.SelectedItem);

            // For some reason its not a real-time databinding.
            // Likely because the bool isn't observable.

            // But, if we remove the data context from the list, and attach it again.
            // Even if they're 1 line apart, it fixes that.

            // I want to put a few lines in between though, just
            // so the hack doesn't get optimized away in ship.
            object hack = currentItem.DataContext;
            currentItem.DataContext = null;
            
            ICardFilter filter = (ICardFilter)(currentItem).DataContext;
            filter.SetUncheckedAll();

            currentItem.DataContext = hack;
        }

        private void CheckAllButton_Click_1(object sender, RoutedEventArgs e)
        {
            PanoramaItem currentItem = (PanoramaItem)(EntirePanorama.SelectedItem);

            // Hack (See above)
            object hack = currentItem.DataContext;
            currentItem.DataContext = null;

            ICardFilter filter = (ICardFilter)(currentItem).DataContext;
            filter.SetCheckedAll();


            currentItem.DataContext = hack;
        }

    }
}