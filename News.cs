using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Project
{
    public class News
    {
        public int NewsID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string PubDate { get; set; }
        public string ImageUrl { get; set; }

        public News(int newsID, string title, string description, string category, string author, string pubDate, string imageUrl)
        {
            this.NewsID = newsID;
            this.Title = title;
            this.Description = description;
            this.Category = category;
            this.Author = author;
            this.PubDate = pubDate;
            this.ImageUrl = imageUrl;
        }
    }
}