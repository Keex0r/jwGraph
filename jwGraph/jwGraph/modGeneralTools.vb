Imports System.Runtime.CompilerServices
Imports System.Drawing
Namespace GeneralTools
    Public Module modGeneralTools

        Public Function RoundSignificant2(x As Double, p As Integer) As Double
            If Double.IsNaN(x) Then Return Double.NaN
            If Double.IsInfinity(x) Then Return x
            If x = 0 Then Return 0
            Dim sign As Integer = Math.Sign(x)
            x = Math.Abs(x)
            Dim n As Double = Math.Floor(Math.Log10(x)) + 1 - p
            Dim value As Double = sign * Math.Round(10 ^ -n * x) * 10 ^ n

            Return value
        End Function

        Public Function SameSigns(a As Single, b As Single) As Boolean
            If a <> b AndAlso (Single.IsNaN(a) OrElse Single.IsNaN(b)) Then Return False
            Return a = b OrElse Math.Sign(a) = Math.Sign(b)
        End Function

        Public Function LinesIntersect(X1 As Single, Y1 As Single, X2 As Single, Y2 As Single,
                                        X3 As Single, Y3 As Single, X4 As Single, Y4 As Single,
                                        ByRef x As Single, ByRef y As Single) As Boolean
            Dim a1 As Single = Y2 - Y1
            Dim b1 As Single = X1 - X2
            Dim c1 As Single = X2 * Y1 - X1 * Y2
            Dim r3 As Single = a1 * X3 + b1 * Y3 + c1
            Dim r4 As Single = a1 * X4 + b1 * Y4 + c1
            If r3 <> 0 AndAlso r4 <> 0 AndAlso SameSigns(r3, r4) Then Return False

            Dim a2 As Single = Y4 - Y3
            Dim b2 As Single = X3 - X4
            Dim c2 As Single = X4 * Y3 - X3 * Y4
            Dim r1 As Single = a2 * X1 + b2 * Y1 + c2
            Dim r2 As Single = a2 * X2 + b2 * Y2 + c2

            If r1 <> 0 AndAlso r2 <> 0 AndAlso SameSigns(r1, r2) Then Return False

            Dim denom As Single = a1 * b2 - a2 * b1
            If denom = 0 Then Return False
            Dim offset As Single
            If denom < 0 Then
                offset = -denom / 2
            Else
                offset = denom / 2
            End If

            Dim num As Single = b1 * c2 - b2 * c1
            If num < 0 Then
                x = num - offset
            Else
                x = num + offset
            End If
            x /= denom

            num = a2 * c1 - a1 * c2
            If num < 0 Then
                y = num - offset
            Else
                y = num + offset
            End If
            y /= denom

            Return True
        End Function

        Public Function IntersectsRectangle(X1 As Single, Y1 As Single, X2 As Single, Y2 As Single, _
                                            Rect As RectangleF, ByRef X As Single, ByRef Y As Single) As Boolean

            If LinesIntersect(X1, Y1, X2, Y2, Rect.X, Rect.Y, Rect.X, Rect.Bottom, X, Y) Then Return True 'Links
            If LinesIntersect(X1, Y1, X2, Y2, Rect.X, Rect.Y, Rect.Right, Rect.Y, X, Y) Then Return True 'Oben
            If LinesIntersect(X1, Y1, X2, Y2, Rect.Right, Rect.Y, Rect.Right, Rect.Bottom, X, Y) Then Return True 'Rechts
            If LinesIntersect(X1, Y1, X2, Y2, Rect.X, Rect.Bottom, Rect.Right, Rect.Bottom, X, Y) Then Return True 'Unten
            Return False
        End Function

        Public Sub DrawGradientRectangle(ByRef g As Graphics, Rect As Rectangle, Width As Integer, c1 As Color, c2 As Color)
            Dim r As New Rectangle(Rect.Location, Rect.Size)
            Dim deltar As Double = (CInt(c2.R) - CInt(c1.R)) / (Width - 1)
            Dim deltag As Double = (CInt(c2.G) - CInt(c1.G)) / (Width - 1)
            Dim deltab As Double = (CInt(c2.B) - CInt(c1.B)) / (Width - 1)

            For i = 0 To Width - 1
                Dim thisr = CInt(i * deltar + c1.R)
                Dim thisg = CInt(i * deltag + c1.G)
                Dim thisb = CInt(i * deltab + c1.B)
                Using b As New SolidBrush(Color.FromArgb(thisr, thisg, thisb))
                    Using p As New Pen(b)
                        g.DrawRectangle(p, r)
                        r.Inflate(-1, -1)
                    End Using
                End Using
            Next
        End Sub

        Public Function GetCompleteExceptionMessage(exception As Exception) As String
            Dim x As Exception = exception.InnerException
            Dim msg As System.Text.StringBuilder = New System.Text.StringBuilder(exception.Message)
            While x IsNot Nothing
                msg.AppendFormat(vbCr & vbLf & vbCr & vbLf & "{0}", x.Message)
                x = x.InnerException
            End While
            msg.Append(vbCr & vbLf & "----Stacktrace----" & vbCr & vbLf)
            msg.Append(exception.StackTrace)
            Return msg.ToString()
        End Function

        Public Sub DrawRotatedString(ByRef g As Graphics, text As String, font As Font, brush As Brush, x As Single, y As Single, angle As Single)
            Dim s As SizeF = g.MeasureString(text, font)
            g.ResetTransform()
            g.TranslateTransform(-s.Width / 2, -s.Height / 2)
            g.RotateTransform(angle, Drawing2D.MatrixOrder.Append)
            g.TranslateTransform(x, y, Drawing2D.MatrixOrder.Append)
            g.DrawString(text, font, brush, 0, 0)
            g.ResetTransform()
        End Sub

        Public Sub DrawRotatedString(ByRef g As Graphics, text As String, font As Font, brush As Brush, x As Single, y As Single, angle As Single, format As StringFormat)
            Dim s As SizeF = g.MeasureString(text, font)
            g.ResetTransform()
            g.TranslateTransform(-s.Width / 2, -s.Height / 2)
            g.RotateTransform(angle, Drawing2D.MatrixOrder.Append)
            g.TranslateTransform(x, y, Drawing2D.MatrixOrder.Append)
            g.DrawString(text, font, brush, 0, 0, format)
            g.ResetTransform()
        End Sub

        Public Function FormatShort(d As Double, ExpDigits As Integer) As String
            If Double.IsNaN(d) OrElse Double.IsInfinity(d) Then Return d.ToString
            Dim addis As String = ""
            If d < 0 Then addis = "-"
            d = Math.Abs(d)
            Dim alen As Integer = addis.Length

            If (Math.Abs(d) >= 10000 OrElse Math.Abs(d) <= 0.0001) AndAlso Math.Abs(d) <> 0 Then
                Return addis & Strings.Format(d, "E" & (ExpDigits - alen).ToString)
            ElseIf Math.Abs(d) = 0 Then
                Return Strings.Format(0, "0.0")
            Else
                Dim log10 As Double = Math.Log10(d)
                Dim zerosbehind As Integer = 0
                If log10 > 0 Then
                    zerosbehind = (5 + ExpDigits) - CInt(Int(log10)) - alen
                Else
                    zerosbehind = (5 + ExpDigits) - alen
                End If
                Dim thisformat As String = "0." & New String(CChar("0"), zerosbehind)
                Return addis & Strings.Format(d, thisformat)
            End If
        End Function

        Public Function FormatShortShort(d As Double, ExpDigits As Integer) As String
            If Double.IsNaN(d) OrElse Double.IsInfinity(d) Then Return d.ToString
            Dim addis As String = ""
            If d < 0 Then addis = "-"
            d = Math.Abs(d)
            Dim alen As Integer = addis.Length

            If (Math.Abs(d) >= 10000 OrElse Math.Abs(d) <= 0.0001) AndAlso Math.Abs(d) <> 0 Then
                Return addis & Strings.Format(d, "E" & (ExpDigits - alen).ToString)
            ElseIf Math.Abs(d) = 0 Then
                Return Strings.Format(0, "0.0")
            Else
                Dim log10 As Double = Math.Log10(d)
                Dim zerosbehind As Integer = 0
                If log10 > 0 Then
                    zerosbehind = (5 + ExpDigits) - CInt(Int(log10)) - alen
                Else
                    zerosbehind = (5 + ExpDigits) - alen
                End If
                Dim thisformat As String = "0." & New String(CChar("#"), zerosbehind)
                Return addis & Strings.Format(d, thisformat)
            End If
        End Function

        Public Function SetClipboardText(s As String, silent As Boolean) As Boolean
            Try
                Clipboard.SetText(s)
                Return True
            Catch ex As Exception
                If silent = False Then MessageBox.Show("Clipboard is not ready!" & vbCrLf & "Please try again.", "Clipboard error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

        Public Function TryGetNumericValue(s As String, ByRef result As Double) As Boolean
            Dim currentdecimal As String
            currentdecimal = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
            If currentdecimal = "," Then
                s = Strings.Replace(s, ".", ",")
            Else
                s = Strings.Replace(s, ",", ".")
            End If
            Return Double.TryParse(s, result)
        End Function

        Public Function GetNumericValue(s As String) As Double
            Dim currentdecimal As String
            currentdecimal = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
            If currentdecimal = "," Then
                s = Strings.Replace(s, ".", ",")
            Else
                s = Strings.Replace(s, ",", ".")
            End If
            Dim res As Double
            If Double.TryParse(s, res) Then Return res Else Return Double.NaN
        End Function

        Public Function StringToDouble(ByVal s As String) As Double
            Return GetNumericValue(s)
        End Function

        Public Function GetOpenFileName(filter As String) As String
            Dim ofd As New OpenFileDialog
            ofd.Filter = filter
            If ofd.ShowDialog = DialogResult.Cancel Then Return ""
            Return ofd.FileName
        End Function

        Public Function GetSaveFileName(filter As String) As String
            Dim sfd As New SaveFileDialog
            sfd.Filter = filter
            sfd.AddExtension = True
            sfd.DefaultExt = ".txt"
            If sfd.ShowDialog = DialogResult.Cancel Then Return ""
            Return sfd.FileName
        End Function

        <Extension>
        Public Function TryFormat(D As Double, nFormat As String) As String
            If nFormat Is Nothing Then Return D.ToString
            Dim value As String
            Try
                If LCase(nFormat) = "short" Then
                    value = FormatShort(D, 2)
                ElseIf LCase(nFormat) = "short2" Then
                    value = FormatShort(D, 4)
                ElseIf LCase(nFormat) = "short2slen" Then
                    If Double.IsNaN(D) Then
                        value = "    NaN    "
                    ElseIf Double.IsPositiveInfinity(D) Then
                        value = "   +Inf    "
                    ElseIf Double.IsNegativeInfinity(D) Then
                        value = "   -Inf    "
                    ElseIf D = 0 Then
                        value = "    0.0    "
                    Else
                        value = FormatShort(D, 4)
                    End If
                ElseIf LCase(nFormat) = "short3" Then
                    value = FormatShort(D, 3)
                ElseIf LCase(nFormat) = "shortshort" Then
                    value = FormatShortShort(D, 2)
                ElseIf LCase(nFormat) = "shortprec" Then
                    If D = 0 Then Return "0"
                    Dim max = If(D < 0, 10000, 100000)
                    Dim min = If(D < 0, 0.0001, 0.00001)
                    If Math.Abs(D) >= max OrElse Math.Abs(D) <= min Then
                        Return Format(D, "0.##E+0")
                    Else
                        Return Format(D, "0.######")
                    End If
                ElseIf LCase(nFormat) = "shortprec2" Then
                    If D = 0 Then Return Strings.Format(0, "0.0")
                    Dim max = If(D < 0, 10000, 100000)
                    Dim min = If(D < 0, 0.0001, 0.0001)
                    If Math.Abs(D) >= max OrElse Math.Abs(D) <= min Then
                        Return Format(D, "0.##E+0")
                    ElseIf D > 100 Then
                        Return Math.Round(D).ToString
                    Else
                        Return Format(RoundSignificant2(D, 3), "0.######")
                    End If
                ElseIf LCase(nFormat) = "shortprec3" Then
                    If D = 0 Then Return Strings.Format(0, "0.0")
                    Dim max = If(D < 0, 10000, 100000)
                    Dim min = If(D < 0, 0.0001, 0.0001)
                    If Math.Abs(D) >= max OrElse Math.Abs(D) <= min Then
                        Return Format(D, "0.###E+0")
                    ElseIf D > 10000 Then
                        Return Math.Round(D).ToString
                    Else
                        Return Format(RoundSignificant2(D, 5), "0.######")
                    End If

                ElseIf nFormat <> "" Then
                    value = Format(D, nFormat)
                Else
                    value = D.ToString
                End If
            Catch
                value = D.ToString
            End Try
            Return value
        End Function

        Public Sub GetMinMax(Values As IEnumerable(Of Double), ByRef Min As Double, ByRef Max As Double)
            GetMinMax(Values, Min, Max, 0, 0)
        End Sub
        Public Sub GetMinMax(Values As IEnumerable(Of Double), ByRef Min As Double, ByRef Max As Double,
                             ByRef iMin As Integer, ByRef iMax As Integer)
            Min = Double.PositiveInfinity
            Max = Double.NegativeInfinity
            For i = 0 To Values.Count - 1
                If Values(i) < Min Then
                    Min = Values(i)
                    iMin = i
                End If
                If Values(i) > Max Then
                    Max = Values(i)
                    iMax = i
                End If
            Next
        End Sub

        Public Function StdDev(values As IEnumerable(Of Double)) As Double
            Dim ret As Double = 0
            If values Is Nothing OrElse values.Count = 0 Then Return Double.NaN
            Dim count As Integer = values.Count()
            If count > 1 Then
                'Compute the Average
                Dim avg As Double = values.Average()

                'Perform the Sum of (value-avg)^2
                Dim sum As Double = values.Sum(Function(d) (d - avg) * (d - avg))

                'Put it all together
                ret = Math.Sqrt(sum / count)
            End If
            Return ret
        End Function
    End Module
End Namespace