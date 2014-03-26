using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;

namespace Hearthopedia
{
    public partial class TierListChallenge : PhoneApplicationPage
    {
        private Arena.Arena ArenaInstance { get; set; }
        private int _score = 0;

        public TierListChallenge()
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
                
                // Changing the Active Tierlist to this class
                string className = Enum.GetName(typeof(CardClass), idVal);
                CardTier.TierClass tierClass;
                if (Enum.TryParse<CardTier.TierClass>(className, out tierClass))
                    TierListManager.Instance.ActiveTierClass = tierClass;

                SetupDataContexts();

                base.OnNavigatedTo(e);
            }
        }

        private void SetupDataContexts()
        {
            ClassIcon.DataContext = ArenaInstance.ClassIcon;

            UpdateCardImages();
        }

        private void ChooseCard(Card c)
        {
            int chosenTier = TierListManager.Instance.GetTierFromCard(c);
            int bestTier = TierListManager.Instance.GetTopTierPick(ArenaInstance.CurrentRoundCards);

            if (chosenTier == bestTier)
            {
                _score++;
                ScoreLabel.Text = "" + _score;
                TextBlockResult.Text = "Wise choice lad!";
            }
            else
            {
                TextBlockResult.Text = "You chose poorly...";
            }

            TextBlockPrevResult0.Text = Enum.GetName(typeof(CardTier.TierRank), ArenaInstance.CurrentRoundCards[0].Tier);
            TextBlockPrevResult1.Text = Enum.GetName(typeof(CardTier.TierRank), ArenaInstance.CurrentRoundCards[1].Tier);
            TextBlockPrevResult2.Text = Enum.GetName(typeof(CardTier.TierRank), ArenaInstance.CurrentRoundCards[2].Tier);

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
            PrevCardImage0.Source = CardImage0.Source;
            PrevCardImage1.Source = CardImage1.Source;
            PrevCardImage2.Source = CardImage2.Source;

            CardImage0.Source = UnloadedCard.Source;
            CardImage1.Source = UnloadedCard.Source;
            CardImage2.Source = UnloadedCard.Source;

            //DownloadImage(ArenaInstance.CurrentRoundCards[0], CardImage0);
            CardImage0.DataContext = ArenaInstance.CurrentRoundCards[0];
            ImageManager.Instance.SetImageFromCard(ArenaInstance.CurrentRoundCards[0], CardImage0);

            //DownloadImage(ArenaInstance.CurrentRoundCards[1], CardImage1);
            CardImage1.DataContext = ArenaInstance.CurrentRoundCards[1];
            ImageManager.Instance.SetImageFromCard(ArenaInstance.CurrentRoundCards[1], CardImage1);

            //DownloadImage(ArenaInstance.CurrentRoundCards[2], CardImage2);
            CardImage2.DataContext = ArenaInstance.CurrentRoundCards[2];
            ImageManager.Instance.SetImageFromCard(ArenaInstance.CurrentRoundCards[2], CardImage2);
        }
    }
}