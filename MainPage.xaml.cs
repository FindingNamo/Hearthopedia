using Hearthopedia.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Hearthopedia
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Bind the listbox to the cards list
            Binding cardsBinding = new Binding();
            cardsBinding.Source = DataManager.Instance.DisplayedCards;
            listCards.SetBinding(ListBox.ItemsSourceProperty, cardsBinding);

            // Populate the cards list
            DataAccess.PopulateDataManagerCards();

            // Check for update (we still need to prompt for update, this simply tells the user theere has been updates but won't read from it until the next boot)
            DataAccess.GetDataFromHearthHead();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.GetDataFromHearthHead();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataAccess.DeleteFromLocalStorage("cards.txt");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DataAccess.PopulateDataManagerCards();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update search time
            DataManager.Instance.LastSearchTime = DateTime.Now;
            DataAccess.PopulateDataManagerDisplayedCards(txtbox1.Text);
        }

        private void listCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Card selectedCard = (Card)(e.AddedItems[0]);
            NavigationService.Navigate(new Uri("/CardPage.xaml?name="+selectedCard.name, UriKind.Relative));
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}