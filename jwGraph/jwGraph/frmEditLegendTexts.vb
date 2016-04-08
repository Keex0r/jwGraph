Namespace jwGraph
    Public Class frmEditLegendTexts
        Public Overloads Shared Function ShowDialog(Owner As IWin32Window, Titles As List(Of frmGraphExportSetup.LegendText)) As DialogResult
            Using frm As New frmEditLegendTexts
                frm.dgvTexts.AutoGenerateColumns = True
                frm.dgvTexts.DataSource = Titles
                Return frm.ShowDialog(Owner)
            End Using
        End Function
    End Class
End Namespace
