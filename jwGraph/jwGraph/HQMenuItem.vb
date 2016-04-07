Namespace GeneralTools
    Public Class HQMenuItem
        Inherits ToolStripMenuItem
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            MyBase.OnPaint(e)

        End Sub
    End Class
End Namespace
