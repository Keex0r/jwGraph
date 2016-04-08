Namespace jwGraph
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmDataExportSetup
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataExportSetup))
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.cbErrors = New System.Windows.Forms.CheckBox()
            Me.cbHeaderAxis = New System.Windows.Forms.CheckBox()
            Me.cbHeaderName = New System.Windows.Forms.CheckBox()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.rbToClip = New System.Windows.Forms.RadioButton()
            Me.rbToFile = New System.Windows.Forms.RadioButton()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnExport = New System.Windows.Forms.Button()
            Me.lblDidcopy = New System.Windows.Forms.Label()
            Me.tmrBlink = New System.Windows.Forms.Timer(Me.components)
            Me.tmrStopBlink = New System.Windows.Forms.Timer(Me.components)
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.rbAsLog = New System.Windows.Forms.RadioButton()
            Me.rbAsOriginal = New System.Windows.Forms.RadioButton()
            Me.GroupBox2.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
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
            Me.lblTitle.Text = "Please define export options"
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.cbErrors)
            Me.GroupBox2.Controls.Add(Me.cbHeaderAxis)
            Me.GroupBox2.Controls.Add(Me.cbHeaderName)
            Me.GroupBox2.Controls.Add(Me.Panel2)
            Me.GroupBox2.ForeColor = System.Drawing.Color.MediumBlue
            Me.GroupBox2.Location = New System.Drawing.Point(41, 59)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(281, 117)
            Me.GroupBox2.TabIndex = 46
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Export options"
            '
            'cbErrors
            '
            Me.cbErrors.AutoSize = True
            Me.cbErrors.Checked = True
            Me.cbErrors.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbErrors.Location = New System.Drawing.Point(6, 89)
            Me.cbErrors.Name = "cbErrors"
            Me.cbErrors.Size = New System.Drawing.Size(167, 17)
            Me.cbErrors.TabIndex = 9
            Me.cbErrors.Text = "Include error bars (if available)"
            Me.cbErrors.UseVisualStyleBackColor = True
            '
            'cbHeaderAxis
            '
            Me.cbHeaderAxis.AutoSize = True
            Me.cbHeaderAxis.Checked = True
            Me.cbHeaderAxis.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbHeaderAxis.Location = New System.Drawing.Point(6, 43)
            Me.cbHeaderAxis.Name = "cbHeaderAxis"
            Me.cbHeaderAxis.Size = New System.Drawing.Size(116, 17)
            Me.cbHeaderAxis.TabIndex = 8
            Me.cbHeaderAxis.Text = "Axis titles in header"
            Me.cbHeaderAxis.UseVisualStyleBackColor = True
            '
            'cbHeaderName
            '
            Me.cbHeaderName.AutoSize = True
            Me.cbHeaderName.Checked = True
            Me.cbHeaderName.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbHeaderName.Location = New System.Drawing.Point(6, 66)
            Me.cbHeaderName.Name = "cbHeaderName"
            Me.cbHeaderName.Size = New System.Drawing.Size(136, 17)
            Me.cbHeaderName.TabIndex = 0
            Me.cbHeaderName.Text = "Series names in header"
            Me.cbHeaderName.UseVisualStyleBackColor = True
            '
            'Panel2
            '
            Me.Panel2.Controls.Add(Me.Label3)
            Me.Panel2.Controls.Add(Me.rbToClip)
            Me.Panel2.Controls.Add(Me.rbToFile)
            Me.Panel2.Location = New System.Drawing.Point(6, 19)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(267, 18)
            Me.Panel2.TabIndex = 7
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(5, 1)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(58, 13)
            Me.Label3.TabIndex = 6
            Me.Label3.Text = "Export to..."
            '
            'rbToClip
            '
            Me.rbToClip.AutoSize = True
            Me.rbToClip.Checked = True
            Me.rbToClip.Location = New System.Drawing.Point(69, -1)
            Me.rbToClip.Name = "rbToClip"
            Me.rbToClip.Size = New System.Drawing.Size(69, 17)
            Me.rbToClip.TabIndex = 4
            Me.rbToClip.TabStop = True
            Me.rbToClip.Text = "Clipboard"
            Me.rbToClip.UseVisualStyleBackColor = True
            '
            'rbToFile
            '
            Me.rbToFile.AutoSize = True
            Me.rbToFile.Location = New System.Drawing.Point(144, -1)
            Me.rbToFile.Name = "rbToFile"
            Me.rbToFile.Size = New System.Drawing.Size(41, 17)
            Me.rbToFile.TabIndex = 5
            Me.rbToFile.Text = "File"
            Me.rbToFile.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Image = My.Resources.Resources.Cancel
            Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnCancel.Location = New System.Drawing.Point(184, 278)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(105, 28)
            Me.btnCancel.TabIndex = 48
            Me.btnCancel.Text = "Close"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnExport
            '
            Me.btnExport.Image = My.Resources.Resources.ArrowSingleRightSmall
            Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnExport.Location = New System.Drawing.Point(73, 278)
            Me.btnExport.Name = "btnExport"
            Me.btnExport.Size = New System.Drawing.Size(105, 28)
            Me.btnExport.TabIndex = 47
            Me.btnExport.Text = "Export"
            Me.btnExport.UseVisualStyleBackColor = True
            '
            'lblDidcopy
            '
            Me.lblDidcopy.AutoSize = True
            Me.lblDidcopy.Location = New System.Drawing.Point(115, 262)
            Me.lblDidcopy.Name = "lblDidcopy"
            Me.lblDidcopy.Size = New System.Drawing.Size(132, 13)
            Me.lblDidcopy.TabIndex = 51
            Me.lblDidcopy.Text = "Image copied to clipboard."
            Me.lblDidcopy.Visible = False
            '
            'tmrBlink
            '
            Me.tmrBlink.Interval = 150
            '
            'tmrStopBlink
            '
            Me.tmrStopBlink.Interval = 3000
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.rbAsLog)
            Me.GroupBox1.Controls.Add(Me.rbAsOriginal)
            Me.GroupBox1.ForeColor = System.Drawing.Color.MediumBlue
            Me.GroupBox1.Location = New System.Drawing.Point(41, 182)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(281, 69)
            Me.GroupBox1.TabIndex = 52
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Logarithmically plotted values"
            '
            'rbAsLog
            '
            Me.rbAsLog.AutoSize = True
            Me.rbAsLog.Location = New System.Drawing.Point(6, 42)
            Me.rbAsLog.Name = "rbAsLog"
            Me.rbAsLog.Size = New System.Drawing.Size(82, 17)
            Me.rbAsLog.TabIndex = 1
            Me.rbAsLog.Text = "As logarithm"
            Me.rbAsLog.UseVisualStyleBackColor = True
            '
            'rbAsOriginal
            '
            Me.rbAsOriginal.AutoSize = True
            Me.rbAsOriginal.Checked = True
            Me.rbAsOriginal.Location = New System.Drawing.Point(6, 19)
            Me.rbAsOriginal.Name = "rbAsOriginal"
            Me.rbAsOriginal.Size = New System.Drawing.Size(111, 17)
            Me.rbAsOriginal.TabIndex = 0
            Me.rbAsOriginal.TabStop = True
            Me.rbAsOriginal.Text = "As original number"
            Me.rbAsOriginal.UseVisualStyleBackColor = True
            '
            'frmDataExportSetup
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.LightSteelBlue
            Me.ClientSize = New System.Drawing.Size(362, 320)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.lblDidcopy)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnExport)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.lblTitle)
            Me.ForeColor = System.Drawing.Color.MediumBlue
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmDataExportSetup"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Raw Data Export"
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.Panel2.ResumeLayout(False)
            Me.Panel2.PerformLayout()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents rbToClip As System.Windows.Forms.RadioButton
        Friend WithEvents rbToFile As System.Windows.Forms.RadioButton
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnExport As System.Windows.Forms.Button
        Friend WithEvents cbHeaderName As System.Windows.Forms.CheckBox
        Friend WithEvents lblDidcopy As Label
        Friend WithEvents tmrBlink As Timer
        Friend WithEvents tmrStopBlink As Timer
        Friend WithEvents cbHeaderAxis As CheckBox
        Friend WithEvents cbErrors As CheckBox
        Friend WithEvents GroupBox1 As GroupBox
        Friend WithEvents rbAsLog As RadioButton
        Friend WithEvents rbAsOriginal As RadioButton
    End Class
End Namespace