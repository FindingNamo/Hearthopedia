using System;
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

                            if (retriesLeft == 0)
                            {
#if NETFX_CORE
                                CoreDispatcher dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                                dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                                {
                                    DataAccess.PopulateDataManagerCards(false);
                                });
#else
                                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    DataAccess.PopulateDataManagerCards(false);
                                });
#endif
                            }
                        }
                    }



                    // Compare to existing string
                    using (StreamReader reader = new StreamReader(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("cards.txt")))
                    {
                        cachedResponseString = await reader.ReadToEndAsync();
#if NETFX_CORE
#else
                        reader.Close();
#endif
                        reader.Dispose();
                    }

                    if ((cachedResponseString == responseString) && !(String.IsNullOrEmpty(responseString)))
                    {
                        // update local text file
#if NETFX_CORE
                        StreamWriter writer = new StreamWriter(await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("cards.txt", CreationCollisionOption.ReplaceExisting));
                        writer.Write(responseString);
                        writer.Flush();
                        writer.Dispose();
#else
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(async () =>
                        {
                            StreamWriter writer = new StreamWriter(await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("cards.txt", CreationCollisionOption.ReplaceExisting));
                            writer.Write(responseString);
                            writer.Flush();
                            writer.Close();
                            writer.Dispose();
                        });
#endif


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
                            DataAccess.PopulateDataManagerCards(false);
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

        public static async Task PopulateDataManagerCards(bool onBoot)
        {
            DataManager.Instance.Cards.Clear();

            // populate from disk
            StreamReader reader = new StreamReader(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("cards.txt"));

            while (reader.Peek() >= 0)
            {
                string currentLine = reader.ReadLine();

                if (currentLine.Contains("\"g_hearthstone_mechanics\""))
                    break;
                    

                if (currentLine.Contains("\"id\""))
                {
                    string jsonString = currentLine.Substring(currentLine.IndexOf("{"), currentLine.IndexOf("}") - currentLine.IndexOf("{") + 1);
                    Card currentCard = Utilities.GetCardFromJson(jsonString);
                    DataManager.Instance.Cards.Add(currentCard);
                }
            }

            // Sort
            DataManager.Instance.SortCards();

            // Display cards if we just booted
            if (onBoot)
            {
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
        }

#if NETFX_CORE
#else
        public static async Task SearchCards(string searchString)
        {
            // Only do the if it's been this many seconds since the textbox changed
            double searchDelaySec = System.Convert.ToDouble("0.2");
            int searchNumMinChar = 2;

            // only do the search if there are this many characters
            if ((searchString.Length >= searchNumMinChar))
            {
                Thread thread = new Thread((ThreadStart)delegate
                {
                    Thread.Sleep(System.Convert.ToInt16(searchDelaySec * 1000));

                    // if it's been long enough and no new search has been requested, actually do the search
                    if (DateTime.Now > DataManager.Instance.LastSearchTime.AddSeconds(searchDelaySec))
                    {
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
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
                });
                thread.Start();
            }
            else if (searchString.Length == 0)
            {
                Thread thread = new Thread((ThreadStart)delegate
                {
                    Thread.Sleep(System.Convert.ToInt16(searchDelaySec * 1000));

                    // if it's been long enough and no new search has been requested, actually do the search
                    if (DateTime.Now > DataManager.Instance.LastSearchTime.AddSeconds(searchDelaySec))
                    {
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
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
                });
                thread.Start();
            }
            FilterManager.Instance.Dirty = false;
        }


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

        public static async Task FirstBootOperations()
        {
            string defaultCardsString = "";

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

#if NETFX_CORE
            StorageFile storageFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Assets\cards.txt");
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

                reader.Close();

            }
#endif
            if (firstRun)
            {
                using (StreamWriter writer = new StreamWriter(await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("cards.txt", CreationCollisionOption.ReplaceExisting)))
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
}
