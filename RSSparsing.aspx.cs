using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using NLog;

namespace Web_Project
{
    public partial class RSSparsing : System.Web.UI.Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ParseRSSFeed();
            }


        }
        private void ParseRSSFeed()
        {
            // Set the security protocol to TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string[] rssUrls = {
                "https://rss.nytimes.com/services/xml/rss/nyt/World.xml", // World news RSS feed
                "http://feeds.bbci.co.uk/sport/rss.xml",
                "https://feeds.bbci.co.uk/news/technology/rss.xml",
            };

            ArrayList newsList = new ArrayList();

            foreach (string rssUrl in rssUrls)
            {
                try
                {
                    WebClient webClient = new WebClient();
                    string rssData = webClient.DownloadString(rssUrl);

                    // Use XmlReader to handle potentially problematic characters
                    XmlReaderSettings settings = new XmlReaderSettings
                    {
                        CheckCharacters = false // This allows invalid XML characters to be handled
                    };

                    using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(rssData), settings))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(reader);

                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
                        nsmgr.AddNamespace("media", "http://search.yahoo.com/mrss/");

                        XmlNodeList xmlNodeList = xmlDocument.SelectNodes("rss/channel/item");

                        foreach (XmlNode xmlNode in xmlNodeList)
                        {
                            string title = xmlNode["title"]?.InnerText ?? "No title";
                            string description = xmlNode["description"]?.InnerText ?? "No description";
                            string category = "General";
                            if (rssUrl.Contains("espn.com"))
                            {
                                category = "Sports";
                            }
                            else if (rssUrl.Contains("nyt/World"))
                            {
                                category = "World";
                            }
                            else if (rssUrl.Contains("techcrunch.com/startups"))
                            {
                                category = "Technology";
                            }

                            string author = xmlNode["author"]?.InnerText ?? "Unknown";
                            string pubDate = xmlNode["pubDate"]?.InnerText ?? "No date";

                            // Check multiple possible locations for the image URL
                            string imageUrl = xmlNode.SelectSingleNode("media:thumbnail/@url", nsmgr)?.Value
                                              ?? xmlNode.SelectSingleNode("media:content/@url", nsmgr)?.Value
                                              ?? xmlNode.SelectSingleNode("enclosure/@url")?.Value
                                              ?? "";

                            // Parse the publication date
                            DateTime parsedPubDate = ParsePubDate(pubDate);

                            // Check if the news is already in the list to prevent duplicates
                            bool isDuplicate = newsList.Cast<News>().Any(n => n.Title == title);
                            if (!isDuplicate)
                            {
                                int newsID = newsList.Count + 1;
                                News news = new News(newsID, title, description, category, author, parsedPubDate.ToString("yyyy-MM-dd"), imageUrl);
                                newsList.Add(news);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    logger.Error(ex, $"Failed to parse RSS feed {rssUrl}");
                }
            }

            SaveNewsToDatabase(newsList);
            Session["NewsList"] = newsList;
            gvNews.DataSource = newsList;
            gvNews.DataBind();
        }

        private DateTime ParsePubDate(string pubDate)
        {
            // Define possible date formats
            string[] formats = { "ddd, dd MMM yyyy HH:mm:ss zzz", "ddd, dd MMM yyyy HH:mm:ss GMT", "ddd, dd MMM yyyy HH:mm:ss Z", "ddd, dd MMM yyyy HH:mm:ss +0000", "R" };
            DateTime parsedDate;

            // Try parsing the date with each format
            if (DateTime.TryParseExact(pubDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                // Handle parsing failure (use current date as fallback)
                return DateTime.Now;
            }
        }

        private void SaveNewsToDatabase(ArrayList newsList)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/news.mdb");

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                foreach (News news in newsList)
                {
                    // Check if the news already exists in the database
                    string checkQuery = "SELECT COUNT(*) FROM News WHERE Title = @Title";
                    using (OleDbCommand checkCommand = new OleDbCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Title", news.Title);
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count == 0)
                        {
                            // Insert the news if it doesn't exist
                            string insertQuery = "INSERT INTO News (Title, Description, Category, Author, PubDate, ImageUrl) " +
                                                 "VALUES (@Title, @Description, @Category, @Author, @PubDate, @ImageUrl)";

                            using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Title", news.Title);
                                insertCommand.Parameters.AddWithValue("@Description", news.Description);
                                insertCommand.Parameters.AddWithValue("@Category", news.Category);
                                insertCommand.Parameters.AddWithValue("@Author", news.Author);
                                insertCommand.Parameters.AddWithValue("@PubDate", DateTime.Parse(news.PubDate));
                                insertCommand.Parameters.AddWithValue("@ImageUrl", news.ImageUrl);

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
    }

    

    