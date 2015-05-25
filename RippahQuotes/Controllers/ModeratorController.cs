using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RippahQuotes.Models;

namespace RippahQuotes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModeratorController : Controller
    {
        private QuotesDb db = new QuotesDb();
        // GET: Moderator
        public ActionResult Index()
        {
            return View();
        }
    }
}