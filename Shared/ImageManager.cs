using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Windows.Storage;
using System.Windows.Media.Imaging;
using System.IO;

#if NETFX_CORE
#else
using Microsoft.Phone.BackgroundTransfer;
using System.Net;
using System.IO.IsolatedStorage;
#endif


namespace Hearthopedia
{
    class ImageManager
    {

        #region Constructor

        private ImageManager()
        {
        }

        #endregion

        #region Singleton Accessor

        private static ImageManager imageManager;

        public static ImageManager Instance
        {
            get
            {
                return imageManager ?? (imageManager = new ImageManager());
            }
        }

        #endregion

        #region Helpers

        public async void SetImageFromCard(Card card, Image cardImage)
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
                Uri imageUri = new Uri(card.backupURI, UriKind.RelativeOrAbsolute);
                BitmapImage image = new BitmapImage(imageUri);
                cardImage.Source = image;
            }

            // try to download to have an updated local copy
            Uri uri = new Uri(card.imageURL);
#if NETFX_CORE
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

#else
            WebClient webClient = new WebClient();
            webClient.OpenReadCompleted += (sender, e) =>
                {
                    // if we don't have web we need to catch the exception
                    try
                    {

                        Utilities.CopyFromStreamToFile(e.Result, card.localFilename);
                    }
                    catch
                    {
                    }
                };
            webClient.OpenReadAsync(uri);
#endif
                
        }

        #endregion
    }
}