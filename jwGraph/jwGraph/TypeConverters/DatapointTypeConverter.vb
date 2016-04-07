Imports System.ComponentModel
Imports System.Reflection
Imports System.Globalization
Imports System.ComponentModel.Design.Serialization

Namespace jwGraph
    Public Class DatapointTypeConverter
        Inherits TypeConverter

        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            Return destinationType = GetType(InstanceDescriptor) OrElse MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType = GetType(InstanceDescriptor) AndAlso TypeOf value Is Datapoint Then

                Dim constructor As System.Reflection.ConstructorInfo = GetType(Datapoint).GetConstructor(New Type() {GetType(Double), GetType(Double),
                                                                                                         GetType(Double), GetType(Double),
                                                                                                         GetType(Boolean), GetType(Object)})

                Dim point = TryCast(value, Datapoint)
                Dim descriptor = New InstanceDescriptor(constructor, New Object() {point.X, point.XError, point.Y, point.YError,
                 point.IsMarked, point.Tag}, True)


                Return descriptor
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

    End Class
End Namespace