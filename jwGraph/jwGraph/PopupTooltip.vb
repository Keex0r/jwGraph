Namespace GeneralTools
    Public Class PopupTooltip
        Inherits Form

        Public Sub New()
            Me.InitializeComponent()
            Me.ShowInTaskbar = False
        End Sub
        Private Shared Function GetInnerBounds(frm As Form) As Rectangle
            Dim deskbounds = frm.DesktopBounds
        End Function
        Public Shared Function ShowPopup(Owner As Form, Duration As Integer, Title As String, Text As String) As PopupTooltip
            Return ShowPopup(Owner, Duration, Title, Text, False)
        End Function
        Public Shared Function ShowPopup(Owner As Form, Duration As Integer, Title As String, Text As String, UseWideFormat As Boolean) As PopupTooltip
            Dim frm As New PopupTooltip
            If UseWideFormat Then
                frm.TableLayoutPanel1.SetColumnSpan(frm.lblText, 1)
                frm.TableLayoutPanel1.SetColumnSpan(frm.lblTitle, 1)
                frm.TableLayoutPanel1.SetCellPosition(frm.lblText, New TableLayoutPanelCellPosition(1, 0))
            End If
            frm.Owner = Owner
            frm.lblText.Text = Text
            frm.lblTitle.Text = Title
            frm.Size = frm.TableLayoutPanel1.GetPreferredSize(New Size(250, 200))
            frm.Duration = Duration
            frm.DoShow()
            frm.ActiveOwner = Owner
            frm.Location = Owner.PointToScreen(New Point(Owner.ClientRectangle.Left, Owner.ClientRectangle.Bottom - frm.Height))
            frm.ResizeHandler = Sub(x, y)
                                    frm.Location = Owner.PointToScreen(New Point(Owner.ClientRectangle.Left, Owner.ClientRectangle.Bottom - frm.Height))
                                End Sub
            AddHandler Owner.Move, frm.ResizeHandler
            Return frm
        End Function


        'Protected Overrides Sub OnPaint(e As PaintEventArgs)
        '    MyBase.OnPaint(e)

        'End Sub
        Public ActiveOwner As Form
        Public ResizeHandler As EventHandler

        Public Duration As Integer = 5
        Private WithEvents Timer As New Timer With {.Interval = Duration * 1000}
        Private WithEvents FadeInTimer As New Timer With {.Interval = 1}
        Private WithEvents FadeOutTimer As New Timer With {.Interval = 1}
        Friend WithEvents btnClose As NoSelectButton
        Private WithEvents TryCloseTimer As New Timer With {.Interval = 1000}

        Public Sub DoShow()
            Timer.Interval = Me.Duration * 1000
            Timer.Start()
            AddHandler Timer.Tick, Sub()
                                       FadeOutTimer.Start()
                                   End Sub
            Me.Opacity = 0
            AddHandler FadeInTimer.Tick, Sub()
                                             Me.Opacity = Math.Min(1, Me.Opacity + 0.02)
                                             If Me.Opacity >= 1 Then FadeInTimer.Stop()
                                         End Sub
            AddHandler FadeOutTimer.Tick, Sub()
                                              Me.Opacity = Math.Max(0, Me.Opacity - 0.02)
                                              If Me.Opacity <= 0 Then
                                                  PerformClose()
                                              End If
                                          End Sub
            Me.Show()
            FadeInTimer.Start()
        End Sub
        Private Sub PerformClose()
            FadeOutTimer.Stop()
            RemoveHandler ActiveOwner.Move, ResizeHandler
            Timer.Dispose()
            FadeInTimer.Dispose()
            FadeOutTimer.Dispose()

            If (Me.Owner IsNot Nothing AndAlso Not Me.Owner.IsDisposed AndAlso Me.Owner.Visible AndAlso Me.Owner.CanFocus = False) Then
                TryCloseTimer.Start()
            Else
                Me.Close()
            End If
        End Sub
        Private Sub TryCloseTick(sender As Object, e As EventArgs) Handles TryCloseTimer.Tick
            If Me.Owner Is Nothing OrElse Me.Owner.IsDisposed Then
                Me.Close()
            Else
                If Not (Me.Owner.Visible AndAlso Me.Owner.CanFocus = False) Then
                    TryCloseTimer.Stop()
                    Me.Close()
                End If
            End If

        End Sub


        Friend WithEvents lblText As System.Windows.Forms.Label
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel


