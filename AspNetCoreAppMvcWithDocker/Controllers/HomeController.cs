using AspNetCoreAppMvcWithDocker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AspNetCoreAppMvcWithDocker.Controllers
{
    public class HomeController : Controller
    {
		public static List<Artikel> Artikelliste = new List<Artikel>();

        public HomeController(ILogger<HomeController> logger)
        {
        }

        public IActionResult Index()
        {
			return View(Artikelliste);
        }

        public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult FuegeArtikelZurListeHinzu(Einkaufszettel zettel)
		{
			Artikelliste.Add(new Artikel() { ID = Artikelliste.Count+1, Bezeichnung = zettel.NeuerArtikel });
			return Redirect("Index");
		}

		public IActionResult Delete(Artikel artikel)
		{
			Artikelliste = Artikelliste.Where(x => x.ID != artikel.ID).ToList();
			return Redirect("/Home/Index");
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
