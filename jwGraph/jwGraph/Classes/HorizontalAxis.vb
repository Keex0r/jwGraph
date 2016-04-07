Imports jwGraph.GeneralTools
Namespace jwGraph

    <System.ComponentModel.Designer(GetType(System.ComponentModel.Design.ComponentDesigner))>
  <System.ComponentModel.TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
    Public Class HorizontalAxis
        Inherits Axis

        Public Overrides Function ToString() As String
            Return If(Me.AxisLocation = enumAxisLocation.Primary, "Primary X axis", "Secondary X axis")
        End Function


        Public Sub New(Parent As jwGraph)
            MyBase.New(Parent)
            Me.TitleDistance = 30
            Me.OriginColor = Color.Blue
        End Sub

        'Public Overrides Sub PaintQuick(ByRef g As Graphics, ByRef AxisPen As Pen, ByRef GridPen As Pen, StartPoint As Integer, EndPoint As Integer, Location As Integer, OtherSide As Integer)
        '    Dim MeOK As Boolean = Me.IsAxisOK
        '    'Draw Axis
        '    g.DrawLine(AxisPen, New Point(StartPoint, Location), New Point(EndPoint, Location))
        '    Dim f As Font = Me.TitleFont
        '    Dim Titlesize As SizeF = g.MeasureString(Me.Title, f)
        '    If AxisLocation = enumAxisLocation.Primary Then
        '        If AutoPositionTitle Then
        '            g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location))
        '        Else
        '            g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location + TitleDistance))
        '        End If

        '    Else
        '        If AutoPositionTitle Then
        '            g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location - Titlesize.Height))
        '        Else
        '            g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location - TitleDistance))
        '        End If
        '    End If
        'End Sub

        Public Overrides Sub Paint(ByRef g As Graphics, ByRef AxisPen As Pen, ByRef GridPen As Pen, StartPoint As Integer, EndPoint As Integer, Location As Integer, OtherSide As Integer)
            Dim MeOK As Boolean = Me.IsAxisOK

            'Ticks top and bottom location
            Dim Tick1 As Integer
            If AxisLocation = enumAxisLocation.Primary Then
                Tick1 = If(TickDirection = enumTickDirection.Inwards, Location - TickWidth, Location + TickWidth)
            Else
                Tick1 = If(TickDirection = enumTickDirection.Outwards, Location - TickWidth, Location + TickWidth)
            End If
            Dim tick2 As Integer = Location

            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

            If GeneralTools.RoundSignificant2(Me.Minimum, 10) = GeneralTools.RoundSignificant2(Me.Maximum, 10) OrElse Double.IsInfinity(Minimum) OrElse Double.IsInfinity(Maximum) OrElse
                Double.IsNaN(Minimum) OrElse Double.IsNaN(Maximum) Then 'Branch on this special case
                'Draw Axis
                g.DrawLine(AxisPen, New Point(StartPoint, Location), New Point(EndPoint, Location))
                If MeOK Then
                    'Draw ticks
                    g.DrawLine(AxisPen, StartPoint, Tick1, StartPoint, tick2)
                    g.DrawLine(AxisPen, EndPoint, Tick1, EndPoint, tick2)

                    'Draw tick labels
                    Dim LabelValue As String
                    If Me.ShowLogarithmic Then
                        LabelValue = (10 ^ Me.Minimum).TryFormat(Me.LabelFormat)
                    Else
                        LabelValue = Me.Minimum.TryFormat(Me.LabelFormat)
                    End If

                    Dim size As SizeF = g.MeasureString(LabelValue, Me.LabelFont)
                    If AxisLocation = enumAxisLocation.Primary Then
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(StartPoint - size.Width / 2, Location + TickWidth + 2))
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(EndPoint - size.Width / 2, Location + TickWidth + 2))
                    Else
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(StartPoint - size.Width / 2, Location - TickWidth - size.Height - 2))
                        g.DrawString(LabelValue, Me.LabelFont, Brushes.Black, New PointF(EndPoint - size.Width / 2, Location - TickWidth - size.Height - 2))
                    End If

                    'Draw title
                    Dim fmt As StringFormat = StringFormat.GenericTypographic
                    fmt.FormatFlags = fmt.FormatFlags Or StringFormatFlags.MeasureTrailingSpaces
                    Dim tSize As SizeF = g.MeasureString(Me.Title, Me.TitleFont, 10000, fmt)
                    tSize = New SizeF(tSize.Width - 6, tSize.Height - 6)

                    g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
                    If AxisLocation = enumAxisLocation.Primary Then
                        g.DrawString(Me.Title, Me.TitleFont, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - tSize.Width / 2), CSng(Location + TitleDistance))
                    Else
                        g.DrawString(Me.Title, Me.TitleFont, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - tSize.Width / 2), CSng(Location - TitleDistance))
                    End If

                End If
                Exit Sub
            End If

            'Calculate major Ticks
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

            Ticks.Sort()
            TickValues.Sort()

            'Draw gridlines
            If MeOK AndAlso ShowGridlines Then
                For i = 0 To Ticks.Count - 1
                    If Ticks(i) > StartPoint AndAlso Ticks(i) < EndPoint Then
                        g.DrawLine(GridPen, Ticks(i), Location, Ticks(i), OtherSide)
                    End If
                Next
            End If

            'Draw Axis
            g.DrawLine(AxisPen, New Point(StartPoint, Location), New Point(EndPoint, Location))

            Dim f As Font
            Dim LabelHeight As Single = 0
            If MeOK Then

                'Draw Ticks

                For i = 0 To Ticks.Count - 1
                    If Ticks(i) <= StartPoint OrElse Ticks(i) >= EndPoint Then Continue For
                    g.DrawLine(AxisPen, Ticks(i), Tick1, Ticks(i), tick2)
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
                        If size.Height > LabelHeight Then LabelHeight = size.Height
                        If AxisLocation = enumAxisLocation.Primary Then
                            g.DrawString(thisval, f, Brushes.Black, New PointF(Ticks(i) - size.Width / 2, Location + TickWidth + 2))
                        Else
                            g.DrawString(thisval, f, Brushes.Black, New PointF(Ticks(i) - size.Width / 2, Location - TickWidth - size.Height - 2))
                        End If
                    Next
                End If
                'Draw minor ticks
                Dim logbase As Integer = 10

                For i = 0 To TickValues.Count - 2
                    '    logbase = CInt(10 ^ (i + 1))
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
                        Tick1 = If(TickDirection = enumTickDirection.Inwards, Location - minortickwidth, Location + minortickwidth)
                    Else
                        Tick1 = If(TickDirection = enumTickDirection.Outwards, Location - minortickwidth, Location + minortickwidth)
                    End If
                    tick2 = Location

                    For Each m As Double In minorticks
                        If m <= Me.Minimum OrElse m >= Me.Maximum Then Continue For
                        Dim loc As Double = Me.ValueToPixelPosition(m) + StartPoint
                        g.DrawLine(AxisPen, CInt(loc), Tick1, CInt(loc), tick2)
                    Next
                Next


            End If

            f = Me.TitleFont

            Dim format As StringFormat = StringFormat.GenericTypographic
            format.FormatFlags = format.FormatFlags Or StringFormatFlags.MeasureTrailingSpaces
            Dim Titlesize As SizeF = g.MeasureString(Me.Title, f, 10000, format)
            Titlesize = New SizeF(Titlesize.Width - 6, Titlesize.Height - 6)

            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            If AxisLocation = enumAxisLocation.Primary Then
                If AutoPositionTitle Then
                    g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location + TickWidth + AutoPositionTitleDistance + LabelHeight + 2))
                Else
                    g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location + TitleDistance))
                End If

            Else
                If AutoPositionTitle Then
                    g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location - TickWidth - AutoPositionTitleDistance - LabelHeight - 2 - Titlesize.Height))
                Else
                    g.DrawString(Me.Title, f, Brushes.Black, CSng((EndPoint - StartPoint) / 2 + StartPoint - Titlesize.Width / 2), CSng(Location - TitleDistance))
                End If

            End If


        End Sub

        Public Overrides Function PixelPositionToValue(Position As Single) As Double
            Dim ca As Rectangle = Me.Parent.InnerChartArea
            Return CDbl((Position - ca.Left) / (ca.Width) * (Maximum - Minimum) + Minimum)
        End Function

        Public Overrides Function ValueToPixelPosition(Value As Double) As Single
            Dim ca As Rectangle = Me.Parent.InnerChartArea
            Return CSng((Value - Minimum) / (Maximum - Minimum) * ca.Width)
        End Function

        Public Overrides Function PixelDistanceToValueDistance(PixelDistance As Single) As Double
            Dim lp As Integer = Me.Parent.InnerChartArea.Width
            Dim delta As Double = Me.Maximum - Me.Minimum
            Return delta / lp * PixelDistance
        End Function

        Public Overrides Function ValueDistanceToPixelDistance(ValueDistance As Double) As Single
            Dim lp As Integer = Me.Parent.InnerChartArea.Width
            Dim delta As Double = Me.Maximum - Me.Minimum
            Return CSng(lp / delta * ValueDistance)
        End Function
    End Class
End Namespace
