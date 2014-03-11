using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hearthopedia
{
    public partial class ArenaPage : PhoneApplicationPage
    {
        private Arena.Arena ArenaInstance { get; set; }

        public ArenaPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string idString = "";
            int idVal;

            if (NavigationContext.QueryString.TryGetValue("classId", out idString))
            {
                if (!int.TryParse(idString, out idVal))
                    throw new ArgumentException();

                ArenaInstance = new Arena.Arena(idVal);
                SetupDataContexts();

                base.OnNavigatedTo(e);
            }
        }


        private void SetupDataContexts()
        {
            ChosenCards.ItemsSource = ArenaInstance.ChosenCards;

            CostBar0.DataContext = ArenaInstance;
            CostBar1.DataContext = ArenaInstance;
            CostBar2.DataContext = ArenaInstance;
            CostBar3.DataContext = ArenaInstance;
            CostBar4.DataContext = ArenaInstance;
            CostBar5.DataContext = ArenaInstance;
            CostBar6.DataContext = ArenaInstance;
            CostBar7.DataContext = ArenaInstance;

            ClassIcon.DataContext = ArenaInstance.ClassIcon;
            RoundPresenter.DataContext = ArenaInstance;

            UpdateCardImages();
        }

        private void ChooseCard(Card c)
        {
            ArenaInstance.ChooseCard(c);
            UpdateCardImages();
        }

        private void ChoosableCard_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FrameworkElement senderCard = (FrameworkElement)sender;
            Card chosenCard = (Card)senderCard.DataContext;

            ChooseCard(chosenCard);
        }

        private void UpdateCardImages()
        {
            CardImage0.Source = UnloadedCard.Source;
            CardImage1.Source = UnloadedCard.Source;
            CardImage2.Source = UnloadedCard.Source;

            //DownloadImage(ArenaInstance.CurrentRoundCards[0], CardImage0);
            CardImage0.DataContext = ArenaInstance.CurrentRoundCards[0];

            //DownloadImage(ArenaInstance.CurrentRoundCards[1], CardImage1);
            CardImage1.DataContext = ArenaInstance.CurrentRoundCards[1];

            //DownloadImage(ArenaInstance.CurrentRoundCards[2], CardImage2);
            CardImage2.DataContext = ArenaInstance.CurrentRoundCards[2];
        }
    }
}