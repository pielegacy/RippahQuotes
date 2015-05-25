using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RippahQuotes.Models;

namespace RippahQuotes.Controllers
{
    public class QuotesApiController : ApiController
    {
        private QuotesDb db = new QuotesDb();

        // GET: api/QuotesApi
        public IQueryable<Quotes> GetQuotes()
        {
            //Includes topics as well//

            return db.Quotes.Include(q => q.Topic.TopicName);
        }

        // GET: api/QuotesApi/5
        [ResponseType(typeof(Quotes))]
        public IHttpActionResult GetQuotes(int id)
        {
            Quotes quotes = db.Quotes.Find(id);
            if (quotes == null)
            {
                return NotFound();
            }

            return Ok(quotes);
        }
        
        // PUT: api/QuotesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuotes(int id, Quotes quotes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quotes.QuoteId)
            {
                return BadRequest();
            }

            db.Entry(quotes).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuotesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/QuotesApi
        [ResponseType(typeof(Quotes))]
        public IHttpActionResult PostQuotes(Quotes quotes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quotes.Add(quotes);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = quotes.QuoteId }, quotes);
        }

        // DELETE: api/QuotesApi/5
        [ResponseType(typeof(Quotes))]
        public IHttpActionResult DeleteQuotes(int id)
        {
            Quotes quotes = db.Quotes.Find(id);
            if (quotes == null)
            {
                return NotFound();
            }

            db.Quotes.Remove(quotes);
            db.SaveChanges();

            return Ok(quotes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        private bool QuotesExists(int id)
        {
            return db.Quotes.Count(e => e.QuoteId == id) > 0;
        }
    }
}