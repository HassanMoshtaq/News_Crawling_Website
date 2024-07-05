<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsDetail.aspx.cs" Inherits="Web_Project.NewsDetail" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>News Detail</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            background-image: url('images/news-background.jpg');
            background-size: cover;
            background-repeat: no-repeat;
            background-attachment: fixed;
            color: white; /* Adjust text color for readability */
        }
        .news-detail-container {
            margin-top: 40px;
            background: rgba(0, 0, 0, 0.7); /* Add a semi-transparent background for readability */
            padding: 20px;
            border-radius: 10px;
        }
        .news-title {
            font-size: 2rem;
            font-weight: bold;
        }
        .news-category, .news-author, .news-date {
            font-size: 1rem;
            color: #bbb; /* Slightly lighter for better contrast */
        }
        .news-description {
            font-size: 1.2rem;
            margin-top: 20px;
        }
        .news-image {
            width: 100%;
            max-width: 600px; /* Set maximum width for the image */
            height: auto;
            margin-top: 20px;
            display: block;
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container news-detail-container">
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblTitle" runat="server" CssClass="news-title" />
                </div>
                <div class="col-md-12">
                    <asp:Label ID="lblCategory" runat="server" CssClass="news-category" />
                </div>
                <div class="col-md-12">
                    <asp:Label ID="lblAuthor" runat="server" CssClass="news-author" />
                </div>
                <div class="col-md-12">
                    <asp:Label ID="lblPubDate" runat="server" CssClass="news-date" />
                </div>
                <div class="col-md-12">
                    <asp:Image ID="imgNews" runat="server" CssClass="news-image" AlternateText="News Image" />
                </div>
                <div class="col-md-12 news-description">
                    <asp:Label ID="lblDescription" runat="server" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>