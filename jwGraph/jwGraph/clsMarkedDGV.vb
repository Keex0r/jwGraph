Namespace GeneralTools
    Public Class MarkedDGV
        Inherits DataGridView

        Public Event VScrollVisibleChanged(sender As Object, e As EventArgs)
        Public Event HScrollVisibleChanged(sender As Object, e As EventArgs)

        Public Property AutoCommitCheckboxColumns As Boolean
        Public Property AutoCommitComboboxColumns As Boolean

        Private WithEvents cms As ContextMenuStrip
        Private WithEvents CopyButton As ToolStripMenuItem
        Private WithEvents PasteButton As ToolStripMenuItem

        Private ShiftDown As Boolean = False
        Private ControlDown As Boolean = False
        Private selectedRowsCache As DataGridViewSelectedRowCollection

        Private _MarkedList As List(Of Integer)
        Public Property Marker As Bitmap
        Public Property NoLinesText As String

        Public Property DontAutoFormat As Boolean

        Public Function ShouldSerializeClipboardCopyMode() As Boolean
            Return Me.ClipboardCopyMode <> DataGridViewClipboardCopyMode.Disable
        End Function
        Public Function ResetClipboardCopyMode() As Boolean
            Return Me.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable
        End Function

        Public Sub New()
            _MarkedList = New List(Of Integer)
            Me.AllowDrop = True
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.UserPaint, True)
            AutoCommitCheckboxColumns = True
            AutoCommitComboboxColumns = True
            cms = New ContextMenuStrip
            CopyButton = New ToolStripMenuItem
            With CopyButton
                .Text = "Copy"
                .ShortcutKeyDisplayString = "Ctrl+C"
                AddHandler .Click, AddressOf CopyClicked
            End With
            PasteButton = New ToolStripMenuItem
            With PasteButton
                .Text = "Paste"
                AddHandler .Click, AddressOf PasteClicked
                .ShortcutKeyDisplayString = "Ctrl+V"
            End With
            cms.Items.Add(CopyButton)
            cms.Items.Add(PasteButton)
            Me.ContextMenuStrip = cms
            Me.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText

            'Styles
            Me.BackgroundColor = Color.GhostWhite
            Me.DefaultCellStyle.ForeColor = Color.MediumBlue
            Me.DefaultCellStyle.BackColor = Color.GhostWhite
            Me.AlternatingRowsDefaultCellStyle.ForeColor = Color.MediumBlue
            Me.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            Me.AllowUserToResizeRows = False
            Me.DontAutoFormat = False
            AddHandler Me.VerticalScrollBar.VisibleChanged, Sub(s, e)
                                                                RaiseEvent VScrollVisibleChanged(s, e)
                                                            End Sub
            AddHandler Me.HorizontalScrollBar.VisibleChanged, Sub(s, e)
                                                                  RaiseEvent HScrollVisibleChanged(s, e)
                                                              End Sub
        End Sub

        Private Sub DoCopy()
            Try
                ' Dim d As DataObject = Me.GetClipboardContent()
                Dim rows As New List(Of List(Of String))
                Dim minx As Integer = 1000000000
                Dim maxx As Integer = -1
                Dim miny As Integer = 1000000000
                Dim maxy As Integer = -1
                For y = 0 To Me.Rows.Count - 1
                    For x = 0 To Me.Rows(y).Cells.Count - 1
                        If Me(x, y).Selected = True Then
                            If x < minx Then minx = x
                            If x > maxx Then maxx = x
                            If y < miny Then miny = y
                            If y > maxy Then maxy = y
                        End If
                    Next
                Next
                Dim data(maxx - minx, maxy - miny) As String
                For y = 0 To Me.Rows.Count - 1
                    For x = 0 To Me.Rows(y).Cells.Count - 1
                        If Me(x, y).Selected = True Then
                            Dim thisx = x - minx
                            Dim thisy = y - miny
                            If TypeOf Me(x, y).Value Is Double Then
                                data(thisx, thisy) = CDbl(Me(x, y).Value).ToString()
                            Else
                                data(thisx, thisy) = Me(x, y).FormattedValue.ToString
                            End If
                        End If
                    Next
                Next
                Dim sb As New System.Text.StringBuilder
                Dim ub0 = data.GetUpperBound(0)
                Dim ub1 = data.GetUpperBound(1)
                For y = 0 To ub1
                    Dim line As String = ""
                    For x = 0 To ub0 - 1
                        line &= If(data(x, y) IsNot Nothing, data(x, y), "") & vbTab
                    Next
                    line &= If(data(ub0, y) IsNot Nothing, data(ub0, y), "")
                    If y < ub1 Then
                        sb.AppendLine(line)
                    Else
                        sb.Append(line)
                    End If


                Next
                Clipboard.SetText(sb.ToString)
                'Clipboard.SetDataObject(d)
            Catch ex As Exception
                MessageBox.Show("Could not copy the selected data successfully or no data was selected. Please try again.", "Unsuccessful clipboard operation.", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub
        Private Sub CopyClicked(sender As Object, e As EventArgs)
            DoCopy()
        End Sub

        Public Event PastingData(sender As Object, e As EventArgs)
        Public Event PastedData(sender As Object, e As EventArgs)
        Private Sub DoPaste1()
            Try
                If Me.SelectedCells.Count = 0 Then Exit Sub
                If Clipboard.ContainsText = False Then Exit Sub
                RaiseEvent PastingData(Me, New EventArgs)
                Dim clip As String = Clipboard.GetText
                Dim rows() As String = Strings.Split(clip, vbLf)
                Dim rowsandcols(rows.Count - 1)() As String
                For i = 0 To rows.Count - 1
                    rowsandcols(i) = Strings.Split(rows(i), vbTab)
                Next
                '---- Corrective code here
                Dim emptycols As Integer = 0
                For i = 0 To rows.Count - 1
                    If IsNumeric(rowsandcols(i)(0)) = False AndAlso rowsandcols(i)(0) <> "" Then rowsandcols(i)(0) = "" : emptycols += 1
                Next
                '----
                Dim startind As Integer = Me.SelectedCells(0).RowIndex
                Dim lastind As Integer = startind + (rows.Count - emptycols)
                Me.Rows.Add(lastind - startind - 1)
                Dim curr As Integer = 0
                Dim startcol As Integer = Me.SelectedCells(0).ColumnIndex

                For Each s As String() In rowsandcols
                    Dim currx As Integer = 0
                    For x = startcol To startcol + s.Count - 1
                        If x >= Me.ColumnCount Then Exit For
                        If s(currx) <> "" Then
                            Me(x, startind + curr).Value = Convert.ChangeType(s(currx), Me(x, startind + curr).ValueType)
                        End If
                        currx += 1
                    Next
                    curr += 1
                Next
            Catch ex As Exception
                MessageBox.Show("An error occured while pasting the data. The clipboard might be busy. Please try again." & vbCrLf & vbCrLf & ex.Message, "Clipboard error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            RaiseEvent PastedData(Me, New EventArgs)
        End Sub
        Private Sub DoPaste()
            If Me.AllowUserToAddRows = True AndAlso Me.DataSource Is Nothing Then DoPaste1() : Exit Sub
            Try
                Dim isDatabound As Boolean = Me.DataSource IsNot Nothing
                If Me.CurrentCell Is Nothing Then Exit Sub
                RaiseEvent PastingData(Me, New EventArgs)
                Dim s As String = Clipboard.GetText()
                Dim lines As String() = s.Split(ControlChars.Lf)
                Dim iFail As Integer = 0, iRow As Integer = Me.CurrentCell.RowIndex
                Dim iCol As Integer = Me.CurrentCell.ColumnIndex
                Dim oCell As DataGridViewCell
                For Each line As String In lines
                    If iRow < Me.RowCount AndAlso line.Length > 0 Then
                        Dim sCells As String() = line.Replace(vbCr, "").Split(ControlChars.Tab)
                        For i As Integer = 0 To sCells.GetLength(0) - 1
                            If iCol + i < Me.ColumnCount Then
                                oCell = Me(iCol + i, iRow)
                                If Not oCell.[ReadOnly] Then
                                    Dim enterValue = (isDatabound AndAlso oCell.Value IsNot Nothing AndAlso oCell.Value.ToString() <> sCells(i)) OrElse
                                        (Not isDatabound AndAlso (oCell.Value Is Nothing OrElse oCell.Value.ToString() <> sCells(i)))
                                    If enterValue Then
                                        oCell.Value = Convert.ChangeType(sCells(i), oCell.ValueType)
                                    Else
                                        iFail += 1
                                    End If
                                End If
                            Else
                                Exit For
                            End If
                        Next
                        iRow += 1
                    Else
                        Exit For
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show("An error occured while pasting the data. The clipboard might be busy. Please try again.", "Clipboard error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            RaiseEvent PastedData(Me, New EventArgs)
        End Sub
        Private Sub PasteClicked(sender As Object, e As EventArgs)
            DoPaste()
        End Sub

        Public Shadows Property DoubleBuffered As Boolean
            Get
                Return MyBase.DoubleBuffered
            End Get
            Set(value As Boolean)
                MyBase.DoubleBuffered = value
            End Set
        End Property

        Public Sub MarkRow(index As Integer)
            If index >= 0 AndAlso index < Me.RowCount AndAlso MarkedList.Contains(index) Then MarkedList.Add(index) : Me.Refresh()
        End Sub
        Public Sub ClearMarkedList()
            Dim dorefresh As Boolean = MarkedList.Count > 0
            _MarkedList.Clear()
            If dorefresh Then Me.Refresh()
        End Sub
        Public Sub SetMarkedList(Rows As List(Of Integer))
            Dim dorefresh As Boolean = False
            If MarkedList.Count <> Rows.Count Then
                dorefresh = True
            Else
                For Each i As Integer In MarkedList
                    If Rows.Contains(i) = False Then dorefresh = True
                Next
            End If
            _MarkedList = Rows
            If dorefresh Then Me.Refresh()
        End Sub
        Public Sub SetMarkedList(Rows As Integer())
            Dim dorefresh As Boolean = False
            If MarkedList.Count <> Rows.Count Then
                dorefresh = True
            Else
                For Each i As Integer In MarkedList
                    If Rows.Contains(i) = False Then dorefresh = True
                Next
            End If
            _MarkedList = Rows.ToList
            If dorefresh Then Me.Refresh()
        End Sub
        Public ReadOnly Property MarkedList As List(Of Integer)
            Get
                Return _MarkedList
            End Get
        End Property
        Protected Overrides Sub OnRowPostPaint(e As DataGridViewRowPostPaintEventArgs)
            MyBase.OnRowPostPaint(e)
            If Marker IsNot Nothing AndAlso MarkedList.Contains(e.RowIndex) Then
                Dim drawwhere As Integer = CInt(e.RowBounds.Top + e.RowBounds.Height / 2 - Marker.Height / 2)
                e.Graphics.DrawImageUnscaled(Marker, 0, drawwhere)
            End If
        End Sub

        Protected Overrides Sub OnDragEnter(drgevent As DragEventArgs)
            MyBase.OnDragEnter(drgevent)
            drgevent.Effect = DragDropEffects.All
        End Sub
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            If Me.RowCount = 0 Then
                Dim test As String = Me.NoLinesText
                Dim font As New Font("Arial", 14, FontStyle.Italic)
                Dim s As SizeF = e.Graphics.MeasureString(test, font)
                Using sf As New System.Drawing.StringFormat
                    sf.LineAlignment = StringAlignment.Near
                    sf.Alignment = StringAlignment.Center
                    e.Graphics.DrawString(test, font, Brushes.LightGray, New Rectangle(3, 55, Me.ClientRectangle.Width - 6, Me.ClientRectangle.Height - 60), sf)
                End Using

            End If
            If Me.Enabled = False Then
                Dim x As Integer
                Dim y As Integer
                If Me.ColumnHeadersVisible And ColumnCount > 0 Then
                    y = Me.ColumnHeadersHeight + 3
                Else
                    y = 3
                End If
                If Me.RowHeadersVisible And ColumnCount > 0 And RowCount > 0 Then
                    x = Me.RowHeadersWidth + 1
                Else
                    x = 1
                End If
                Dim c As Color = Color.FromArgb(155, 155, 155, 155)
                Using b As New SolidBrush(c)
                    e.Graphics.FillRectangle(b, Me.ClientRectangle)
                End Using
                e.Graphics.DrawImageUnscaled(My.Resources.Lock, x, y)
            End If
        End Sub

        Private Sub MarkedDGV_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles Me.CellEnter
            Try
                Dim dgv As DataGridView = CType(sender, DataGridView)

                If dgv(e.ColumnIndex, e.RowIndex).EditType IsNot Nothing AndAlso dgv(e.ColumnIndex, e.RowIndex).EditType.ToString() = "System.Windows.Forms.DataGridViewComboBoxEditingControl" Then
                    SendKeys.Send("{F4}")
                End If
            Catch ex As Exception
                Debug.WriteLine(ex.ToString)
            End Try
        End Sub

        Private Sub MarkedDGV_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles Me.CurrentCellDirtyStateChanged
            If AutoCommitCheckboxColumns AndAlso TypeOf Me.Columns(Me.CurrentCell.ColumnIndex) Is DataGridViewCheckBoxColumn Then
                Me.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
            If AutoCommitComboboxColumns AndAlso TypeOf Me.Columns(Me.CurrentCell.ColumnIndex) Is DataGridViewComboBoxColumn Then
                Me.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End Sub

        Private Sub MarkedDGV_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            ControlDown = e.Control
            ShiftDown = e.Shift
            If e.Control AndAlso e.KeyCode = Keys.V Then
                DoPaste()
                e.Handled = True
            ElseIf e.Control AndAlso e.KeyCode = Keys.C Then
                DoCopy()
                e.Handled = True
            End If
        End Sub

        Private Sub MarkedDGV_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
            ControlDown = e.Control
            ShiftDown = e.Shift
        End Sub
        Private Sub MarkedDGV_SelectionChanged(sender As Object, e As EventArgs) Handles Me.SelectionChanged
            If Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect AndAlso ControlDown AndAlso ShiftDown AndAlso selectedRowsCache IsNot Nothing Then
                For Each row As DataGridViewRow In selectedRowsCache
                    row.Selected = True
                Next
            End If
        End Sub
        Private Sub MarkedDGV_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Me.CellMouseDown
            If Me.SelectionMode = DataGridViewSelectionMode.FullRowSelect AndAlso Not ShiftDown Then
                selectedRowsCache = Me.SelectedRows
            End If
        End Sub

        Protected Overrides Function ProcessDataGridViewKey(e As KeyEventArgs) As Boolean
            Try
                Return MyBase.ProcessDataGridViewKey(e)
            Catch ex As Exception
                MessageBox.Show($"An error occured while processing the key event.{vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf} Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        Private Sub MarkedDGV_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles Me.CellFormatting
            If DontAutoFormat = True Then Exit Sub
            If e.FormattingApplied Then Exit Sub
            If TypeOf e.Value Is Double Then
                e.Value = TryFormat(CType(e.Value, Double), "shortprec2")
                e.FormattingApplied = True
            End If
        End Sub

        Public ReadOnly Property VScrollVisible As Boolean
            Get
                Dim visible = Me.Controls.OfType(Of VScrollBar).Any(Function(v) v.Visible)
                Return visible
            End Get
        End Property
        Public ReadOnly Property HScrollVisible As Boolean
            Get
                Dim visible = Me.Controls.OfType(Of HScrollBar).Any(Function(v) v.Visible)
                Return visible
            End Get
        End Property
    End Class
End Namespace
