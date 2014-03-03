﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Windows.Foundation;
using Windows.Storage;
using System.Threading;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using System.Reflection;
using Hearthopedia.Filters;

#if NETFX_CORE
using Windows.Storage.Streams;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Windows.System.Threading;
#endif

namespace Hearthopedia
{
    class DataAccess
    {
        public static async Task GetDataFromHearthHead()
        {
            string urlGetCardHH = "http://www.hearthhead.com/data=hearthstone-cards";
            int retriesLeft = 5;
            string responseString = "";
            string cachedResponseString = "";

                // Wrap the entire thing in a try-catch due to that weird bug
                HttpWebRequest request = WebRequest.CreateHttp(urlGetCardHH);
                request.BeginGetResponse(async (asyncResult) =>
                {
                    HttpWebRequest request2 = (HttpWebRequest)asyncResult.AsyncState;

                    // End the operation
                    while (retriesLeft > 0)
                    {
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse)request2.EndGetResponse(asyncResult);
                            Stream streamResponse = response.GetResponseStream();
                            StreamReader streamRead = new StreamReader(streamResponse);
                            responseString = streamRead.ReadToEnd();

                            // No need to retry anymore since we've succeeded
                            retriesLeft = 0;



#if NETFX_CORE
#else
                            // Close the stream object
                            streamResponse.Close();
                            streamRead.Close();

                            // Release the HttpWebResponse
                            response.Close();
#endif
                        }
                        catch
                        {
                            retriesLeft--;
                        }
                    }



                    // Compare to existing string
                    StreamReader reader = new StreamReader(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("cards.txt"));
                    cachedResponseString = await reader.ReadToEndAsync();
#if NETFX_CORE
#else
                    reader.Close();
#endif
                    reader.Dispose();
                    
                    if ((cachedResponseString != responseString) && !(String.IsNullOrEmpty(responseString)) && retriesLeft != 0)
                    {
                        // update local text file
                        StreamWriter writer = new StreamWriter(await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("cards.txt", CreationCollisionOption.ReplaceExisting));
                        writer.Write(responseString);
                        writer.Flush();
#if NETFX_CORE
#else
                        writer.Close();
#endif
                        writer.Dispose();



                        // Tell the app that there has been updates and let user choose when to update
#if NETFX_CORE
                        CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

                        await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            MessageDialog dialog = new MessageDialog("It looks like cards have been updated!  Next time you start Hearthopedia, the cards will be available ^_^!");

                            //Show message
                            dialog.ShowAsync();
                        });
#else
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBoxResult result = MessageBox.Show("It looks like cards have been updated!  Next time you start Hearthopedia, the cards will be available ^_^!");
                        });
