''' <summary>
'''    The TreeViewDataAccess class allows the nodes within a TreeView to be serialized to XML for later retrevial. 
'''    Many thanks to Tom John's article over at CodeProject (http://www.codeproject.com/vb/net/TreeViewDataAccess.asp)
'''    for the initial code used in building this app. I've only modified it a bit to make it web component compatible
'''    to  serve HackSaw's needs.
''' </summary>
''' <remarks></remarks>
''' <history>
''' 	[Sean Patterson]	02/06/2007	Created
''' </history>
Public Class TreeViewDataAccess

#Region "Structures"

   ''' <summary>
   '''    TreeViewData structure represents the root node collection of a TreeView and provides the PopulateTreeView 
   '''    function to add these nodes to a specified TreeView instance.
   ''' </summary>
   ''' <remarks>This class is serializable.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   <Serializable()> Public Structure TreeViewData

      ''' <summary>
      ''' Array of TreeNodeData objects representing the root nodes in a TreeView.
      ''' </summary>
      ''' <remarks></remarks>
      Public Nodes() As TreeNodeData

      ''' <summary>
      '''    Creates new instance of the TreeViewData structure based from the specified TreeView.
      ''' </summary>
      ''' <param name="SourceTree">TreeView to build the TreeViewData instance from.</param>
      ''' <remarks></remarks>
      ''' <history>
      ''' 	[Sean Patterson]	02/06/2007	Created
      ''' </history>
      Public Sub New(ByVal SourceTree As TreeView)

         ' Check to see if there are any root nodes in the TreeView
         If SourceTree.Nodes.Count > 0 Then

            ' Populate the Nodes array with child nodes
            ReDim Nodes(SourceTree.Nodes.Count - 1)

            For i As Integer = 0 To SourceTree.Nodes.Count - 1

               Nodes(i) = New TreeNodeData(SourceTree.Nodes(i))

            Next

         End If

      End Sub

      ''' <summary>
      '''    Populates the specified TreeView with the current TreeViewData instance.
      ''' </summary>
      ''' <param name="SourceTree">TreeView instance to populate.</param>
      ''' <remarks></remarks>
      ''' <history>
      ''' 	[Sean Patterson]	02/06/2007	Created
      ''' </history>
      Public Sub PopulateTree(ByVal SourceTree As TreeView)

         'Populate the TreeView with child nodes
         For i As Integer = 0 To Me.Nodes.Length - 1

            SourceTree.Nodes.Add(Me.Nodes(i).ToTreeNode)

         Next

      End Sub

   End Structure

   ''' <summary>
   '''    TreeNodeData structure represents a TreeNode and provides the ToTreeNode function to convert the instance to 
   '''    a TreeNode object.
   ''' </summary>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   <Serializable()> Public Structure TreeNodeData

      ''' <summary>String representing the Text property of the TreeNode.</summary>
      Public Text As String

      ''' <summary>String representing the Value property of the TreeNode.</summary>
      Public Value As String

      ''' <summary>URL representing the icon file displayed next to the TreeNode.</summary>
      Public ImageURL As String

      ''' <summary>Boolean representing the Checked state of the TreeNode.</summary>
      Public Checked As Boolean

      ''' <summary>Array of TreeNodeData objects representing the root nodes in a TreeView.</summary>
      Public ChildNodes() As TreeNodeData

      ''' <summary>
      '''    Creates new instance of the TreeNodeData structure based on the specified TreeNode.
      ''' </summary>
      ''' <param name="SourceNode">TreeNode to build the TreeNodeData instance from.</param>
      ''' <remarks></remarks>
      ''' <history>
      ''' 	[Sean Patterson]	02/06/2007	Created
      ''' </history>
      Public Sub New(ByVal SourceNode As TreeNode)

         ' Set the basic TreeNode properties
         Me.Text = SourceNode.Text
         Me.Value = SourceNode.Value

         If Not SourceNode Is Nothing AndAlso SourceNode.ImageUrl <> "" Then
            Me.ImageURL = SourceNode.ImageUrl
         End If

         Me.Checked = SourceNode.Checked

         'Create any child nodes that exist.
         If SourceNode.ChildNodes.Count > 0 Then

            'Recurse through child nodes and add to Nodes array
            ReDim ChildNodes(SourceNode.ChildNodes.Count - 1)

            For i As Integer = 0 To SourceNode.ChildNodes.Count - 1
               ChildNodes(i) = New TreeNodeData(SourceNode.ChildNodes(i))
            Next

         End If

      End Sub

      ''' <summary>
      '''    Returns as TreeNode built from the instance of the TreeNodeData object.
      ''' </summary>
      ''' <returns>TreeNode object containing data.</returns>
      ''' <remarks></remarks>
      ''' <history>
      ''' 	[Sean Patterson]	02/06/2007	Created
      ''' </history>
      Public Function ToTreeNode() As TreeNode

         Dim Results As TreeNode

         ' Create TreeNode based on instance of TreeNodeData and set basic properties
         Results = New TreeNode(Me.Text, Me.ImageURL)

         Results.Checked = Me.Checked
         Results.Value = Me.Value

         If Not Me.ImageURL Is Nothing AndAlso Me.ImageURL <> "" Then
            Results.ImageUrl = Me.ImageURL
         End If

         ' Set the tooltip of the treenode to the value of the node.
         Results.ToolTip = Me.Value

         ' Recurse through any child nodes and add to Nodes collection
         If Not Me.ChildNodes Is Nothing AndAlso Me.ChildNodes.Length > 0 Then

            For i As Integer = 0 To Me.ChildNodes.Length - 1

               Results.ChildNodes.Add(Me.ChildNodes(i).ToTreeNode)

            Next

         End If

         Return Results

      End Function

   End Structure

