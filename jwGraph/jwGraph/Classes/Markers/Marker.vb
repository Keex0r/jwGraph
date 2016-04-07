Imports System.ComponentModel
Namespace jwGraph
    Public MustInherit Class Marker
        Implements INotifyPropertyChanged

        Private _Color As Color
        Public Property Color As Color
            Get
                Return _Color
            End Get
            Set(value As Color)
                _Color = value
                Notify("Color")
            End Set
        End Property

        Private _X As Double
        Public Property X As Double
            Get
                Return _X
            End Get
            Set(value As Double)
                _X = value
                Notify("X")
            End Set
        End Property

        Private _Y As Double
        Public Property Y As Double
            Get
                Return _Y
            End Get
            Set(value As Double)
                _Y = value
                Notify("Y")
            End Set
        End Property

        Private _YAxisType As Axis.enumAxisLocation
        Public Property YAxisType As Axis.enumAxisLocation
            Get
                Return _YAxisType
            End Get
            Set(value As Axis.enumAxisLocation)
                _YAxisType = value
                Notify("YAxisType")
            End Set
        End Property


        Public MustOverride Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, MarkerPen As Pen, Bounds As Rectangle)
        Public MustOverride Function HitTest(location As Point, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle) As Boolean
        Protected MarkerDragWidth As Integer = 7
        Protected Function LightenColor(source As Color, amount As Double) As Color
            Dim r As Double = source.R * amount
            Dim g As Double = source.G * amount
            Dim b As Double = source.B * amount

            Dim threshold = 255.999
            Dim m = Math.Max(Math.Max(r, g), b)
            If m <= threshold Then Return Color.FromArgb(CInt(Int(r)), CInt(Int(g)), CInt(Int(b)))
            Dim total = r + g + b
            If total >= 3 * threshold Then Return Color.FromArgb(CInt(Int(threshold)), CInt(Int(threshold)), CInt(Int(threshold)))
            Dim x = (3 * threshold - total) / (3 * m - total)
            Dim gray = threshold - x * m
            Return Color.FromArgb(CInt(Int(gray + x * r)), CInt(Int(gray + x * g)), CInt(Int(gray + x * b)))
        End Function

        Public Sub Notify(name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace