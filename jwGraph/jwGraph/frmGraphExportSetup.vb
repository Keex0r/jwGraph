Namespace jwGraph
    Public Class frmGraphExportSetup
        Private workingChart As jwGraph
        Public Class LegendText
            Public Sub New(Entry As String, Title As String)
                _Entry = Entry
                Me.Title = Title
            End Sub
            Private _Entry As String
            Public ReadOnly Property Entry As String
                Get
                    Return _Entry
                End Get
            End Property
            Public Property Title As String
        End Class
        Private LegendTitles As List(Of LegendText)

        Public Shadows Sub ShowDialog(Owner As System.Windows.Forms.IWin32Window, ByRef WorkOnChart As jwGraph)
            workingChart = WorkOnChart
            LegendTitles = New List(Of LegendText)
            LegendTitles.Add(New LegendText("Legend Title", workingChart.LegendTitle))
            For Each s In workingChart.Series
                LegendTitles.Add(New LegendText(s.Name, s.LegendText))
            Next
            MyBase.ShowDialog(Owner)
        End Sub
        Private Function GetSize(index As Integer) As Size
            Select Case index
                Case 0 : Return workingChart.Size
                Case 1 : Return New Size(320, 240)
                Case 2 : Return New Size(640, 480)
                Case 3 : Return New Size(800, 600)
                Case 4 : Return New Size(1024, 768)
                Case 5 : Return New Size(1280, 720)
                Case 6 : Return New Size(1280, 800)
                Case 7 : Return New Size(1280, 1024)
                Case 8 : Return New Size(1600, 900)
                Case 9 : Return New Size(1600, 1200)
                Case 10 : Return New Size(1680, 1050)
                Case 11 : Return New Size(1920, 1080)
                Case 12 : Return New Size(2048, 1536)
                Case Else : Return New Size(800, 600)
            End Select
        End Function



        Private Sub frmGraphExportSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            If gbResolutions.SelectedIndex < 0 Then gbResolutions.SelectedIndex = 0

        End Sub
        Private Sub UpdateEnabled()
            Panel1.Enabled = rbCustom.Checked
            gbResolutions.Enabled = Not rbCustom.Checked
        End Sub
        Private Sub rbPreDef_CheckedChanged(sender As Object, e As EventArgs) Handles rbPreDef.CheckedChanged
            UpdateEnabled()
        End Sub

        Private Sub rbCustom_CheckedChanged(sender As Object, e As EventArgs) Handles rbCustom.CheckedChanged
            UpdateEnabled()
        End Sub
        Dim lastAR As Double = 4 / 3
        Private Sub tbWidth_Validated(sender As Object, e As EventArgs) Handles tbWidth.Validated
            If cbLockAR.Checked Then
                tbHeight.Value = Math.Round(tbWidth.Value / lastAR)
            End If
        End Sub

        Private Sub tbHeight_Validated(sender As Object, e As EventArgs) Handles tbHeight.Validated
            If cbLockAR.Checked Then
                tbWidth.Value = Math.Round(tbHeight.Value * lastAR)
            End If
        End Sub

        Private Sub cbLockAR_CheckedChanged(sender As Object, e As EventArgs) Handles cbLockAR.CheckedChanged
            lastAR = tbWidth.Value / tbHeight.Value
        End Sub
        Public Shared Function ScaleImage(image As Bitmap, maxWidth As Integer, maxHeight As Integer) As Bitmap
            Dim ratioX = CDbl(maxWidth) / image.Width
            Dim ratioY = CDbl(maxHeight) / image.Height
            Dim ratio = Math.Min(ratioX, ratioY)

            Dim newWidth = CInt(image.Width * ratio)
            Dim newHeight = CInt(image.Height * ratio)

            Dim newImage As New Bitmap(newWidth, newHeight)
            Using g As Graphics = Graphics.FromImage(newImage)
                g.DrawImage(image, 0, 0, newWidth, newHeight)
            End Using
            Dim resbmp As New Bitmap(maxWidth, maxHeight)
            Using g As Graphics = Graphics.FromImage(resbmp)
                g.DrawImageUnscaled(newImage, New Point(CInt(resbmp.Width / 2 - newImage.Width / 2), CInt(resbmp.Height / 2 - newImage.Height / 2)))
            End Using
            newImage.Dispose()
            Return resbmp
        End Function

        Private Function GetChartScaledImage(NewSize As Size) As Bitmap
            ' Dim bmp As New Bitmap(NewSize.Width, NewSize.Height) 'Create output bitmap
            With workingChart
                'Dim oldsize As Size = .Size 'Save chart's old size
                'Dim lastdock As DockStyle = .Dock 'Save chart's old docking style
                '.SuspendLayout()
                '.Dock = DockStyle.None 'Disable dock to allow resize
                '.Size = NewSize 'Set the temporary size
                '.DoAutoScale() 'Internal
                Return .GetGraphImage(NewSize, rbHeight.Checked)
                '  .DrawToBitmap(bmp, New Rectangle(0, 0, NewSize.Width, NewSize.Height)) 'Draw the image
                '.Dock = lastdock 'Recreate the old behaviour
                '.Size = oldsize 'Recreate the old size
                '.DoAutoScale() 'Internal
                '  .ResumeLayout()
            End With
            ' Return bmp
        End Function

        Private Function GetImageScaledImage(size As Size) As Bitmap
            Dim oldsize As Size = workingChart.Size
            Dim wantedaspectratio As Double = size.Width / size.Height
            Dim chartaspectratio As Double = oldsize.Width / oldsize.Height
            If wantedaspectratio < chartaspectratio Then
                workingChart.Width = CInt(wantedaspectratio * workingChart.Height)
            Else
                workingChart.Height = CInt(workingChart.Width / wantedaspectratio)
            End If
            workingChart.DoAutoScale()
            Dim tempbmp As New Bitmap(workingChart.Width, workingChart.Height)
            workingChart.DrawToBitmap(tempbmp, New Rectangle(0, 0, workingChart.Width, workingChart.Height))
            workingChart.Size = oldsize
            workingChart.DoAutoScale()

            Dim wantsize As Size = size
            Dim bmp As Bitmap = ScaleImage(tempbmp, wantsize.Width, wantsize.Height)
            tempbmp.Dispose()
            Return bmp
        End Function

        Private Sub btnOkSingle_Click(sender As Object, e As EventArgs) Handles btnOkSingle.Click
            'Set imagesize from options
            Dim imagesize As Size
            If rbPreDef.Checked Then
                imagesize = GetSize(gbResolutions.SelectedIndex)
            Else
                imagesize = New Size(CInt(tbWidth.Value), CInt(tbHeight.Value))
            End If
            workingChart.SuspendLayout()
            Dim lastcol1 As Color = workingChart.TopLeftColor
            Dim lastcol2 As Color = workingChart.BottomRightColor
            Dim lastAreaBack As Color = workingChart.GraphBackColor
            If cbClearBack.Checked Then
                workingChart.TopLeftColor = Color.White
                workingChart.BottomRightColor = Color.White
                workingChart.GraphBackColor = Color.White
            End If
            Dim lastX1 As String = workingChart.XAxis.Title
            Dim lastY1 As String = workingChart.Y1Axis.Title
            Dim lastY2 As String = workingChart.Y2Axis.Title
            Dim lastLeg As String = workingChart.LegendTitle
            If cbX1.Checked Then workingChart.XAxis.Title = tbX1.Text
            If cbY1.Checked Then workingChart.Y1Axis.Title = tbY1.Text
            If cbY2.Checked Then workingChart.Y2Axis.Title = tbY2.Text

            Dim prevtitles As New List(Of LegendText)
            For Each s In workingChart.Series
                prevtitles.Add(New LegendText(s.Name, s.LegendText))
                Dim newtext = (From l In LegendTitles Where l.Entry = s.Name Select l).FirstOrDefault
                If newtext IsNot Nothing Then s.LegendText = newtext.Title
            Next
            workingChart.LegendTitle = LegendTitles(0).Title

            Dim legforce = workingChart.LegendAlwaysVisible = True
            If cbForceLegend.Checked Then workingChart.LegendAlwaysVisible = True

            'Get image
            Dim resbmp As Bitmap
            If rbScaleImage.Checked Then
                resbmp = GetImageScaledImage(imagesize)
            Else
                resbmp = GetChartScaledImage(imagesize)
            End If

            workingChart.LegendAlwaysVisible = legforce
            workingChart.TopLeftColor = lastcol1
            workingChart.BottomRightColor = lastcol2
            workingChart.GraphBackColor = lastAreaBack
            If cbX1.Checked Then workingChart.XAxis.Title = lastX1
            If cbY1.Checked Then workingChart.Y1Axis.Title = lastY1
            If cbY2.Checked Then workingChart.Y2Axis.Title = lastY2
            For Each s In workingChart.Series
                Dim newtext = (From l In prevtitles Where l.Entry = s.Name Select l).FirstOrDefault
                If newtext IsNot Nothing Then s.LegendText = newtext.Title
            Next
            workingChart.LegendTitle = lastLeg
            workingChart.ResumeLayout()
            'Output the image
            If rbToClip.Checked Then
                Try
                    Clipboard.SetImage(resbmp)
                    BlinkMessage("Image copied to clipboard!")
                Catch ex As Exception
                    MessageBox.Show("The clipboard operation was not successful." & vbCrLf & "Please try again.", "Clipboard error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                Try
                    Dim sfd As New SaveFileDialog
                    sfd.Filter = "PNG images|*.png"
                    sfd.AddExtension = True
                    sfd.DefaultExt = ".png"
                    If sfd.ShowDialog(Me) = System.Windows.Forms.DialogResult.Cancel Then Exit Sub
                    resbmp.Save(sfd.FileName, Drawing.Imaging.ImageFormat.Png)
                    resbmp.Dispose()
                    Process.Start(sfd.FileName)
                    BlinkMessage("Image saved successfully!")
                Catch ex As Exception
                    MessageBox.Show("An error occured while saving the image:" & vbCrLf & vbCrLf & ex.Message, "Export error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If


        End Sub

        Private Sub cbX1_CheckedChanged(sender As Object, e As EventArgs) Handles cbX1.CheckedChanged
            tbX1.Enabled = cbX1.Enabled
        End Sub
        Private Sub cbY1_CheckedChanged(sender As Object, e As EventArgs) Handles cbY1.CheckedChanged
            tbY1.Enabled = cbY1.Enabled
        End Sub

        Private Sub cbY2_CheckedChanged(sender As Object, e As EventArgs) Handles cbY2.CheckedChanged
            tbY2.Enabled = cbY2.Enabled
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

        Private Sub btnEditLegend_Click(sender As Object, e As EventArgs) Handles btnEditLegend.Click
            frmEditLegendTexts.ShowDialog(Me.Owner, LegendTitles)
        End Sub
#End Region

    End Class
End Namespace
