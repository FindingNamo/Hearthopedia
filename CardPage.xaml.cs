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
using System.IO;

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

            imageCard.Source = new BitmapImage(new Uri("\\Assets\\UnloadedCard.png", UriKind.Relative));

            DownloadImage(selectedCard.imageURL);
            DownloadFlavourText(selectedCard.flavourTextURL);
        }

        private void  DownloadImage(string url)
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += DownloadImageCompleted;
            webClient.OpenReadAsync(new Uri(url));
        }

        private void DownloadImageCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //TODO: Trigger a sweet flipping animation

            if (!e.Cancelled && e.Error == null)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.Result);
                imageCard.Source = bmp;
            }
        }

        private void DownloadFlavourText(string url)
        {
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += DownloadFlavourTextCompleted;
            webClient.OpenReadAsync(new Uri(url));
        }

        private void DownloadFlavourTextCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
               StreamReader reader =  new StreamReader(e.Result);
               string responseBody = reader.ReadToEnd();
               string flavourText = responseBody.Substring(responseBody.IndexOf("<i>") + 3);
               flavourText = flavourText.Substring(0, flavourText.IndexOf("</i>"));
               flavourText = Utilities.FilterHTML(flavourText);
               textBlockFlavourText.Text = flavourText;
            }
        }
    }
}