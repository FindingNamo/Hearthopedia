using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Hearthopedia
{
    public partial class CardPage : PhoneApplicationPage
    {
        private Card selectedCard;

        public CardPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string name = "";

            if (NavigationContext.QueryString.TryGetValue("name", out name))
            {
                foreach (Card card in DataManager.Instance.Cards)
                {
                    if (card.name.Equals(name))
                    {
                        selectedCard = card;
                    }
                }
            }
            this.DataContext = selectedCard;

            mainItem.Header = selectedCard.name;
            DownloadImage(selectedCard.imageURL);
        }

        private void  DownloadImage(string url)
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += DownloadImageCompleted;
            webClient.OpenReadAsync(new Uri(url));
        }

        private void DownloadImageCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.Result);
                imageCard.Source = bmp;
            }
        }

        private void PanoramaObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Panorama pan = (Panorama)sender;
        }
    }
}