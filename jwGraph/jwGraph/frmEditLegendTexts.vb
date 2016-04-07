Namespace jwGraph
    Public Class frmEditLegendTexts
        Public Overloads Shared Function ShowDialog(Owner As IWin32Window, Titles As List(Of frmGraphExportSetup.LegendText)) As DialogResult
            Using frm As New frmEditLegendTexts
                frm.MarkedDGV1.AutoGenerateColumns = True
                frm.MarkedDGV1.DataSource = Titles
                Return frm.ShowDialog(Owner)
            End Using
        End Function
    End Class
End Namespace
