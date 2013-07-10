Imports Hacksaw.HackSawDAL

Partial Public Class ManageLogSources
   Inherits System.Web.UI.Page

   Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

      Dim astrParsedExtensions As String()
      Dim alParsedExtensions As New ArrayList

      If Not Page.IsPostBack Then

         ViewState("LogExtensions") = New ArrayList()

         ' Retrieve list of valid log extensions from web.config file and load into ArrayList
         astrParsedExtensions = ConfigurationManager.AppSettings("LogExtensions").ToString.Split(Char.Parse(","))

         For i As Integer = 0 To astrParsedExtensions.Length - 1
            alParsedExtensions.Add(astrParsedExtensions(i))
         Next

         ViewState("LogExtensions") = alParsedExtensions

         lblStatus.Visible = False
         lblError.Visible = False
         BindLogSources()

      End If

   End Sub

   ''' <summary>
   '''    Retrieves log sources and binds to the TreeView for selection.
   ''' </summary>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Private Sub BindLogSources()

      HackSawDAL.LoadLogTree(tvLogSources)
      tvLogSources.ExpandAll()

   End Sub

   ''' <summary>
   '''    Adds a folder node to the TreeView.
   ''' </summary>
   ''' <param name="sender">Add Log Folder Button Click Event.</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks>Folder nodes do not link to log files, they serve for grouping purposes.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Protected Sub AddLogFolder(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddLogCategory.Click

      Dim NewNode As TreeNode

      ' Create new node using information specified.
      NewNode = New TreeNode(txtNewCategoryName.Text.Trim, Nothing)

      If txtNewCategoryIcon.Text.Trim() <> "" Then
         NewNode.ImageUrl = txtNewCategoryIcon.Text.Trim()
      End If

      ' Add folder node to root node if no folder has been selected.
      If tvLogSources.SelectedNode Is Nothing Then

         tvLogSources.Nodes.Add(NewNode)

         ClearTextBoxes()
         lblError.Visible = False
         lblStatus.Text = "Folder Added."
         lblStatus.Visible = True

      Else

         tvLogSources.SelectedNode.ChildNodes.Add(NewNode)
         tvLogSources.SelectedNode.Expand()

         ClearTextBoxes()
         lblError.Visible = False
         lblStatus.Text = "Folder Added."
         lblStatus.Visible = True

      End If

   End Sub

   ''' <summary>
   '''    Adds a log node to the TreeView
   ''' </summary>
   ''' <param name="sender">Add Log Node Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks>Log nodes contain the path to the XML file to view.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Protected Sub AddLogNode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddLogNode.Click

      Dim NewNode As TreeNode

      ' Create new node using information specified.
      NewNode = New TreeNode(txtNewLogName.Text.Trim, txtNewLogPath.Text.Trim)

      If txtNewLogIcon.Text.Trim() <> "" Then
         NewNode.ImageUrl = txtNewLogIcon.Text.Trim()
      End If

      ' Add log node to root node if no folder has been selected.
      If tvLogSources.SelectedNode Is Nothing Then

         tvLogSources.Nodes(0).ChildNodes.Add(NewNode)

         ClearTextBoxes()
         lblError.Visible = False
         lblStatus.Text = "Log Added."
         lblStatus.Visible = True

      Else

         tvLogSources.SelectedNode.ChildNodes.Add(NewNode)
         tvLogSources.SelectedNode.Expand()

         ClearTextBoxes()
         lblError.Visible = False
         lblStatus.Text = "Log Added."
         lblStatus.Visible = True

      End If

   End Sub

   ''' <summary>
   '''    Saves the log sources TreeView to an XML file.
   ''' </summary>
   ''' <param name="sender">Save Sources Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/07/2007	Created
   ''' </history>
   Protected Sub SaveTree(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveTree.Click

      Try

         HackSawDAL.SaveLogTree(tvLogSources)
         lblStatus.Text = "Log Sources saved to file."
         lblStatus.Visible = True
         lblError.Visible = False

      Catch ex As Exception

         lblError.Text = "An error occurred saving log sources to file: <br/>" & ex.ToString()

      End Try


   End Sub


   ''' <summary>
   '''    Loads log sources XML file into the TreeView.
   ''' </summary>
   ''' <param name="sender">Reload Sources Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   '''    Sean Patterson   02/07/2007   [Created]
   ''' </history>
   Protected Sub ReloadTree(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReloadTree.Click

      Try

         ' Clear out any existing items in the tree before reloading.
         tvLogSources.Nodes.Clear()
         BindLogSources()

         lblStatus.Text = "Log Sources reloaded from file."
         lblStatus.Visible = True
         lblError.Visible = False

      Catch ex As Exception

         lblError.Text = "An error occurred loading log sources from file: <br/>" & ex.ToString()

      End Try

   End Sub

   ''' <summary>
   '''    Processes the appropriate action when a Node in the TreeView is clicked.
   ''' </summary>
   ''' <param name="sender">TreeView SelectedNodeChanged Event</param>
   ''' <param name="e">SelectedNodeChanged Even Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/07/2007	Created
   ''' </history>
   Private Sub ProcessNode(ByVal sender As Object, ByVal e As System.EventArgs) _
               Handles tvLogSources.SelectedNodeChanged

      ' Clear textbox items and reset labels before updating display.
      txtSelectedNodeName.Text = ""
      txtSelectedNodePath.Text = ""
      lblStatus.Visible = False
      lblError.Visible = False

      ' Process the node based on the node selected.
      Select Case True

         Case tvLogSources.SelectedNode.Parent Is Nothing

            ' The root has its name and icon URL displayed.
            txtSelectedNodeName.Text = tvLogSources.SelectedNode.Text
            txtSelectedNodeIcon.Text = tvLogSources.SelectedNode.ImageUrl

         Case tvLogSources.SelectedNode.Text = tvLogSources.SelectedNode.Value

            ' For folders (text and value are the same), display the name and the icon URL.
            txtSelectedNodeName.Text = tvLogSources.SelectedNode.Text
            txtSelectedNodeIcon.Text = tvLogSources.SelectedNode.ImageUrl

         Case IsLogFile(tvLogSources.SelectedNode.Value, DirectCast(ViewState("LogExtensions"), ArrayList))

            ' For logs (value has a registered extension), find the parent node and delete it from its child.
            txtSelectedNodeName.Text = tvLogSources.SelectedNode.Text
            txtSelectedNodePath.Text = tvLogSources.SelectedNode.Value
            txtSelectedNodeIcon.Text = tvLogSources.SelectedNode.ImageUrl

      End Select

   End Sub

   ''' <summary>
   '''    Deletes the node selected.
   ''' </summary>
   ''' <param name="sender">Delete Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks>If a folder node is deleted, all child log nodes will automatically be deleted.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/07/2007	Created
   ''' </history>
   Protected Sub DeleteNode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteNode.Click

      ' Process the node based on the node selected.
      Select Case True

         Case tvLogSources.SelectedNode.Parent Is Nothing

            ' The root node cannot be removed. 
            lblError.Text = "Removing of root node is not permitted."
            lblError.Visible = True
            lblStatus.Visible = False

         Case tvLogSources.SelectedNode.Text = tvLogSources.SelectedNode.Value

            ' For folders (text and value are the same), find the parent node and delete it from its child.
            tvLogSources.SelectedNode.Parent.ChildNodes.Remove(tvLogSources.SelectedNode)

            ClearTextBoxes()
            lblError.Visible = False
            lblStatus.Text = "Folder Removed."
            lblStatus.Visible = True

         Case IsLogFile(tvLogSources.SelectedNode.Value, DirectCast(ViewState("LogExtensions"), ArrayList))

            ' For logs (value has a registered extension), find the parent node and delete it from its child.
            tvLogSources.SelectedNode.Parent.ChildNodes.Remove(tvLogSources.SelectedNode)

            ClearTextBoxes()
            lblError.Visible = False
            lblStatus.Text = "Log Removed."
            lblStatus.Visible = True

      End Select

   End Sub

   ''' <summary>
   '''    Updates selected node to the new name/path specified by the user.
   ''' </summary>
   ''' <param name="sender">Update Button Click Event</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks></remarks>
   ''' <history>
   '''    Sean Patterson   02/07/2007   [Created]
   ''' </history>
   Protected Sub UpdateNode(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateNode.Click

      ' Process the node based on the node selected.
      Select Case True

         Case tvLogSources.SelectedNode.Parent Is Nothing

            ' For root node, update the name TextBox only.
            If txtSelectedNodeName.Text.Trim = "" Then

               lblError.Text = "Please enter a root name before continuing."
               lblError.Visible = True
               lblStatus.Visible = False

            Else

               tvLogSources.SelectedNode.Text = txtSelectedNodeName.Text.Trim()

               If txtSelectedNodeIcon.Text.Trim <> "" Then
                  tvLogSources.SelectedNode.ImageUrl = txtSelectedNodeIcon.Text.Trim()
               End If

               ClearTextBoxes()
               lblStatus.Text = "Root node updated."
               lblStatus.Visible = True
               lblError.Visible = False

            End If

         Case tvLogSources.SelectedNode.Text = tvLogSources.SelectedNode.Value

            ' For folders, update the name TextBox only.
            If txtSelectedNodeName.Text.Trim = "" Then

               lblError.Text = "Please enter a folder name before continuing."
               lblError.Visible = True
               lblStatus.Visible = False

            Else

               tvLogSources.SelectedNode.Text = txtSelectedNodeName.Text.Trim()

               If txtSelectedNodeIcon.Text.Trim <> "" Then
                  tvLogSources.SelectedNode.ImageUrl = txtSelectedNodeIcon.Text.Trim()
               End If

               ClearTextBoxes()
               lblStatus.Text = "Folder node updated."
               lblStatus.Visible = True
               lblError.Visible = False

            End If


         Case IsLogFile(tvLogSources.SelectedNode.Value, DirectCast(ViewState("LogExtensions"), ArrayList))

            ' For logs (value has a registered extension), find the parent node and delete it from its child.
            If txtSelectedNodeName.Text.Trim = "" OrElse txtSelectedNodePath.Text.Trim = "" Then

               lblError.Text = "Please enter a log name and path before continuing."
               lblError.Visible = True
               lblStatus.Visible = False

            Else

               tvLogSources.SelectedNode.Text = txtSelectedNodeName.Text.Trim()
               tvLogSources.SelectedNode.Value = txtSelectedNodePath.Text.Trim()

               If txtSelectedNodeIcon.Text.Trim <> "" Then
                  tvLogSources.SelectedNode.ImageUrl = txtSelectedNodeIcon.Text.Trim()
               End If

               ClearTextBoxes()
               lblStatus.Text = "Log node updated."
               lblStatus.Visible = True
               lblError.Visible = False

            End If

      End Select

   End Sub

   ''' <summary>
   '''    Moves the selected node up in the tree.
   ''' </summary>
   ''' <param name="sender">Move Node Up Button Click Event.</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks>
   '''    A node cannot move up in the heirarchy of the tree. It can only move up amongst the child nodes it is a part
   '''    of.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/16/2007	Created
   ''' </history>
   Protected Sub MoveNodeUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveNodeUp.Click

      Dim intNodeIndex As Integer
      Dim tnClone As TreeNode
      Dim tnNodeToMove As TreeNode

      If Not tvLogSources.SelectedNode Is Nothing Then

         Try

            tnNodeToMove = tvLogSources.SelectedNode

            ' Get the index of the node.
            intNodeIndex = tnNodeToMove.Parent.ChildNodes.IndexOf(tnNodeToMove)

            ' If the index is not 0 (first child) then create a clone of the node and insert at the index - 1.
            If intNodeIndex > 0 Then

               tnClone = New TreeNode(tnNodeToMove.Text, tnNodeToMove.Value, tnNodeToMove.ImageUrl, _
                             tnNodeToMove.NavigateUrl, tnNodeToMove.Target)
               tnNodeToMove.Parent.ChildNodes.AddAt(intNodeIndex - 1, tnClone)
               tnNodeToMove.Parent.ChildNodes.Remove(tnNodeToMove)

               ClearAllButSelectedTextBoxes()
               tnClone.Select()

               lblError.Visible = False
               lblStatus.Text = "Node moved."
               lblStatus.Visible = True

            End If

         Catch ex As Exception

            lblError.Text = ex.ToString
            lblError.Visible = True

         End Try

      End If

   End Sub

   ''' <summary>
   '''    Moves the selected node down in the tree.
   ''' </summary>
   ''' <param name="sender">Move Node Down Button Click Event.</param>
   ''' <param name="e">Button Click Event Arguments</param>
   ''' <remarks>
   '''    A node cannot move down in the heirarchy of the tree. It can only move up amongst the child nodes it is a 
   '''    part of.
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/16/2007	Created
   ''' </history>
   Protected Sub MoveNodeDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveNodeDown.Click

      Dim intNodeIndex As Integer
      Dim tnClone As TreeNode
      Dim tnNodeToMove As TreeNode

      If Not tvLogSources.SelectedNode Is Nothing Then

         Try

            tnNodeToMove = tvLogSources.SelectedNode

            ' Get the index of the node.
            intNodeIndex = tnNodeToMove.Parent.ChildNodes.IndexOf(tnNodeToMove)

            ' If the index is not the last node in the list, create a clone of the node and insert at the index + 2.
            If intNodeIndex <> tnNodeToMove.Parent.ChildNodes.Count - 1 Then

               tnClone = New TreeNode(tnNodeToMove.Text, tnNodeToMove.Value, tnNodeToMove.ImageUrl, _
                                      tnNodeToMove.NavigateUrl, tnNodeToMove.Target)
               tnNodeToMove.Parent.ChildNodes.AddAt(intNodeIndex + 2, tnClone)
               tnNodeToMove.Parent.ChildNodes.Remove(tnNodeToMove)

               lblError.Visible = False
               lblStatus.Text = "Node moved."
               lblStatus.Visible = True

               ClearAllButSelectedTextBoxes()
               tnClone.Select()

            End If

         Catch ex As Exception

            lblError.Text = ex.ToString
            lblError.Visible = True

         End Try

      End If

   End Sub

   ''' <summary>
   '''    Sets all TextBoxes Text parameter to blank.
   ''' </summary>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/16/2007	Created
   ''' </history>
   Private Sub ClearTextBoxes()

      txtSelectedNodeName.Text = ""
      txtSelectedNodePath.Text = ""
      txtSelectedNodeIcon.Text = ""
      txtNewCategoryName.Text = ""
      txtNewCategoryIcon.Text = ""
      txtNewLogName.Text = ""
      txtNewLogPath.Text = ""
      txtNewLogIcon.Text = ""

   End Sub

   ''' <summary>
   '''    Sets all TextBoxes Text parameter to blank except for those relating to the selected node.
   ''' </summary>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/16/2007	Created
   ''' </history>
   Private Sub ClearAllButSelectedTextBoxes()

      txtNewCategoryName.Text = ""
      txtNewCategoryIcon.Text = ""
      txtNewLogName.Text = ""
      txtNewLogPath.Text = ""
      txtNewLogIcon.Text = ""

   End Sub

End Class