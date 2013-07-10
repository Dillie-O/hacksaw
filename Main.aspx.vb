Imports Hacksaw.HackSawDAL
Imports Hacksaw.HacksawTables

Partial Public Class Main
   Inherits System.Web.UI.Page

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      Dim astrParsedExtensions As String()
      Dim alParsedExtensions As New ArrayList

      ' Set GridView options the first time the page is loaded from default session values.
      If Not Page.IsPostBack Then

         ' Initialize variables used to change sorting, grid pages, etc. during the log view.
         ViewState("LogName") = "None Specified"
         ViewState("LogFilePath") = ""
         ViewState("SortColumn") = "ID"
         ViewState("SortDirection") = "ASC"
         ViewState("EventsPerPage") = 25
         ViewState("UsePaging") = True
         ViewState("GridFilter") = ""
         ViewState("LogExtensions") = New ArrayList()

         ' Refresh rate for grid is stored in seconds. (0 = Manual Refresh)
         ViewState("GridRefresh") = 0

         ' Retrieve list of valid log extensions from web.config file and load into ArrayList
         astrParsedExtensions = ConfigurationManager.AppSettings("LogExtensions").ToString.Split(Char.Parse(","))

         For i As Integer = 0 To astrParsedExtensions.Length - 1
            alParsedExtensions.Add(astrParsedExtensions(i))
         Next

         ViewState("LogExtensions") = alParsedExtensions

         ' Set other labels and refresh options
         lblVersionInformation.Text = Application("Version").ToString

         If Boolean.Parse(ViewState("UsePaging").ToString) = True Then
            chkUsePaging.Checked = True
         Else
            gvEvents.AllowPaging = False
            chkUsePaging.Checked = False
         End If

         rblRefreshRate.Items.FindByValue(ViewState("GridRefresh").ToString).Selected = True

         If CType(ViewState("GridRefresh"), Integer) > 0 Then

            timUpdate.Enabled = True
            ' Refresh rate is counted in milliseconds
            timUpdate.Interval = CType(ViewState("GridRefresh"), Integer) * 1000

         Else

            timUpdate.Enabled = False

         End If

         gvEvents.PageSize = Integer.Parse(ViewState("EventsPerPage").ToString)
         txtEventsPerPage.Text = ViewState("EventsPerPage").ToString

         lblNoEntries.Visible = False
         lblError.Visible = False

         BindLogSources()

      End If

   End Sub

   ''' <summary>
   '''    Displays the Log Sources box.
   ''' </summary>
   ''' <param name="sender">Show Logs ImageButton Click Event.</param>
   ''' <param name="e">ImageButton Click Event Arguements.</param>
   ''' <remarks></remarks>
   ''' <history>
   '''    Sean Patterson   4/24/2008   [Created]
   ''' </history>
   Protected Sub ShowLogSourcesPanel(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) _
                Handles imbShowLogs.Click

      If pnlLogSources.Visible = False Then
         pnlLogSources.Visible = True
      Else
         pnlLogSources.Visible = False
      End If

   End Sub

   ''' <summary>
   '''    Hides the Log Sources box.
   ''' </summary>
   ''' <param name="sender">Close Panel ImageButton Click Event.</param>
   ''' <param name="e">ImageButton Button Click Event Arguments.</param>
   ''' <remarks></remarks>
   ''' <history>
   '''    Sean Patterson   4/24/2008   [Created]
   ''' </history>
   Protected Sub CloseSourcesPanel(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) _
                Handles imbPanelClose.Click

      pnlLogSources.Visible = False

   End Sub

   ''' <summary>
   '''    Retrieves log sources and binds to the TreeView for selection.
   ''' </summary>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	01/18/2007	Created
   ''' </history>
   Private Sub BindLogSources()

      HackSawDAL.LoadLogTree(tvLogSources)

   End Sub

   ''' <summary>
   '''    Loads log file stored in ViewState variable and binds to GridView for display. Display options stored in 
   '''    ViewState are applied to GridView.
   ''' </summary>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	01/18/2007	Created
   ''' </history>
   Private Sub BindLogFile()

      Dim dtLogEntries As LogEventDataTable
      Dim dvLogEntries As DataView

      Try

         ' Retrieve the source file that has been loaded.
         dtLogEntries = HackSawDAL.LoadLog(ViewState("LogFilePath").ToString)

         If dtLogEntries.Rows.Count > 0 Then

            txtLogName.Text = ViewState("LogFilePath").ToString

            ' Create a view of the log based on the sort parameters and row filter specified in the ViewState.
            dvLogEntries = New DataView(dtLogEntries)
            dvLogEntries.Sort = ViewState("SortColumn").ToString & " " & ViewState("SortDirection").ToString
            dvLogEntries.RowFilter = ViewState("GridFilter").ToString

            gvEvents.DataSource = dvLogEntries
            gvEvents.DataBind()
            gvEvents.Visible = True
            pnlSearch.Visible = True
            pnlGridOptions.Visible = True
            lblLastRefresh.Text = Now.ToString("M/d/yyyy h:mm.ss tt")

            lblNoEntries.Visible = False
            lblError.Visible = False

         Else

            gvEvents.Visible = False
            pnlSearch.Visible = False
            pnlGridOptions.Visible = False
            lblError.Visible = False
            lblNoEntries.Visible = True

         End If

      Catch fnf As System.IO.FileNotFoundException

         gvEvents.Visible = False
         pnlSearch.Visible = False
         pnlGridOptions.Visible = False
         lblNoEntries.Visible = False
         lblError.Text = "<br/>Log file cannot be found. Please check that file exists or that path is valid."
         lblError.Visible = True

      Catch ex As Exception

         gvEvents.Visible = False
         pnlSearch.Visible = False
         pnlGridOptions.Visible = False
         lblNoEntries.Visible = False
         lblError.Text = "<br/>An error occurred loading log: <br/>" & ex.ToString
         lblError.Visible = True

      End Try

   End Sub

   ''' <summary>
   '''    Changes GridView page to page specified.
   ''' </summary>
   ''' <param name="sender">GridView Page Changed Event</param>
   ''' <param name="e">Page Changed Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	01/18/2007	Created
   ''' </history>
   Private Sub ChangeGridPage(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) _
              Handles gvEvents.PageIndexChanging

      gvEvents.PageIndex = e.NewPageIndex
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Changes GridView sort to column specified.
   ''' </summary>
   ''' <param name="sender">GridView Sorting Event</param>
   ''' <param name="e">Sorting Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	01/18/2007	Created
   ''' </history>
   Private Sub ChangeGridSort(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) _
              Handles gvEvents.Sorting

      ' Set the new sort column to the column specified.
      ViewState("SortColumn") = e.SortExpression

      ' Based on the current ViewState value, change the sort direction to its opposite.
      If ViewState("SortDirection").ToString = "ASC" Then
         ViewState("SortDirection") = "DESC"
      Else
         ViewState("SortDirection") = "ASC"
      End If

      ' Set the page index back to the first page to prevent confusion after a new sort.
      gvEvents.PageIndex = 0

      BindLogFile()

   End Sub

   ''' <summary>
   '''    Formats row data in GridView
   ''' </summary>
   ''' <param name="sender">GridView RowDataBound Event</param>
   ''' <param name="e">RowDataBound Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	01/18/2007	Created
   ''' </history>
   Private Sub FormatGrid(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) _
              Handles gvEvents.RowDataBound

      Dim drEvent As LogEventRow
      Dim gvSource As GridView
      Dim intCellIndex As Integer

      ' The layout of the grid cells is as follows:
      ' 0 - ID
      ' 1 - Level
      ' 2 - Timestamp
      ' 3 - Logger
      ' 4 - Message
      ' 5 - Exception

      ' Set sorting arrow based on column specified
      gvSource = DirectCast(sender, GridView)

      If e.Row.RowType = DataControlRowType.Header Then

         Select Case ViewState("SortColumn").ToString

            Case "ID"
               intCellIndex = 0
            Case "Level"
               intCellIndex = 1
            Case "Timestamp"
               intCellIndex = 2
            Case "Logger"
               intCellIndex = 3
            Case "Message"
               intCellIndex = 4
            Case "Exception"
               intCellIndex = 5

         End Select

         'this is a header row, set the sort style
         If ViewState("SortDirection").ToString = "ASC" Then
            e.Row.Cells(intCellIndex).CssClass += " sortasc"
         Else
            e.Row.Cells(intCellIndex).CssClass += " sortdesc"
         End If

      End If

      ' Only work with data rows
      If e.Row.RowType = DataControlRowType.DataRow Then

         drEvent = DirectCast(DirectCast(e.Row.DataItem, System.Data.DataRowView).Row, LogEventRow)

         ' Apply stylesheet to level based on it's type. Stylesheet name is based on the level itself. Make sure the
         ' level name is in proper case format (e.g. Debug) when specifying.
         e.Row.Cells(1).CssClass = drEvent.Level.Substring(0, 1).ToUpper & _
                                   drEvent.Level.Substring(1, drEvent.Level.Length - 1).ToLower & _
                                   "Status"

         ' Replace any new line characters in the message with the equivalent
         If Not drEvent.IsMessageNull Then

            e.Row.Cells(4).Text = drEvent.Message.Replace(Environment.NewLine, "<br/>")

         End If

         ' Replace any new line characters in the exception with the equivalent
         If Not drEvent.IsExceptionNull Then

            e.Row.Cells(5).Text = drEvent.Exception.Replace(Environment.NewLine, "<br/>")

         End If

      End If

   End Sub

   ''' <summary>
   '''    Stores settings in ViewState variables and applies changes to GridView.
   ''' </summary>
   ''' <param name="sender">Apply Optios Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	01/18/2007	Created
   ''' </history>
   Protected Sub ApplyOptions(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplyOptions.Click

      ' Apply paging settings if paging is set to true.
      If chkUsePaging.Checked Then
         gvEvents.AllowPaging = True
         gvEvents.PageSize = Integer.Parse(txtEventsPerPage.Text)
      Else
         gvEvents.AllowPaging = False
      End If

      ViewState("UsePaging") = chkUsePaging.Checked.ToString
      ViewState("EventsPerPage") = txtEventsPerPage.Text

      BindLogFile()

   End Sub

   ''' <summary>
   '''    Refreshes the GridView without resetting the sort/paging attributes.
   ''' </summary>
   ''' <param name="sender">Refresh Grid Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks>Method redirects to the BindLogFile method.</remarks>
   Protected Sub RefreshGridView(ByVal sender As System.Object, ByVal e As System.EventArgs) _
             Handles btnRefreshGrid.Click

      ViewState("GridRefresh") = rblRefreshRate.SelectedValue

      ' If a refresh rate has been selected, enable the timer.
      If CType(rblRefreshRate.SelectedValue, Integer) > 0 Then

         timUpdate.Enabled = True
         timUpdate.Interval = CType(rblRefreshRate.SelectedValue, Integer) * 1000

      Else

         timUpdate.Enabled = False

      End If

      ' Regardless, bind the grid file for an update.
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Processes the appropriate action when a Node in the TreeView is clicked.
   ''' </summary>
   ''' <param name="sender">TreeView SelectedNodeChanged Event</param>
   ''' <param name="e">SelectedNodeChanged Even Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Private Sub ProcessNode(ByVal sender As Object, ByVal e As System.EventArgs) _
              Handles tvLogSources.SelectedNodeChanged

      ' Only load log for nodes that have a file extension (.xml) in their name.
      If IsLogFile(tvLogSources.SelectedNode.Value, DirectCast(ViewState("LogExtensions"), ArrayList)) Then

         ' For logs (value has a registered extension), find the parent node and delete it from its child.
         ViewState("LogName") = tvLogSources.SelectedNode.Text
         ViewState("LogFilePath") = tvLogSources.SelectedNode.Value
         ViewState("SortColumn") = "ID"
         ViewState("SortDirection") = "ASC"
         ViewState("GridFilter") = ""
         txtLogName.Text = tvLogSources.SelectedNode.Value
         pnlLogSources.Visible = False
         txtGridFilter.Text = ""
         gvEvents.PageIndex = 0

         BindLogFile()

      End If

   End Sub

   ''' <summary>
   '''    Shows/Hides Log Sources TreeView based on current state.
   ''' </summary>
   ''' <param name="sender">Log Source Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Protected Sub ShowHideLogTree(ByVal sender As System.Object, ByVal e As System.EventArgs)

      ' Set the Log Source TreeView to the opposite of its current value.
      If tvLogSources.Visible = True Then
         tvLogSources.Visible = False
      Else
         tvLogSources.Visible = True
      End If

   End Sub

   ''' <summary>
   '''    Applies the filter criteria specified to the grid view and refreshes the display.
   ''' </summary>
   ''' <param name="sender">Apply Filter Button Click Event.</param>
   ''' <param name="e">Button Click Event Arguments.</param>
   ''' <remarks>Method sets the GridFilter ViewState parameter and calls the BindLogFile method.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/15/2007	Created
   ''' </history>
   Protected Sub StartAdvancedSearch(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartAdvancedSearch.Click

      ViewState("GridFilter") = txtGridFilter.Text.Trim()
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Clears the filter applied to the grid view and refreshes the display.
   ''' </summary>
   ''' <param name="sender">Clear Filter Button Click Event.</param>
   ''' <param name="e">Button Click Event Arguments.</param>
   ''' <remarks>Method resets the GridFilter ViewState parameter to blank and calls the BindLogFile method.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/15/2007	Created
   ''' </history>
   Protected Sub ClearAdvancedSearch(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAdvancedSearch.Click

      txtGridFilter.Text = ""
      ViewState("GridFilter") = ""
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Creates a grid view filter based on user basic search parameters and refreshes the display.
   ''' </summary>
   ''' <param name="sender">Basic Search Button Click.</param>
   ''' <param name="e">Button Click Event Arguments.</param>
   ''' <remarks>Method sets the GridFilter ViewState parameter and calls the BindLogFile method.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	03/09/2007	Created
   ''' </history>
   Private Sub StartBasicSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStartBasicSearch.Click

      ' Modify criteria based on column specified.
      Select Case ddlBasicSearchColumn.SelectedValue

         Case "ID"
            ' ID is an Integer/Number field, so no additional characters are necessary.
            ViewState("GridFilter") = ddlBasicSearchColumn.SelectedValue & " " & ddlBasicSearchOperator.SelectedValue & _
                                    " " & txtBasicSearchCriteria.Text.Trim

         Case "Timestamp"
            ' Since the DateTime object is such a finicky beast, some weird massaging is necessary. The method does
            ' a "fresh" conversion within the RowFilter of the date in Timestamp column to a DateTime field and 
            ' comparing that to a DateTime conversion of the string value specified.            
            ViewState("GridFilter") = "Convert(Convert(Timestamp, 'System.String'), 'System.DateTime') " & _
                                    ddlBasicSearchOperator.SelectedValue & " Convert('" & _
                                    txtBasicSearchCriteria.Text.Trim & "', 'System.DateTime')"

         Case Else
            ' Append a ' character around the timestamp value.
            ViewState("GridFilter") = ddlBasicSearchColumn.SelectedValue & " " & ddlBasicSearchOperator.SelectedValue & _
                                    " '" & txtBasicSearchCriteria.Text.Trim & "'"

      End Select

      BindLogFile()

   End Sub

   ''' <summary>
   '''    Clears the filter applied to the basic search and refreshes the display.
   ''' </summary>
   ''' <param name="sender">Clear Basic Search Button Click.</param>
   ''' <param name="e">Button Click Event Arguments.</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	03/09/2007	Created
   ''' </history>
   Private Sub ClearBasicSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearBasicSearch.Click

      txtBasicSearchCriteria.Text = ""
      ViewState("GridFilter") = ""
      ddlBasicSearchColumn.SelectedIndex = 0
      ddlBasicSearchOperator.SelectedIndex = 0
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Creates a grid view filter based on user keyword search parameters and refreshes the display.
   ''' </summary>
   ''' <param name="sender">Keyword Search Button Click.</param>
   ''' <param name="e">Button Click Event Arguments.</param>
   ''' <remarks>Method sets the GridFilter ViewState parameter and calls the BindLogFile method.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	03/17/2007	Created
   ''' </history>
   Private Sub StartKeywordSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStartKeywordSearch.Click

      Dim astrKeywords As String()
      Dim sbFilter As New StringBuilder()

      ' Build row filter based on number of keywords specified.
      astrKeywords = txtKeywordSearchCriteria.Text.Trim.Split(Char.Parse(","))

      For i As Integer = 0 To astrKeywords.Length - 1

         ' If more than 1 keyword is specifed, add an OR to the clause
         If i > 0 Then
            sbFilter.Append(" OR ")
         End If

         sbFilter.Append(ddlKeywordSearchColumn.SelectedValue & " LIKE '%" & astrKeywords(i).Trim & "%'")

      Next

      ViewState("GridFilter") = sbFilter.ToString()
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Clears the filter applied to the keyword search and refreshes the display.
   ''' </summary>
   ''' <param name="sender">Clear Keyword Search Button Click.</param>
   ''' <param name="e">Button Click Event Arguments.</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	03/12/2007	Created
   ''' </history>
   Private Sub ClearKeywordSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearKeywordSearch.Click

      txtKeywordSearchCriteria.Text = ""
      ViewState("GridFilter") = ""
      ddlBasicSearchColumn.SelectedIndex = 0
      ddlBasicSearchOperator.SelectedIndex = 0
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Calls the BindLogFile method to refresh the GridView.
   ''' </summary>
   ''' <param name="sender">Timer Tick Event</param>
   ''' <param name="e">Tick Event Arguments</param>
   ''' <remarks>
   '''    Interval of update is controlled based on ViewState information and user options.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	04/27/2007	Created
   ''' </history>
   Protected Sub TickUpdateGrid(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timUpdate.Tick

      BindLogFile()

   End Sub

   ''' <summary>
   '''    Loads the log files specified for viewing.
   ''' </summary>
   ''' <param name="sender">Load Log ImageButton Click Event.</param>
   ''' <param name="e">ImageButton Click Event Arguments.</param>
   ''' <remarks>
   '''    This method runs when the user does not use the saved log sources TreeView and types in the log source instead.
   ''' </remarks>
   ''' <history>
   '''    Sean Patterson   4/26/2008   [Created]
   ''' </history>
   Protected Sub ManualLoadLog(ByVal sender As Object, ByVal e As EventArgs) Handles imbLoadLog.Click

      ' For logs (value has a registered extension), find the parent node and delete it from its child.
      ViewState("LogName") = "Not Specified"
      ViewState("LogFilePath") = txtLogName.Text
      ViewState("SortColumn") = "ID"
      ViewState("SortDirection") = "ASC"
      ViewState("GridFilter") = ""
      pnlLogSources.Visible = False
      txtGridFilter.Text = ""
      gvEvents.PageIndex = 0
      BindLogFile()

   End Sub

   ''' <summary>
   '''    Redirects the user to the Manage Log Sources page.
   ''' </summary>
   ''' <param name="sender">Manage Log Sources ImageButton Click Event.</param>
   ''' <param name="e">ImageButton Click Event Arguments.</param>
   ''' <remarks></remarks>
   ''' <history>
   '''    Sean Patterson   4/26/2008   [Created]
   ''' </history>
   Protected Sub ManageLogs(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) _
                Handles imbManageLogs.Click, imbPanelManageLogs.Click

      Response.Redirect("ManageLogSources.aspx", True)

   End Sub
End Class