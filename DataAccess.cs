using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Windows.Storage;

namespace Hearthopedia
{
    class DataAccess
    {
        public async static void GetDataFromHearthHead()
        {
            string urlGetCardHH = "http://www.hearthhead.com/data=hearthstone-cards";

            HttpWebRequest request = WebRequest.CreateHttp(urlGetCardHH);
            request.BeginGetResponse((asyncResult) =>
            {
                HttpWebRequest request2 = (HttpWebRequest)asyncResult.AsyncState;

                // End the operation
                HttpWebResponse response = (HttpWebResponse)request2.EndGetResponse(asyncResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();

                // Compare to existing string

                // If there has been change, update that string and stream to disk and update the DataManager

                // Stream to Disk
                // Get the local folder.
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                if (local != null)
                {
                    // Get the file.
                    try
                    {
                        // Get the DataFolder folder.
                        var dataFolder = local.GetFolderAsync("DataFolder");

                        var file = await dataFolder.OpenStreamForReadAsync("cards.dat");
                    }
                    catch
                    {
                    }
                }

                // Update DataManager
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // Update code here DataManager.Instance....
                });


                // Close the stream object
                streamResponse.Close();
                streamRead.Close();

                // Release the HttpWebResponse
                response.Close();
            }, request);
        }
    }
}
