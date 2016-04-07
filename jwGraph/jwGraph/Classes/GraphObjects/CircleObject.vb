Namespace jwGraph
    Public Class CircleObject
        Inherits clsGraphObject

#Region "Properties"
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

                If IsImpedanceCircle Then
                    If Me.ClipArea = enClipArea.Above0 Then
                        Me._Y = Math.Max(Math.Min(Me._Y, 0), -Radius * 0.99)
                    ElseIf Me.ClipArea = enClipArea.Under0 Then
                        Me._Y = Math.Min(Math.Max(Me._Y, 0), Radius * 0.99)
                    Else
                        Me._Y = Math.Min(Math.Max(Me._Y, -Radius * 0.99), Radius * 0.99)
                    End If
                End If

                Notify("Y")
            End Set
        End Property
        Private _Radius As Double
        Public Property Radius As Double
            Get
                Return _Radius
            End Get
            Set(value As Double)
                _Radius = value
                Notify("Radius")
            End Set
        End Property

        Private _IsImpedanceCircle As Boolean
        Public Property IsImpedanceCircle As Boolean
            Get
                Return _IsImpedanceCircle
            End Get
            Set(value As Boolean)
                _IsImpedanceCircle = value
                Notify("IsImpedanceCircle")
            End Set
        End Property
        Private _BackColor As Color
        Public Property BackColor As Color
            Get
                Return _BackColor
            End Get
            Set(value As Color)
                _BackColor = value
                Notify("BackColor")
            End Set
        End Property
        Private _BorderColor As Color
        Public Property BorderColor As Color
            Get
                Return _BorderColor
            End Get
            Set(value As Color)
                _BorderColor = value
                Notify("BorderColor")
            End Set
        End Property
#End Region
#Region "Constructor"
        Public Sub New(X As Double, Y As Double, Radius As Double, ClipArea As enClipArea)
            Me._X = X
            Me._Y = Y
            Me._Radius = Radius
            Me.ClipArea = ClipArea
            Me.Name = "Circle"
            Me._IsImpedanceCircle = True
            BorderColor = Color.OrangeRed
            BackColor = Color.OrangeRed
        End Sub
