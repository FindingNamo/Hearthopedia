using System;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Web.UI.Design;

namespace HearthopediaImageDownloader
{
	class Program
	{
		private static string c_cardsFileLocation = @"c:\Users\davidngo\Desktop\cards.txt";

		static void Main(string[] args)
		{
			// Get current folder
			var currentFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			// get cards
			var cards = DataAccess.GetCards(c_cardsFileLocation);

			// download each card image
			foreach (var card in cards)
			{
				var client = new WebClient();
				
				var uri = new Uri(card.imageURL);
				var filename  = System.IO.Path.GetFileName(uri.LocalPath);
				var outFilePath = String.Format("{0}{1}{2}",
					currentFolder,
					System.IO.Path.DirectorySeparatorChar,
					filename);

				try
				{
					Console.WriteLine("Getting {0}", card.imageURL);
					client.DownloadFileTaskAsync(card.imageURL, outFilePath).Wait();
				}
				catch (AggregateException e)
				{
					Console.WriteLine(e.InnerException.Message);
				}
			}

			Console.ReadKey();
		}
	}
}
