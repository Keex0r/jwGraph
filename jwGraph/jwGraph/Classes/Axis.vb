Imports System.ComponentModel
Namespace jwGraph

    Public MustInherit Class Axis
        Implements INotifyPropertyChanged
        Implements IDisposable

        Protected Parent As jwGraph

        Public Enum enumTickDirection
            Outwards = 0
            Inwards = 1
        End Enum
        Public Enum enumAxisLocation
            Primary = 0
            Secondary = 1
        End Enum

        Private _ShowLogarithmic As Boolean
        Public Property ShowLogarithmic As Boolean
            Get
                Return _ShowLogarithmic
            End Get
            Set(value As Boolean)
                _ShowLogarithmic = value
                Notify("ShowLogarithmic")
            End Set
        End Property

        Private _LabelFont As Font
        Public Property LabelFont As Font
            Get
                Return _LabelFont
            End Get
            Set(value As Font)
                _LabelFont = value
                Notify("LabelFont")
            End Set
        End Property

        Private _LabelFormat As String
        Public Property LabelFormat As String
            Get
                Return _LabelFormat
            End Get
            Set(value As String)
                _LabelFormat = value
                Notify("LabelFormat")
            End Set
        End Property


        Private _nTicks As Integer
        Public Property nTicks As Integer
            Get
                Return _nTicks
            End Get
            Set(value As Integer)
                _nTicks = value
                Notify("nTicks")
            End Set
        End Property
        Private _nMinorTicks As Integer
        Public Property nMinorTicks As Integer
            Get
                Return _nMinorTicks
            End Get
            Set(value As Integer)
                _nMinorTicks = value
                Notify("nMinorTicks")
            End Set
        End Property

        Private _TickWidth As Integer
        Public Property TickWidth As Integer
            Get
                Return _TickWidth
            End Get
            Set(value As Integer)
                _TickWidth = value
                Notify("TickWidth")
            End Set
        End Property

        Private _TickDirection As enumTickDirection
        Public Property TickDirection As enumTickDirection
            Get
                Return _TickDirection
            End Get
            Set(value As enumTickDirection)
                _TickDirection = value
                Notify("TickDirection")
            End Set
        End Property

        Private _Minimum As Double
        Public Property Minimum As Double
            Get
                Return _Minimum
            End Get
            Set(value As Double)
                _Minimum = value
                Notify("Minimum")
            End Set
        End Property

        Private _Maximum As Double
        Public Property Maximum As Double
            Get
                Return _Maximum
            End Get
            Set(value As Double)
                _Maximum = value
                Notify("Maximum")
            End Set
        End Property

        Private _Title As String
        Public Property Title As String
            Get
                Return _Title
            End Get
            Set(value As String)
                _Title = value
                Notify("Title")
            End Set
        End Property

        Private _TitleFont As Font
        Public Property TitleFont As Font
            Get
                Return _TitleFont
            End Get
            Set(value As Font)
                _TitleFont = value
                Notify("TitleFont")
            End Set
        End Property

        Private _AxisLocation As enumAxisLocation
        <Browsable(False)> _
        Public Property AxisLocation As enumAxisLocation
            Get
                Return _AxisLocation
            End Get
            Set(value As enumAxisLocation)
                _AxisLocation = value
                Notify("AxisLocation")
            End Set
        End Property

        Private _ShowGridlines As Boolean
        Public Property ShowGridlines As Boolean
            Get
                Return _ShowGridlines
            End Get
            Set(value As Boolean)
                _ShowGridlines = value
                Notify("ShowGridlines")
            End Set
        End Property

        Private _ShowOrigin As Boolean
        Public Property ShowOrigin As Boolean
            Get
                Return _ShowOrigin
            End Get
            Set(value As Boolean)
                _ShowOrigin = value
                Notify("ShowOrigin")
            End Set
        End Property

        Private _AutoPositionTitle As Boolean
        Public Property AutoPositionTitle As Boolean
            Get
                Return _AutoPositionTitle
            End Get
            Set(value As Boolean)
                _AutoPositionTitle = value
                Notify("AutoPositionTitle")
            End Set
        End Property

        Private _AutoPositionTitleDistance As Integer
        Public Property AutoPositionTitleDistance As Integer
            Get
                Return _AutoPositionTitleDistance
            End Get
            Set(value As Integer)
                _AutoPositionTitleDistance = value
                Notify("AutoPositionTitleDistance")
            End Set
        End Property


        Private _TitleDistance As Integer
        Public Property TitleDistance As Integer
            Get
                Return _TitleDistance
            End Get
            Set(value As Integer)
                _TitleDistance = value
                Notify("TitleDistance")
            End Set
        End Property
        Private _FirstAndLastTickTextOnly As Boolean
        Public Property FirstAndLastTickTextOnly As Boolean
            Get
                Return _FirstAndLastTickTextOnly
            End Get
            Set(value As Boolean)
                _FirstAndLastTickTextOnly = value
                Notify("FirstAndLastTickTextOnly")
            End Set
        End Property
        Private _ShowTickText As Boolean
        Public Property ShowTickText As Boolean
            Get
                Return _ShowTickText
            End Get
            Set(value As Boolean)
                _ShowTickText = value
                Notify("ShowTickText")
            End Set
        End Property

        Private _OriginColor As Color
        Public Property OriginColor As Color
            Get
                Return _OriginColor
            End Get
            Set(value As Color)
                _OriginColor = value
                Notify("OriginColor")
            End Set
        End Property

        Public ReadOnly Property IsAxisOK As Boolean
            Get
                Return (jwGraph.IsOK(Me.Minimum) AndAlso jwGraph.IsOK(Me.Maximum))
            End Get
        End Property

        Private _UseExactSteps As Boolean
        Public Property UseExactSteps As Boolean
            Get
                Return _UseExactSteps
            End Get
            Set(value As Boolean)
                _UseExactSteps = value
                Notify("UseExactSteps")
            End Set
        End Property

        Protected Function RoundSignificant(x As Double, p As Integer) As Double
            If x = 0 Then Return 0
            Dim sign As Integer = Math.Sign(x)
            x = Math.Abs(x)
            Dim n As Double = Math.Floor(Math.Log10(x)) + 1 - p
            Dim value As Double = sign * Math.Round(10 ^ -n * x) * 10 ^ n

            Return value
        End Function

        Private Function RoundMidpointSignificant(x As Double) As Double
            If x = 0 Then Return 0
            Dim sign = Math.Sign(x)
            If x < 0 Then x = Math.Abs(x)
            Dim n1 = Math.Floor(Math.Log10(x))
            Dim n2 = Math.Ceiling(Math.Log10(x))
            Dim value As Double = x / 10 ^ n2
            If value <= 0.25 Then
                Return sign * 10 ^ n1
            ElseIf value > 0.25 AndAlso value < 0.75 Then
                Return sign * 0.5 * 10 ^ n2
            Else
                Return sign * 10 ^ n2
            End If
        End Function

        Protected Function GetSteps(x1 As Double, x2 As Double, steps As Integer) As List(Of Double)
            Dim res As New List(Of Double)
            If Not jwGraph.IsOK(x1) OrElse Not jwGraph.IsOK(x2) Then Return res
            If x2 < x1 Then
                Dim temp = x1
                x1 = x2
                x2 = temp
            End If
            Dim delta As Double = RoundSignificant((x2 - x1) / (steps), 1)
            If x1 = x2 Or Math.Abs(delta / ((x2 + x1) / 2)) < 0.000000000000001 Then
                Return {x1, x2}.ToList
            End If
            Dim startr As Double = RoundSignificant(x1, 1)
            Dim ender As Double = RoundSignificant(x2, 1)

            If startr < 0 AndAlso ender > 0 Then
                Dim current1 As Double = 0
                Dim current2 As Double = 0
                res.Add(0)
                Do
                    current1 += delta
                    res.Add(current1)
                Loop Until current1 > x2
                Do
                    current2 -= delta
                    res.Add(current2)
                Loop Until current2 < x1
            Else
                Dim mitteInd As Integer = 0
                Dim mitte As Double = (x1 + x2) / 2
                Do
                    mitteInd += 1
                Loop Until RoundSignificant(mitte, mitteInd) > x1 AndAlso RoundSignificant(mitte, mitteInd) < x2
                startr = RoundSignificant(mitte, mitteInd)

                Dim current1 As Double = startr
                Dim current2 As Double = startr
                res.Add(startr)
                Do
                    current1 += delta
                    res.Add(current1)
                Loop Until current1 > x2
                Do
                    current2 -= delta
                    res.Add(current2)
                Loop Until current2 < x1

            End If



            Return res
        End Function

        Public Sub New(Parent As jwGraph)
            Me.Parent = Parent
            _Minimum = 0
            _Maximum = 1
            _nTicks = 5
            _nMinorTicks = 4
            _TickDirection = enumTickDirection.Outwards
            _TickWidth = 5
            _Title = "Axis"
            _ShowTickText = True
            _FirstAndLastTickTextOnly = False
            _ShowOrigin = True
            _ShowGridlines = True

            '_LabelFont = New Font("Arial Narrow", 9)
            '_LabelFormat = "shortprec2"
            '_TitleFont = New Font("Arial", 14)


            _LabelFont = New Font("Arial Narrow", 12)
            _TitleFont = New Font("Arial", 20)
            _AutoPositionTitle = True
            _LabelFormat = "shortprec2"

            '_AutoPositionTitle = False

            _AutoPositionTitleDistance = 2
            _UseExactSteps = False
        End Sub

        Protected Function RoundNearest(d As Double, Stellen As Integer) As Double
            Dim log As Double = Math.Log10(Math.Abs(d))
            log = 10 ^ (Math.Floor(log) - Stellen)
            Dim newvalue As Double = Math.Ceiling(d / log)
            Return newvalue * log
        End Function
        Protected Function GetRoundDelta(Start As Double, Ende As Double, nTicks As Integer) As Double
            Dim wantdelta As Double = (Ende - Start) / (nTicks - 1)
            Dim log As Double = Math.Log10(Math.Abs(wantdelta))
            log = 10 ^ Math.Floor(log)
            Dim newvalue As Double = Math.Floor(wantdelta / log)
            Return newvalue * log
        End Function
        Protected Function GetTickValues(Start As Double, Ende As Double, nTicks As Integer) As List(Of Double)
            Dim res As New List(Of Double)
            Dim delta As Double = GetRoundDelta(Start, Ende, nTicks)
            Dim stellen As Double = Math.Log10(Math.Abs(delta))
            Dim realstart As Double = RoundNearest(Start, 0)
            For i = 0 To nTicks - 1
                res.Add(realstart + i * delta)
            Next
            Return res
        End Function


        Public MustOverride Sub Paint(ByRef g As Graphics, ByRef AxisPen As Pen, ByRef GridPen As Pen, StartPoint As Integer, EndPoint As Integer, Location As Integer, OtherSide As Integer)

        Public MustOverride Function ValueToPixelPosition(Value As Double) As Single
        Public MustOverride Function PixelPositionToValue(Position As Single) As Double

        Public MustOverride Function PixelDistanceToValueDistance(PixelDistance As Single) As Double
        Public MustOverride Function ValueDistanceToPixelDistance(ValueDistance As Double) As Single

        Private Sub Notify(Name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(Name))
        End Sub
        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

#Region "IDisposable Support"
        Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: Verwalteten Zustand löschen (verwaltete Objekte).
                    If _LabelFont IsNot Nothing Then _LabelFont.Dispose()
                    If _TitleFont IsNot Nothing Then _TitleFont.Dispose()

                End If

                ' TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalize() unten überschreiben.
                ' TODO: Große Felder auf NULL festlegen.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: Finalize() nur überschreiben, wenn Dispose(ByVal disposing As Boolean) oben über Code zum Freigeben von nicht verwalteten Ressourcen verfügt.
        'Protected Overrides Sub Finalize()
        '    ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(disposing As Boolean) Bereinigungscode ein.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace
