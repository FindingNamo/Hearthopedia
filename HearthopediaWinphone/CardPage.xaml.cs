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
        // Need this hack because 2-way binding to the active tier list is not trivial
        private bool listPickerDoneBinding = false;

        private Card selectedCard;

        public CardPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string idString = "";
            int idVal;

            // Bind to selected card passed in via url
            if (NavigationContext.QueryString.TryGetValue("id", out idString))
            {
                if (!int.TryParse(idString, out idVal))
                    throw new ArgumentException();

                foreach (Card card in DataManager.Instance.Cards)
                {
                    if (card.id == idVal)
                        selectedCard = card;
                }
            }
            this.DataContext = selectedCard;

            imageCard.Source = new BitmapImage(new Uri("\\Assets\\UnloadedCard.png", UriKind.Relative));

            ImageManager.Instance.SetImageFromCard(selectedCard, imageCard);
            DownloadFlavourText(selectedCard.flavourTextURL);

            // Bind the Tier List Class Dropdown
            ListPickerTierClass.ItemsSource = Enum.GetValues(typeof(CardTier.TierClass));
            ListPickerTierClass.SelectedIndex = (int)TierListManager.Instance.ActiveTierClass;
            listPickerDoneBinding = true;
            
        }

        private void DownloadFlavourText(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.OpenReadCompleted += DownloadFlavourTextCompleted;
                webClient.OpenReadAsync(new Uri(url));
            }
            catch 
            {
            }
        }

        private void DownloadFlavourTextCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                try
                {
                    StreamReader reader = new StreamReader(e.Result);
                    string responseBody = reader.ReadToEnd();
                    string flavourText = responseBody.Substring(responseBody.IndexOf("<i>") + 3);
                    flavourText = flavourText.Substring(0, flavourText.IndexOf("</i>"));
                    flavourText = Utilities.FilterHTML(flavourText);
                    textBlockFlavourText.Text = flavourText;
                }
                catch
                {
                    textBlockFlavourText.Text = "";
                }
            }
        }

        private void ListPickerTierClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Need that weird bool because of a listpicker bug that causes the event to fire multiple time upon creation
            if (listPickerDoneBinding && (e.AddedItems.Count!= 0))
            {
                CardTier.TierClass selectedClass;
                Enum.TryParse<CardTier.TierClass>(e.AddedItems[0].ToString(), out selectedClass);
                if (selectedClass != TierListManager.Instance.ActiveTierClass)
                    TierListManager.Instance.ActiveTierClass = selectedClass;
                TextBlockTierRank.Text = selectedCard.TierString;
            }
        }
    }
}