Namespace jwGraph
    <System.ComponentModel.TypeConverter(GetType(HorizontalMarkerTypeConverter))>
    Public Class HorizontalMarker
        Inherits Marker

        Public Sub New()
            Me.Y = 0
            Me.X = 0
            Me.YAxisType = Axis.enumAxisLocation.Primary
            Me.Color = Color.Blue
        End Sub

        Public Sub New(Y As Double, YAxis As Axis.enumAxisLocation)
            Me.Y = Y
            Me.YAxisType = YAxis
            Me.Color = Drawing.Color.Blue
        End Sub

        Public Overrides Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, MarkerPen As Pen, Bounds As Rectangle)
            Dim locationY As Single = YAxis.ValueToPixelPosition(Me.Y)
            Using lightPen As New Pen(LightenColor(MarkerPen.Color, 1.75))
                'Draw horizontal lines
                g.DrawLine(lightPen, 0, locationY - 1, Bounds.Width, locationY - 1)
                g.DrawLine(lightPen, 0, locationY + 1, Bounds.Width, locationY + 1)
                g.DrawLine(MarkerPen, 0, locationY, Bounds.Width, locationY)
            End Using
        End Sub
        Public Overrides Function HitTest(location As Point, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle) As Boolean
            Dim locationY As Single = YAxis.ValueToPixelPosition(Me.Y) + Bounds.Top
            Dim rect1 As New RectangleF(Bounds.Left, locationY - CInt(Int(MarkerDragWidth / 2)), Bounds.Width, MarkerDragWidth)
            If rect1.Contains(location) Then Return True
            Return False
        End Function
    End Class
End Namespace
