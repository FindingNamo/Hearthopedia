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

namespace Hearthopedia
{
    class DataAccess
    {
        public static async Task GetDataFromHearthHead()
        {
            bool firstRun = false;
            string urlGetCardHH = "http://www.hearthhead.com/data=hearthstone-cards";
            int retriesLeft = 5;

            while (retriesLeft > 0)
            {
                // Wrap the entire thing in a try-catch due to that weird bug
                try
                {
                    HttpWebRequest request = WebRequest.CreateHttp(urlGetCardHH);
                    request.BeginGetResponse(async (asyncResult) =>
                    {
                        HttpWebRequest request2 = (HttpWebRequest)asyncResult.AsyncState;

                        // End the operation
                        HttpWebResponse response = (HttpWebResponse)request2.EndGetResponse(asyncResult);
                        Stream streamResponse = response.GetResponseStream();
                        StreamReader streamRead = new StreamReader(streamResponse);
                        string responseString = streamRead.ReadToEnd();

                        // Compare to existing string
                        using (StreamReader reader = new StreamReader(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("cards.txt")))
                        {
                            string cachedResponseString = await reader.ReadToEndAsync();
                            reader.Close();
                            reader.Dispose();

                            if (cachedResponseString != responseString)
                            {
                                // update local text file
                                using (StreamWriter writer = new StreamWriter(
                                await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("cards.txt", CreationCollisionOption.ReplaceExisting)))
                                {
                                    writer.Write(responseString);
                                    writer.Flush();
                                }

                                // Tell the app that there has been updates and let user choose when to update
                                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                                {
                                    MessageBoxResult result = MessageBox.Show("It looks like cards have been updated!  Use new cards?", "Updates Available!", MessageBoxButton.OKCancel);
                                    if (result == MessageBoxResult.OK)
                                    {
                                        DataAccess.PopulateDataManagerCards(false);
                                    }
                                });
                            }
                        }

                        // Close the stream object
                        streamResponse.Close();
                        streamRead.Close();

                        // Release the HttpWebResponse
                        response.Close();
                    }, request);

                    // No need to retry anymore since we've succeeded
                    retriesLeft = 0;
                }
                catch
                {
                    retriesLeft--;

                    if (retriesLeft == 0)
                    {
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            DataAccess.PopulateDataManagerCards(false);
                        });
                    }
                }
            }
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

            using (StreamReader reader = new StreamReader(
            await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("cards.txt")))
            {
                while (reader.Peek() >= 0)
                {
                    string currentLine = reader.ReadLine();
                    if (currentLine.Contains("\"id\""))
                    {
                        string jsonString = currentLine.Substring(currentLine.IndexOf("{"), currentLine.IndexOf("}") - currentLine.IndexOf("{") + 1);
                        Card currentCard = Utilities.GetCardFromJson(jsonString);
                        DataManager.Instance.Cards.Add(currentCard);
                    }

                    // Sort
                    DataManager.Instance.SortCards();

                    // Display cards if we just booted
                    if (onBoot)
                    {
                        DataManager.Instance.SearchedCards.Clear();

                        foreach (Card card in DataManager.Instance.Cards)
                        {
                            if (!(card.CardTypeString.Equals("Unknown")))
                                DataManager.Instance.SearchedCards.Add(card);
                        }
                    }
                }
            }
        }

        public static async Task SearchCards(string searchString)
        {
            // Only do the if it's been this many seconds since the textbox changed
            int searchDelaySec = 1;
            int searchNumMinChar = 2;

            // only do the search if there are this many characters
            if ((searchString.Length >= searchNumMinChar))
            {
                Thread thread = new Thread((ThreadStart)delegate
                {
                    Thread.Sleep(searchDelaySec * 1000);

                    // if it's been long enough and no new search has been requested, actually do the search
                    if (DateTime.Now > DataManager.Instance.LastSearchTime.AddSeconds(searchDelaySec))
                    {
                        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            DataManager.Instance.SearchedCards.Clear();
                            foreach (Card card in DataManager.Instance.Cards)
                            {
                                if (card.name.ToLower().Contains(searchString.ToLower()) &&
                                    !(card.CardTypeString.Equals("Unknown")) &&
                                    FilterManager.Instance.Check(card))
                                {
                                    DataManager.Instance.SearchedCards.Add(card);
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
                    Thread.Sleep(searchDelaySec * 1000);

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

        public static async Task FirstBootOperations()
        {
            // If the file doesn't exist it means it's our first run ever
            bool firstRun = !(System.IO.File.Exists("cards.txt"));

            string defaultCardsString ="";

            Uri cardUri = new Uri("Hearthopedia;component/Assets/cards.txt", UriKind.Relative);

            using (StreamReader reader = new StreamReader(Application.GetResourceStream(cardUri).Stream))
            {
                defaultCardsString = reader.ReadToEnd();
                reader.Close();
            }

            if (firstRun)
            {
                using (StreamWriter writer = new StreamWriter(await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("cards.txt", CreationCollisionOption.ReplaceExisting)))
                {
                    writer.Write(defaultCardsString);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
    }
}