#End Region

#Region "Public Methods"

   ''' <summary>
   '''    Populates the specified TreeView from the serialized TreeViewData structure file specified.
   ''' </summary>
   ''' <param name="DestinationTree">TreeView instance to populate.</param>
   ''' <param name="TreeDataPath">Path to XML file to deserialize TreeView data.</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Public Shared Sub LoadTreeViewData(ByVal DestinationTree As TreeView, ByVal TreeDataPath As String)

      ' Create as serializer and get the file to deserialize
      Dim xsrTreeData As New System.Xml.Serialization.XmlSerializer(GetType(TreeViewData))
      Dim filTreeData As New System.IO.FileStream(TreeDataPath, IO.FileMode.Open)
      Dim xtrTreeData As New System.Xml.XmlTextReader(filTreeData)

      Try

         ' Deserialize the file and populate the TreeView.
         Dim treeData As TreeViewData = CType(xsrTreeData.Deserialize(xtrTreeData), TreeViewData)
         treeData.PopulateTree(DestinationTree)

      Catch ex As Exception

         Throw ex

      Finally

         ' Clean up resources.
         xtrTreeData.Close()
         filTreeData.Close()
         filTreeData = Nothing

      End Try

   End Sub

   ''' <summary>
   '''    Saves the specified TreeView in serialized TreeViewData structure file specified.
   ''' </summary>
   ''' <param name="SourceTree">TreeView instance to save.</param>
   ''' <param name="DestinationFilePath">Path to XML file to serialize TreeView data.</param>
   ''' <remarks></remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Public Shared Sub SaveTreeViewData(ByVal SourceTree As TreeView, ByVal DestinationFilePath As String)

      ' Create serializer and file to save TreeViewData
      Dim xsrTreeData As New System.Xml.Serialization.XmlSerializer(GetType(TreeViewData))
      Dim filTreeData As New System.IO.FileStream(DestinationFilePath, IO.FileMode.Create)
      Dim xtrTreeData As New System.Xml.XmlTextWriter(filTreeData, Nothing)

      xtrTreeData.Formatting = System.Xml.Formatting.Indented
      xtrTreeData.Indentation = 3

      Try

         ' Serialize TreeViewData from TreeView into the file.
         xsrTreeData.Serialize(xtrTreeData, New TreeViewData(SourceTree))

      Catch ex As Exception

         Throw ex

      Finally

         ' Clean up resources.
         xtrTreeData.Close()
         filTreeData.Close()
         filTreeData = Nothing

      End Try

   End Sub

#End Region

End Class