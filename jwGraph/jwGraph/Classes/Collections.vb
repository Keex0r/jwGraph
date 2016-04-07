Imports System.ComponentModel
Imports jwGraph
Namespace jwGraph

    Public Class HorizontalMarkerCollection
        Inherits System.Collections.ObjectModel.ObservableCollection(Of HorizontalMarker)
    End Class
    Public Class VerticalMarkerCollection
        Inherits System.Collections.ObjectModel.ObservableCollection(Of VerticalMarker)
    End Class
    Public Class FreeMarkerCollection
        Inherits System.Collections.ObjectModel.ObservableCollection(Of FreeMarker)
    End Class
    Public Class DatapointCollection
        Inherits System.Collections.ObjectModel.ObservableCollection(Of Datapoint)
    End Class

    Public Class SeriesCollection
        Inherits System.Collections.ObjectModel.ObservableCollection(Of Series)

        Private ColorList As Color() = {Color.Black, Color.Red, Color.Blue, Color.ForestGreen, Color.Magenta, Color.Orange, Color.Gold, Color.MediumBlue, Color.DarkRed}
        Private StylesListLine As Drawing2D.DashStyle() = {Drawing2D.DashStyle.Solid, Drawing2D.DashStyle.Dash, Drawing2D.DashStyle.Dot, Drawing2D.DashStyle.DashDot}
        Private StylesListMarker As Series.enumMarkerStyle() = {Series.enumMarkerStyle.Circle, Series.enumMarkerStyle.Rectangle, Series.enumMarkerStyle.Diamond, Series.enumMarkerStyle.Cross}

        Private ColorCyclesY1 As Dictionary(Of Series.enumSeriesType, Integer)
        Private ColorCyclesY2 As Dictionary(Of Series.enumSeriesType, Integer)

        Private StyleCyclesY1 As Dictionary(Of Series.enumSeriesType, Integer)
        Private StyleCyclesY2 As Dictionary(Of Series.enumSeriesType, Integer)

        Public SyncColorCycles As Boolean = False
        Public SyncStyleCycles As Boolean = False

        Public AdvanceColors As Boolean = True
        Public AdvanceStyles As Boolean = False

        Public Overloads Sub Clear()
            ResetColorList()
            MyBase.Clear()
        End Sub

        Public Overloads Function Contains(Name As String) As Boolean
            For i = 0 To Me.Items.Count - 1
                If Me.Item(i).Name = Name Then Return True
            Next
            Return False
        End Function

        Public Overloads Function Remove(Name As String) As Boolean
            Dim s1 = Me.Item(Name)
            If s1 IsNot Nothing Then
                Me.Remove(s1)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overloads Function Remove(Names As String()) As Boolean
            Dim RemovedSome As Boolean = False
            For Each s In Names
                RemovedSome = RemovedSome Or Me.Remove(s)
            Next
            Return RemovedSome
        End Function


        Public Property MarkerDefaultSizeY1 As Integer
        Public Property MarkerDefaultSizeY2 As Integer

        Public Property MarkerDefaultStyleY1 As Series.enumMarkerStyle
        Public Property LineDefaultStyleY1 As Drawing2D.DashStyle
        Public Property MarkerDefaultStyleY2 As Series.enumMarkerStyle
        Public Property LineDefaultStyleY2 As Drawing2D.DashStyle

        Public Property LinePenDefaultWidthY1 As Integer
        Public Property LinePenDefaultWidthY2 As Integer



        Default Public Overloads Property Item(s As String) As Series
            Get
                For i = 0 To Me.Items.Count - 1
                    If Me.Item(i).Name = s Then Return Me.Item(i)
                Next
                Return Nothing
            End Get
            Set(value As Series)
                For i = 0 To Me.Items.Count - 1
                    If Me.Item(i).Name = s Then
                        Me.Item(i) = value
                        If Parent IsNot Nothing Then Parent.Invalidate()
                        Exit Property
                    End If
                Next
            End Set
        End Property

        Public Parent As jwGraph

        ''' <summary>
        ''' Initialisiert die Schritte für die Style und Color Listen. Jeder Diagrammtyp bekommt einen eigenen Counter.
        ''' </summary>
        Private Function InitCycleList() As Dictionary(Of Series.enumSeriesType, Integer)
            Dim ColorCycles = New Dictionary(Of Series.enumSeriesType, Integer)
            ColorCycles.Add(Series.enumSeriesType.Line, 0)
            ColorCycles.Add(Series.enumSeriesType.Histogram, 0)
            ColorCycles.Add(Series.enumSeriesType.LineScatter, 0)
            ColorCycles.Add(Series.enumSeriesType.Scatter, 0)
            Return ColorCycles
        End Function



        Public Sub New(Parent As jwGraph)
            MyBase.New()

            SyncColorCycles = True
            ColorCyclesY1 = InitCycleList()
            ColorCyclesY2 = InitCycleList()
            StyleCyclesY1 = InitCycleList()
            StyleCyclesY2 = InitCycleList()

            Me.Parent = Parent
            MarkerDefaultSizeY1 = 8
            MarkerDefaultStyleY1 = Series.enumMarkerStyle.Circle
            LineDefaultStyleY1 = Drawing2D.DashStyle.Solid
            LinePenDefaultWidthY1 = 1

            MarkerDefaultSizeY2 = 8
            MarkerDefaultStyleY2 = Series.enumMarkerStyle.Rectangle
            LineDefaultStyleY2 = Drawing2D.DashStyle.Solid
            LinePenDefaultWidthY2 = 1

        End Sub

#Region "Reset color lists"
        Public Sub ResetColorList()
            ResetColorList(Axis.enumAxisLocation.Primary)
            ResetColorList(Axis.enumAxisLocation.Secondary)
        End Sub
        Public Sub ResetColorList(Axis As Axis.enumAxisLocation)
            Dim ks As New List(Of Series.enumSeriesType)
            ks.AddRange(ColorCyclesY1.Keys)

            If Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary Then
                For Each k As Series.enumSeriesType In ks
                    ColorCyclesY1(k) = 0
                Next
            Else
                For Each k As Series.enumSeriesType In ks
                    ColorCyclesY2(k) = 0
                Next
            End If
        End Sub
        Public Sub ResetColorList(Type As Series.enumSeriesType)
            ColorCyclesY1(Type) = 0
            ColorCyclesY2(Type) = 0
        End Sub
        Public Sub ResetColorList(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation)
            If Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary Then
                ColorCyclesY1(Type) = 0
            Else
                ColorCyclesY2(Type) = 0
            End If
        End Sub
#End Region
#Region "Advance color lists"
        Private Sub AdvanceColorList(List As Dictionary(Of Series.enumSeriesType, Integer), Type As Series.enumSeriesType)
            List(Type) += 1
            If List(Type) >= ColorList.Count Then List(Type) = 0
        End Sub
        Private Sub AdvanceStyleList(List As Dictionary(Of Series.enumSeriesType, Integer), Type As Series.enumSeriesType)
            List(Type) += 1
            If List(Type) >= StylesListLine.Count Then List(Type) = 0
        End Sub

        Private Sub AdvanceCorrectList(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation)
            If AdvanceColors Then
                If SyncColorCycles Then
                    AdvanceColorList(ColorCyclesY1, Type)
                    AdvanceColorList(ColorCyclesY2, Type)
                Else
                    AdvanceColorList(If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, ColorCyclesY1, ColorCyclesY2), Type)
                End If
            End If
            If AdvanceStyles Then
                If SyncStyleCycles Then
                    AdvanceStyleList(StyleCyclesY1, Type)
                    AdvanceStyleList(StyleCyclesY2, Type)
                Else
                    AdvanceStyleList(If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, StyleCyclesY1, StyleCyclesY2), Type)
                End If
            End If
        End Sub
#End Region
#Region "Add Series"
        Public Function AddSeries(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation) As Series
            Return AddSeries(Type, Axis, New List(Of Double), New List(Of Double))
        End Function

        Public Function AddSeries(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation, XData As IEnumerable(Of Double), YData As IEnumerable(Of Double)) As Series
            Return AddSeries(Type, Axis, XData, YData, New List(Of Double))
        End Function

        Public Function AddSeries(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation, XData As IEnumerable(Of Double), YData As IEnumerable(Of Double), Tags As IEnumerable(Of Object)) As Series
            Return AddSeries(Type, Axis, XData, YData, New List(Of Double), Tags)
        End Function


        Private Function GetDefaultLinestyle(Axis As Axis.enumAxisLocation) As Drawing2D.DashStyle
            Return If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, LineDefaultStyleY1, LineDefaultStyleY2)
        End Function
        Private Function GetDefaultLineWidth(Axis As Axis.enumAxisLocation) As Integer
            Return If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, LinePenDefaultWidthY1, LinePenDefaultWidthY2)
        End Function

        Private Function GetDefaultMarkerstyle(Axis As Axis.enumAxisLocation) As Series.enumMarkerStyle
            Return If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, MarkerDefaultStyleY1, MarkerDefaultStyleY2)
        End Function
        Private Function GetDefaultMarkerSize(Axis As Axis.enumAxisLocation) As Integer
            Return If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, MarkerDefaultSizeY1, MarkerDefaultSizeY2)
        End Function

        Public Function AddSeries(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation,
                                  XData As IEnumerable(Of Double), YData As IEnumerable(Of Double),
                                  YErrors As IEnumerable(Of Double)) As Series
            Return AddSeries(Type, Axis, XData, YData, YErrors, Nothing)
        End Function
        Public Function AddSeries(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation,
                                  XData As IEnumerable(Of Double), YData As IEnumerable(Of Double),
                                  YErrors As IEnumerable(Of Double), Tags As IEnumerable(Of Object)) As Series
            Dim count As Integer = 0
            Dim hasTags As Boolean = Tags IsNot Nothing
            While Me.Contains("Series" & count.ToString)
                count += 1
            End While
            Dim Name As String = "Series" & count.ToString
            Dim s As New Series(Name, Axis, Type)
            s.DrawYErrors = True

            Dim colorcycles = If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, ColorCyclesY1, ColorCyclesY2)
            Dim stylecycles = If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, StyleCyclesY1, StyleCyclesY2)

            s.LineColor = ColorList(colorcycles(Type))
            s.LineWidth = GetDefaultLineWidth(Axis)
            s.LineStyle = If(Me.AdvanceStyles, StylesListLine(stylecycles(Type)), GetDefaultLinestyle(Axis))
            s.ErrorLineColor = ColorList(colorcycles(Type))


            s.MarkerBorderColor = Color.Black
            s.MarkerFillColor = ColorList(colorcycles(Type))
            s.MarkerBorderLineWidth = 1
            s.MarkerSize = GetDefaultMarkerSize(Axis)
            s.MarkerStyle = If(AdvanceStyles, StylesListMarker(stylecycles(Type)), GetDefaultMarkerstyle(Axis))
            Dim tagCount As Integer = 0
            For i = 0 To XData.Count - 1
                If i < YErrors.Count Then
                    Dim p = s.AddXYErr(XData(i), 0, YData(i), YErrors(i))
                    If hasTags Then p.Tag = Tags(tagCount)
                Else
                    Dim p = s.AddXY(XData(i), YData(i))
                    If hasTags Then p.Tag = Tags(tagCount)
                End If
                tagCount += 1
            Next

            Me.Add(s)

            AdvanceCorrectList(Type, Axis)

            If Parent IsNot Nothing Then Parent.Invalidate()
            Return s
        End Function

        Public Function AddSeriesLogarithmic(Type As Series.enumSeriesType, Axis As Axis.enumAxisLocation, XData As List(Of Double), YData As List(Of Double), XLog As Boolean, YLog As Boolean) As Series
            Dim count As Integer = 0
            While Me.Contains("Series" & count.ToString)
                count += 1
            End While
            Dim Name As String = "Series" & count.ToString
            Dim s As New Series(Name, Axis, Type)

            Dim colorcycles = If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, ColorCyclesY1, ColorCyclesY2)
            Dim stylecycles = If(Axis = Global.jwGraph.jwGraph.Axis.enumAxisLocation.Primary, StyleCyclesY1, StyleCyclesY2)

            s.LineColor = ColorList(colorcycles(Type))
            s.LineWidth = GetDefaultLineWidth(Axis)
            s.LineStyle = If(Me.AdvanceStyles, StylesListLine(stylecycles(Type)), GetDefaultLinestyle(Axis))

            s.MarkerBorderColor = Color.Black
            s.MarkerFillColor = ColorList(colorcycles(Type))
            s.MarkerBorderLineWidth = 1
            s.MarkerSize = GetDefaultMarkerSize(Axis)
            s.MarkerStyle = If(AdvanceStyles, StylesListMarker(stylecycles(Type)), GetDefaultMarkerstyle(Axis))


            s.AddXYRangeLogarithmic(XData.ToArray, YData.ToArray, XLog, YLog)

            Me.Add(s)

            AdvanceCorrectList(Type, Axis)

            If Parent IsNot Nothing Then Parent.Invalidate()
            Return s
        End Function
#End Region
        ''' <summary>
        ''' Calculates histogram X and Y values for the given data.
        ''' </summary>
        ''' <param name="Input">The list of values to plot the histogram for</param>
        ''' <param name="nBins">The number of bins to sort the values into</param>
        ''' <param name="CalculateNormal">If true, the normal distribution plot will be calculated and set as indizes 2+3 in the result</param>
        ''' <returns></returns>
        Public Function GetHistogramData(Input As IEnumerable(Of Double), nBins As Integer, CalculateNormal As Boolean) As Double()()
            If Input Is Nothing OrElse Input.Count = 0 Then Return Nothing
            Dim correctedinput = (From d In Input Where Not Double.IsNaN(d) AndAlso Not Double.IsInfinity(d) Select d).ToArray
            If correctedinput Is Nothing OrElse correctedinput.Count = 0 Then Return Nothing
            Dim res(If(CalculateNormal, 3, 1))() As Double
            Dim min As Double = correctedinput.Min
            Dim max As Double = correctedinput.Max
            If GeneralTools.RoundSignificant2(min, 9) = GeneralTools.RoundSignificant2(max, 9) Then
                res(0) = {min}
                res(1) = {correctedinput.Count}
                If CalculateNormal Then
                    res(2) = {}
                    res(3) = {}
                End If


                Return res
            End If
            Dim delta As Double = (max - min) / (nBins)
            ReDim res(0)(nBins - 1)
            ReDim res(1)(nBins - 1)

            For i = 1 To nBins
                Dim thisLower As Double = min + (i - 1) * delta
                Dim thisUpper As Double = min + (i) * delta
                res(0)(i - 1) = (thisUpper + thisLower) / 2 'Center of Bin
                res(1)(i - 1) = CDbl((From d As Double In correctedinput Where d >= thisLower AndAlso d < thisUpper Select d).Count)
            Next
            res(1)(nBins - 1) += 1 'Because the highest value will not be counted otherwise
            If CalculateNormal Then
                Dim GaussX As New List(Of Double)
                Dim GaussY As New List(Of Double)

                Dim avg As Double = correctedinput.Average
                Dim std = Global.jwGraph.GeneralTools.StdDev(correctedinput)
                Dim a = res(1).Max '1 / (std * Math.Sqrt(2 * Math.PI))
                Dim b = avg
                Dim c = std

                For x = correctedinput.Min To correctedinput.Max Step (correctedinput.Max - correctedinput.Min) / (correctedinput.Count - 1)
                    GaussX.Add(x)
                    GaussY.Add(a * Math.Exp(-(x - b) ^ 2 / (2 * c ^ 2)))
                Next
                Return {res(0), res(1), GaussX.ToArray, GaussY.ToArray}
            End If
            Return res
        End Function
        ''' <summary>
        ''' Calculates the standard normal distribution data for the given input
        ''' </summary>
        ''' <param name="Input">The data to calculate the normal distribution for</param>
        ''' <returns></returns>
        Public Function GetNormalDistributionData(Input As IEnumerable(Of Double)) As Double()()
            Dim GaussX As New List(Of Double)
            Dim GaussY As New List(Of Double)

            Dim avg As Double = Input.Average
            Dim std = Global.jwGraph.GeneralTools.StdDev(Input)
            Dim a = 1 / (std * Math.Sqrt(2 * Math.PI))
            Dim b = avg
            Dim c = std

            For x = Input.Min To Input.Max Step (Input.Max - Input.Min) / (Input.Count - 1)
                GaussX.Add(x)
                GaussY.Add(a * Math.Exp(-(x - b) ^ 2 / (2 * c ^ 2)))
            Next
            Return {GaussX.ToArray, GaussY.ToArray}
        End Function




    End Class
End Namespace
