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
    public partial class NewsDetail : System.Web.UI.Page
    {
        protected Label lblTitle;
        protected Label lblCategory;
        protected Label lblAuthor;
        protected Label lblPubDate;
        protected Label lblDescription;
        protected Image imgNews;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    LoadNewsDetail(int.Parse(id));
                }
            }
        }


        private void LoadNewsDetail(int newsID)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/news.mdb");

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM News WHERE NewsID = @NewsID";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewsID", newsID);

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTitle.Text = reader["Title"].ToString();
                            lblCategory.Text = reader["Category"].ToString();
                            lblAuthor.Text = "By " + reader["Author"].ToString();
                            lblPubDate.Text = DateTime.Parse(reader["PubDate"].ToString()).ToString("MMMM d, yyyy");
                            lblDescription.Text = reader["Description"].ToString();
                            imgNews.ImageUrl = reader["ImageUrl"].ToString();
                        }
                        else
                        {
                            lblTitle.Text = "No data found";
                        }
                    }
                }
            }
        }
    }
}