#endif
                    }
                }, request);
        }

        public static async Task DeleteFromLocalStorage(string fileName)
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new file named DataFile.txt.
            var file = await local.GetFileAsync(fileName);
            await file.DeleteAsync();
        }

        public static async Task PopulateDataManagerCards()
        {
            DataManager.Instance.Cards.Clear();
            DataManager.Instance.Mechanics.Clear();

            // populate from disk
            StreamReader reader = new StreamReader(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("cards.txt"));
            
            bool parsingMechanics = false;
            string currentLine = "";
            string jsonString = "";
            Card currentCard = null;

            while (reader.Peek() >= 0)
            {
                currentLine = reader.ReadLine();

                if (currentLine.Contains("g_hearthstone_mechanics"))
                {
                    parsingMechanics = true;
                    continue;
                }

                if (!parsingMechanics && currentLine.Contains("\"id\""))
                {
                    jsonString = currentLine.Substring(currentLine.IndexOf("{"), currentLine.IndexOf("}") - currentLine.IndexOf("{") + 1);
                    currentCard = Utilities.GetCardFromJson(jsonString);
                    DataManager.Instance.Cards.Add(currentCard);
                }

                if (parsingMechanics && currentLine.Contains("\"id\""))
                {
                    jsonString = currentLine.Substring(currentLine.IndexOf("{"), currentLine.IndexOf("}") - currentLine.IndexOf("{") + 1);
                    Mechanic newMechanic = Utilities.GetMechanicFromJson(jsonString);
                    DataManager.Instance.Mechanics.Add(newMechanic);
                }
            }

#if NETFX_CORE
#else
            reader.Close();
#endif
            reader.Dispose();

            // Sort
            DataManager.Instance.SortCards();

            // Display cards if we just booted
            DataManager.Instance.SearchedCards.Clear();

            foreach (Card card in DataManager.Instance.Cards)
            {
                if (card.CardTypeString != null)
                {
                    if (!(card.CardTypeString.Equals("Unknown")))
                        DataManager.Instance.SearchedCards.Add(card);
                }
            }
        }



        public static async Task SearchCards(string searchString)
        {
            // Only do the if it's been this many seconds since the textbox changed
            double searchDelaySec = System.Convert.ToDouble("0.2");
            int searchNumMinChar = 2;

            // only do the search if there are this many characters
            if ((searchString.Length >= searchNumMinChar))
            {
#if NETFX_CORE
                TimeSpan delay = TimeSpan.FromSeconds(searchDelaySec);
                ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreateTimer(
                    (source) =>
                    {
#else
                Thread thread = new Thread((ThreadStart)delegate
                {
                    Thread.Sleep(System.Convert.ToInt16(searchDelaySec * 1000));
#endif


                    // if it's been long enough and no new search has been requested, actually do the search
                    if (DateTime.Now > DataManager.Instance.LastSearchTime.AddSeconds(searchDelaySec))
                    {
#if NETFX_CORE
                        CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
                        dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
#else
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
#endif
                            DataManager.Instance.SearchedCards.Clear();
                            foreach (Card card in DataManager.Instance.Cards)
                            {
                                if (card.CardTypeString != null)
                                {
                                    if (card.name.ToLower().Contains(searchString.ToLower()) &&
                                        !(card.CardTypeString.Equals("Unknown")) &&
                                        FilterManager.Instance.Check(card))
                                    {
                                        DataManager.Instance.SearchedCards.Add(card);
                                    }
                                }
                            }
                        });
                    }
#if NETFX_CORE
                }, delay);
#else
                });
                thread.Start();
#endif
            }
            else if (searchString.Length == 0)
            {
#if NETFX_CORE
                TimeSpan delay = TimeSpan.FromSeconds(searchDelaySec);
                ThreadPoolTimer DelayTimer = ThreadPoolTimer.CreateTimer(
                    (source) =>
                    {
#else
                Thread thread = new Thread((ThreadStart)delegate
                {
                    Thread.Sleep(System.Convert.ToInt16(searchDelaySec * 1000));
#endif

                    // if it's been long enough and no new search has been requested, actually do the search
                    if (DateTime.Now > DataManager.Instance.LastSearchTime.AddSeconds(searchDelaySec))
                    {
#if NETFX_CORE
                        CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

                        dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
#else
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
#endif
                            DataManager.Instance.SearchedCards.Clear();
                            foreach (Card card in DataManager.Instance.Cards)
                            {
                                if (FilterManager.Instance.Check(card))
                                {
                                    DataManager.Instance.SearchedCards.Add(card);
                                }
                            }
                        });
                    }
#if NETFX_CORE
                }, delay);
#else
                });
                thread.Start();
#endif
            }
            FilterManager.Instance.Dirty = false;
        }
#if NETFX_CORE
#else
        public static async Task SearchCardsLINQ(string searchString)
        {
            // Only do the if it's been this many seconds since the textbox changed
            int searchDelaySec = 1;
            int searchNumMinChar = 2;

            // only do the search if there are this many characters
            if (searchString.Length >= searchNumMinChar)
            {
                Thread thread = new Thread((ThreadStart)delegate
                {
                    Thread.Sleep(searchDelaySec * 1000);

                    ObservableCollection<Card> cards = new ObservableCollection<Card>(from card in DataManager.Instance.Cards where card.name.Contains(searchString) select card);

                    

                    System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        DataManager.Instance.SearchedCards.Clear();
                        foreach (Card card in cards)
                        {
                            DataManager.Instance.SearchedCards.Add(card);
                        }
                    });
                });
                thread.Start();
            }
        }
