Namespace jwGraph

    <System.ComponentModel.TypeConverter(GetType(DatapointTypeConverter))>
    Public Class Datapoint
        Private _IsOKVal As Boolean
        Public ReadOnly Property IsOKVal As Boolean
            Get
                Return _IsOKVal
            End Get
        End Property
        Private _IsOKXErr As Boolean
        Public ReadOnly Property IsOKXErr As Boolean
            Get
                Return _IsOKXErr
            End Get
        End Property
        Private _IsOKYErr As Boolean
        Public ReadOnly Property IsOKYErr As Boolean
            Get
                Return _IsOKYErr
            End Get
        End Property
        Private Sub UpdateOK()
            Me._IsOKVal = jwGraph.IsOK(Me.X) AndAlso jwGraph.IsOK(Me.Y)
            Me._IsOKXErr = jwGraph.IsOK(Me.XError) AndAlso jwGraph.IsOK(Me.X)
            Me._IsOKYErr = jwGraph.IsOK(Me.YError) AndAlso jwGraph.IsOK(Me.Y)
        End Sub
        Public Sub SetMarked(Value As Boolean)
            Me.IsMarked = Value
        End Sub
        Private _X As Double
        Public Property X As Double
            Get
                Return _X
            End Get
            Set(value As Double)
                _X = value
                UpdateOK()
            End Set
        End Property
        Private _Y As Double
        Public Property Y As Double
            Get
                Return _Y
            End Get
            Set(value As Double)
                _Y = value
                UpdateOK()
            End Set
        End Property
        Private _XError As Double
        Public Property XError As Double
            Get
                Return _XError
            End Get
            Set(value As Double)
                _XError = value
                UpdateOK()
            End Set
        End Property
        Private _YError As Double
        Public Property YError As Double
            Get
                Return _YError
            End Get
            Set(value As Double)
                _YError = value
                UpdateOK()
            End Set
        End Property

        Public Property IsMarked As Boolean

        Public Sub New()
            Me.New(0, 0, 0, 0)
        End Sub

        Public Sub New(X As Double, Y As Double)
            Me.New(X, 0, Y, 0)
        End Sub
        Public Sub New(X As Double, XError As Double, Y As Double, YError As Double)
            Me.X = X
            Me.Y = Y
            Me.XError = XError
            Me.YError = YError
            IsMarked = False
            UpdateOK()
        End Sub
        Public Sub New(X As Double, XError As Double, Y As Double, YError As Double, IsMarked As Boolean, Tag As Object)
            Me.X = X
            Me.Y = Y
            Me.XError = XError
            Me.YError = YError
            IsMarked = IsMarked
            Me.Tag = Tag
            UpdateOK()
        End Sub
        Public Property Tag As Object


    End Class
End Namespace
