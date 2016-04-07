Imports jwGraph.GeneralTools
Namespace jwGraph
    Public Class PointLineSeriesDesigner
        Public Shared Sub Paint(ByRef g As Graphics, Series As Series, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
            If Series.Data Is Nothing OrElse Series.Data.Count = 0 Then Exit Sub

            Dim count As Integer = 0
            While count < Series.Data.Count AndAlso (Series.Data(count).IsOKVal = False) ' OrElse Series.Data(count).DatapointInrange(XAxis, YAxis) = False)
                count += 1
            End While
            If count >= Series.Data.Count Then Exit Sub

            LineSeriesDesigner.Paint(g, Series, XAxis, YAxis, Bounds)
            PointSeriesDesigner.Paint(g, Series, XAxis, YAxis, Bounds)
        End Sub
        Public Shared Sub PaintLegend(ByRef g As Graphics, Series As Series, Bounds As RectangleF)
            Dim middleX As Double = (Bounds.Left + Bounds.Right) / 2
            Dim middleY As Double = (Bounds.Top + Bounds.Bottom) / 2
            g.DrawLine(Series.LinePen, Bounds.Left, CSng(middleY), Bounds.Right, CSng(middleY))
            PointStyles.DrawPoint(g, Series, middleX, middleY)
        End Sub
    End Class
End Namespace