#endif

        public static async Task OnBootOperations()
        {
            // If the file doesn't exist it means it's our first run ever
            bool firstRun = false;
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("cards.txt");
                //no exception means file exists
            }
            catch (FileNotFoundException ex)
            {
                firstRun = true;
            }

            if (firstRun)
            {
                await WriteResourceToStorage(@"Assets/cards.txt", "cards.txt");
            }

            await DataAccess.PopulateDataManagerCards();

            await DataAccess.GetDataFromHearthHead();

            await DataAccess.PopulateTierListData(CardTier.TierClass.Druid);
        }

        public static async Task PopulateTierListData(CardTier.TierClass tierClass, CardTier.TierSource tierSource = CardTier.TierSource.Antigravity)
        {
            string urlGetTier = GetTierListURL(tierClass, tierSource);
            int retriesLeft = 5;
            string responseString = "";
            string cachedResponseString = "";

            // Wrap the entire thing in a try-catch due to that weird bug
            HttpWebRequest request = WebRequest.CreateHttp(urlGetTier);
            request.BeginGetResponse(async (asyncResult) =>
            {
                HttpWebRequest request2 = (HttpWebRequest)asyncResult.AsyncState;

                // End the operation
                while (retriesLeft > 0)
                {
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request2.EndGetResponse(asyncResult);
                        Stream streamResponse = response.GetResponseStream();
                        StreamReader streamRead = new StreamReader(streamResponse);
                        responseString = streamRead.ReadToEnd();

                        List<CardTier> cardTierList = Utilities.GetCardTierFromJson(responseString);

                        foreach (CardTier tierInfo in cardTierList)
                        {
                            foreach (Card card in DataManager.Instance.Cards)
                            {
                                if (tierInfo.name.ToLower() == card.name.ToLower())
                                    card.tier = (CardTier.TierRank)tierInfo.tier;

                            }
                        }

                        // No need to retry anymore since we've succeeded
                        retriesLeft = 0;



#if NETFX_CORE
#else
                            // Close the stream object
                            streamResponse.Close();
                            streamRead.Close();

                            // Release the HttpWebResponse
                            response.Close();
#endif
                    }
                    catch
                    {
                        retriesLeft--;
                    }
                }

                // Compare to existing string
                StreamReader reader = new StreamReader(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(GetLocalTierListPath(tierClass)));
                cachedResponseString = await reader.ReadToEndAsync();
#if NETFX_CORE
#else
                    reader.Close();
#endif
                reader.Dispose();

                if ((cachedResponseString != responseString) && !(String.IsNullOrEmpty(responseString)) && retriesLeft != 0)
                {
                    // update local text file
                    StreamWriter writer = new StreamWriter(await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(GetLocalTierListPath(tierClass), CreationCollisionOption.ReplaceExisting));
                    writer.Write(responseString);
                    writer.Flush();
#if NETFX_CORE
#else
                        writer.Close();
#endif
                    writer.Dispose();



                    // Tell the app that there has been updates and let user choose when to update
#if NETFX_CORE
                    CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        MessageDialog dialog = new MessageDialog("It looks like cards have been updated!  Next time you start Hearthopedia, the cards will be available ^_^!");

                        //Show message
                        dialog.ShowAsync();
                    });
#else
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            MessageBoxResult result = MessageBox.Show("It looks like cards have been updated!  Next time you start Hearthopedia, the cards will be available ^_^!");
                        });
