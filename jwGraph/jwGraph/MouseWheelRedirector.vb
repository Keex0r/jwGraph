'
' Credit to: http://www.codeproject.com/Articles/280310/Redirect-Mouse-Wheel-Events-To-Unfocused-Windows-F
' Protected by the Code Project Open License: http://www.codeproject.com/info/cpol10.aspx
Namespace GeneralTools
    Public Class MouseWheelRedirector
        Implements IMessageFilter

        Private Shared instance As MouseWheelRedirector = Nothing
        Private Shared _active As Boolean = False

        Public Shared Property Active As Boolean
            Set(ByVal value As Boolean)
                If _active <> value Then
                    _active = value
                    If _active Then
                        If instance Is Nothing Then
                            instance = New MouseWheelRedirector
                        End If
                        Application.AddMessageFilter(instance)
                    Else
                        If instance IsNot Nothing Then
                            Application.RemoveMessageFilter(instance)
                        End If
                    End If
                End If
            End Set
            Get
                Return _active
            End Get
        End Property

        Public Shared Sub Attach(ByVal control As Control)
            If Not _active Then Active = True
            AddHandler control.MouseEnter, AddressOf instance.ControlMouseEnter
            AddHandler control.MouseLeave, AddressOf instance.ControlMouseLeaveOrDisposed
            AddHandler control.Disposed, AddressOf instance.ControlMouseLeaveOrDisposed
        End Sub

        Public Shared Sub Detach(ByVal control As Control)
            If instance Is Nothing Then Return
            RemoveHandler control.MouseEnter, AddressOf instance.ControlMouseEnter
            RemoveHandler control.MouseLeave, AddressOf instance.ControlMouseLeaveOrDisposed
            RemoveHandler control.Disposed, AddressOf instance.ControlMouseLeaveOrDisposed
            If instance.currentControl Is control Then instance.currentControl = Nothing
        End Sub

        Private Sub New()
        End Sub

        Private currentControl As Control

        Private Sub ControlMouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim control = DirectCast(sender, Control)
            If Not control.Focused Then
                currentControl = control
            Else
                currentControl = Nothing
            End If
        End Sub

        Private Sub ControlMouseLeaveOrDisposed(ByVal sender As Object, ByVal e As System.EventArgs)
            If currentControl Is sender Then
                currentControl = Nothing
            End If
        End Sub

        Private Const WM_MOUSEWHEEL As Integer = &H20A
        Public Function PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage
            If currentControl IsNot Nothing AndAlso m.Msg = WM_MOUSEWHEEL Then
                SendMessage(currentControl.Handle, m.Msg, m.WParam, m.LParam)
                Return True
            Else
                Return False
            End If
        End Function

        <DllImport("user32.dll", SetLastError:=False)> _
        Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function

    End Class
End Namespace