    <%@ Page Language="C#" MasterPageFile="Shared/Site.master" AutoEventWireup="true" CodeFile="GetPictures.aspx.cs" Inherits="Architecture" %>    

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="w3-content" style="height:70%">
    <asp:Repeater runat="server" ID="photoInfo">
        <ItemTemplate>
            <a href="#">
                <img src="<%# DataBinder.Eval(Container.DataItem, "photoFileLocation") %><%# DataBinder.Eval(Container.DataItem, "photoFileName") %>" 
                     alt="<%# DataBinder.Eval(Container.DataItem, "photoDescription") %>" 
                     title="<%# DataBinder.Eval(Container.DataItem, "photoDescription") %>" 
                     class="mySlides"
                     style="margin:auto; max-width:100%; max-height:100%"
                />
            </a>
        </ItemTemplate>
    </asp:Repeater>
    </div>
    <div class="w3-center" style="height:15%">
      <div class="w3-section">
        <button class="w3-btn" onclick="plusDivs(-1)">❮ Prev</button>
        <button class="w3-btn" onclick="plusDivs(1)">Next ❯</button>
      </div>
        <asp:Repeater runat="server" ID="photoInfo1">
            <ItemTemplate>
                <button class="w3-btn demo" onclick="currentDiv(1)">
                    <img src="<%# DataBinder.Eval(Container.DataItem, "photoFileLocation") %>thumbs/<%# DataBinder.Eval(Container.DataItem, "photoFileName") %>" 
                             alt="<%# DataBinder.Eval(Container.DataItem, "photoDescription") %>" 
                             title="<%# DataBinder.Eval(Container.DataItem, "photoDescription") %>" />
                </button>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>