Imports System.ComponentModel
Namespace jwGraph
    <TypeConverter(GetType(SeriesTypeConverter))>
    Public Class Series
        Implements INotifyPropertyChanged
        Implements IDisposable

#Region "Enums"
        Public Enum enumMarkerStyle
            None = 0
            Circle = 1
            Rectangle = 2
            Cross = 3
            Diamond = 4
        End Enum
        Public Enum enumSeriesType
            Scatter = 0
            Line = 1
            LineScatter = 2
            Histogram = 4
        End Enum
#End Region
#Region "Properties"

#Region "Line and Marker Style"
        Private _LineWidth As Integer
        Public Property LineWidth As Integer
            Get
                Return _LineWidth
            End Get
            Set(value As Integer)
                _LineWidth = value
                '    RecreateStyles()
                Notify("LineWidth")
            End Set
        End Property
        Private _ErrorLineWidth As Integer
        Public Property ErrorWidth As Integer
            Get
                Return _ErrorLineWidth
            End Get
            Set(value As Integer)
                _ErrorLineWidth = value
                '   RecreateStyles()
                Notify("ErrorWidth")
            End Set
        End Property
        Private _MarkerBorderLineWidth As Integer
        Public Property MarkerBorderLineWidth As Integer
            Get
                Return _MarkerBorderLineWidth
            End Get
            Set(value As Integer)
                _MarkerBorderLineWidth = value
                '    RecreateStyles()
                Notify("MarkerBorderLineWidth")
            End Set
        End Property

        Private _LineColor As Color
        Public Property LineColor As Color
            Get
                Return _LineColor
            End Get
            Set(value As Color)
                _LineColor = value
                ' RecreateStyles()
                Notify("LineColor")
            End Set
        End Property

        Private _LineStyle As Drawing2D.DashStyle
        Public Property LineStyle As Drawing2D.DashStyle
            Get
                Return _LineStyle
            End Get
            Set(value As Drawing2D.DashStyle)
                _LineStyle = value
                Notify("LineStyle")
            End Set
        End Property

        Private _MarkerBorderColor As Color
        Public Property MarkerBorderColor As Color
            Get
                Return _MarkerBorderColor
            End Get
            Set(value As Color)
                _MarkerBorderColor = value
                '  RecreateStyles()
                Notify("MarkerBorderColor")
            End Set
        End Property
        Private _MarkerFillColor As Color
        Public Property MarkerFillColor As Color
            Get
                Return _MarkerFillColor
            End Get
            Set(value As Color)
                _MarkerFillColor = value
                '  RecreateStyles()
                Notify("MarkerFillColor")
            End Set
        End Property
        Private _ErrorLineColor As Color
        Public Property ErrorLineColor As Color
            Get
                Return _ErrorLineColor
            End Get
            Set(value As Color)
                _ErrorLineColor = value
                '   RecreateStyles()
                Notify("ErrorLineColor")
            End Set
        End Property