#Region "Initialize Component"
        Public Sub InitializeComponent()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.lblText = New System.Windows.Forms.Label()
            Me.btnClose = New NoSelectButton()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.AutoSize = True
            Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.TableLayoutPanel1.BackColor = System.Drawing.Color.LightGoldenrodYellow
            Me.TableLayoutPanel1.ColumnCount = 3
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.TableLayoutPanel1.Controls.Add(Me.lblTitle, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.lblText, 0, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.btnClose, 2, 0)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(20, 6)
            Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(259, 235)
            Me.TableLayoutPanel1.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New System.Drawing.Font("Cambria", 11.0!, System.Drawing.FontStyle.Underline)
            Me.lblTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblTitle.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(37, 17)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Title"
            '
            'lblText
            '
            Me.lblText.AutoSize = True
            Me.TableLayoutPanel1.SetColumnSpan(Me.lblText, 2)
            Me.lblText.Font = New System.Drawing.Font("Cambria", 9.0!)
            Me.lblText.Location = New System.Drawing.Point(0, 23)
            Me.lblText.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
            Me.lblText.MaximumSize = New System.Drawing.Size(250, 0)
            Me.lblText.Name = "lblText"
            Me.lblText.Size = New System.Drawing.Size(29, 14)
            Me.lblText.TabIndex = 1
            Me.lblText.Text = "Text"
            '
            'btnClose
            '
            Me.btnClose.BackColor = System.Drawing.Color.OrangeRed
            Me.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black
            Me.btnClose.FlatAppearance.BorderSize = 0
            Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClose.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClose.ForeColor = System.Drawing.Color.White
            Me.btnClose.Location = New System.Drawing.Point(242, 3)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(14, 14)
            Me.btnClose.TabIndex = 2
            Me.btnClose.UseCompatibleTextRendering = True
            Me.btnClose.UseVisualStyleBackColor = False
            '
            'PopupTooltip
            '
            Me.AutoSize = True
            Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.BackColor = System.Drawing.Color.HotPink
            Me.ClientSize = New System.Drawing.Size(284, 261)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "PopupTooltip"
            Me.Padding = New System.Windows.Forms.Padding(20, 6, 5, 20)
            Me.TransparencyKey = System.Drawing.Color.HotPink
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
#End Region

        Private Sub PopupTooltip_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

            Dim r = TableLayoutPanel1.Bounds
            r.Inflate(New Size(5, 5))
            'r.Location = New Point(r.Location.X + 1, r.Location.Y + 1)
            r.Size = New Size(r.Width - 1, r.Height - 1)
            Dim p As New Drawing2D.GraphicsPath
            p.AddLine(20, Me.ClientRectangle.Bottom - 30, 0, Me.ClientRectangle.Bottom)
            p.AddLine(0, Me.ClientRectangle.Bottom, 30, Me.ClientRectangle.Bottom - 20)
            p.CloseAllFigures()
            '    e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            '   e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.Bicubic
            Using br As New SolidBrush(Color.Black) 'Drawing2D.LinearGradientBrush(New Point(0, Me.ClientRectangle.Bottom), New Point(30, Me.ClientRectangle.Bottom - 30), Color.Blue, Color.Red)
                e.Graphics.FillPath(br, p)
            End Using
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            DrawGradientRectangle(e.Graphics, r, 5, Color.Black, Me.TableLayoutPanel1.BackColor)

        End Sub

        Private Sub btnClose_Paint(sender As Object, e As PaintEventArgs) Handles btnClose.Paint
            Dim s = e.Graphics.SmoothingMode
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            Using p As New Pen(Brushes.White, 2)
                Const ab = 4
                e.Graphics.DrawLine(p, ab, ab, btnClose.ClientSize.Width - ab - 1, btnClose.ClientSize.Height - ab - 1)
                e.Graphics.DrawLine(p, btnClose.ClientSize.Width - ab - 1, ab, ab, btnClose.ClientSize.Height - ab - 1)
            End Using
        End Sub

        Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click, lblText.Click, lblTitle.Click, TableLayoutPanel1.Click
            Timer.Stop()
            FadeOutTimer.Stop()
            FadeInTimer.Stop()
            PerformClose()
        End Sub
    End Class
End Namespace
