Namespace jwGraph
    <System.ComponentModel.TypeConverter(GetType(FreeMarkerTypeConverter))>
    Public Class FreeMarker
        Inherits Marker

        Public Property MarkerSize As Single

        Public Sub New()
            Me.X = 0
            Me.Y = 0
            Me.YAxisType = Axis.enumAxisLocation.Primary
            MarkerSize = 20
            Me.Color = Drawing.Color.Green
        End Sub

        Public Sub New(X As Double, Y As Double, YAxis As Axis.enumAxisLocation)
            Me.X = X
            Me.Y = Y
            Me.YAxisType = YAxis
            MarkerSize = 20
            Me.Color = Drawing.Color.Green
        End Sub

        Public Overrides Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, MarkerPen As Pen, Bounds As Rectangle)
            Dim locationX As Single = XAxis.ValueToPixelPosition(Me.X)
            Dim locationY As Single = YAxis.ValueToPixelPosition(Me.Y)

            Dim msHalf As Single = MarkerSize / 2

            Using lightPen As New Pen(LightenColor(MarkerPen.Color, 1.75))
                'Draw cross
                g.DrawLine(lightPen, locationX - msHalf, locationY - 1, locationX + msHalf, locationY - 1)
                g.DrawLine(lightPen, locationX - msHalf, locationY + 1, locationX + msHalf, locationY + 1)

                g.DrawLine(lightPen, locationX - 1, locationY - msHalf, locationX - 1, locationY + msHalf)
                g.DrawLine(lightPen, locationX + 1, locationY - msHalf, locationX + 1, locationY + msHalf)

                g.DrawLine(MarkerPen, locationX - msHalf, locationY, locationX + msHalf, locationY)
                g.DrawLine(MarkerPen, locationX, locationY - msHalf, locationX, locationY + msHalf)
            End Using
        End Sub

        Public Overrides Function HitTest(location As Point, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle) As Boolean
            Dim locationX As Single = XAxis.ValueToPixelPosition(Me.X) + Bounds.Left
            Dim locationY As Single = YAxis.ValueToPixelPosition(Me.Y) + Bounds.Top
            Dim msHalf As Single = MarkerSize / 2
            Dim rect1 As New RectangleF(locationX - msHalf, locationY - CInt(Int(MarkerDragWidth / 2)), MarkerSize, MarkerDragWidth)
            If rect1.Contains(location) Then Return True
            Dim rect2 As New RectangleF(locationX - CInt(Int(MarkerDragWidth / 2)), locationY - msHalf, MarkerDragWidth, MarkerSize)
            If rect2.Contains(location) Then Return True
            Return False
        End Function
    End Class
End Namespace