#End Region

        Private _LinePen As Pen
        Private _ErrorPen As Pen
        Private _MarkerBorderPen As Pen
        Private _MarkerBrush As Brush
        Protected Friend ReadOnly Property LinePen As Pen
            Get
                Return _LinePen
            End Get
        End Property
        Protected Friend ReadOnly Property ErrorPen As Pen
            Get
                Return _ErrorPen
            End Get
        End Property
        Protected Friend ReadOnly Property MarkerBorderPen As Pen
            Get
                Return _MarkerBorderPen
            End Get
        End Property
        Protected Friend ReadOnly Property MarkerBrush As Brush
            Get
                Return _MarkerBrush
            End Get
        End Property

        Private Sub RecreateStyles()
            If _LinePen IsNot Nothing Then _LinePen.Dispose()
            _LinePen = New Pen(New SolidBrush(Me.LineColor), Me.LineWidth)
            _LinePen.DashStyle = Me.LineStyle
            If _ErrorPen IsNot Nothing Then _ErrorPen.Dispose()
            _ErrorPen = New Pen(New SolidBrush(Me.ErrorLineColor), Me.ErrorWidth)
            If _MarkerBorderPen IsNot Nothing Then _MarkerBorderPen.Dispose()
            _MarkerBorderPen = New Pen(New SolidBrush(Me.MarkerBorderColor), Me.MarkerBorderLineWidth)
            If _MarkerBrush IsNot Nothing Then _MarkerBrush.Dispose()
            _MarkerBrush = New SolidBrush(Me.MarkerFillColor)
        End Sub

        Private _ErrorCapWidth As Integer
        Public Property ErrorCapWidth As Integer
            Get
                Return _ErrorCapWidth
            End Get
            Set(value As Integer)
                If value Mod 2 = 0 Then value += 1
                _ErrorCapWidth = value
                Notify("ErrorCapWidth")
            End Set
        End Property

        Private _MarkerSize As Single
        Public Property MarkerSize As Single
            Get
                Return _MarkerSize
            End Get
            Set(value As Single)
                _MarkerSize = value
                Notify("MarkerSize")
            End Set
        End Property
        Private _MarkerStyle As enumMarkerStyle
        Public Property MarkerStyle As enumMarkerStyle
            Get
                Return _MarkerStyle
            End Get
            Set(value As enumMarkerStyle)
                _MarkerStyle = value
                Notify("MarkerStyle")
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


        Private _UseInAutoScale As Boolean
        Public Property UseInAutoScale As Boolean
            Get
                Return _UseInAutoScale
            End Get
            Set(value As Boolean)
                _UseInAutoScale = value
                Notify("UseInAutoScale")
            End Set
        End Property

        Private _SeriesType As enumSeriesType
        Public Property SeriesType As enumSeriesType
            Get
                Return _SeriesType
            End Get
            Set(value As enumSeriesType)
                _SeriesType = value
                Notify("SeriesType")
            End Set
        End Property

        Private _DrawXErrors As Boolean
        Public Property DrawXErrors As Boolean
            Get
                Return _DrawXErrors
            End Get
            Set(value As Boolean)
                _DrawXErrors = value
                Notify("DrawXErrors")
            End Set
        End Property
        Private _DrawYErrors As Boolean
        Public Property DrawYErrors As Boolean
            Get
                Return _DrawYErrors
            End Get
            Set(value As Boolean)
                _DrawYErrors = value
                Notify("DrawYErrors")
            End Set
        End Property

        Private _IsVisibleInLegend As Boolean
        Public Property IsVisibleInLegend As Boolean
            Get
                Return _IsVisibleInLegend
            End Get
            Set(value As Boolean)
                _IsVisibleInLegend = value
                Notify("IsVisibleInLegend")
            End Set
        End Property
        Private _LegendText As String
        Public Property LegendText As String
            Get
                Return _LegendText
            End Get
            Set(value As String)
                _LegendText = value
                Notify("LegendText")
            End Set
        End Property
