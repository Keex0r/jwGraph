Namespace GeneralTools
    Public Class NumericInputbox
        Inherits TextBox
        Public Property Value As Double
            Get
                Dim res As Double = Double.NaN
                If Double.TryParse(Me.Text, res) Then Return Math.Min(Math.Max(res, Minimum), Maximum) Else Return Double.NaN
            End Get
            Set(newvalue As Double)
                If newvalue < Minimum Then newvalue = Minimum
                If newvalue > Maximum Then newvalue = Maximum
                Try
                    Me.Text = Format(newvalue, FormatString)
                Catch
                    Me.Text = newvalue.ToString
                End Try
            End Set
        End Property
        Private _FormatString As String
        Public Property FormatString As String
            Get
                Return _FormatString
            End Get
            Set(value As String)
                _FormatString = value
                If Me.IsValid Then
                    Me.Text = Strings.Format(Me.Value, FormatString)
                End If
            End Set
        End Property

        Public ReadOnly Property IsValid As Boolean
            Get
                Dim test As Double = 0.0
                Return Double.TryParse(Me.Text, test) AndAlso test <= Maximum AndAlso test >= Minimum
            End Get
        End Property
        Public Property ValidForeColor As Color
        Public Property InvalidForeColor As Color
        Public Property ChangeForecolor As Boolean

        Public Property ValidBackColor As Color
        Public Property InvalidBackColor As Color
        Public Property ChangeBackcolor As Boolean

        Private _min As Double
        Public Property Minimum As Double
            Get
                Return _min
            End Get
            Set(newvalue As Double)
                If Value < newvalue Then Value = newvalue
                _min = newvalue
            End Set
        End Property
        Private _max As Double
        Public Property Maximum As Double
            Get
                Return _max
            End Get
            Set(newvalue As Double)
                If Value > newvalue Then Value = newvalue
                _max = newvalue
            End Set
        End Property




        Public Overrides Property Text As String
            Get
                Return MyBase.Text
            End Get
            Set(value As String)
                If Double.TryParse(value, 0.0) Then MyBase.Text = value : lastvalid = value
            End Set
        End Property



        Public Sub New()
            FormatString = "G"
            Me.Value = 0
            Me.ChangeBackcolor = False
            Me.ChangeForecolor = True
            Me.ValidBackColor = Color.White
            Me.ValidForeColor = Color.Green
            Me.InvalidForeColor = Color.Red
            Me.InvalidBackColor = Color.PaleVioletRed
            Me.ForeColor = Me.ValidForeColor
            Me.Minimum = -10000000
            Me.Maximum = 10000000
            lastvalid = "0"
        End Sub

        Private Sub NumericInputbox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
            If e.KeyChar = Chr(27) Then
                Me.Text = lastvalid
                Me.Enabled = False
                Me.Enabled = True
                e.Handled = True
            End If
            If e.KeyChar = Chr(13) Then
                Me.Enabled = False
                Me.Enabled = True
                e.Handled = True
            End If
            If (Not Char.IsDigit(e.KeyChar)) AndAlso {System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, "e", "-", "E"}.Contains(e.KeyChar) = False AndAlso Not Char.IsControl(e.KeyChar) Then
                e.Handled = True
            End If
        End Sub

        Private Sub NumericInputbox_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
            If Me.IsValid Then
                Me.Text = Strings.Format(Me.Value, FormatString)
            End If
        End Sub

        Private Sub NumericInputbox_TextChanged(sender As Object, e As EventArgs) Handles Me.TextChanged
            If Not Double.TryParse(Me.Text, 0) Then
                If ChangeForecolor Then Me.ForeColor = Me.InvalidForeColor
                If ChangeBackcolor Then Me.BackColor = Me.InvalidBackColor
            Else
                If ChangeForecolor Then Me.ForeColor = Me.ValidForeColor
                If ChangeBackcolor Then Me.BackColor = Me.ValidBackColor
            End If
        End Sub
        Private lastvalid As String
        Private Sub NumericInputbox_Validated(sender As Object, e As EventArgs) Handles Me.Validated
            If IsValid = False Then Me.Text = lastvalid : Exit Sub
            Dim curr As Double = CDbl(Me.Text)
            If curr < Minimum Then Me.Text = Minimum.ToString
            If curr > Maximum Then Me.Text = Maximum.ToString
            lastvalid = Me.Text
        End Sub
    End Class
End Namespace