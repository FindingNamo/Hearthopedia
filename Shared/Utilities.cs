using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Hearthopedia.Filters;
using Windows.Storage;

namespace Hearthopedia
{
    class Utilities
    {
        public static Card GetCardFromJson(string json)
        {
            Card card = JsonConvert.DeserializeObject<Card>(json);
            return card;
        }

        public static List<CardTier> GetCardTierFromJson(string json)
        {
            List<CardTier> cardTierList = JsonConvert.DeserializeObject<List<CardTier>>(json);
            return cardTierList;
        }

        public static Mechanic GetMechanicFromJson(string json)
        {
            Mechanic mechanic = JsonConvert.DeserializeObject<Mechanic>(json);
            return mechanic;
        }

        public static string FilterHTML(string text)
        {
            string noHTML = Regex.Replace(text, @"<[^>]+>|&nbsp;", "").Trim();
            noHTML = noHTML.Replace(@"\", "");
            string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");
            return noHTMLNormalised;
        }

        public async static void CopyFromStreamToFile(Stream stream, string filePath)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile storageFile = await localFolder.CreateFileAsync(filePath, CreationCollisionOption.ReplaceExisting);
                using (Stream outputStream = await storageFile.OpenStreamForWriteAsync())
                {
                    await stream.CopyToAsync(outputStream);
                }
            }
            catch
            {
            }
        }
    }
}
