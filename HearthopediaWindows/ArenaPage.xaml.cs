using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.ViewManagement;
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
    /// This page is locked to this aspect:
    /// 1366x768
    /// </summary>
    public sealed partial class ArenaPage : HearthopediaWindows.Common.LayoutAwarePage
    {
        private Arena ArenaInstance {get; set;}

        public double LockedAspectScaleY
        {
            get
            {
                return this.ActualWidth / 768f;
            }
        }

        public ArenaPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
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

        private async void DownloadImage(Card card, Image cardImage)
        {
            // use local copy first
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(card.localFilename);
                Uri imageUri; BitmapImage image = null;
                if (Uri.TryCreate(localFile.Path, UriKind.RelativeOrAbsolute, out imageUri))
                {
                    image = new BitmapImage(imageUri);
                }
                cardImage.Source = image;
            }
            // if things go horribly wrong or if it's the first time and we don't have a local copy, use backup
            catch
            {
                Uri imageUri = new Uri(card.backupURI, UriKind.Absolute);
                BitmapImage image = new BitmapImage(imageUri);
                cardImage.Source = image;
            }

            // try to download to have an updated local copy
            try
            {
                Uri uri = new Uri(card.imageURL);
                StorageFile destinationFile;
                try
                {
                    destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("temp" + ".png", CreationCollisionOption.GenerateUniqueName);
                }
                catch (FileNotFoundException ex)
                {
                    return;
                }

                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(uri, destinationFile);
                await download.StartAsync();
                await destinationFile.CopyAsync(ApplicationData.Current.LocalFolder, card.localFilename, NameCollisionOption.ReplaceExisting);

                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(card.localFilename);
                Uri imageUri; BitmapImage image = null;
                if (Uri.TryCreate(localFile.Path, UriKind.RelativeOrAbsolute, out imageUri))
                {
                    image = new BitmapImage(imageUri);
                }
                cardImage.Source = image;
            }
            catch (Exception e)
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
            base.OnNavigatedTo(e);
        }

        private void UpdateCardImages()
        {
            CardImage0.Source = UnloadedCard.Source;
            CardImage1.Source = UnloadedCard.Source;
            CardImage2.Source = UnloadedCard.Source;

            DownloadImage(ArenaInstance.CurrentRoundCards[0], CardImage0);
            CardImage0.DataContext = ArenaInstance.CurrentRoundCards[0];

            DownloadImage(ArenaInstance.CurrentRoundCards[1], CardImage1);
            CardImage1.DataContext = ArenaInstance.CurrentRoundCards[1];

            DownloadImage(ArenaInstance.CurrentRoundCards[2], CardImage2);
            CardImage2.DataContext = ArenaInstance.CurrentRoundCards[2];
        }

        private void ChooseCard(Card c)
        {
            ArenaInstance.ChooseCard(c);
            UpdateCardImages();
        }

        private void ChoosableCard_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement senderCard = (FrameworkElement)sender;
            Card chosenCard = (Card)senderCard.DataContext;

            ChooseCard(chosenCard);
        }

        /// <summary>
        /// Resize the root element to maintain a locked aspect ratio
        /// It'll stretch it to fill the shortest distance
        /// Very wide screens stretch till the height is filled.
        /// Narrow screens stretch till the width is filled.
        /// </summary>
        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double screenWidth = e.NewSize.Width;
            double screenHeight = e.NewSize.Height;
            
            double targetWidth = 1980;
            double targetHeight = 1080;
            double targetAspect = targetWidth / targetHeight;

            double scaleY = screenHeight / targetHeight;
            double scaleX = screenWidth / targetWidth;

            if (scaleY < scaleX)
                scaleX = (targetAspect * screenHeight) / targetWidth;
            else
                scaleY = (screenWidth / targetAspect) / targetHeight;

            RootLayoutTransform.ScaleX = scaleX;
            RootLayoutTransform.ScaleY = scaleY;
        }
    }
}
