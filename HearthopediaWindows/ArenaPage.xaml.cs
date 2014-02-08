using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Hearthopedia.Arena
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ArenaPage : HearthopediaWindows.Common.LayoutAwarePage
    {
        private Arena ArenaInstance {get; set;}

        public ArenaPage()
        {
            this.InitializeComponent();
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

            UpdateCardImages();
        }

        private async void DownloadImage(Card c, Image cardImage)
        {
            try
            {
                Uri uri = new Uri(c.imageURL);
                StorageFile destinationFile;
                try
                {
                    destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                        c.image, CreationCollisionOption.GenerateUniqueName);
                }
                catch (FileNotFoundException ex)
                {

                    return;
                }

                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(uri, destinationFile);
                await download.StartAsync();
                ResponseInformation response = download.GetResponseInformation();
                Uri imageUri;
                BitmapImage image = null;

                if (Uri.TryCreate(destinationFile.Path, UriKind.RelativeOrAbsolute, out imageUri))
                {
                    image = new BitmapImage(imageUri);
                }
                else
                {
                    throw new Exception();
                }
                cardImage.Source = image;
            }
            catch
            {
            }
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int classId = (int)e.Parameter;

            ArenaInstance = new Arena(classId);
            SetupDataContexts();

            var parameter = e.Parameter as string;
        }

        private void UpdateCardImages()
        {
            CardImage0.Source = UnloadedCard.Source;
            CardImage1.Source = UnloadedCard.Source;
            CardImage2.Source = UnloadedCard.Source;

            DownloadImage(ArenaInstance.CurrentRoundCards[0], CardImage0);
            CardButton0.DataContext = ArenaInstance.CurrentRoundCards[0];

            DownloadImage(ArenaInstance.CurrentRoundCards[1], CardImage1);
            CardButton1.DataContext = ArenaInstance.CurrentRoundCards[1];

            DownloadImage(ArenaInstance.CurrentRoundCards[2], CardImage2);
            CardButton2.DataContext = ArenaInstance.CurrentRoundCards[2];
        }

        private void ChooseCard(Card c)
        {
            ArenaInstance.ChooseCard(c);
            UpdateCardImages();
        }

        private void ChoosableCard_Clicked(object sender, RoutedEventArgs e)
        {
            FrameworkElement senderCard = (FrameworkElement)sender;
            Card chosenCard = (Card)senderCard.DataContext;

            ChooseCard(chosenCard);
        }
    }
}
