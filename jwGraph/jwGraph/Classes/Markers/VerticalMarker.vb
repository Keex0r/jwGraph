Namespace jwGraph
    <System.ComponentModel.TypeConverter(GetType(VerticalMarkerTypeConverter))>
    Public Class VerticalMarker
        Inherits Marker

        Public Sub New()
            Me.Y = 0
            Me.X = 0
            Me.YAxisType = Axis.enumAxisLocation.Primary
            Me.Color = Color.Blue
        End Sub

        Public Sub New(X As Double)
            Me.X = X
            Me.Color = Drawing.Color.Green
        End Sub
        Public Overrides Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, MarkerPen As Pen, Bounds As Rectangle)
            Dim locationX As Single = XAxis.ValueToPixelPosition(Me.X)
            Using lightPen As New Pen(LightenColor(MarkerPen.Color, 1.75))
                'Draw vertical lines
                g.DrawLine(lightPen, locationX - 1, 0, locationX - 1, Bounds.Height)
                g.DrawLine(lightPen, locationX + 1, 0, locationX + 1, Bounds.Height)
                g.DrawLine(MarkerPen, locationX, 0, locationX, Bounds.Height)
            End Using
        End Sub
        Public Overrides Function HitTest(location As Point, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle) As Boolean
            Dim locationX As Single = XAxis.ValueToPixelPosition(Me.X) + Bounds.Left
            Dim rect2 As New RectangleF(locationX - CInt(Int(MarkerDragWidth / 2)), Bounds.Top, MarkerDragWidth, Bounds.Height)
            If rect2.Contains(location) Then Return True
            Return False
        End Function
    End Class
End Namespace
