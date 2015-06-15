using System.Collections.Generic;
using System.IO;
using System.Net;
using HearthopediaImageDownloader.CardData;
using Newtonsoft.Json;

namespace HearthopediaImageDownloader
{
	public class DataAccess
	{
		public static List<Card> GetCards(string filepath)
		{
			List<Card> cards = new List<Card>();
			bool parsingMechanics = false;

			using (StreamReader reader = new StreamReader(filepath))
			{
				while (reader.Peek() >= 0)
				{
					var currentLine = reader.ReadLine();

					if (currentLine.Contains("g_hearthstone_mechanics"))
					{
						parsingMechanics = true;
						continue;
					}

					if (!parsingMechanics && currentLine.Contains("\"id\""))
					{
						var jsonString = currentLine.Substring(currentLine.IndexOf("{"),
							currentLine.IndexOf("}") - currentLine.IndexOf("{") + 1);
						var currentCard = GetCardFromJson(jsonString);
						cards.Add(currentCard);
					}

					if (parsingMechanics && currentLine.Contains("\"id\""))
					{
						// do nothing
					}
				}
			}

			return cards;
		}

		public static Card GetCardFromJson(string json)
		{
			Card card = JsonConvert.DeserializeObject<Card>(json);
			return card;
		}
	}
}
