Public Class ObjectMouseHandle
    Public Enum HandleType
        Move = 1
        ResizeVertical = 2
        ResizeHorizontal = 3
        ResizeDiagonalTRBL = 4
        ResizeDiagonalTLBR = 5
        SwitchDirection = 6
        SwitchDirectionAndMove = 7
    End Enum
    Public Property X As Double
    Public Property Y As Double
    Public Property Type As HandleType
    Public Property IsSelected As Boolean

    Public Sub Paint(ByRef g As Graphics, Size As Integer)
        Dim b As Brush
        If Me.IsSelected Then b = Brushes.Red Else b = Brushes.Blue
        Using p As New Pen(b)
            p.EndCap = Drawing2D.LineCap.ArrowAnchor
            p.StartCap = Drawing2D.LineCap.ArrowAnchor
            p.Width = 3
            Select Case Me.Type
                Case HandleType.Move
                    g.DrawLine(p, CSng(Me.X - Size), CSng(Me.Y), CSng(Me.X + Size), CSng(Me.Y))
                    g.DrawLine(p, CSng(Me.X), CSng(Me.Y - Size), CSng(Me.X), CSng(Me.Y + Size))
                Case HandleType.ResizeVertical
                    g.DrawLine(p, CSng(Me.X), CSng(Me.Y - Size), CSng(Me.X), CSng(Me.Y + Size))
                Case HandleType.ResizeHorizontal
                    g.DrawLine(p, CSng(Me.X - Size), CSng(Me.Y), CSng(Me.X + Size), CSng(Me.Y))
                Case HandleType.ResizeDiagonalTLBR
                    g.DrawLine(p, CSng(Me.X - Size), CSng(Me.Y - Size), CSng(Me.X + Size), CSng(Me.Y + Size))
                Case HandleType.ResizeDiagonalTLBR
                    g.DrawLine(p, CSng(Me.X + Size), CSng(Me.Y - Size), CSng(Me.X - Size), CSng(Me.Y + Size))
                Case HandleType.SwitchDirection
                    p.StartCap = Drawing2D.LineCap.Round
                    g.DrawArc(p, New RectangleF(CSng(Me.X - Size), CSng(Me.Y - Size), 2 * Size, 2 * Size), 0, 320)
                Case HandleType.SwitchDirectionAndMove
                    g.DrawLine(p, CSng(Me.X - Size), CSng(Me.Y), CSng(Me.X + Size), CSng(Me.Y))
                    g.DrawLine(p, CSng(Me.X), CSng(Me.Y - Size), CSng(Me.X), CSng(Me.Y + Size))
                    p.StartCap = Drawing2D.LineCap.Round
                    g.DrawArc(p, New RectangleF(CSng(Me.X - Size), CSng(Me.Y - Size), 2 * Size, 2 * Size), 0, 320)
            End Select
        End Using
    End Sub
End Class
