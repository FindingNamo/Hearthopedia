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

namespace Hearthopedia
{
    public partial class MainPage : PhoneApplicationPage
    {
        ObservableCollection<Card> displayedCards = new ObservableCollection<Card>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Bind the listbox to the cards list
            Binding cardsBinding = new Binding();
            cardsBinding.Source = displayedCards;
            listCards.SetBinding(ListBox.ItemsSourceProperty, cardsBinding);

            // Populate the cards list
            DataAccess.PopulateDataManager();

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
            DataAccess.PopulateDataManager();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            displayedCards.Clear();
            foreach(Card card in DataManager.Instance.Cards)
            {
                if (card.name.Contains(txtbox1.Text))
                    displayedCards.Add(card);
            }
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