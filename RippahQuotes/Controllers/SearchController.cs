using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using RippahQuotes.Models;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;


namespace RippahQuotes.Controllers
{
    public class SearchController : Controller
    {
        private QuotesDb db = new QuotesDb();
        // GET: Search
        // Allows for quotes to be searched by what's contained in the text
        public ActionResult Index()
        {
            ViewBag.Searched = false;
            if (Request.QueryString["searchquery"] != null)
            {
                // Basic (probably inefficient) Regex Searching Algorithm
                ViewBag.Search = Request.QueryString["searchquery"];
                List<Quotes> searchResults = new List<Quotes>();
                ViewBag.Searched = true;
                string query = Request.QueryString["searchquery"];
                List<Quotes> quotes = db.Quotes.Include(q => q.Topic).ToList();
                foreach (var q in quotes)
                {
                    if (Regex.IsMatch(q.QuoteText, query, RegexOptions.IgnoreCase))
                    {
                        searchResults.Add(q);
                    }
                }
                return View(searchResults.ToList());
            }
            return View();
        }
    }
}