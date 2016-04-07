Imports jwGraph.jwGraph
Namespace jwGraph
    Public Class HistrogramSeriesDesigner
        Public Shared Sub Paint(ByRef g As Graphics, Series As Series, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
            If Series.Data Is Nothing OrElse Series.Data.Count = 0 Then Exit Sub
            Dim MinX As Double = (From d As Datapoint In Series.Data Select d.X).Min
            Dim MaxX As Double = (From d As Datapoint In Series.Data Select d.X).Max
            Dim DWidth As Double = (MaxX - MinX) / Series.Data.Count

            Dim width As Single = Math.Max(CSng(XAxis.ValueDistanceToPixelDistance(DWidth) - Bounds.Width * 0.01), 1)
            Dim zeroY As Single = YAxis.ValueToPixelPosition(0)
            zeroY = Math.Min(Math.Max(zeroY, -1.0E+9F), 1.0E+9F)

            '  g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            For i = 0 To Series.Data.Count - 1
                If Series.Data(i).IsOKVal = False Then Continue For
                Dim X As Single = XAxis.ValueToPixelPosition(Series.Data(i).X) 'Middle
                Dim Y As Single = YAxis.ValueToPixelPosition(Series.Data(i).Y) 'Top
                'enforce hard limits for drawing coordinates
                If jwGraph.IsOK(X) = False OrElse jwGraph.IsOK(Y) = False Then Continue For
                X = Math.Min(Math.Max(X, -1.0E+9F), 1.0E+9F)
                Y = Math.Min(Math.Max(Y, -1.0E+9F), 1.0E+9F)

                Dim rect As RectangleF
                If zeroY <= Y Then
                    rect = New RectangleF((X - width / 2), zeroY, width, Math.Abs(Y - zeroY))
                Else
                    rect = New RectangleF((X - width / 2), Y, width, Math.Abs(Y - zeroY))
                End If

                'rect.Height += zeroY - rect.Bottom
                g.FillRectangle(Series.MarkerBrush, rect)
                g.DrawRectangle(Series.MarkerBorderPen, rect.X, rect.Y, rect.Width, rect.Height)
            Next
        End Sub
        Public Shared Sub PaintLegend(ByRef g As Graphics, Series As Series, Bounds As RectangleF)
            ' g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            Dim middleY As Double = (Bounds.Top + Bounds.Bottom) / 2
            g.DrawLine(Series.LinePen, Bounds.Left, CSng(middleY), Bounds.Right, CSng(middleY))
        End Sub
    End Class

End Namespace
