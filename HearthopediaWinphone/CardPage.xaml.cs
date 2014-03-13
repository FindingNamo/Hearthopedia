﻿using System;
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

            string idString = "";
            int idVal;

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

            //DownloadImage(selectedCard.imageURL);
            ImageManager.Instance.SetImageFromCard(selectedCard, imageCard);
            DownloadFlavourText(selectedCard.flavourTextURL);
        }

        private void  DownloadImage(string url)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.OpenReadCompleted += DownloadImageCompleted;
                webClient.OpenReadAsync(new Uri(url));
            }
            catch
            {
            }
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
    }
}