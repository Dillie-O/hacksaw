<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ManageLogSources.aspx.vb" Inherits="Hacksaw.ManageLogSources" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Hacksaw: Manage Log Sources</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <link href="RoundedStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
   <form id="form1" runat="server">
   <div>
      <div class="rounded">
         <div class="top"><div class="right"></div></div>
         <div class="middle"><div class="right">
            <div class="content">

                  <p class="align-right">
                     Hacksaw: A Log4Net Viewing Tool<br />
                     Manage Log Sources
                  </p>
                  
            </div>
         </div></div>
         <div class="bottom"><div class="right"></div></div>
      </div>       
   </div>
   
   <div style="width: 95%">
   
      <div class="align-center">
         <asp:HyperLink ID="hypMain" CssClass="MainLink" NavigateUrl="~/Main.aspx" runat="server">Return to Main Page</asp:HyperLink>
         <br /><br />
      </div>
      
      <div style="float:left; text-align:left">
         <asp:TreeView ID="tvLogSources" runat="server" ImageSet="XPFileExplorer" NodeIndent="15">
            <SelectedNodeStyle Font-Bold="True" Font-Underline="False" HorizontalPadding="0px" VerticalPadding="0px" />
            <ParentNodeStyle Font-Bold="False" />
            <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
            <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
         </asp:TreeView>
         <div style="text-align: center">
            <br />
            <asp:Button ID="btnDeleteNode" runat="server" CssClass="MiniText" Text="Delete Log/Category" /><br />
            <asp:Button ID="btnMoveNodeUp" runat="server" CssClass="MiniText" Text="Move Log/Category Up" /><br />
            <asp:Button ID="btnMoveNodeDown" runat="server" CssClass="MiniText" Text="Move Log/Category Down" /><br />
            <br />
            <asp:Button ID="btnSaveTree" runat="server" CssClass="MiniText" Text="Save Log Sources" /><br />
            <asp:Button ID="btnReloadTree" runat="server" CssClass="MiniText" Text="Reload Log Sources" />
         </div>         
      </div>
      
      <div style="float:left; margin-left: 30px;" class="align-left">
         <b>Selected Node:</b><br />
         <span class="MiniText">Name: </span><asp:TextBox ID="txtSelectedNodeName" 
            runat="server" CssClass="MiniText" Columns="60" MaxLength="60"></asp:TextBox>
         <br />
         <span class="MiniText">Path (Log Only): </span>
         <asp:TextBox ID="txtSelectedNodePath" runat="server" CssClass="MiniText" 
            Columns="60" MaxLength="100"></asp:TextBox>
         <br />
         <span class="MiniText">Icon URL (Optional): </span>
         <asp:TextBox ID="txtSelectedNodeIcon" runat="server" Columns="60" 
            CssClass="MiniText" MaxLength="100"></asp:TextBox>
         <br />
         <div class="align-center">
            <asp:Button ID="btnUpdateNode" runat="server" CssClass="MiniText" Text="Update" />
         </div>
         <br />
         <b>Add Log:</b><br />
         <span class="MiniText">Name: </span>
         <asp:TextBox ID="txtNewLogName" runat="server" CssClass="MiniText" Columns="60" 
            MaxLength="30"></asp:TextBox>
         <br />
         <span class="MiniText">Path: </span>
         <asp:TextBox ID="txtNewLogPath" runat="server" CssClass="MiniText" Columns="60" 
            MaxLength="100"></asp:TextBox>
         <br />
         <span class="MiniText">Icon URL (Optional): </span>
         <asp:TextBox ID="txtNewLogIcon" runat="server" Columns="60" CssClass="MiniText" 
            MaxLength="100"></asp:TextBox><br />
         <div class="align-center">
            <asp:Button ID="btnAddLogNode" runat="server" CssClass="MiniText" Text="Add" />
         </div>         
         <br/>
         <b>Add Category:</b><br />
         <span class="MiniText">Name: </span>
         <asp:TextBox ID="txtNewCategoryName" runat="server" CssClass="MiniText" 
            Columns="60" MaxLength="30"></asp:TextBox>
         <br />
         <span class="MiniText">Icon URL (Optional): </span>
         <asp:TextBox ID="txtNewCategoryIcon" runat="server" Columns="60" 
            CssClass="MiniText" MaxLength="100"></asp:TextBox>
         <br />
         <div class="align-center">
            <asp:Button ID="btnAddLogCategory" runat="server" CssClass="MiniText" Text="Add" />
         </div>         
      </div>
      
      <div class="align-center" style="clear:both">
         <br />
         <asp:Label ID="lblStatus" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
         <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label><br />         
      </div>
     
   </div>
   
   </form>
</body>
</html>