#endif
                }
            }, request);
        }

        private static string GetLocalTierListPath(CardTier.TierClass tierClass)
        {
            switch (tierClass)
            {
                case CardTier.TierClass.Druid:
                    return "antigravity_druid.json";
                case CardTier.TierClass.Hunter:
                    return "antigravity_hunter.json";
                case CardTier.TierClass.Mage:
                    return "antigravity_mage.json";
                case CardTier.TierClass.Paladin:
                    return "antigravity_paladin.json";
                case CardTier.TierClass.Priest:
                    return "antigravity_priest.json";
                case CardTier.TierClass.Rogue:
                    return "antigravity_rogue.json";
                case CardTier.TierClass.Shaman:
                    return "antigravity_shaman.json";
                case CardTier.TierClass.Warlock:
                    return "antigravity_warlock.json";
                case CardTier.TierClass.Warrior:
                    return "antigravity_warrior.json";
                default:
                    return "bad times son...";
            }
        }

        private static string GetTierListURL(CardTier.TierClass tierClass, CardTier.TierSource tierSource)
        {
            string baseURL = @"http://stillatthebottom.com/hearthopedia/";
            switch (tierSource)
            {
                case CardTier.TierSource.Antigravity:
                    {
                        switch (tierClass)
                        {
                            case CardTier.TierClass.Druid:
                                return baseURL + "antigravity_druid.json";
                            case CardTier.TierClass.Hunter:
                                return baseURL + "antigravity_hunter.json";
                            case CardTier.TierClass.Mage:
                                return baseURL + "antigravity_mage.json";
                            case CardTier.TierClass.Paladin:
                                return baseURL + "antigravity_paladin.json";
                            case CardTier.TierClass.Priest:
                                return baseURL + "antigravity_priest.json";
                            case CardTier.TierClass.Rogue:
                                return baseURL + "antigravity_rogue.json";
                            case CardTier.TierClass.Shaman:
                                return baseURL + "antigravity_shaman.json";
                            case CardTier.TierClass.Warlock:
                                return baseURL + "antigravity_warlock.json";
                            case CardTier.TierClass.Warrior:
                                return baseURL + "antigravity_warrior.json";
                            default:
                                return "antigravity_all.json";
                        }
                        break;
                    }
                case CardTier.TierSource.Trump:
                    {
                        switch (tierClass)
                        {
                            case CardTier.TierClass.Druid:
                                return baseURL + "trump_druid.json";
                            case CardTier.TierClass.Hunter:
                                return baseURL + "trump_druid.json";
                            case CardTier.TierClass.Mage:
                                return baseURL + "trump_mage.json";
                            case CardTier.TierClass.Paladin:
                                return baseURL + "trump_paladin.json";
                            case CardTier.TierClass.Priest:
                                return baseURL + "trump_priest.json";
                            case CardTier.TierClass.Rogue:
                                return baseURL + "trump_rogue.json";
                            case CardTier.TierClass.Shaman:
                                return baseURL + "trump_shaman.json";
                            case CardTier.TierClass.Warlock:
                                return baseURL + "trump_warlock.json";
                            case CardTier.TierClass.Warrior:
                                return baseURL + "trump_warrior.json";
                            default:
                                return "trump_all.json";
                        }
                        break;
                    }
            }

            // if we get here... it's bad times...
            return "error";
        }

        private static async Task WriteResourceToStorage(string resourcePath, string resultingFileName)
        {
            string defaultCardsString = "";

#if NETFX_CORE
            StorageFile storageFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(resourcePath);
            Stream stream = await storageFile.OpenStreamForReadAsync();
            StreamReader reader = new StreamReader(stream);
            await Task.Run(() =>
            {
                defaultCardsString = reader.ReadToEnd();
            });

#else
            Uri cardUri = new Uri("Hearthopedia;component/Assets/cards.txt", UriKind.Relative);

            using (StreamReader reader = new StreamReader(Application.GetResourceStream(cardUri).Stream))
            {
                defaultCardsString = reader.ReadToEnd();

                // reader.Close();
                // reader.Dispose();
            }
#endif

            using (StreamWriter writer = new StreamWriter(await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(resultingFileName, CreationCollisionOption.ReplaceExisting)))
            {
                writer.Write(defaultCardsString);
                writer.Flush();
#if NETFX_CORE
#else
                    writer.Close();
#endif
                writer.Dispose();
            }
        }
    }
   

}
