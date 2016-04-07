Namespace jwGraph

    Public Class YErrorBars
        Public Shared Sub Paint(ByRef g As Graphics, Series As Series, XAxis As HorizontalAxis, YAxis As VerticalAxis)
            If Series.Data Is Nothing OrElse Series.Data.Count = 0 Then Exit Sub
            Dim w As Integer = CInt((Series.ErrorCapWidth - 1) / 2)
            ' g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            For Each d As Datapoint In Series.Data
                If d.IsOKYErr = False OrElse d.YError <= 0 Then Continue For
                Dim X As Double = XAxis.ValueToPixelPosition(d.X)
                Dim Y As Double = YAxis.ValueToPixelPosition(d.Y)
                Dim YErr1 As Double = YAxis.ValueToPixelPosition(d.Y - d.YError)
                Dim YErr2 As Double = YAxis.ValueToPixelPosition(d.Y + d.YError)
                g.DrawLine(Series.ErrorPen, CInt(X), CInt(YErr1), CInt(X), CInt(YErr2))
                g.DrawLine(Series.ErrorPen, CInt(X - w), CInt(YErr1), CInt(X + w), CInt(YErr1))
                g.DrawLine(Series.ErrorPen, CInt(X - w), CInt(YErr2), CInt(X + w), CInt(YErr2))
            Next
        End Sub
    End Class

    Public Class XErrorBars
        Public Shared Sub Paint(ByRef g As Graphics, Series As Series, XAxis As HorizontalAxis, YAxis As VerticalAxis)
            If Series.Data Is Nothing OrElse Series.Data.Count = 0 Then Exit Sub
            Dim w As Integer = CInt((Series.ErrorCapWidth - 1) / 2)
            g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            For Each d As Datapoint In Series.Data
                If d.IsOKXErr = False OrElse d.XError <= 0 Then Continue For
                Dim X As Double = XAxis.ValueToPixelPosition(d.X)
                Dim Y As Double = YAxis.ValueToPixelPosition(d.Y)
                Dim XErr1 As Double = XAxis.ValueToPixelPosition(d.X - d.XError)
                Dim XErr2 As Double = XAxis.ValueToPixelPosition(d.X + d.XError)

                g.DrawLine(Series.ErrorPen, CInt(XErr1), CInt(Y), CInt(XErr2), CInt(Y))
                g.DrawLine(Series.ErrorPen, CInt(XErr1), CInt(Y - w), CInt(XErr1), CInt(Y + w))
                g.DrawLine(Series.ErrorPen, CInt(XErr2), CInt(Y - w), CInt(XErr2), CInt(Y + w))
            Next
        End Sub
    End Class
End Namespace
