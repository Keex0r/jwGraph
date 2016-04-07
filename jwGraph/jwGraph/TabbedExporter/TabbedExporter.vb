Imports System.Text
Namespace GeneralTools
    Namespace TabbedExporter
        Public Class TabbedExporter
            Public Property Header As List(Of String)
            Public Property Columns As List(Of Column)
            Public Property IgnoreExporterHeader As Boolean
            Public Property IgnoreColumnHeaders As Boolean
            Public Property Delimiter As String
            Public Property IgnoredColumnList As List(Of Integer)
            Public Property LastAddedColumn As Column

            Public Sub New()
                Header = New List(Of String)
                Columns = New List(Of Column)
                IgnoreColumnHeaders = False
                IgnoreExporterHeader = False
                Delimiter = vbTab
                IgnoredColumnList = New List(Of Integer)
                LastAddedColumn = Nothing
            End Sub

            Private Sub AddHeader(ByRef sb As StringBuilder)
                For Each s As String In Me.Header
                    sb.AppendLine(s)
                Next
            End Sub

            Private Sub AddColumnHeaders(ByRef sb As StringBuilder, Delimiter As String)
                Dim MaxLength As Integer = 0
                For c As Integer = 0 To Me.Columns.Count - 1
                    If IgnoredColumnList.Contains(c) Then Continue For
                    MaxLength = Math.Max(MaxLength, Me.Columns(c).Header.Count)
                Next
                Dim HeaderLines As New List(Of String)
                For i = 0 To MaxLength - 1
                    Dim thisline As New StringBuilder
                    For c As Integer = 0 To Me.Columns.Count - 1
                        If IgnoredColumnList.Contains(c) Then Continue For
                        'Add items from last to start
                        With Me.Columns(c)
                            If .Header.Count - 1 - i >= 0 Then
                                thisline.Append(.Header(.Header.Count - 1 - i))
                            End If
                            If c < Me.Columns.Count - 1 Then thisline.Append(Delimiter)
                        End With
                    Next
                    HeaderLines.Add(thisline.ToString)
                Next
                'Add the lines from last to first
                For i = HeaderLines.Count - 1 To 0 Step -1
                    sb.AppendLine(HeaderLines(i))
                Next
            End Sub

            Private Sub AddAllData(ByRef sb As StringBuilder, Delimiter As String)
                Dim ColumnCount As Integer = Me.Columns.Count

                Dim MaxLength As Integer = 0
                For c As Integer = 0 To Me.Columns.Count - 1
                    If IgnoredColumnList.Contains(c) Then Continue For
                    With Me.Columns(c)
                        MaxLength = Math.Max(MaxLength, .Length)
                    End With
                Next
                For i = 0 To MaxLength - 1
                    Dim thisline As New StringBuilder
                    For c As Integer = 0 To Me.Columns.Count - 1
                        If IgnoredColumnList.Contains(c) Then Continue For
                        With Me.Columns(c)
                            If i < .Length Then
                                thisline.Append(GetStringRepresentation(.Item(i)))
                            End If
                            If c < Me.Columns.Count - 1 Then thisline.Append(Delimiter)
                        End With
                    Next
                    sb.AppendLine(thisline.ToString)
                Next
            End Sub

#Region "Add column functions"
#Region "String"
            Public Sub AddColumn(Header As String(), Data As String(), Optional Tag As Object = Nothing)
                Dim c As New Column
                c.Items.AddRange(Data)
                c.Header.AddRange(Header)
                Me.Columns.Add(c)
                LastAddedColumn = c
                c.Tag = Tag
            End Sub
            Public Sub AddColumn(Data As String(), Optional Tag As Object = Nothing)
                AddColumn({}, Data, Tag)
            End Sub
            Public Sub AddColumn(Header As String(), Data As List(Of String), Optional Tag As Object = Nothing)
                AddColumn(Header, Data.ToArray, Tag)
            End Sub
            Public Sub AddColumn(Header As List(Of String), Data As List(Of String), Optional Tag As Object = Nothing)
                AddColumn(Header.ToArray, Data.ToArray, Tag)
            End Sub
            Public Sub AddColumn(Data As List(Of String), Optional Tag As Object = Nothing)
                AddColumn({}, Data.ToArray, Tag)
            End Sub
#End Region
#Region "Double"
            Public Sub AddColumn(Header As String(), Data As Double(), Optional Tag As Object = Nothing)
                AddColumn(Header, Array.ConvertAll(Data, New Converter(Of Double, String)(Function(d As Double) GetStringRepresentation(d))), Tag)
            End Sub
            Public Sub AddColumn(Data As Double(), Optional Tag As Object = Nothing)
                AddColumn({}, Data, Tag)
            End Sub
            Public Sub AddColumn(Header As String(), Data As List(Of Double), Optional Tag As Object = Nothing)
                AddColumn(Header, Data.ToArray, Tag)
            End Sub
            Public Sub AddColumn(Header As List(Of String), Data As List(Of Double), Optional Tag As Object = Nothing)
                AddColumn(Header.ToArray, Data.ToArray, Tag)
            End Sub
            Public Sub AddColumn(Data As List(Of Double), Optional Tag As Object = Nothing)
                AddColumn({}, Data.ToArray, Tag)
            End Sub
#End Region
#Region "Integer"
            Public Sub AddColumn(Header As String(), Data As Integer(), Optional Tag As Object = Nothing)
                AddColumn(Header, Array.ConvertAll(Data, New Converter(Of Integer, String)(Function(d As Integer) GetStringRepresentation(d))), Tag)
            End Sub
            Public Sub AddColumn(Data As Integer(), Optional Tag As Object = Nothing)
                AddColumn({}, Data, Tag)
            End Sub
            Public Sub AddColumn(Header As String(), Data As List(Of Integer), Optional Tag As Object = Nothing)
                AddColumn(Header, Data.ToArray, Tag)
            End Sub
            Public Sub AddColumn(Header As List(Of String), Data As List(Of Integer), Optional Tag As Object = Nothing)
                AddColumn(Header.ToArray, Data.ToArray, Tag)
            End Sub
            Public Sub AddColumn(Data As List(Of Integer), Optional Tag As Object = Nothing)
                AddColumn({}, Data.ToArray, Tag)
            End Sub
#End Region

#End Region

            Public Sub Clear()
                For Each c As Column In Me.Columns
                    c.Items.Clear()
                    c.Header.Clear()
                Next
                Me.Columns.Clear()
                Me.Header.Clear()
            End Sub

            Public Overridable Function GetStringRepresentation(value As Object) As String
                Return If(value IsNot Nothing, value.ToString, "null")
            End Function

            Public Overrides Function ToString() As String
                Dim res As New StringBuilder
                If Not IgnoreExporterHeader Then
                    AddHeader(res)
                End If
                If Not IgnoreColumnHeaders Then
                    AddColumnHeaders(res, Delimiter)
                End If
                AddAllData(res, Delimiter)
                Return res.ToString
            End Function
            Public Function DoExport(ToClip As Boolean) As Boolean
                If ToClip Then
                    Return GeneralTools.SetClipboardText(Me.ToString, True)
                Else
                    Try
                        Dim sfd As New SaveFileDialog
                        sfd.Filter = "Text files|*.txt"
                        sfd.AddExtension = True
                        sfd.DefaultExt = ".txt"
                        If sfd.ShowDialog = DialogResult.Cancel Then Return False
                        IO.File.WriteAllText(sfd.FileName, Me.ToString)
                        Return True
                    Catch
                        Return False
                    End Try
                End If
            End Function
        End Class
    End Namespace
End Namespace
