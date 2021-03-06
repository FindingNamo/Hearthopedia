﻿using Hearthopedia.Resources;
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
using System.Windows.Media;
using System.Windows.Input;

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
            cardsBinding.Source = DataManager.Instance.SearchedCards;
            listCards.SetBinding(ListBox.ItemsSourceProperty, cardsBinding);

            // Check if it's the first boot ever an do the right thing
            DataAccess.OnBootOperations();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Update search time
            DataManager.Instance.LastSearchTime = DateTime.Now;
            DataAccess.SearchCards(textBoxSearch.Text);
        }

        private void TextBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxSearch.Background = new SolidColorBrush(Colors.Transparent);
            textBoxSearch.BorderBrush = new SolidColorBrush(Colors.Transparent);
            textBoxSearch.SelectionBackground = new SolidColorBrush(Colors.Transparent);
        }

        private void OnListItemClick(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FrameworkElement clickedObject = (FrameworkElement)sender;
            Card selectedCard = (Card)clickedObject.DataContext;
            NavigationService.Navigate(new Uri("/CardPage.xaml?id=" + selectedCard.id, UriKind.Relative));
        }

        private void textBoxSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.Focus();
        }

        private void listCards_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            this.Focus();
        }

        private void Image_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FilterPage.xaml?search=" + textBoxSearch.Text, UriKind.Relative));
        }

        private void ClassIcon_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FrameworkElement icon = (FrameworkElement)sender;
            NavigationService.Navigate(new Uri("/TierListChallenge.xaml?classId=" + icon.Tag, UriKind.Relative));
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