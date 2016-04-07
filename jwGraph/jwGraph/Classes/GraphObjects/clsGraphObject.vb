Namespace jwGraph
    Public MustInherit Class clsGraphObject
        Implements System.ComponentModel.INotifyPropertyChanged
#Region "Enums"
        Public Enum enClipArea
            Above0 = 0
            Under0 = 1
            Both = 2
        End Enum
#End Region
#Region "Methods"
        Public Shared Function GetClipArea(cliparea As enClipArea, yaxis As VerticalAxis, bounds As Rectangle) As Rectangle
            Dim zeropos As Integer = CInt(yaxis.ValueToPixelPosition(0)) '- Bounds.Top
            Dim rect As Rectangle
            Select Case cliparea
                Case enClipArea.Under0
                    rect = New Rectangle(0, zeropos, bounds.Width, bounds.Height - zeropos)
                Case enClipArea.Above0
                    rect = New Rectangle(0, 0, bounds.Width, zeropos)
                Case enClipArea.Both
                    rect = New Rectangle(0, 0, bounds.Width, bounds.Height)
            End Select
            Return rect
        End Function
        Public Shared Function GetRectFromPoint(p As Point, Size As Integer) As Rectangle
            Return New Rectangle(p.X - Size, p.Y - Size, 2 * Size, 2 * Size)
        End Function

#End Region

        
        Private _ClipArea As enClipArea
        Public Property ClipArea As enClipArea
            Get
                Return _ClipArea
            End Get
            Set(value As enClipArea)
                _ClipArea = value
                Notify("ClipArea")
            End Set
        End Property

        Private _Name As String
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
                Notify("Name")
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
        Public Enum enMouseAction
            Down = 0
            Move = 1
            Up = 2
        End Enum
        Public Property SelectedIndizes As List(Of Integer)
        Public MustOverride Sub Paint(ByRef g As Graphics, XAxis As HorizontalAxis, YAxis As VerticalAxis, Bounds As Rectangle)
        Public MustOverride Function GetInfos() As Object()
        Public MustOverride Function GetMouseHandles(XAxis As HorizontalAxis, YAxis As VerticalAxis) As ObjectMouseHandle()
        Public MustOverride Sub UpDateByMouse(index As Integer, deltaX As Integer, deltaY As Integer, XAxis As HorizontalAxis, yAxis As VerticalAxis, MouseAction As enMouseAction)
        Public MustOverride Sub UpdateOnCreate(Startpoint As Point, CurrentPoint As Point, XAxis As HorizontalAxis, YAxis As VerticalAxis)

        Public Sub New()
            SelectedIndizes = New List(Of Integer)
        End Sub
        Public Sub Notify(name As String)
            RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(name))
        End Sub
        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class
End Namespace
