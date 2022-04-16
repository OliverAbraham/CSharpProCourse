using System.Collections.Generic;

namespace AspNetCoreMvcApp.Models
{
	public class Einkaufszettel
	{
		public string NeuerArtikel { get; set; }

		public List<Artikel> AlleArtikel { get; set; }
	}
}
