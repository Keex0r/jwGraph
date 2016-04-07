Namespace jwGraph
    Public Class LineObject
        Inherits clsGraphObject

#Region "Properties"
        Private _X1 As Double
        Public Property X1 As Double
            Get
                Return _X1
            End Get
            Set(value As Double)
                _X1 = value
                Notify("X1")
            End Set
        End Property
        Private _Y1 As Double
        Public Property Y1 As Double
            Get
                Return _Y1
            End Get
            Set(value As Double)
                _Y1 = value
                CorrectYValues()
                Notify("Y1")
            End Set
        End Property
        Private _X2 As Double
        Public Property X2 As Double
            Get
                Return _X2
            End Get
            Set(value As Double)
                _X2 = value
                Notify("X2")
            End Set
        End Property
        Private _Y2 As Double
        Public Property Y2 As Double
            Get
                Return _Y2
            End Get
            Set(value As Double)
                _Y2 = value
                CorrectYValues()
                Notify("Y2")
            End Set
        End Property
        Private _IsImpedanceLine As Boolean
        Public Property IsImpedanceLine As Boolean
            Get
                Return _IsImpedanceLine
            End Get
            Set(value As Boolean)
                _IsImpedanceLine = value
                Notify("IsImpedanceLine")
            End Set
        End Property
        Private Sub CorrectYValues()
            If IsImpedanceLine Then
                If Me.ClipArea = enClipArea.Above0 Then
                    Me._Y1 = Math.Max(Me._Y1, 0)
                    Me._Y2 = Math.Max(Me._Y2, 0)
                ElseIf Me.ClipArea = enClipArea.Under0 Then
                    Me._Y1 = Math.Min(Me._Y1, 0)
                    Me._Y2 = Math.Min(Me._Y2, 0)
                End If
            End If
        End Sub
        Private _BorderColor As Color
        Public Property BorderColor As Color
            Get
                Return _BorderColor
            End Get
            Set(value As Color)
                _BorderColor = Value
                Notify("BorderColor")
            End Set
        End Property
#End Region
#Region "Constructor"
        Public Sub New(X1 As Double, Y1 As Double, X2 As Double, Y2 As Double, ClipArea As enClipArea)
            Me._X1 = X1
            Me._Y1 = Y1
            Me._X2 = X2
            Me._Y2 = Y2
            Me.ClipArea = ClipArea
            Me.Name = "Line"
            Me._IsImpedanceLine = True
            BorderColor = Color.OrangeRed
        End Sub
#End Region

        Public Overrides Function GetInfos() As Object()
            Return {X1, Y1, X2, Y2}
        End Function

        Public Overrides Function GetMouseHandles(XAxis As HorizontalAxis, YAxis As VerticalAxis) As ObjectMouseHandle()
            Dim res As New List(Of ObjectMouseHandle)
            Dim p1 As New PointF(XAxis.ValueToPixelPosition(X1), YAxis.ValueToPixelPosition(Y1))
            Dim o1 As ObjectMouseHandle = New ObjectMouseHandle With {.Type = ObjectMouseHandle.HandleType.Move, .X = p1.X, .Y = p1.Y}
            res.Add(o1)

            Dim p2 As New PointF(XAxis.ValueToPixelPosition(X2), YAxis.ValueToPixelPosition(Y2))
            Dim o2 As ObjectMouseHandle = New ObjectMouseHandle With {.Type = ObjectMouseHandle.HandleType.Move, .X = p2.X, .Y = p2.Y}
            res.Add(o2)
            Return res.ToArray
        End Function

        Public Overrides Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
            Dim p1 As New PointF(XAxis.ValueToPixelPosition(Me.X1), YAxis.ValueToPixelPosition(Me.Y1))
            Dim p2 As New PointF(XAxis.ValueToPixelPosition(Me.X2), YAxis.ValueToPixelPosition(Me.Y2))
            Using b As New SolidBrush(Me.BorderColor)
                Using p As New Pen(b, 2)
                    g.DrawLine(p, p1, p2)
                End Using
            End Using
            'Draw name parallel to line

            'Calculate angle from line
            Dim angle As Double = Math.Atan2((Y2 - Y1), (X2 - X1))
            Dim senkAngle As Double = angle - Math.PI / 2
            'Needed for correct placement
            Dim min1 As Integer = 1
            Dim addiang As Integer = 0
            If X2 > X1 Then min1 = 1 : addiang = 0 Else min1 = -1 : addiang = 180
            'Middle point
            Dim middleX As Double = (X1 + X2) / 2
            Dim middleY As Double = (Y1 + Y2) / 2
            'Get new text center with certain distance from line
            Dim newX As Double = (middleX + min1 * Math.Cos(senkAngle) * XAxis.PixelDistanceToValueDistance(7))
            Dim newy As Double = (middleY + min1 * Math.Sin(senkAngle) * YAxis.PixelDistanceToValueDistance(7))

            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            GeneralTools.DrawRotatedString(g, Me.Name, New Font("Arial", 8), Brushes.Black, XAxis.ValueToPixelPosition(newX), YAxis.ValueToPixelPosition(newy), CSng(-angle / Math.PI * 180 + addiang))

        End Sub

        Public Overrides Sub UpDateByMouse(index As Integer, deltaX As Integer, deltaY As Integer, XAxis As HorizontalAxis, yAxis As VerticalAxis, MouseAction As enMouseAction)
            Dim x1 As Double = XAxis.PixelPositionToValue(1)
            Dim x2 As Double = XAxis.PixelPositionToValue(2)
            Dim deltaXReal As Double = x2 - x1
            deltaXReal *= deltaX

            Dim y1 As Double = yAxis.PixelPositionToValue(1)
            Dim y2 As Double = yAxis.PixelPositionToValue(2)
            Dim deltayReal As Double = y2 - y1
            deltayReal *= deltaY

            Select Case index
                Case 0
                    Me.X1 += deltaXReal
                    Me.Y1 += deltayReal
                Case 1
                    Me.X2 += deltaXReal
                    Me.Y2 += deltayReal
            End Select
        End Sub
        Public Overrides Sub UpdateOnCreate(Startpoint As Point, CurrentPoint As Point, XAxis As HorizontalAxis, YAxis As VerticalAxis)
            Dim startX As Double = XAxis.PixelPositionToValue(Startpoint.X)
            Dim startY As Double = YAxis.PixelPositionToValue(Startpoint.Y)
            Dim CurrX As Double = XAxis.PixelPositionToValue(CurrentPoint.X)
            Dim CurrY As Double = YAxis.PixelPositionToValue(CurrentPoint.Y)
            Me.X1 = startX
            Me.Y1 = startY
            Me.X2 = CurrX
            Me.Y2 = CurrY
        End Sub

    End Class
End Namespace

