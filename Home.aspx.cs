using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_Project
{
    public partial class Home_aspx : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string category = Request.QueryString["category"];
                DisplayNews(category);
            }
        }

        private void DisplayNews(string category)
        {
            ArrayList newsList = new ArrayList();
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/news.mdb");

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string query;

                if (string.IsNullOrEmpty(category))
                {
                    query = "SELECT * FROM News WHERE PubDate >= @StartOfDay AND PubDate < @EndOfDay";
                    phTitle.Visible = true; // Show the title for today's news
                }
                else if (category == "World")
                {
                    query = "SELECT * FROM News";
                    phTitle.Visible = false; // Hide the title for the World category
                }
                else if (category == "Technology")
                {
                    query = "SELECT * FROM News WHERE Category = 'Technology'";
                    phTitle.Visible = false; // Hide the title for the Technology category
                }
                else if (category == "Sports")
                {
                    query = "SELECT * FROM News WHERE Category = 'Sports'";
                    phTitle.Visible = false; // Hide the title for the Sports category
                }
                else
                {
                    query = "SELECT * FROM News WHERE Category = @Category";
                    phTitle.Visible = true; // Show the title for other categories
                }

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    if (string.IsNullOrEmpty(category))
                    {
                        DateTime startOfDay = DateTime.Today;
                        DateTime endOfDay = DateTime.Today.AddDays(1);

                        command.Parameters.AddWithValue("@StartOfDay", startOfDay);
                        command.Parameters.AddWithValue("@EndOfDay", endOfDay);
                    }
                    else if (category != "World" && category != "Technology" && category != "Sports")
                    {
                        command.Parameters.AddWithValue("@Category", category);
                    }

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int newsID = reader.GetInt32(reader.GetOrdinal("NewsID"));
                            string title = reader.GetString(reader.GetOrdinal("Title"));
                            string description = reader.GetString(reader.GetOrdinal("Description"));
                            string categoryValue = reader.GetString(reader.GetOrdinal("Category"));
                            string author = reader.GetString(reader.GetOrdinal("Author"));
                            string pubDate = reader.GetDateTime(reader.GetOrdinal("PubDate")).ToString("yyyy-MM-dd");
                            string imageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));

                            // Check if the news is already in the list to prevent duplicates
                            bool isDuplicate = newsList.Cast<News>().Any(n => n.Title == title);
                            if (!isDuplicate)
                            {
                                News news = new News(newsID, title, description, categoryValue, author, pubDate, imageUrl);
                                newsList.Add(news);
                            }
                        }
                    }
                }
            }

            if (newsList.Count == 0)
            {
                // Handle the case where the news list is not available
                NewsRepeater.DataSource = null;
                NewsRepeater.DataBind();
                return;
            }

            // Set the session
            Session["NewsList"] = newsList;

            NewsRepeater.DataSource = newsList;
            NewsRepeater.DataBind();
        }
    }
}