#End Region

        Public Overrides Function GetInfos() As Object()
            Dim left As Double = -Math.Sqrt(Me.Radius * Me.Radius - Me.Y * Me.Y) + Me.X
            Dim right As Double = Math.Sqrt(Me.Radius * Me.Radius - Me.Y * Me.Y) + Me.X
            Dim width As Double = Math.Abs(left - right)
            Dim radius As Double = Me.Radius
            Dim y0 As Double = Me.Y
            Return {left, right, width, radius, y0}
        End Function

        Public Overrides Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
            'Calculates the cliparea above or below the y=0 axis
            Dim rect As Rectangle = GetClipArea(Me.ClipArea, YAxis, Bounds)
            If rect.Width <= 0 OrElse rect.Height <= 0 Then Exit Sub
            Using bmp As New Bitmap(rect.Width, rect.Height)
                Using gTemp As Graphics = Graphics.FromImage(bmp)
                    gTemp.TranslateTransform(-rect.Left, -rect.Top)
                    gTemp.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                    Using b As New SolidBrush(Color.FromArgb(100, BackColor.R, BackColor.G, BackColor.B))
                        Dim x As Single = XAxis.ValueToPixelPosition(Me.X)
                        Dim y As Single = YAxis.ValueToPixelPosition(Me.Y)
                        Dim Rad As Single = x - XAxis.ValueToPixelPosition(Me.X - Me.Radius)
                        '  x = Math.Max(Math.Min(x, 1000000), -1000000)
                        '   y = Math.Max(Math.Min(y, 1000000), -1000000)
                        '  Rad = Math.Max(Math.Min(Rad, 1000000), -1000000)
                        gTemp.FillEllipse(b, x - Rad, y - Rad, 2 * Rad, 2 * Rad)
                        Using b2 As New SolidBrush(Me.BorderColor)
                            Using p As New Pen(b2)
                                gTemp.DrawEllipse(p, x - Rad, y - Rad, 2 * Rad, 2 * Rad)
                            End Using
                        End Using
                    End Using
                    'Draw title
                    Dim yt As Single = YAxis.ValueToPixelPosition(0)
                    Dim xt As Single = XAxis.ValueToPixelPosition(Me.X)
                    '    xt = Math.Max(Math.Min(xt, 1000000), -1000000)
                    '  yt = Math.Max(Math.Min(yt, 1000000), -1000000)

                    If Me.ClipArea = enClipArea.Under0 Then
                        yt += 20
                    Else
                        yt -= 20
                    End If
                    g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                    GeneralTools.DrawRotatedString(g, Me.Name, New Font("Arial", 8), Brushes.Black, xt, yt, 0)
                End Using
                g.DrawImageUnscaled(bmp, rect.Location)
            End Using
        End Sub

        Public Overrides Function GetMouseHandles(XAxis As HorizontalAxis, YAxis As VerticalAxis) As ObjectMouseHandle()
            'Rechter Handle (verschieben)
            'Y=0, X wird aus kreis berechnet
            Dim res As New List(Of ObjectMouseHandle)

            Dim Xr1 As Double = Math.Sqrt(Me.Radius * Me.Radius - Me.Y * Me.Y) + Me.X
            Dim p1 As New PointF(XAxis.ValueToPixelPosition(Xr1), YAxis.ValueToPixelPosition(0))
            Dim o1 As ObjectMouseHandle = New ObjectMouseHandle With {.Type = ObjectMouseHandle.HandleType.ResizeHorizontal, .X = p1.X, .Y = p1.Y}
            res.Add(o1)

            Dim Xr2 As Double = -Math.Sqrt(Me.Radius * Me.Radius - Me.Y * Me.Y) + Me.X
            Dim p2 As New PointF(XAxis.ValueToPixelPosition(Xr2), YAxis.ValueToPixelPosition(0))
            Dim o2 As ObjectMouseHandle = New ObjectMouseHandle With {.Type = ObjectMouseHandle.HandleType.ResizeHorizontal, .X = p2.X, .Y = p2.Y}
            res.Add(o2)
            Dim Xr3 As Double = Me.X
            Dim p3 As New PointF(XAxis.ValueToPixelPosition(Xr3), YAxis.ValueToPixelPosition(0))
            Dim o3 As ObjectMouseHandle = New ObjectMouseHandle With {.Type = ObjectMouseHandle.HandleType.SwitchDirectionAndMove, .X = p3.X, .Y = p3.Y}
            res.Add(o3)
            'Oberer Handle
            'KLAPPT NUR BEI SAME SCALING!!!!!
            If Me.ClipArea = enClipArea.Above0 Or Me.ClipArea = enClipArea.Both Then
                Dim o4 As ObjectMouseHandle
                Dim p4 As New PointF(XAxis.ValueToPixelPosition(Me.X), YAxis.ValueToPixelPosition(Me.Y + Me.Radius))
                o4 = New ObjectMouseHandle With {.Type = ObjectMouseHandle.HandleType.ResizeVertical, .X = p4.X, .Y = p4.Y}
                res.Add(o4)
            End If
            If Me.ClipArea = enClipArea.Under0 Or Me.ClipArea = enClipArea.Both Then
                Dim o4 As ObjectMouseHandle
                Dim p4 As New PointF(XAxis.ValueToPixelPosition(Me.X), YAxis.ValueToPixelPosition(Me.Y - Me.Radius))
                o4 = New ObjectMouseHandle With {.Type = ObjectMouseHandle.HandleType.ResizeVertical, .X = p4.X, .Y = p4.Y}
                res.Add(o4)
            End If
            Return res.ToArray
        End Function

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
                    'Right baseline marker moved
                    Dim Xlold As Double = -Math.Sqrt(Me.Radius * Me.Radius - Me.Y * Me.Y) + Me.X
                    Dim Xrold As Double = Math.Sqrt(Me.Radius * Me.Radius - Me.Y * Me.Y) + Me.X
                    Dim xrneu As Double = Xrold + deltaXReal
                    Dim x0neu As Double = (Xlold + xrneu) / 2
                    Dim rneu As Double = Math.Sqrt((xrneu - x0neu) ^ 2 + (0 - Me.Y) ^ 2)
                    Me.Radius = rneu
                    Me.X = x0neu
                Case 1
                    'Left baseline marker moved
                    Dim Xlold As Double = -Math.Sqrt(Me.Radius * Me.Radius - Me.Y ^ 2) + Me.X
                    Dim Xrold As Double = Math.Sqrt(Me.Radius * Me.Radius - Me.Y ^ 2) + Me.X
                    Dim xlneu As Double = Xlold + deltaXReal
                    Dim x0neu As Double = (Xrold + xlneu) / 2
                    Dim rneu As Double = Math.Sqrt((Xrold - x0neu) ^ 2 + (0 - Me.Y) ^ 2)
                    Me.Radius = rneu
                    Me.X = x0neu
                Case 2
                    'Switch

                    If deltaX = 0 AndAlso deltaY = 0 AndAlso MouseAction = enMouseAction.Up Then
                        If Me.ClipArea = enClipArea.Above0 Then
                            Me.ClipArea = enClipArea.Under0
                        Else
                            Me.ClipArea = enClipArea.Above0
                        End If
                        If IsImpedanceCircle Then Me.Y *= -1
                    Else
                        Me.X += deltaXReal
                    End If
                Case 3, 4
                    'Oberer/Unterer Anfasser

                    Dim X_1 As Double = Math.Sqrt(Me.Radius * Me.Radius - Me.Y ^ 2) + Me.X 'Xr
                    Dim Y_1 As Double = 0
                    Dim X_2 As Double = -Math.Sqrt(Me.Radius * Me.Radius - Me.Y ^ 2) + Me.X 'Xl
                    Dim Y_2 As Double = 0
                    Dim X_3 As Double = Me.X
                    Dim Y_3 As Double
                    If Me.ClipArea = enClipArea.Above0 Then
                        Y_3 = Me.Y + Me.Radius + deltayReal
                    ElseIf Me.ClipArea = enClipArea.Under0 Then
                        Y_3 = Me.Y - Me.Radius + deltayReal
                    Else
                        If index = 3 Then
                            Y_3 = Me.Y + Me.Radius + deltayReal
                        Else
                            Y_3 = Me.Y - Me.Radius + deltayReal
                        End If
                    End If

                    Dim D As Double = 2 * ((-X_1) * Y_3 + X_2 * Y_3) '2 * (X_1 * (Y_2 - Y_3) + X_2 * (Y_3 - Y_1) + X_3 * (Y_1 - Y_2))
                    Dim Uy As Double = ((X_1 ^ 2 + Y_1 ^ 2) * (X_3 - X_2) + (X_2 ^ 2 + Y_2 ^ 2) * (X_1 - X_3) + (X_3 ^ 2 + Y_3 ^ 2) * (X_2 - X_1)) / D

                    Me.Y = Uy
                    Dim R As Double = Math.Sqrt((X_1 - Me.X) ^ 2 + (Y_1 - Me.Y) ^ 2)
                    Me.Radius = R


            End Select

        End Sub

        Public Overrides Sub UpdateOnCreate(Startpoint As Point, CurrentPoint As Point, XAxis As HorizontalAxis, YAxis As VerticalAxis)
            Dim startX As Double = XAxis.PixelPositionToValue(Startpoint.X)
            Dim startY As Double = YAxis.PixelPositionToValue(Startpoint.Y)
            Dim CurrX As Double = XAxis.PixelPositionToValue(CurrentPoint.X)
            Dim CurrY As Double = YAxis.PixelPositionToValue(CurrentPoint.Y)
            Me.X = startX
            Me.Y = 0
            Me.Radius = Math.Sqrt((CurrX - startX) ^ 2 + (CurrY - startY) ^ 2)
        End Sub

    End Class
End Namespace
