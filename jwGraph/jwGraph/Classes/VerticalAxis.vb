Imports jwGraph.GeneralTools
Namespace jwGraph

    <System.ComponentModel.Designer(GetType(System.ComponentModel.Design.ComponentDesigner))>
    <System.ComponentModel.TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
    Public Class VerticalAxis
        Inherits Axis

        Public Overrides Function ToString() As String
            Return If(Me.AxisLocation = enumAxisLocation.Primary, "Primary Y axis", "Secondary Y axis")
        End Function

        Public Sub New(Parent As jwGraph)
            MyBase.New(Parent)
            Me.TitleDistance = 55
            Me.OriginColor = Color.Blue
        End Sub

        Public Overrides Sub Paint(ByRef g As Graphics, ByRef AxisPen As Pen, ByRef GridPen As Pen, StartPoint As Integer, EndPoint As Integer, Location As Integer, OtherSide As Integer)
            Dim MeOK As Boolean = Me.IsAxisOK

            'Ticks left and right location
            Dim Tick1 As Integer
            If AxisLocation = enumAxisLocation.Primary Then
                Tick1 = If(TickDirection = enumTickDirection.Outwards, Location - TickWidth, Location + TickWidth)
            Else
                Tick1 = If(TickDirection = enumTickDirection.Inwards, Location - TickWidth, Location + TickWidth)
            End If
            Dim tick2 As Integer = Location

            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

            If GeneralTools.RoundSignificant2(Me.Minimum, 10) = GeneralTools.RoundSignificant2(Me.Maximum, 10) OrElse Double.IsInfinity(Minimum) OrElse Double.IsInfinity(Maximum) OrElse
                Double.IsNaN(Minimum) OrElse Double.IsNaN(Maximum) Then 'Branch on this special case
                'Draw Axis
                g.DrawLine(AxisPen, New Point(Location, StartPoint), New Point(Location, EndPoint))
                If MeOK Then
                    'Draw ticks
                    g.DrawLine(AxisPen, Tick1, StartPoint, tick2, StartPoint)
                    g.DrawLine(AxisPen, Tick1, EndPoint, tick2, EndPoint)
                    'Draw tick labels
                    Dim LabelValue As String
                    If Me.ShowLogarithmic Then
                        LabelValue = (10 ^ Me.Minimum).TryFormat(Me.LabelFormat)
                    Else
                        LabelValue = Me.Minimum.TryFormat(Me.LabelFormat)
                    End If

                    Dim size As SizeF = g.MeasureString(LabelValue, Me.LabelFont)
                    If AxisLocation = enumAxisLocation.Primary Then
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(Location - size.Width - TickWidth + 2, EndPoint - size.Height / 2))
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(Location - size.Width - TickWidth + 2, StartPoint - size.Height / 2))
                    Else
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(Location + TickWidth + 2, EndPoint - size.Height / 2))
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(Location + TickWidth + 2, StartPoint - size.Height / 2))
                    End If

                    'Draw title
                    Dim fmt As StringFormat = StringFormat.GenericTypographic
                    Dim tSize As SizeF = g.MeasureString(Me.Title, Me.TitleFont, 100000, fmt)
                    tSize = New SizeF(tSize.Width - 6, tSize.Height - 6)
                    'Draw title
                    g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
                    If AxisLocation = enumAxisLocation.Primary Then
                        GeneralTools.DrawRotatedString(g, Me.Title, Me.TitleFont, Brushes.Black, CSng(Location - TitleDistance - tSize.Height), CSng((EndPoint - StartPoint) / 2 + StartPoint), 270, fmt)
                    Else
                        GeneralTools.DrawRotatedString(g, Me.Title, Me.TitleFont, Brushes.Black, CSng(Location + Me.TitleDistance), CSng((EndPoint - StartPoint) / 2 + StartPoint), 90, fmt)
                    End If
                End If
                Exit Sub
            End If


            'Calculate Ticks


            Dim Ticks As New List(Of Single)
            Dim TickValues As New List(Of Double)

            If Me.UseExactSteps Then
                Ticks.Add(StartPoint)
                Ticks.Add(EndPoint)
                TickValues.Add(Me.Minimum)
                TickValues.Add(Me.Maximum)
                Dim Delta As Single = CSng((EndPoint - StartPoint) / nTicks)
                Dim DeltaVal As Double = CSng((Maximum - Minimum) / nTicks)
                For i = 1 To nTicks - 1
                    Ticks.Add(StartPoint + i * Delta)
                    TickValues.Add(Me.Minimum + i * DeltaVal)
                Next
            Else
                TickValues.AddRange(GetSteps(Me.Minimum, Me.Maximum, nTicks))
                For i = 0 To TickValues.Count - 1
                    Ticks.Add(Me.ValueToPixelPosition(TickValues(i)) + StartPoint)
                Next
            End If

            'Ticks.Add(StartPoint)
            'Ticks.Add(EndPoint)
            'TickValues.Add(Me.Minimum)
            'TickValues.Add(Me.Maximum)

            'Dim Delta As Single = CSng((EndPoint - StartPoint) / nTicks)
            'Dim DeltaVal As Double = CSng((Maximum - Minimum) / nTicks)
            'For i = 1 To nTicks - 1
            '    Ticks.Add(StartPoint + i * Delta)
            '    TickValues.Add(Me.Minimum + i * DeltaVal)
            'Next
            Ticks.Sort()
            TickValues.Sort()
            TickValues.Reverse()

            'Draw gridlines
            If MeOK AndAlso ShowGridlines Then
                For i = 0 To Ticks.Count - 1
                    If Ticks(i) > StartPoint AndAlso Ticks(i) < EndPoint Then
                        g.DrawLine(GridPen, Location, Ticks(i), OtherSide, Ticks(i))
                    End If
                Next
            End If

            'Draw Axis
            g.DrawLine(AxisPen, New Point(Location, StartPoint), New Point(Location, EndPoint))

            Dim f As Font
            Dim LabelHeight As Single = 0
            If MeOK Then
                'Draw Ticks

                For i = 0 To Ticks.Count - 1
                    If Ticks(i) <= StartPoint OrElse Ticks(i) >= EndPoint Then Continue For
                    g.DrawLine(AxisPen, Tick1, Ticks(i), tick2, Ticks(i))
                Next

                'Draw tick labels
                f = Me.LabelFont
                If Me.ShowTickText Then
                    For i = 0 To Ticks.Count - 1
                        If Ticks(i) <= StartPoint OrElse Ticks(i) >= EndPoint Then Continue For
                        If Me.FirstAndLastTickTextOnly AndAlso i > 1 AndAlso i < Ticks.Count - 2 Then Continue For

                        Dim thisval As String
                        If Me.ShowLogarithmic Then
                            thisval = (10 ^ TickValues(i)).TryFormat(Me.LabelFormat)
                        Else
                            thisval = TickValues(i).TryFormat(Me.LabelFormat)
                        End If

                        Dim size As SizeF = g.MeasureString(thisval, f)
                        If size.Width > LabelHeight Then LabelHeight = size.Width
                        If AxisLocation = enumAxisLocation.Primary Then
                            g.DrawString(thisval, f, Brushes.Black, New PointF(Location - size.Width - TickWidth - 2, Ticks(i) - size.Height / 2))
                        Else
                            g.DrawString(thisval, f, Brushes.Black, New PointF(Location + TickWidth + 2, Ticks(i) - size.Height / 2))
                        End If
                    Next
                End If

                'Draw minor ticks
                Dim logbase As Integer = 10

                For i = 0 To TickValues.Count - 2
                    '  logbase = CInt(10 ^ (i + 1))
                    Dim minorticks As New List(Of Double)

                    'Calculate logarithmically scaled minor ticks in case the data is shown as a logarithm
                    If ShowLogarithmic Then
                        Dim mdelta As Double = (logbase ^ TickValues(i + 1) - logbase ^ TickValues(i)) / (Me.nMinorTicks + 1)

                        For a = 1 To Me.nMinorTicks
                            Dim value As Double = logbase ^ TickValues(i) + (a) * mdelta
                            minorticks.Add(Math.Log(value, logbase))
                        Next
                    Else
                        'Or just use linear
                        Dim mdelta As Double = (TickValues(i + 1) - TickValues(i)) / (Me.nMinorTicks + 1)

                        For a = 1 To Me.nMinorTicks
                            Dim value As Double = TickValues(i) + (a) * mdelta
                            minorticks.Add(value)
                        Next
                    End If

                    'Draw the ticks
                    Dim minortickwidth As Integer = CInt(TickWidth / 2)

                    If AxisLocation = enumAxisLocation.Primary Then
                        Tick1 = If(TickDirection = enumTickDirection.Outwards, Location - minortickwidth, Location + minortickwidth)
                    Else
                        Tick1 = If(TickDirection = enumTickDirection.Inwards, Location - minortickwidth, Location + minortickwidth)
                    End If
                    tick2 = Location

                    For Each m As Double In minorticks
                        If m <= Me.Minimum OrElse m >= Me.Maximum Then Continue For
                        Dim loc As Double = Me.ValueToPixelPosition(m) + StartPoint
                        g.DrawLine(AxisPen, Tick1, CInt(loc), tick2, CInt(loc))
                    Next
                Next

            End If

            f = Me.TitleFont
            Dim format As StringFormat = StringFormat.GenericTypographic
            Dim Titlesize As SizeF = g.MeasureString(Me.Title, f, 100000, format)
            Titlesize = New SizeF(Titlesize.Width - 6, Titlesize.Height - 6)
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            'Draw title
            If AxisLocation = enumAxisLocation.Primary Then
                If Me.AutoPositionTitle Then
                    GeneralTools.DrawRotatedString(g, Me.Title, f, Brushes.Black, CSng(Location - TickWidth - AutoPositionTitleDistance - LabelHeight - 2 - Titlesize.Height / 2), CSng((EndPoint - StartPoint) / 2 + StartPoint), 270, format)
                Else
                    GeneralTools.DrawRotatedString(g, Me.Title, f, Brushes.Black, CSng(Location - TitleDistance - Titlesize.Height), CSng((EndPoint - StartPoint) / 2 + StartPoint), 270, format)
                End If
            Else
                If Me.AutoPositionTitle Then
                    GeneralTools.DrawRotatedString(g, Me.Title, f, Brushes.Black, CSng(Location + TickWidth + AutoPositionTitleDistance + LabelHeight + 2 + Titlesize.Height / 2), CSng((EndPoint - StartPoint) / 2 + StartPoint), 90, format)
                Else
                    GeneralTools.DrawRotatedString(g, Me.Title, f, Brushes.Black, CSng(Location + Me.TitleDistance), CSng((EndPoint - StartPoint) / 2 + StartPoint), 90, format)
                End If
            End If

        End Sub

        Public Overrides Function PixelPositionToValue(Position As Single) As Double
            Dim ca As Rectangle = Me.Parent.InnerChartArea
            Return CDbl((1 - (Position - ca.Top) / (ca.Height)) * (Maximum - Minimum) + Minimum)
        End Function

        Public Overrides Function ValueToPixelPosition(Value As Double) As Single
            Dim ca As Rectangle = Me.Parent.InnerChartArea
            Return CSng(ca.Height - (Value - Minimum) / (Maximum - Minimum) * ca.Height)
        End Function

        Public Overrides Function PixelDistanceToValueDistance(PixelDistance As Single) As Double
            Dim lp As Integer = Me.Parent.InnerChartArea.Height
            Dim delta As Double = Me.Maximum - Me.Minimum
            Return delta / lp * PixelDistance
        End Function

        Public Overrides Function ValueDistanceToPixelDistance(ValueDistance As Double) As Single
            Dim lp As Integer = Me.Parent.InnerChartArea.Height
            Dim delta As Double = Me.Maximum - Me.Minimum
            Return CSng(lp / delta * ValueDistance)
        End Function
    End Class
End Namespace
