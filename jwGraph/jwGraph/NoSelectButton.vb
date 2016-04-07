Namespace GeneralTools
    Public Class NoSelectButton
        Inherits Button
        Public Sub New()
            SetStyle(ControlStyles.Selectable, False)
            SetStyle(ControlStyles.UserPaint, True)
        End Sub
    End Class
End Namespace