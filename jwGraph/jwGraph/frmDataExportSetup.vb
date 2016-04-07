Imports jwGraph.GeneralTools
Namespace jwGraph
    Public Class frmDataExportSetup
        Private workingChart As jwGraph

        Public Shadows Sub ShowDialog(Owner As System.Windows.Forms.IWin32Window, ByRef WorkOnChart As jwGraph)
            workingChart = WorkOnChart
            MyBase.ShowDialog(Owner)
        End Sub

        Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
            Dim tr As New TabbedExporter.TabbedExporter()
            Dim useheaderName As Boolean = cbHeaderName.Checked
            Dim useheaderAxis As Boolean = cbHeaderAxis.Checked
            Dim xtitle As String = workingChart.XAxis.Title
            Dim y1title As String = workingChart.Y1Axis.Title
            Dim y2title As String = workingChart.Y2Axis.Title
            Dim xLog As Boolean = workingChart.XAxis.ShowLogarithmic And rbAsOriginal.Checked
            Dim y1Log As Boolean = workingChart.Y1Axis.ShowLogarithmic And rbAsOriginal.Checked
            Dim y2Log As Boolean = workingChart.Y2Axis.ShowLogarithmic And rbAsOriginal.Checked

            Dim overallErrors As Boolean = cbErrors.Checked
            For Each s In workingChart.Series
                If s.Datapoints.Count = 0 Then Continue For
                Dim ShowXErr = overallErrors And s.DrawXErrors
                Dim ShowYErr = overallErrors And s.DrawYErrors

                Dim cx As New TabbedExporter.Column()
                Dim cx_e As New TabbedExporter.Column()
                Dim cy As New TabbedExporter.Column()
                Dim cy_e As New TabbedExporter.Column()
                If useheaderName Then
                    cx.Header.Add(s.LegendText)
                    cy.Header.Add(s.LegendText)
                    cx_e.Header.Add(s.LegendText & "; Error")
                    cy_e.Header.Add(s.LegendText & "; Error")
                End If
                If useheaderAxis Then
                    cx.Header.Add(xtitle)
                    cy.Header.Add(If(s.YAxisType = Axis.enumAxisLocation.Primary, y1title, y2title))
                    cx_e.Header.Add(xtitle)
                    cy_e.Header.Add(If(s.YAxisType = Axis.enumAxisLocation.Primary, y1title, y2title))
                End If
                Dim dology = If(s.YAxisType = Axis.enumAxisLocation.Primary, y1Log, y2Log)
                For Each d As Datapoint In s.Datapoints
                    Dim x, y, ye, xe As Double
                    If xLog Then
                        x = 10 ^ d.X
                        xe = d.XError * x * Math.Log(10)
                    Else
                        x = d.X
                        xe = d.XError
                    End If
                    If dology Then
                        y = 10 ^ d.Y
                        ye = d.YError * y * Math.Log(10)
                    Else
                        y = d.Y
                        ye = d.YError
                    End If

                    cx.Items.Add(x.ToString)
                    cy.Items.Add(y.ToString)
                    cx_e.Items.Add(xe.ToString)
                    cy_e.Items.Add(ye.ToString)
                Next
                tr.Columns.Add(cx)
                If ShowXErr Then tr.Columns.Add(cx_e)
                tr.Columns.Add(cy)
                If ShowYErr Then tr.Columns.Add(cy_e)
            Next

            If tr.DoExport(rbToClip.Checked) Then
                BlinkMessage("Data exported successfully.")
            Else
                MessageBox.Show("The export could not be completed successfully!" & vbCrLf & "Please try again.", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Sub

#Region "Blinking"
        Private Blink As Boolean = True
        Private Sub tmrBlink_Tick(sender As Object, e As EventArgs) Handles tmrBlink.Tick
            If Blink Then
                lblDidcopy.ForeColor = Color.Red
            Else
                lblDidcopy.ForeColor = Me.ForeColor
            End If
            Blink = Not Blink
        End Sub

        Private Sub tmrStopBlink_Tick(sender As Object, e As EventArgs) Handles tmrStopBlink.Tick
            tmrBlink.Stop()
            lblDidcopy.Visible = False
        End Sub

        Private Sub BlinkMessage(Text As String)
            Blink = True
            lblDidcopy.Text = Text
            lblDidcopy.Visible = True
            lblDidcopy.Left = CInt(Me.Width / 2 - lblDidcopy.Width / 2)
            tmrBlink.Start()
            tmrStopBlink.Start()
        End Sub
#End Region

    End Class
End Namespace
