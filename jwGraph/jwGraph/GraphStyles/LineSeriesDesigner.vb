Imports jwGraph.GeneralTools
Namespace jwGraph

    Public Class LineSeriesDesigner
        Public Shared Sub Paint(ByRef g As Graphics, Series As Series, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
            If Series.Data Is Nothing OrElse Series.Data.Count = 0 Then Exit Sub
       
            ' g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            Dim points As New List(Of Tuple(Of Single, Single))
            For i = 0 To Series.Data.Count - 1
                If Series.Data(i).IsOKVal = False Then Continue For
                Dim X As Single = XAxis.ValueToPixelPosition(Series.Data(i).X)
                Dim Y As Single = YAxis.ValueToPixelPosition(Series.Data(i).Y)
                If jwGraph.IsOK(X) = False OrElse jwGraph.IsOK(Y) = False Then Continue For
                points.Add(Tuple.Create(X, Y))
            Next
            If points.Count = 0 Then Exit Sub
            Dim cb As RectangleF = g.VisibleClipBounds

            Dim PointsInRange As List(Of Boolean) = (From t As Tuple(Of Single, Single) In points Select t.Item1 >= 0 AndAlso t.Item1 <= cb.Width AndAlso _
                              t.Item2 >= 0 AndAlso t.Item2 <= cb.Height).ToList

            Dim lastX As Single = points(0).Item1
            Dim lastY As Single = points(0).Item2
        
            For i = 1 To points.Count - 1
                Dim x1 As Single = points(i).Item1
                Dim y1 As Single = points(i).Item2
                Dim x2 As Single = points(i - 1).Item1
                Dim y2 As Single = points(i - 1).Item2

                If PointsInRange(i - 1) = False AndAlso PointsInRange(i) = False Then  'None of the two in range
                    'Check if line crosses the visible rectangle
                    If LinesIntersect(x1, y1, x2, y2, 0, 0, g.VisibleClipBounds.Right, g.VisibleClipBounds.Bottom, 0.0, 0.0) OrElse _
                        LinesIntersect(x1, y1, x2, y2, 0, g.VisibleClipBounds.Bottom, g.VisibleClipBounds.Right, 0, 0.0, 0.0) Then
                        'Line crosses with at least one diagonal --> goes through the visible area
                        'Need to find and calculate the two border points on the outer rectangle

                        Dim intersect1x As Single, intersect1y As Single
                        Dim intersect2x As Single, intersect2y As Single
                        Dim found As Boolean = False
                        found = LinesIntersect(x1, y1, x2, y2, 0, 0, g.VisibleClipBounds.Right, 0, intersect1x, intersect1y) 'Top

                        If found = False Then
                            found = LinesIntersect(x1, y1, x2, y2, 0, 0, 0, g.VisibleClipBounds.Bottom, intersect1x, intersect1y) 'Left
                        Else
                            LinesIntersect(x1, y1, x2, y2, 0, 0, 0, g.VisibleClipBounds.Bottom, intersect2x, intersect2y) 'Left
                        End If

                        If found = False Then
                            found = LinesIntersect(x1, y1, x2, y2, g.VisibleClipBounds.Right, 0, g.VisibleClipBounds.Right, g.VisibleClipBounds.Bottom, intersect1x, intersect1y) 'Right
                        Else
                            LinesIntersect(x1, y1, x2, y2, g.VisibleClipBounds.Right, 0, g.VisibleClipBounds.Right, g.VisibleClipBounds.Bottom, intersect2x, intersect2y) 'Right
                        End If

                        'The first intersection point has to have been found up to now
                        LinesIntersect(x1, y1, x2, y2, 0, g.VisibleClipBounds.Bottom, g.VisibleClipBounds.Right, g.VisibleClipBounds.Bottom, intersect2x, intersect2y) 'Bottom

                        g.DrawLine(Series.LinePen, intersect1x, intersect1y, intersect2x, intersect2y)
                    End If
                ElseIf PointsInRange(i - 1) = False AndAlso PointsInRange(i) Then 'Current in Range
                    Dim intersectX As Single = 0
                    Dim intersectY As Single = 0
                    IntersectsRectangle(x1, y1, x2, y2, g.VisibleClipBounds, intersectX, intersectY)
                    g.DrawLine(Series.LinePen, intersectX, intersectY, x1, y1)
                    If Series.Data(i).IsMarked Then g.DrawEllipse(Pens.Red, -3, y1 - 3, 6, 6)
                ElseIf PointsInRange(i - 1) = True AndAlso PointsInRange(i) = False Then 'Current not in Range
                    Dim intersectX As Single = 0
                    Dim intersectY As Single = 0
                    IntersectsRectangle(x1, y1, x2, y2, g.VisibleClipBounds, intersectX, intersectY)
                    g.DrawLine(Series.LinePen, intersectX, intersectY, x2, y2)
                Else
                    g.DrawLine(Series.LinePen, x1, y1, x2, y2)
                    If Series.Data(i).IsMarked Then
                        g.DrawEllipse(Pens.Red, -3, y1 - 3, 6, 6)
                    End If
                End If
            Next

        End Sub
        Public Shared Sub PaintLegend(ByRef g As Graphics, Series As Series, Bounds As RectangleF)
            '   g.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            Dim middleY As Double = (Bounds.Top + Bounds.Bottom) / 2
            g.DrawLine(Series.LinePen, Bounds.Left, CSng(middleY), Bounds.Right, CSng(middleY))
        End Sub

    End Class
End Namespace