#End Region
        Public Sub New()
            Me.New("Series", Axis.enumAxisLocation.Primary, enumSeriesType.Line)
        End Sub
        Public Sub New(Name As String, YAxis As Axis.enumAxisLocation, Type As enumSeriesType)
            Me.New(Name, YAxis, Type, Nothing, Nothing)
        End Sub
        Public Sub New(Name As String, YAxis As Axis.enumAxisLocation, Type As enumSeriesType, X() As Double, Y() As Double)
            Data = New BindingList(Of Datapoint)
            If X IsNot Nothing AndAlso Y IsNot Nothing AndAlso X.Count = Y.Count Then
                For i = 0 To X.Count - 1
                    Data.Add(New Datapoint(X(i), Y(i)))
                Next
            End If
            Me.Name = Name
            _LegendText = Name
            _IsVisibleInLegend = True
            _YAxisType = YAxis
            _UseInAutoScale = True
            _SeriesType = Type

            Me.MarkerBorderColor = Color.Black
            Me.MarkerBorderLineWidth = 1
            Me.MarkerFillColor = Color.Black
            Me.LineWidth = 1
            Me.LineStyle = Drawing2D.DashStyle.Solid
            Me.ErrorWidth = 1
            Me.LineColor = Color.Black
            Me.ErrorLineColor = Color.Black


            _MarkerSize = 8
            _MarkerStyle = enumMarkerStyle.Circle

            _ErrorCapWidth = 5
        End Sub

        Public Property Name As String

        <Browsable(False)>
        Public ReadOnly Property Datapoints As List(Of Datapoint)
            Get
                Return Data.ToList
            End Get
        End Property


        Friend WithEvents Data As BindingList(Of Datapoint)

        Public Sub RemoveAt(Index As Integer)
            Data.RemoveAt(Index)
        End Sub

        Public Sub Remove(Point As Datapoint)
            Data.Remove(Point)
        End Sub

        Public Sub AddPoint(d As Datapoint)
            Data.Add(d)
        End Sub
        Public Function AddXY(X As Double, Y As Double) As Datapoint
            Dim newd = New Datapoint(X, Y)
            Data.Add(newd)
            Return newd
        End Function
        Public Function AddXYErr(X As Double, XErr As Double, Y As Double, YErr As Double) As Datapoint
            Dim newd = New Datapoint(X, XErr, Y, YErr)
            Data.Add(newd)
            Return newd
        End Function
        Public Sub AddXYRange(X() As Double, Y() As Double)
            If X IsNot Nothing AndAlso Y IsNot Nothing AndAlso X.Count = Y.Count Then
                Data.RaiseListChangedEvents = False

                For i = 0 To X.Count - 1
                    Data.Add(New Datapoint(X(i), Y(i)))
                Next
                Data.RaiseListChangedEvents = True
                Data.ResetBindings()
            End If
        End Sub
        Public Sub AddXYRangeLogarithmic(X() As Double, Y() As Double, XLog As Boolean, YLog As Boolean)
            If X IsNot Nothing AndAlso Y IsNot Nothing AndAlso X.Count = Y.Count Then
                Data.RaiseListChangedEvents = False
                For i = 0 To X.Count - 1
                    If (XLog AndAlso X(i) <= 0) OrElse (YLog AndAlso Y(i) <= 0) Then Continue For
                    Dim thisx As Double = X(i)
                    Dim thisy As Double = Y(i)
                    If XLog Then thisx = Math.Log10(thisx)
                    If YLog Then thisy = Math.Log10(thisy)

                    Data.Add(New Datapoint(thisx, thisy))
                Next
                Data.RaiseListChangedEvents = True
                Data.ResetBindings()
            End If
        End Sub


        Public Sub ClearData()
            If Data IsNot Nothing Then Data.Clear()
        End Sub

        Public Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
            RecreateStyles()
            If DrawXErrors Then
                XErrorBars.Paint(g, Me, XAxis, YAxis)
            End If
            If DrawYErrors Then
                YErrorBars.Paint(g, Me, XAxis, YAxis)
            End If

            Select Case Me.SeriesType
                Case enumSeriesType.Scatter
                    PointSeriesDesigner.Paint(g, Me, XAxis, YAxis, Bounds)
                Case enumSeriesType.Line
                    LineSeriesDesigner.Paint(g, Me, XAxis, YAxis, Bounds)
                Case enumSeriesType.LineScatter
                    PointLineSeriesDesigner.Paint(g, Me, XAxis, YAxis, Bounds)
                Case enumSeriesType.Histogram
                    HistrogramSeriesDesigner.Paint(g, Me, XAxis, YAxis, Bounds)
            End Select
            If Me.MarkerBorderPen IsNot Nothing Then Me.MarkerBorderPen.Dispose()
            If Me.MarkerBrush IsNot Nothing Then Me.MarkerBrush.Dispose()
            If Me.LinePen IsNot Nothing Then Me.LinePen.Dispose()
            If Me.ErrorPen IsNot Nothing Then Me.ErrorPen.Dispose()
        End Sub
        Public Sub PaintLegend(ByRef g As Graphics, Bounds As RectangleF)
            RecreateStyles()
            Select Case Me.SeriesType
                Case enumSeriesType.Scatter
                    PointSeriesDesigner.PaintLegend(g, Me, Bounds)
                Case enumSeriesType.Line
                    LineSeriesDesigner.PaintLegend(g, Me, Bounds)
                Case enumSeriesType.LineScatter
                    PointLineSeriesDesigner.PaintLegend(g, Me, Bounds)
                Case enumSeriesType.Histogram
                    HistrogramSeriesDesigner.PaintLegend(g, Me, Bounds)
            End Select
            If Me.MarkerBorderPen IsNot Nothing Then Me.MarkerBorderPen.Dispose()
            If Me.MarkerBrush IsNot Nothing Then Me.MarkerBrush.Dispose()
            If Me.LinePen IsNot Nothing Then Me.LinePen.Dispose()
            If Me.ErrorPen IsNot Nothing Then Me.ErrorPen.Dispose()
        End Sub
        Private Sub Data_ListChanged(sender As Object, e As ListChangedEventArgs) Handles Data.ListChanged
            Notify("Data")
        End Sub

        Public Sub Notify(name As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
        Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged


#Region "IDisposable Support"
        Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: Verwalteten Zustand löschen (verwaltete Objekte).
                    If Me._MarkerBorderPen IsNot Nothing Then Me._MarkerBorderPen.Dispose()
                    If Me._MarkerBrush IsNot Nothing Then Me._MarkerBrush.Dispose()
                    If Me._LinePen IsNot Nothing Then Me._LinePen.Dispose()
                    If Me._ErrorPen IsNot Nothing Then Me._ErrorPen.Dispose()
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
