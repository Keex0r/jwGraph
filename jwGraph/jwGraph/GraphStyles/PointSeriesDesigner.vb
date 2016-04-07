Imports jwGraph.GeneralTools
Namespace jwGraph

    Public Class PointSeriesDesigner
        Public Shared Sub Paint(ByRef g As Graphics, Series As Series, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
            If Series.Data Is Nothing OrElse Series.Data.Count = 0 Then Exit Sub
            '    g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            For Each d As Datapoint In Series.Data
                '  If Not d.DatapointInrange(XAxis, YAxis) Then Continue For
                If d.IsOKVal = False Then Continue For
                Dim X As Single = XAxis.ValueToPixelPosition(d.X)
                Dim Y As Single = YAxis.ValueToPixelPosition(d.Y)
                'enforce hard limits for drawing coordinates
                If jwGraph.IsOK(X) = False OrElse jwGraph.IsOK(Y) = False Then Continue For
                X = Math.Min(Math.Max(X, -1.0E+9F), 1.0E+9F)
                Y = Math.Min(Math.Max(Y, -1.0E+9F), 1.0E+9F)
                PointStyles.DrawPoint(g, Series, X, Y)
                If d.IsMarked Then
                    Dim size As Single = CSng(Series.MarkerSize / 1.2)
                    g.DrawLine(Pens.Green, X - size, Y - size, X + size, Y + size)
                    g.DrawLine(Pens.Green, X - size, Y + size, X + size, Y - size)
                    g.DrawLine(Pens.Green, X - size, Y, X + size, Y)
                    g.DrawLine(Pens.Green, X, Y + size, X, Y - size)
                End If
            Next
        End Sub
        Public Shared Sub PaintLegend(ByRef g As Graphics, Series As Series, Bounds As RectangleF)
            '   g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            PointStyles.DrawPoint(g, Series, (Bounds.Left + Bounds.Right) / 2, (Bounds.Top + Bounds.Bottom) / 2)
        End Sub
    End Class
End Namespace
