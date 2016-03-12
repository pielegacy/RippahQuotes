using System;
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

namespace RippahQuotes.Controllers
{
    public class QuotesController : Controller
    {
        private QuotesDb db = new QuotesDb();
        string[] triggerwords = { "delete", "cucked", "xD" };
        bool isPremium = true;
        // GET: Quotes
        public ActionResult Index(int? p)
        {
            if (p == null || p == 0)
            {
                p = 1;
            }
            if (Request.Cookies["Informed"] == null)
            {
                ViewBag.FirstTime = true;
                HttpCookie informed = new HttpCookie("Informed");
                Response.SetCookie(informed);
            }
            ViewBag.page = p;
            var quotes = db.Quotes.Include(q => q.Topic);
            return View(quotes.OrderByDescending(q => q.QuoteId).ToList());
        }

        public ActionResult Topics(int? id)
        {
            var quotes = db.Quotes.Include(q => q.Topic);
            ViewBag.TopicName = db.Topics.Find(id).TopicName;
            ViewBag.TopicId = id;
            if (db.Topics.Find(id).TopicPassword != null)
            {
                ViewBag.PasswordExists = true;
            }
            else
            {
                ViewBag.PasswordExists = false;
            }
            List<Quotes> quotes_filtered = new List<Quotes>();
            var quotes_f = quotes.Where(q => q.TopicId == id);
            quotes_filtered = quotes_f.ToList();
            /*foreach (var fq in quotes)
            {
                if (fq.Topic.TopicId == id)
                {
                    quotes_filtered.Add(fq);
                }
            }*/
            return View(quotes_filtered);
        }
        // GET: Quotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotes quotes = db.Quotes.Find(id);
            if (quotes == null)
            {
                return HttpNotFound();
            }
            return View(quotes);
        }
        // Reset Shit Now
        //public ActionResult SetVotes()
        //{
        //    foreach (var q in db.Quotes)
        //    {
        //        q.QuoteRating = 0;
        //    }
        //    return RedirectToAction("Index");
        //}
        // GET: Quotes/Create
        public ActionResult Create()
        {
            if (TempData["Worked"] != null)
            {
                ViewBag.Worked = TempData["Worked"].ToString();
            }
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName");
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuoteId,TopicId,QuoteText,QuoteAuthor,QuotePassword")] Quotes quotes)
        {
            var verification = Request.Form["verify"];
            List<string> QuoteSplit = quotes.QuoteText.Split(' ').ToList();
            //Lower Case Quote Effects//
            switch (QuoteSplit[0]){
                case ":marquee":
                    quotes.QuoteEffect = "marquee";
                    QuoteSplit.RemoveAt(0);
                    break;
                case ":colour":
                case ":color":
                        quotes.QuoteEffect = "color " + QuoteSplit[1].ToString();
                        QuoteSplit.RemoveAt(0);
                        QuoteSplit.RemoveAt(0);
                    break;
                default:
                    quotes.QuoteEffect = null;
                    break;
            }
            quotes.QuoteText = String.Join<string>(" ", QuoteSplit);
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (string.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ModelState.AddModelError("", "Shit's fried mate");
            }
            RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                ModelState.AddModelError("", "Incorrect captcha answer.");
            }

            //if (ModelState.IsValid && QuoteSplit.Count > 0)
            if (QuoteSplit.Count > 0 && verification == "on")
            {
                //Init Topic Count//
                /*
                Dictionary<int, int> top_topics = new Dictionary<int, int>();
                foreach (var t in db.Topics)
                {
                    top_topics.Add(t.TopicId, 0);
                }
                foreach (var q in db.Quotes)
                {
                    top_topics[q.TopicId] += 1;
                    q.Topic.TopicAmount = top_topics[q.TopicId];
                }
                 * */
                foreach (var t in db.Topics)
                {
                    if (t.TopicId == quotes.TopicId)
                    {
                        t.TopicAmount += 1;
                    }
                }
                quotes.QuoteRating = 0;
                db.Quotes.Add(quotes);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = quotes.QuoteId });
            }
            else
            {
                TempData["Worked"] = "False";
                return RedirectToAction("Create");
            }

            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", quotes.TopicId);
            return View("Details", quotes);
        }
        public ActionResult AddVote(int id)
        {
            string ip = Request.UserHostAddress;
            Quotes quote = db.Quotes.Find(id);
            IEnumerable<Vote> votecheck = from v in db.Votes where v.IP == ip && v.QuoteId == id select v;
            if (votecheck.Count() == 0)
            {
                Vote newvote = new Vote()
                {
                    IP = ip,
                    QuoteId = id,
                    Quote = db.Quotes.Find(id)
                };
                db.Votes.Add(newvote);
                quote.QuoteRating += 1;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Marquee(int id)
        {
            Quotes quote = db.Quotes.Find(id);
            return View("Marquee", quote);
        }
        // GET: Quotes/Edit/5
        /*public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotes quotes = db.Quotes.Find(id);
            if (quotes == null)
            {
                return HttpNotFound();
            }
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", quotes.TopicId);
            return View(quotes);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuoteId,TopicId,QuoteText,QuoteAuthor")] Quotes quotes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quotes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", quotes.TopicId);
            return View(quotes);
        }*/
        public ActionResult CreateByTopic(int? id)
        {
            ViewBag.Topic = db.Topics.Find(id).TopicName;
            return View();
        }
        [HttpPost]
        public ActionResult CreateByTopic(int id,[Bind(Include = "QuoteId,TopicId,QuoteText,QuoteAuthor,QuotePassword")] Quotes quotes)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (string.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ModelState.AddModelError("", "Shit's fried mate");
            }
            RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                ModelState.AddModelError("", "Incorrect captcha answer.");
            }
            quotes.TopicId = id;
            if (ModelState.IsValid)
            {
                db.Quotes.Add(quotes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "TopicName", quotes.TopicId);
            return View(quotes);
        }
        // GET: Quotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotes quotes = db.Quotes.Find(id);
            if (quotes == null)
            {
                return HttpNotFound();
            }
            return View(quotes);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, FormCollection formcollection)
        {
            Quotes quotes = db.Quotes.Find(id);
            if (quotes.QuotePassword != null && quotes.QuotePassword == formcollection["checker"] || User.Identity.IsAuthenticated)
            {
                db.Quotes.Remove(quotes);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
