<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main.aspx.vb" Inherits="Hacksaw.Main" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Hacksaw: A Log4Net Viewing Tool</title>
    <link href="RoundedStyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <link href="RoundedGrid.css" rel="stylesheet" type="text/css" />
    <link href="Grid.css" rel="stylesheet" type="text/css" />
</head>
<body>
   <form id="form1" runat="server">
   <ajaxToolkit:ToolkitScriptManager ID="tsmAjax" runat="server"></ajaxToolkit:ToolkitScriptManager>
   <div>

      <div class="rounded">
         <div class="top"><div class="right"></div></div>
         <div class="middle"><div class="right">
            <div class="content">

                  <p class="align-right">
                     Hacksaw: A Log4Net Viewing Tool<br />
                     <asp:Label ID="lblVersionInformation" CssClass="MiniText" runat="server"></asp:Label>
                  </p>

                  <div class="CenterContent">
                     
                     <asp:Panel ID="pnlLog" runat="server" DefaultButton="imbLoadLog">                                          
                        <asp:ImageButton ID="imbShowLogs" runat="server" CssClass="vertical-middle" AlternateText="Show Logs" ImageUrl="~/images/iconLog.png" ToolTip="Click to Show/Hide Log Sources." CausesValidation="false" />
                        <asp:TextBox ID="txtLogName" runat="server" CssClass="LogNameTextBox" Columns="100"></asp:TextBox>
                        <asp:ImageButton ID="imbLoadLog" runat="server" CssClass="vertical-middle" AlternateText="Load Log" ImageUrl="~/images/iconLoad.png" ToolTip="Load Log." />&nbsp;
                        <asp:ImageButton ID="imbManageLogs" runat="server" CssClass="vertical-middle" AlternateText="Manage Log Sources" ImageUrl="~/images/iconManageLogs.png" ToolTip="Manage Log Sources." CausesValidation="false" />
                        <ajaxToolkit:TextBoxWatermarkExtender ID="tweLogName" runat="server" TargetControlID="txtLogName" WatermarkCssClass="TextWatermark" WatermarkText="Click the icon to select a log or enter path..."></ajaxToolkit:TextBoxWatermarkExtender>                                         
                        <asp:RequiredFieldValidator ID="valLogNameRequired" runat="server" CssClass="vertical-middle" ControlToValidate="txtLogName" Display="Dynamic" ErrorMessage="Log Path Required" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="White"></asp:RequiredFieldValidator>
                     
                        <div style="position:relative">
                          
                           <asp:Panel ID="pnlLogSources" runat="server" CssClass="LogSourcePanel" Visible="false">
                              
                              <asp:Panel ID="pnlLogSourcesHeader" runat="server" CssClass="LogSourceHeader">
                                 Log Sources                              
                                 <asp:ImageButton ID="imbPanelClose" runat="server" CssClass="CloseImage" AlternateText="Hide Logs" ImageUrl="~/images/iconPanelClose.png" ToolTip="Click to Hide Log Sources." CausesValidation="false" />      
                                 <asp:ImageButton ID="imbPanelManageLogs" runat="server" CssClass="PanelManageLogsImage" AlternateText="Manage Log Sources" ImageUrl="~/images/iconPanelManageLogs.png" ToolTip="Manage Log Sources." CausesValidation="false" />
                              </asp:Panel>
                              
                              <asp:Panel  ID="pnlLogSourcesContent" runat="server" CssClass="LogSourceContent">
                                 <asp:TreeView ID="tvLogSources" runat="server" ExpandDepth="0" ImageSet="XPFileExplorer" NodeIndent="15">
                                    <SelectedNodeStyle Font-Bold="True" Font-Underline="False" 
                                       HorizontalPadding="0px" VerticalPadding="0px" />
                                    <ParentNodeStyle Font-Bold="False" />
                                    <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                                 </asp:TreeView>
                              </asp:Panel>                        
                           </asp:Panel>                     
                        </div>                     
                     </asp:Panel> 
                     
                     <div>
                        <asp:Panel ID="pnlSearch" runat="server" Visible="false">
                           <ajaxToolkit:Accordion ID="MyAccordion" runat="server" Width="100%" SelectedIndex="-1" HeaderCssClass="AccordionHeader" ContentCssClass="AccordionContent"
                                          FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                              <Panes>         
                                <ajaxToolkit:AccordionPane ID="acpBasicSearch" runat="server">
                                   <Header>
                                      --== Basic Search ==--
                                   </Header>
                                   <Content>
                                      <span class="MiniText">Criteria:</span>
                                      <asp:DropDownList ID="ddlBasicSearchColumn" runat="server" CssClass="MiniText">
                                         <asp:ListItem>ID</asp:ListItem>
                                         <asp:ListItem>Level</asp:ListItem>
                                         <asp:ListItem>Timestamp</asp:ListItem>
                                         <asp:ListItem>Logger</asp:ListItem>
                                         <asp:ListItem>Message</asp:ListItem>
                                         <asp:ListItem>Exception</asp:ListItem>
                                      </asp:DropDownList>&nbsp;
                                      <asp:DropDownList ID="ddlBasicSearchOperator" runat="server" CssClass="MiniText">
                                         <asp:ListItem>=</asp:ListItem>
                                         <asp:ListItem>&lt;&gt;</asp:ListItem>
                                         <asp:ListItem>&gt;</asp:ListItem>
                                         <asp:ListItem>&gt;=</asp:ListItem>
                                         <asp:ListItem>&lt;</asp:ListItem>
                                         <asp:ListItem>&lt;=</asp:ListItem>
                                      </asp:DropDownList>
                                      <asp:TextBox ID="txtBasicSearchCriteria" runat="server" Columns="30" CssClass="MiniText" MaxLength="100"></asp:TextBox>
                                      <asp:Button ID="btnStartBasicSearch" runat="server" CssClass="MiniText" Text="Search" />
                                      <asp:Button ID="btnClearBasicSearch" runat="server" CssClass="MiniText" Text="Clear" />
                                   </Content>
                                   </ajaxToolkit:AccordionPane>
                             
                                   <ajaxToolkit:AccordionPane ID="acpKeywordSearch" runat="server">
                                      <Header>
                                         --== Keyword Search ==--
                                      </Header>
                                      <Content>
                                         <span class="MiniText">Column:</span>
                                         <asp:DropDownList ID="ddlKeywordSearchColumn" runat="server" CssClass="MiniText">
                                            <asp:ListItem>Level</asp:ListItem>
                                            <asp:ListItem>Logger</asp:ListItem>
                                            <asp:ListItem>Message</asp:ListItem>
                                            <asp:ListItem>Exception</asp:ListItem>
                                         </asp:DropDownList>&nbsp;
                                         <span class="MiniText">Keyword(s):</span>
                                         <asp:TextBox ID="txtKeywordSearchCriteria" runat="server" Columns="30" CssClass="MiniText" MaxLength="100"></asp:TextBox>
                                         <asp:Button ID="btnStartKeywordSearch" runat="server" CssClass="MiniText" Text="Search" />
                                         <asp:Button ID="btnClearKeywordSearch" runat="server" CssClass="MiniText" Text="Clear" /><br />
                                         <span class="MiniText">Use a comma (,) to seperate multiple keywords.</span>
                                      </Content>
                                   </ajaxToolkit:AccordionPane>
                             
                                   <ajaxToolkit:AccordionPane ID="acpAdvancedSearch" runat="server">
                                      <Header>
                                         --== Advanced Search ==--
                                      </Header>
                                      <Content>
                                         Filter:<asp:TextBox ID="txtGridFilter" runat="server" Columns="75" CssClass="MiniText" MaxLength="300"></asp:TextBox>
                                         <asp:Button ID="btnStartAdvancedSearch" runat="server" CssClass="MiniText" Text="Apply" />
                                         <asp:Button ID="btnClearAdvancedSearch" runat="server" CssClass="MiniText" Text="Clear" /><br />
                                         <span class="MiniText">Use SQL style criteria such as: Level = 'INFO' AND Timestamp < '1/25/2007 9:16:30 AM'</span>
                                       </Content>
                                   </ajaxToolkit:AccordionPane>
                              </Panes>
                           </ajaxToolkit:Accordion>
                        </asp:Panel>
                     </div>
                  </div>                              
            </div>
         </div></div>
         <div class="bottom"><div class="right"></div></div>
      </div>

      <asp:UpdatePanel ID="updEvents" runat="server">
                        
         <ContentTemplate>
                       
            <div class="grid">
               <div class="roundedgrid">
                  <div class="top-outer"><div class="top-inner">
                  <div class="top">
                     
                     <asp:Panel ID="pnlGridOptions" runat="server" Visible="False">
                        
                        <div class="float-left">
                           <h2>
                              <asp:CheckBox ID="chkUsePaging" runat="server" CssClass="MiniText" Text="Use Paging Grid" TextAlign="Left" />&nbsp;
                              <span class="MiniText">Events per Page to display:</span>
                              <asp:TextBox ID="txtEventsPerPage" runat="server" CssClass="MiniText" Columns="4"></asp:TextBox>
                              <asp:Button ID="btnApplyOptions" runat="server" CssClass="MiniText" Text="Apply" />
                           </h2>
                        </div>
                        
                        <div class="float-right">
                           <h2>
                              <asp:Timer ID="timUpdate" runat="server"></asp:Timer>
                              <span class="MiniText"><b>Refresh Every:&nbsp;</b></span>

                              <asp:RadioButtonList ID="rblRefreshRate" runat="server" CssClass="MiniText" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                 <asp:ListItem Value="15">15 Seconds</asp:ListItem>
                                 <asp:ListItem Value="30">30 Seconds</asp:ListItem>
                                 <asp:ListItem Value="60">60 Seconds</asp:ListItem>
                                 <asp:ListItem Value="0">Manual</asp:ListItem>
                              </asp:RadioButtonList>
               
                              <asp:Button ID="btnRefreshGrid" runat="server" Text="Refresh" CssClass="MiniText" />
                              <asp:Label ID="lblLastRefresh" runat="server" CssClass="RefreshLabel" ToolTip="Date/Time log data was last refreshed."></asp:Label>
                           </h2>
                           
                        </div>
                     </asp:Panel>                              
                     
                  </div></div></div> 
                  <div class="mid-outer"><div class="mid-inner"><div class="mid">
                     <div class="content">
                        <asp:GridView ID="gvEvents" CssClass="datatable" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" GridLines="None" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False">
                           <PagerStyle CssClass="pager-row" />
                           <RowStyle CssClass="row" />
                           <PagerSettings Mode="NumericFirstLast" PageButtonCount="7" FirstPageText="«" LastPageText="»" />
                           <Columns>
                              <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" HeaderStyle-CssClass="first" ItemStyle-CssClass="first">
                                 <ItemStyle VerticalAlign="Top" Width="40px" HorizontalAlign="Center" />
                              </asp:BoundField>
                              <asp:BoundField DataField="Level" HeaderText="Level" SortExpression="Level">
                                 <ItemStyle VerticalAlign="Top" Width="30px" HorizontalAlign="Center" />
                              </asp:BoundField>
                              <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" SortExpression="Timestamp">
                                 <ItemStyle VerticalAlign="Top" Width="125px" HorizontalAlign="Center" />
                              </asp:BoundField>
                              <asp:BoundField DataField="Logger" HeaderText="Logger" SortExpression="Logger">
                                 <ItemStyle VerticalAlign="Top" Width="150px" HorizontalAlign="Center" />
                              </asp:BoundField>
                              <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message">
                                 <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
                              </asp:BoundField>
                              <asp:BoundField DataField="Exception" HeaderText="Exception" SortExpression="Exception">
                                 <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                              </asp:BoundField>
                           </Columns>
                           <PagerSettings Position="TopAndBottom" />
                        </asp:GridView>
                     </div>
                  </div></div></div>
                  <div class="bottom-outer"><div class="bottom-inner"><div class="bottom"></div></div></div>
               </div>
            </div>

         </ContentTemplate>      
      </asp:UpdatePanel>

      <asp:Label ID="lblNoEntries" runat="server" Font-Bold="True" ForeColor="Blue" Text="<br/>No log entries found in file specified." Visible="False"></asp:Label><asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>               
   </div>
   </form>
</body>
</html>