using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RippahQuotes.Models
{
    //Quote Model//
    public class Quotes
    {
        [Key]
        public int QuoteId { get; set; }
        public int TopicId { get; set; }
        [Display(Name = "Quote")]
        [Required]
        [MaxLength(300)]
        public string QuoteText { get; set; }
        [Display(Name = "Author")]
        [MaxLength(250)]
        [Required]
        public string QuoteAuthor { get; set; }
        [Display(Name = "Topic")]
        public Topic Topic { get; set; }
        [Display(Name = "Delete Password")]
        [MaxLength(20)]
        [ScriptIgnore]
        public string QuotePassword { get; set; }
        [Display(Name = "Quote Effect")]
        public string QuoteEffect { get; set; }
        [Display(Name = "Quote Ripperness")]
        public int QuoteRating { get; set; }
    }
    //Topic Model//
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        [Required]
        [Display(Name = "Topic")]
        [StringLength(150)]
        public string TopicName { get; set; }
        [Display(Name = "Description")]
        [StringLength(200)]
        public string TopicDescription { get; set; }
        [Display(Name="Delete Password")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        //[ScriptIgnore]
        public string TopicPassword { get; set; }
        [Display(Name="Topic Amount")]
        public int TopicAmount { get; set; }
    }
    // Vote Model
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }
        [Required]
        public virtual int QuoteId { get; set; }
        [Required]
        public string IP { get; set; }
        [Required]
        public Quotes Quote { get; set; }
    }
    
    public class QuotesDbInit : System.Data.Entity.DropCreateDatabaseAlways<QuotesDb>
    {
        protected override void Seed(QuotesDb context)
        {
            context.Topics.Add(new Topic
            {
                TopicName = "Memes"
            });
            context.Topics.Add(new Topic
            {
                TopicName = "Out of Context"
            });
            context.Topics.Add(new Topic
            {
                TopicName = "Embarassing Stories"
            });
            context.Topics.Add(new Topic
            {
                TopicName = "Possibly Offensive"
            });
            context.Topics.Add(new Topic
            {
                TopicName = "Jarrod 2.0"
            });
            context.Topics.Add(new Topic
            {
                TopicName = "Terrible Quotes"
            });
            context.Topics.Add(new Topic
            {
                TopicName = "Roasts"
            });
            context.Topics.Add(new Topic {
                TopicName = "Weed"
            });
            base.Seed(context);
        }

    }
}