Imports HackSaw.HackSawTables

''' <summary>
'''    This class serves as the data access layer for the HackSaw application. All processes involving loading data 
'''    from a file or dataset function through this class.
''' </summary>
''' <remarks></remarks>
''' <history>
''' 	[Sean Patterson]	01/12/2007	Created
''' </history>
Public Class HackSawDAL

   ''' <summary>
   '''    Method loads data from file specified using log4j-events XML schema.
   ''' </summary>
   ''' <param name="LogPath">Full path of file to load.</param>
   ''' <returns>DataTable containing loaded data.</returns>
   ''' <remarks>
   '''    By default, log4j/log4net log files are appended in a "malformed" format since there is no root XML header 
   '''    or footer tags included. This method will stream the file in, apply the appropriate header/footer, and then 
   '''    pass this "properly formed" XML information to the DataSet for loading. The header and footer tags are 
   '''    located in seperate files to allow for future header changes without recompiling the application.
   ''' </remarks>
   Public Shared Function LoadLog(ByVal LogPath As String) As LogEventDataTable

      Dim dtResults As New LogEventDataTable
      Dim drResults As LogEventRow
      Dim dsLog As New DataSet
      Dim sbBodyContent As New StringBuilder()
      Dim srBodyContent As System.IO.StreamReader
      Dim sbHeaderContent As New StringBuilder()
      Dim srHeaderContent As System.IO.StreamReader
      Dim sbFooterContent As New StringBuilder()
      Dim srFooterContent As System.IO.StreamReader
      Dim sbXmlContent As New StringBuilder()
      Dim srXmlContent As System.IO.StringReader
      Dim strApplicationPath As String = System.Web.HttpContext.Current.Request.PhysicalApplicationPath
      Dim strXMLResourcesPath As String = strApplicationPath & "XML\"
      Dim boolExceptionColumnExists As Boolean
      Dim boolThrowableColumnExists As Boolean
      Dim strHeaderName As String
      Dim strFooterName As String
      Dim strSchemaName As String
      Dim fiLogPathInfo As System.IO.FileInfo

      ' Verify log file exists before continuing.
      If Not System.IO.File.Exists(LogPath) Then

         Throw New System.IO.FileNotFoundException()

      End If

      ' Verify that there is data in the file to read. If no data is found, do nothing and return an empty table.
      fiLogPathInfo = New System.IO.FileInfo(LogPath)

      If fiLogPathInfo.Length > 0 Then

         srBodyContent = New System.IO.StreamReader(LogPath)
         sbBodyContent.Append(srBodyContent.ReadToEnd())
         srBodyContent.Close()

         ' Check for the "version" of logging used (log4j / log4net) and load the appropriate header, footer, and schema
         ' for the log.
         If sbBodyContent.ToString.IndexOf("log4net") > -1 Then

            strHeaderName = "Log4NetHeader.txt"
            strFooterName = "Log4NetFooter.txt"
            strSchemaName = "log4net-events.xsd"

         ElseIf sbBodyContent.ToString.IndexOf("log4j") > -1 Then

            strHeaderName = "Log4JHeader.txt"
            strFooterName = "Log4JFooter.txt"
            strSchemaName = "log4j-events.xsd"

         Else

            Throw New FormatException("Unable to determine log file format. File must have 'log4net' or 'log4j' tag.")

         End If

         srHeaderContent = New System.IO.StreamReader(strXMLResourcesPath & strHeaderName)
         sbHeaderContent.Append(srHeaderContent.ReadToEnd())
         srHeaderContent.Close()

         srFooterContent = New System.IO.StreamReader(strXMLResourcesPath & strFooterName)
         sbFooterContent.Append(srFooterContent.ReadToEnd())
         srFooterContent.Close()

         ' Append pieces together into a "proper" XML file
         sbXmlContent.Append(sbHeaderContent.ToString)
         sbXmlContent.Append(sbBodyContent.ToString)
         sbXmlContent.Append(sbFooterContent.ToString)

         srXmlContent = New System.IO.StringReader(sbXmlContent.ToString)

         dsLog.ReadXmlSchema(strXMLResourcesPath & strSchemaName)
         dsLog.ReadXml(srXmlContent)

         ' The exception column (Log4Net) might not exist in the log and must be tested before adding the field.
         If dsLog.Tables(1).Columns.IndexOf("exception") = -1 Then
            boolExceptionColumnExists = False
         Else
            boolExceptionColumnExists = True
         End If

         ' The throwable column (Log4J) might not exist in the log and must be tested before adding the field.
         If dsLog.Tables(1).Columns.IndexOf("throwable") = -1 Then
            boolThrowableColumnExists = False
         Else
            boolThrowableColumnExists = True
         End If


         ' Copy loaded data into strongly-typed DataTable
         For i As Integer = 0 To dsLog.Tables(1).Rows.Count - 1

            drResults = dtResults.NewLogEventRow
            drResults.ID = Integer.Parse(dsLog.Tables(1).Rows(i)("event_id").ToString)
            drResults.Logger = dsLog.Tables(1).Rows(i)("logger").ToString
            drResults.timestamp = DateTime.Parse(dsLog.Tables(1).Rows(i)("timestamp").ToString)
            drResults.level = dsLog.Tables(1).Rows(i)("level").ToString
            drResults.thread = dsLog.Tables(1).Rows(i)("thread").ToString
            drResults.Message = dsLog.Tables(1).Rows(i)("message").ToString

            If boolExceptionColumnExists Then
               drResults.Exception = dsLog.Tables(1).Rows(i)("exception").ToString
            End If

            If boolThrowableColumnExists Then
               drResults.Exception = dsLog.Tables(1).Rows(i)("throwable").ToString
            End If

            dtResults.Rows.Add(drResults)

         Next

      End If

      Return dtResults

   End Function

   ''' <summary>
   '''    Serializes the TreeView object specified into an XML file.
   ''' </summary>
   ''' <param name="SourceTree">TreeView to serialize.</param>
   ''' <remarks>
   '''    Method uses the TreeViewDataAccess class to facilitate translation of the TreeView into a serializable 
   '''    object. The XML file used for serialization is located in the configuration file (web.config).
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Public Shared Sub SaveLogTree(ByVal SourceTree As TreeView)

      Dim strApplicationPath As String = System.Web.HttpContext.Current.Request.PhysicalApplicationPath
      strApplicationPath = strApplicationPath & ConfigurationManager.AppSettings("LogSourcesName").ToString()

      TreeViewDataAccess.SaveTreeViewData(SourceTree, strApplicationPath)

   End Sub

   ''' <summary>
   '''    Deserializes the XML file specified in the configuration into the TreeView object specified.
   ''' </summary>
   ''' <param name="SourceTree">TreeView to deserialize into.</param>
   ''' <remarks>
   '''    Method uses the TreeViewDataAccess class to facilitate translation of the TreeView into a serializable 
   '''    object. The XML file used for deserialization is located in the configuration file (web.config).
   ''' </remarks>
   ''' <history>
   ''' 	[Sean Patterson]	02/06/2007	Created
   ''' </history>
   Public Shared Sub LoadLogTree(ByVal SourceTree As TreeView)

      Dim strApplicationPath As String = System.Web.HttpContext.Current.Request.PhysicalApplicationPath
      strApplicationPath = strApplicationPath & ConfigurationManager.AppSettings("LogSourcesName").ToString()

      TreeViewDataAccess.LoadTreeViewData(SourceTree, strApplicationPath)

   End Sub

   ''' <summary>
   '''    Determines if the value specified contains a valid log file extension in its name.
   ''' </summary>
   ''' <param name="SourceValue">Value to evaluate.</param>
   ''' <returns>Boolean indicating if value contains a proper extension in it.</returns>
   ''' <remarks>Method uses the LogExtensions ArrayList in the user's session to compare against.</remarks>
   ''' <history>
   ''' 	[Sean Patterson]	04/24/2007	Created
   ''' </history>
   Public Shared Function IsLogFile(ByVal SourceValue As String, ByVal LogExtensions As ArrayList) As Boolean

      Dim boolResults As Boolean = False

      For Each Extension As String In LogExtensions

         If SourceValue.IndexOf(Extension) > -1 Then
            boolResults = True
         End If

      Next

      Return boolResults

   End Function

End Class