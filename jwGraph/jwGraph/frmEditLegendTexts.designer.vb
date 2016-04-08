Imports jwGraph.GeneralTools
Namespace jwGraph
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmEditLegendTexts
        Inherits System.Windows.Forms.Form

        'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Wird vom Windows Form-Designer benötigt.
        Private components As System.ComponentModel.IContainer

        'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
        'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
        'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditLegendTexts))
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.tmrBlink = New System.Windows.Forms.Timer(Me.components)
            Me.tmrStopBlink = New System.Windows.Forms.Timer(Me.components)
            Me.dgvTexts = New DataGridView()
            CType(Me.dgvTexts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblTitle
            '
            Me.lblTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTitle.BackColor = System.Drawing.Color.GhostWhite
            Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
            Me.lblTitle.ForeColor = System.Drawing.Color.MediumBlue
            Me.lblTitle.Location = New System.Drawing.Point(12, 9)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(338, 36)
            Me.lblTitle.TabIndex = 44
            Me.lblTitle.Text = "Please adjust the legend entries"
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnOk
            '
            Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOk.Image = My.Resources.Resources.ArrowSingleRightSmall
            Me.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnOk.Location = New System.Drawing.Point(129, 274)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(105, 28)
            Me.btnOk.TabIndex = 47
            Me.btnOk.Text = "OK"
            Me.btnOk.UseVisualStyleBackColor = True
            '
            'tmrBlink
            '
            Me.tmrBlink.Interval = 150
            '
            'tmrStopBlink
            '
            Me.tmrStopBlink.Interval = 3000
            '
            'MarkedDGV1
            '
            Me.dgvTexts.AllowDrop = True
            Me.dgvTexts.AllowUserToResizeRows = False
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue
            DataGridViewCellStyle1.ForeColor = System.Drawing.Color.MediumBlue
            Me.dgvTexts.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            Me.dgvTexts.BackgroundColor = System.Drawing.Color.GhostWhite
            Me.dgvTexts.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
            Me.dgvTexts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.GhostWhite
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle2.ForeColor = System.Drawing.Color.MediumBlue
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvTexts.DefaultCellStyle = DataGridViewCellStyle2
            Me.dgvTexts.Location = New System.Drawing.Point(12, 48)
            Me.dgvTexts.Name = "dgvTexts"
            Me.dgvTexts.Size = New System.Drawing.Size(338, 220)
            Me.dgvTexts.TabIndex = 49
            '
            'frmEditLegendTexts
            '
            Me.AcceptButton = Me.btnOk
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.LightSteelBlue
            Me.ClientSize = New System.Drawing.Size(362, 314)
            Me.Controls.Add(Me.dgvTexts)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.lblTitle)
            Me.ForeColor = System.Drawing.Color.MediumBlue
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmEditLegendTexts"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Graph image export"
            CType(Me.dgvTexts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents tmrBlink As Timer
        Friend WithEvents tmrStopBlink As Timer
        Friend WithEvents dgvTexts As DataGridView
    End Class
End Namespace