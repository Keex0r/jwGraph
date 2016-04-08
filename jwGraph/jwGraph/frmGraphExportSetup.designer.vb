Imports jwGraph.GeneralTools
Namespace jwGraph
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmGraphExportSetup
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGraphExportSetup))
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.Panel4 = New System.Windows.Forms.Panel()
            Me.rbHeight = New System.Windows.Forms.RadioButton()
            Me.rbWidth = New System.Windows.Forms.RadioButton()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.cbLockAR = New System.Windows.Forms.CheckBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.tbWidth = New NumericInputbox()
            Me.tbHeight = New NumericInputbox()
            Me.rbCustom = New System.Windows.Forms.RadioButton()
            Me.rbPreDef = New System.Windows.Forms.RadioButton()
            Me.gbResolutions = New System.Windows.Forms.ComboBox()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.cbClearBack = New System.Windows.Forms.CheckBox()
            Me.Panel3 = New System.Windows.Forms.Panel()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.rbScaleImage = New System.Windows.Forms.RadioButton()
            Me.rbScaleDiagram = New System.Windows.Forms.RadioButton()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.rbToClip = New System.Windows.Forms.RadioButton()
            Me.rbToFile = New System.Windows.Forms.RadioButton()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOkSingle = New System.Windows.Forms.Button()
            Me.GroupBox3 = New System.Windows.Forms.GroupBox()
            Me.btnEditLegend = New System.Windows.Forms.Button()
            Me.tbY2 = New System.Windows.Forms.TextBox()
            Me.cbY2 = New System.Windows.Forms.CheckBox()
            Me.tbY1 = New System.Windows.Forms.TextBox()
            Me.cbY1 = New System.Windows.Forms.CheckBox()
            Me.tbX1 = New System.Windows.Forms.TextBox()
            Me.cbX1 = New System.Windows.Forms.CheckBox()
            Me.GroupBox4 = New System.Windows.Forms.GroupBox()
            Me.cbForceLegend = New System.Windows.Forms.CheckBox()
            Me.lblDidcopy = New System.Windows.Forms.Label()
            Me.tmrBlink = New System.Windows.Forms.Timer(Me.components)
            Me.tmrStopBlink = New System.Windows.Forms.Timer(Me.components)
            Me.GroupBox1.SuspendLayout()
            Me.Panel4.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.Panel3.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.GroupBox3.SuspendLayout()
            Me.GroupBox4.SuspendLayout()
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
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.Panel4)
            Me.GroupBox1.Controls.Add(Me.Label5)
            Me.GroupBox1.Controls.Add(Me.Panel1)
            Me.GroupBox1.Controls.Add(Me.rbCustom)
            Me.GroupBox1.Controls.Add(Me.rbPreDef)
            Me.GroupBox1.Controls.Add(Me.gbResolutions)
            Me.GroupBox1.ForeColor = System.Drawing.Color.MediumBlue
            Me.GroupBox1.Location = New System.Drawing.Point(41, 59)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(281, 168)
            Me.GroupBox1.TabIndex = 45
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Image size"
            '
            'Panel4
            '
            Me.Panel4.Controls.Add(Me.rbHeight)
            Me.Panel4.Controls.Add(Me.rbWidth)
            Me.Panel4.Location = New System.Drawing.Point(14, 142)
            Me.Panel4.Name = "Panel4"
            Me.Panel4.Size = New System.Drawing.Size(255, 19)
            Me.Panel4.TabIndex = 6
            '
            'rbHeight
            '
            Me.rbHeight.AutoSize = True
            Me.rbHeight.Location = New System.Drawing.Point(58, 0)
            Me.rbHeight.Name = "rbHeight"
            Me.rbHeight.Size = New System.Drawing.Size(56, 17)
            Me.rbHeight.TabIndex = 1
            Me.rbHeight.Text = "Height"
            Me.rbHeight.UseVisualStyleBackColor = True
            '
            'rbWidth
            '
            Me.rbWidth.AutoSize = True
            Me.rbWidth.Checked = True
            Me.rbWidth.Location = New System.Drawing.Point(0, 0)
            Me.rbWidth.Name = "rbWidth"
            Me.rbWidth.Size = New System.Drawing.Size(53, 17)
            Me.rbWidth.TabIndex = 0
            Me.rbWidth.TabStop = True
            Me.rbWidth.Text = "Width"
            Me.rbWidth.UseVisualStyleBackColor = True
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(11, 124)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(207, 13)
            Me.Label5.TabIndex = 5
            Me.Label5.Text = "Precise value, when scaling proportionally:"
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.cbLockAR)
            Me.Panel1.Controls.Add(Me.Label1)
            Me.Panel1.Controls.Add(Me.Label2)
            Me.Panel1.Controls.Add(Me.tbWidth)
            Me.Panel1.Controls.Add(Me.tbHeight)
            Me.Panel1.Location = New System.Drawing.Point(96, 40)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(179, 75)
            Me.Panel1.TabIndex = 4
            '
            'cbLockAR
            '
            Me.cbLockAR.AutoSize = True
            Me.cbLockAR.Location = New System.Drawing.Point(27, 54)
            Me.cbLockAR.Name = "cbLockAR"
            Me.cbLockAR.Size = New System.Drawing.Size(108, 17)
            Me.cbLockAR.TabIndex = 7
            Me.cbLockAR.Text = "Lock aspect ratio"
            Me.cbLockAR.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(6, 5)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(38, 13)
            Me.Label1.TabIndex = 5
            Me.Label1.Text = "Width:"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(3, 31)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(41, 13)
            Me.Label2.TabIndex = 6
            Me.Label2.Text = "Height:"
            '
            'tbWidth
            '
            Me.tbWidth.ChangeBackcolor = False
            Me.tbWidth.ChangeForecolor = True
            Me.tbWidth.ForeColor = System.Drawing.Color.Green
            Me.tbWidth.FormatString = "G"
            Me.tbWidth.InvalidBackColor = System.Drawing.Color.PaleVioletRed
            Me.tbWidth.InvalidForeColor = System.Drawing.Color.Red
            Me.tbWidth.Location = New System.Drawing.Point(50, 2)
            Me.tbWidth.Maximum = 10000000.0R
            Me.tbWidth.Minimum = -10000000.0R
            Me.tbWidth.Name = "tbWidth"
            Me.tbWidth.Size = New System.Drawing.Size(126, 20)
            Me.tbWidth.TabIndex = 5
            Me.tbWidth.Text = "1280"
            Me.tbWidth.ValidBackColor = System.Drawing.Color.White
            Me.tbWidth.ValidForeColor = System.Drawing.Color.Green
            Me.tbWidth.Value = 1280.0R
            '
            'tbHeight
            '
            Me.tbHeight.ChangeBackcolor = False
            Me.tbHeight.ChangeForecolor = True
            Me.tbHeight.ForeColor = System.Drawing.Color.Green
            Me.tbHeight.FormatString = "G"
            Me.tbHeight.InvalidBackColor = System.Drawing.Color.PaleVioletRed
            Me.tbHeight.InvalidForeColor = System.Drawing.Color.Red
            Me.tbHeight.Location = New System.Drawing.Point(50, 28)
            Me.tbHeight.Maximum = 10000000.0R
            Me.tbHeight.Minimum = -10000000.0R
            Me.tbHeight.Name = "tbHeight"
            Me.tbHeight.Size = New System.Drawing.Size(126, 20)
            Me.tbHeight.TabIndex = 6
            Me.tbHeight.Text = "1024"
            Me.tbHeight.ValidBackColor = System.Drawing.Color.White
            Me.tbHeight.ValidForeColor = System.Drawing.Color.Green
            Me.tbHeight.Value = 1024.0R
            '
            'rbCustom
            '
            Me.rbCustom.AutoSize = True
            Me.rbCustom.Location = New System.Drawing.Point(14, 43)
            Me.rbCustom.Name = "rbCustom"
            Me.rbCustom.Size = New System.Drawing.Size(63, 17)
            Me.rbCustom.TabIndex = 3
            Me.rbCustom.Text = "Custom:"
            Me.rbCustom.UseVisualStyleBackColor = True
            '
            'rbPreDef
            '
            Me.rbPreDef.AutoSize = True
            Me.rbPreDef.Checked = True
            Me.rbPreDef.Location = New System.Drawing.Point(14, 14)
            Me.rbPreDef.Name = "rbPreDef"
            Me.rbPreDef.Size = New System.Drawing.Size(82, 17)
            Me.rbPreDef.TabIndex = 2
            Me.rbPreDef.TabStop = True
            Me.rbPreDef.Text = "Pre-defined:"
            Me.rbPreDef.UseVisualStyleBackColor = True
            '
            'gbResolutions
            '
            Me.gbResolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.gbResolutions.FormattingEnabled = True
            Me.gbResolutions.Items.AddRange(New Object() {"Current size", "320x240 (4:3)", "640x480 (4:3)", "800x600 (4:3)", "1024x768 (4:3)", "1280x720 (720p, 16:9)", "1280x800 (16:10)", "1280x1024 (4:3)", "1600x900 (16:9)", "1600x1200 (4:3)", "1680x1050 (16:10)", "1920x1080 (1080p, 16:9)", "2048x1536 (4:3)"})
            Me.gbResolutions.Location = New System.Drawing.Point(96, 13)
            Me.gbResolutions.Name = "gbResolutions"
            Me.gbResolutions.Size = New System.Drawing.Size(135, 21)
            Me.gbResolutions.TabIndex = 1
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.cbClearBack)
            Me.GroupBox2.Controls.Add(Me.Panel3)
            Me.GroupBox2.Controls.Add(Me.Panel2)
            Me.GroupBox2.ForeColor = System.Drawing.Color.MediumBlue
            Me.GroupBox2.Location = New System.Drawing.Point(41, 233)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(281, 91)
            Me.GroupBox2.TabIndex = 46
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Export options"
            '
            'cbClearBack
            '
            Me.cbClearBack.AutoSize = True
            Me.cbClearBack.Location = New System.Drawing.Point(14, 65)
            Me.cbClearBack.Name = "cbClearBack"
            Me.cbClearBack.Size = New System.Drawing.Size(114, 17)
            Me.cbClearBack.TabIndex = 9
            Me.cbClearBack.Text = "White background"
            Me.cbClearBack.UseVisualStyleBackColor = True
            '
            'Panel3
            '
            Me.Panel3.Controls.Add(Me.Label4)
            Me.Panel3.Controls.Add(Me.rbScaleImage)
            Me.Panel3.Controls.Add(Me.rbScaleDiagram)
            Me.Panel3.Location = New System.Drawing.Point(6, 41)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(225, 18)
            Me.Panel3.TabIndex = 8
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(5, 1)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(37, 13)
            Me.Label4.TabIndex = 6
            Me.Label4.Text = "Scale:"
            '
            'rbScaleImage
            '
            Me.rbScaleImage.AutoSize = True
            Me.rbScaleImage.Checked = True
            Me.rbScaleImage.Location = New System.Drawing.Point(69, -1)
            Me.rbScaleImage.Name = "rbScaleImage"
            Me.rbScaleImage.Size = New System.Drawing.Size(54, 17)
            Me.rbScaleImage.TabIndex = 4
            Me.rbScaleImage.TabStop = True
            Me.rbScaleImage.Text = "Image"
            Me.rbScaleImage.UseVisualStyleBackColor = True
            '
            'rbScaleDiagram
            '
            Me.rbScaleDiagram.AutoSize = True
            Me.rbScaleDiagram.Location = New System.Drawing.Point(144, -1)
            Me.rbScaleDiagram.Name = "rbScaleDiagram"
            Me.rbScaleDiagram.Size = New System.Drawing.Size(64, 17)
            Me.rbScaleDiagram.TabIndex = 5
            Me.rbScaleDiagram.Text = "Diagram"
            Me.rbScaleDiagram.UseVisualStyleBackColor = True
            '
            'Panel2
            '
            Me.Panel2.Controls.Add(Me.Label3)
            Me.Panel2.Controls.Add(Me.rbToClip)
            Me.Panel2.Controls.Add(Me.rbToFile)
            Me.Panel2.Location = New System.Drawing.Point(6, 19)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(225, 18)
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
            Me.btnCancel.Location = New System.Drawing.Point(184, 472)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(105, 28)
            Me.btnCancel.TabIndex = 48
            Me.btnCancel.Text = "Close"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnOkSingle
            '
            Me.btnOkSingle.Image = My.Resources.Resources.ArrowSingleRightSmall
            Me.btnOkSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnOkSingle.Location = New System.Drawing.Point(73, 472)
            Me.btnOkSingle.Name = "btnOkSingle"
            Me.btnOkSingle.Size = New System.Drawing.Size(105, 28)
            Me.btnOkSingle.TabIndex = 47
            Me.btnOkSingle.Text = "Export"
            Me.btnOkSingle.UseVisualStyleBackColor = True
            '
            'GroupBox3
            '
            Me.GroupBox3.Controls.Add(Me.btnEditLegend)
            Me.GroupBox3.Controls.Add(Me.tbY2)
            Me.GroupBox3.Controls.Add(Me.cbY2)
            Me.GroupBox3.Controls.Add(Me.tbY1)
            Me.GroupBox3.Controls.Add(Me.cbY1)
            Me.GroupBox3.Controls.Add(Me.tbX1)
            Me.GroupBox3.Controls.Add(Me.cbX1)
            Me.GroupBox3.ForeColor = System.Drawing.Color.MediumBlue
            Me.GroupBox3.Location = New System.Drawing.Point(41, 330)
            Me.GroupBox3.Name = "GroupBox3"
            Me.GroupBox3.Size = New System.Drawing.Size(281, 71)
            Me.GroupBox3.TabIndex = 49
            Me.GroupBox3.TabStop = False
            Me.GroupBox3.Text = "Override axis and legend titles"
            '
            'btnEditLegend
            '
            Me.btnEditLegend.Location = New System.Drawing.Point(142, 42)
            Me.btnEditLegend.Name = "btnEditLegend"
            Me.btnEditLegend.Size = New System.Drawing.Size(130, 23)
            Me.btnEditLegend.TabIndex = 8
            Me.btnEditLegend.Text = "Edit Legend"
            Me.btnEditLegend.UseVisualStyleBackColor = True
            '
            'tbY2
            '
            Me.tbY2.Enabled = False
            Me.tbY2.Location = New System.Drawing.Point(190, 20)
            Me.tbY2.Name = "tbY2"
            Me.tbY2.Size = New System.Drawing.Size(82, 20)
            Me.tbY2.TabIndex = 7
            '
            'cbY2
            '
            Me.cbY2.AutoSize = True
            Me.cbY2.Location = New System.Drawing.Point(142, 22)
            Me.cbY2.Name = "cbY2"
            Me.cbY2.Size = New System.Drawing.Size(42, 17)
            Me.cbY2.TabIndex = 6
            Me.cbY2.Text = "Y2:"
            Me.cbY2.UseVisualStyleBackColor = True
            '
            'tbY1
            '
            Me.tbY1.Enabled = False
            Me.tbY1.Location = New System.Drawing.Point(53, 42)
            Me.tbY1.Name = "tbY1"
            Me.tbY1.Size = New System.Drawing.Size(82, 20)
            Me.tbY1.TabIndex = 3
            '
            'cbY1
            '
            Me.cbY1.AutoSize = True
            Me.cbY1.Location = New System.Drawing.Point(14, 45)
            Me.cbY1.Name = "cbY1"
            Me.cbY1.Size = New System.Drawing.Size(42, 17)
            Me.cbY1.TabIndex = 2
            Me.cbY1.Text = "Y1:"
            Me.cbY1.UseVisualStyleBackColor = True
            '
            'tbX1
            '
            Me.tbX1.Enabled = False
            Me.tbX1.Location = New System.Drawing.Point(53, 20)
            Me.tbX1.Name = "tbX1"
            Me.tbX1.Size = New System.Drawing.Size(82, 20)
            Me.tbX1.TabIndex = 1
            '
            'cbX1
            '
            Me.cbX1.AutoSize = True
            Me.cbX1.Location = New System.Drawing.Point(14, 22)
            Me.cbX1.Name = "cbX1"
            Me.cbX1.Size = New System.Drawing.Size(36, 17)
            Me.cbX1.TabIndex = 0
            Me.cbX1.Text = "X:"
            Me.cbX1.UseVisualStyleBackColor = True
            '
            'GroupBox4
            '
            Me.GroupBox4.Controls.Add(Me.cbForceLegend)
            Me.GroupBox4.ForeColor = System.Drawing.Color.MediumBlue
            Me.GroupBox4.Location = New System.Drawing.Point(41, 407)
            Me.GroupBox4.Name = "GroupBox4"
            Me.GroupBox4.Size = New System.Drawing.Size(281, 45)
            Me.GroupBox4.TabIndex = 50
            Me.GroupBox4.TabStop = False
            Me.GroupBox4.Text = "Legend"
            '
            'cbForceLegend
            '
            Me.cbForceLegend.AutoSize = True
            Me.cbForceLegend.Location = New System.Drawing.Point(14, 19)
            Me.cbForceLegend.Name = "cbForceLegend"
            Me.cbForceLegend.Size = New System.Drawing.Size(115, 17)
            Me.cbForceLegend.TabIndex = 0
            Me.cbForceLegend.Text = "Force legend open"
            Me.cbForceLegend.UseVisualStyleBackColor = True
            '
            'lblDidcopy
            '
            Me.lblDidcopy.AutoSize = True
            Me.lblDidcopy.Location = New System.Drawing.Point(115, 456)
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
            'frmGraphExportSetup
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.LightSteelBlue
            Me.ClientSize = New System.Drawing.Size(362, 516)
            Me.Controls.Add(Me.lblDidcopy)
            Me.Controls.Add(Me.GroupBox4)
            Me.Controls.Add(Me.GroupBox3)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOkSingle)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.lblTitle)
            Me.ForeColor = System.Drawing.Color.MediumBlue
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmGraphExportSetup"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Graph Image Export"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.Panel4.ResumeLayout(False)
            Me.Panel4.PerformLayout()
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.Panel3.ResumeLayout(False)
            Me.Panel3.PerformLayout()
            Me.Panel2.ResumeLayout(False)
            Me.Panel2.PerformLayout()
            Me.GroupBox3.ResumeLayout(False)
            Me.GroupBox3.PerformLayout()
            Me.GroupBox4.ResumeLayout(False)
            Me.GroupBox4.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents gbResolutions As System.Windows.Forms.ComboBox
        Friend WithEvents rbPreDef As System.Windows.Forms.RadioButton
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents cbLockAR As System.Windows.Forms.CheckBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents tbWidth As NumericInputbox
        Friend WithEvents tbHeight As NumericInputbox
        Friend WithEvents rbCustom As System.Windows.Forms.RadioButton
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents rbScaleImage As System.Windows.Forms.RadioButton
        Friend WithEvents rbScaleDiagram As System.Windows.Forms.RadioButton
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents rbToClip As System.Windows.Forms.RadioButton
        Friend WithEvents rbToFile As System.Windows.Forms.RadioButton
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOkSingle As System.Windows.Forms.Button
        Friend WithEvents cbClearBack As System.Windows.Forms.CheckBox
        Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
        Friend WithEvents tbY2 As System.Windows.Forms.TextBox
        Friend WithEvents cbY2 As System.Windows.Forms.CheckBox
        Friend WithEvents tbY1 As System.Windows.Forms.TextBox
        Friend WithEvents cbY1 As System.Windows.Forms.CheckBox
        Friend WithEvents tbX1 As System.Windows.Forms.TextBox
        Friend WithEvents cbX1 As System.Windows.Forms.CheckBox
        Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
        Friend WithEvents cbForceLegend As System.Windows.Forms.CheckBox
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents rbHeight As System.Windows.Forms.RadioButton
        Friend WithEvents rbWidth As System.Windows.Forms.RadioButton
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents lblDidcopy As Label
        Friend WithEvents tmrBlink As Timer
        Friend WithEvents tmrStopBlink As Timer
        Friend WithEvents btnEditLegend As Button
    End Class
End Namespace