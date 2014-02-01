using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Hearthopedia;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234
using Hearthopedia.Arena;


namespace HearthopediaWindows
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for the
    /// currently selected item.
    /// </summary>
    public sealed partial class MainPage : HearthopediaWindows.Common.LayoutAwarePage
    {
        // damn son!
        private Card selectedCard;

        public MainPage()
        {
            this.InitializeComponent();

            // Bind the listbox to the cards list
            Binding cardsBinding = new Binding();
            cardsBinding.Source = DataManager.Instance.SearchedCards;
            itemListView.SetBinding(ListBox.ItemsSourceProperty, cardsBinding);

            // Check if it's the first boot ever an do the right thing
            DataAccess.OnBootOperations();
        }

        #region Page state management

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
            if (pageState == null)
            {
                this.itemListView.SelectedItem = null;
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (pageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    // var selectedItem = SampleDataSource.GetItem((String)pageState["SelectedItem"]);
                    // this.itemsViewSource.View.MoveCurrentTo(selectedItem);
                    throw new NotImplementedException("Need to implement previously saved state");
                }
            }
        }

        #endregion

        #region Logical page navigation

        // Visual state management typically reflects the four application view states directly
        // (full screen landscape and portrait plus snapped and filled views.)  The split page is
        // designed so that the snapped and portrait view states each have two distinct sub-states:
        // either the item list or the details are displayed, but not both at the same time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed, or null
        /// for the current view state.  This parameter is optional with null as the default
        /// value.</param>
        /// <returns>True when the view state in question is portrait or snapped, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation(ApplicationViewState? viewState = null)
        {
            if (viewState == null) viewState = ApplicationView.Value;
            return viewState == ApplicationViewState.FullScreenPortrait ||
                viewState == ApplicationViewState.Snapped;
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is Snapped)
        /// displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
            if (e.AddedItems.Count == 1)
            {
                if (gridCardInfo.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
                    gridCardInfo.Visibility = Windows.UI.Xaml.Visibility.Visible;

                DownloadFlavourText(((Card)e.AddedItems[0]).flavourTextURL);
                DownloadImage(((Card)e.AddedItems[0]).imageURL);

                selectedCard = (Card)e.AddedItems[0];
                this.DataContext = selectedCard;
            }
        }

        /// <summary>
        /// Invoked when the page's back button is pressed.
        /// </summary>
        /// <param name="sender">The back button instance.</param>
        /// <param name="e">Event data that describes how the back button was clicked.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation() && itemListView.SelectedItem != null)
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return
                // to the item list.  From the user's point of view this is a logical backward
                // navigation.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                // When logical page navigation is not in effect, or when there is no selected
                // item, use the default back button behavior.
                base.GoBack(sender, e);
            }
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed.</param>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        protected override string DetermineVisualState(ApplicationViewState viewState)
        {
            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation(viewState) && this.itemListView.SelectedItem != null;
            var physicalPageBack = this.Frame != null && this.Frame.CanGoBack;
            this.DefaultViewModel["CanGoBack"] = logicalPageBack || physicalPageBack;

            // Determine visual states for landscape layouts based not on the view state, but
            // on the width of the window.  This page has one layout that is appropriate for
            // 1366 virtual pixels or wider, and another for narrower displays or when a snapped
            // application reduces the horizontal space available to less than 1366.
            if (viewState == ApplicationViewState.Filled ||
                viewState == ApplicationViewState.FullScreenLandscape)
            {
                var windowWidth = Window.Current.Bounds.Width;
                if (windowWidth >= 1366) return "FullScreenLandscapeOrWide";
                return "FilledOrNarrow";
            }

            // When in portrait or snapped start with the default visual state name, then add a
            // suffix when viewing details instead of the list
            var defaultStateName = base.DetermineVisualState(viewState);
            return logicalPageBack ? defaultStateName + "_Detail" : defaultStateName;
        }

        #endregion

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update search time
            DataManager.Instance.LastSearchTime = DateTime.Now;
            
            DataAccess.SearchCards(TextBoxSearch.Text);
        }

        private async void DownloadImage(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                StorageFile destinationFile;
                try
                {
                    destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                        "tempimage.jpg", CreationCollisionOption.GenerateUniqueName);
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
                
                imageCard.Source = image;
            }
            catch
            {
            }
        }


        private async void DownloadFlavourText(string url)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);


                string responseBody = await response.Content.ReadAsStringAsync();
                string flavourText = responseBody.Substring(responseBody.IndexOf("<i>") + 3);
                flavourText = flavourText.Substring(0, flavourText.IndexOf("</i>"));
                textBlockFlavourText.Text = flavourText;
                flavourText = Utilities.FilterHTML(flavourText);
            }
            catch
            {
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArenaPage));
        }
    }
}
