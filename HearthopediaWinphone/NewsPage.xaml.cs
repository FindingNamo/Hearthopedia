using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Hearthopedia
{
    public partial class NewsPage : PhoneApplicationPage
    {
        public NewsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string newsUrl = string.Empty;

            // Bind to selected card passed in via url
            if (NavigationContext.QueryString.TryGetValue("newsUrl", out newsUrl))
            {
                WebView.Navigate(new Uri(HttpUtility.UrlDecode(newsUrl)));
            }
        }
    }
}