<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RSSparsing.aspx.cs" Inherits="Web_Project.RSSparsing" %>



<!DOCTYPE html>
<html>
<head runat="server">
    <title>RSS Parsing</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <asp:GridView ID="gvNews" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="NewsID" HeaderText="ID" />
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="Author" HeaderText="Author" />
                    <asp:BoundField DataField="PubDate" HeaderText="Published Date" />
                    <asp:TemplateField HeaderText="Image">
                        <ItemTemplate>
                            <asp:Image ID="imgNews" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' Width="100px" Height="100px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>