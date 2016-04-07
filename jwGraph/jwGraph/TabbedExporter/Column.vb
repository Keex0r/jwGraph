Namespace GeneralTools
    Namespace TabbedExporter
        Public Class Column
            Public Tag As Object
            Public Property Items As List(Of String)
            Public Property Header As List(Of String)
            Public Sub New()
                Items = New List(Of String)
                Header = New List(Of String)
            End Sub
            Public Sub New(Header As String)
                Me.New()
                Me.Header.Add(Header)
            End Sub

            Public ReadOnly Property Length As Integer
                Get
                    If Items IsNot Nothing Then
                        Return Items.Count
                    Else
                        Return -1
                    End If
                End Get
            End Property
            Default Public Property Item(index As Integer) As String
                Get
                    Return Items(index)
                End Get
                Set(value As String)
                    Items(index) = value
                End Set
            End Property
        End Class
    End Namespace
End Namespace
