using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Hearthopedia.NewsFeed
{
    public class NewsItem
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string PostUrl { get; set; }
    }

    public class NewsManager
    {
        private const string HearthHeadNewsRSSUrl = "http://www.hearthhead.com/news&rss";

        public ObservableCollection<NewsItem> NewsItems = new ObservableCollection<NewsItem>();

        #region Singleton
        private static NewsManager _instance;
        public static NewsManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NewsManager();

                return _instance;
            }
        }
        private NewsManager()
        {
            NewsItems = new ObservableCollection<NewsItem>();
        }
        #endregion
        
        public async void GetNews()
        {
            try
            {
                HttpClient client = new HttpClient();
                string rssFeed = await client.GetStringAsync(HearthHeadNewsRSSUrl);

                var newsItems = XElement
                    .Parse(rssFeed)          // Parse the RSS feed.
                    .Descendants("item")     // for each 'item' element.
                    .Select(item =>          // create one of these.
                    {
                        string desc = item.Element("description").Value;
                        desc = Regex.Replace(desc, "<br ?/>", "\n");
                        desc = Regex.Replace(desc, @"<(.|\n)*?>", string.Empty);
                        desc = HttpUtility.HtmlDecode(desc);

                        return new NewsItem
                        {
                            Title = item.Element("title").Value,
                            Date = item.Element("pubDate").Value,
                            Description = desc,
                            PostUrl = item.Element("link").Value
                        };
                    });

                // Populate our news items.
                foreach (var item in newsItems)
                {
                    NewsItems.Add(item);
                }
            }
            catch
            {
                // Something went wrong.
                NewsItems.Clear();
            }
        }
    }
}
