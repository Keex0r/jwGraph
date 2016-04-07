Namespace jwGraph

    Public Class PointStyles
        Public Shared Sub DrawPoint(ByRef p As Drawing2D.GraphicsPath, Series As Series, X As Double, Y As Double)
            Dim halfMS As Single = Series.MarkerSize / 2
            Select Case Series.MarkerStyle
                Case Series.enumMarkerStyle.Circle
                    p.AddEllipse(CSng(X - halfMS), CSng(Y - halfMS), Series.MarkerSize, Series.MarkerSize)
                Case Series.enumMarkerStyle.Rectangle
                    p.AddRectangle(New RectangleF(CSng(X - halfMS), CSng(Y - halfMS), Series.MarkerSize, Series.MarkerSize))
                Case Series.enumMarkerStyle.Cross
                    p.AddLine(CSng(X - halfMS), CSng(Y - halfMS), CSng(X + halfMS), CSng(Y + halfMS))
                    p.AddLine(CSng(X - halfMS), CSng(Y + halfMS), CSng(X + halfMS), CSng(Y - halfMS))
                Case Series.enumMarkerStyle.Diamond
                    p.AddPolygon({New PointF(CSng(X - halfMS), CSng(Y)), New PointF(CSng(X), CSng(Y - halfMS)), New PointF(CSng(X + halfMS), CSng(Y)), New PointF(CSng(X), CSng(Y + halfMS))})
            End Select
        End Sub
        Public Shared Sub DrawPoint(ByRef g As Graphics, Series As Series, X As Double, Y As Double)
            Dim realsize As Single = Series.MarkerSize
            Select Case Series.MarkerStyle
                Case Series.enumMarkerStyle.Circle
                    realsize *= 1.0F
                Case Series.enumMarkerStyle.Rectangle
                    realsize *= 0.88F
                Case Series.enumMarkerStyle.Diamond
                    realsize *= 1.25F
                Case Series.enumMarkerStyle.Cross
                    realsize *= 1.07F
            End Select

            Dim halfMS As Single = realsize / 2
            Dim fill As Brush = Series.MarkerBrush
            Dim line As Pen = Series.MarkerBorderPen
            Select Case Series.MarkerStyle
                Case Series.enumMarkerStyle.Circle
                    g.FillEllipse(fill, CSng(X - halfMS), CSng(Y - halfMS), realsize, realsize)
                    g.DrawEllipse(line, CSng(X - halfMS), CSng(Y - halfMS), realsize, realsize)
                Case Series.enumMarkerStyle.Rectangle
                    g.FillRectangle(fill, New RectangleF(CSng(X - halfMS), CSng(Y - halfMS), realsize, realsize))
                    g.DrawRectangle(line, CSng(X - halfMS), CSng(Y - halfMS), realsize, realsize)
                Case Series.enumMarkerStyle.Cross
                    g.DrawLine(line, CSng(X - halfMS), CSng(Y - halfMS), CSng(X + halfMS), CSng(Y + halfMS))
                    g.DrawLine(line, CSng(X - halfMS), CSng(Y + halfMS), CSng(X + halfMS), CSng(Y - halfMS))
                Case Series.enumMarkerStyle.Diamond
                    g.FillPolygon(fill, {New PointF(CSng(X - halfMS), CSng(Y)), New PointF(CSng(X), CSng(Y - halfMS)), New PointF(CSng(X + halfMS), CSng(Y)), New PointF(CSng(X), CSng(Y + halfMS))})
                    g.DrawPolygon(line, {New PointF(CSng(X - halfMS), CSng(Y)), New PointF(CSng(X), CSng(Y - halfMS)), New PointF(CSng(X + halfMS), CSng(Y)), New PointF(CSng(X), CSng(Y + halfMS))})
            End Select
        End Sub
    End Class
End Namespace

