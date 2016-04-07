Imports System.ComponentModel
Imports System.Reflection
Imports System.Globalization
Imports System.ComponentModel.Design.Serialization

Namespace jwGraph
    Public Class VerticalMarkerTypeConverter
        Inherits TypeConverter

        Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
            Return destinationType = GetType(InstanceDescriptor) OrElse MyBase.CanConvertTo(context, destinationType)
        End Function

        Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As CultureInfo, value As Object, destinationType As Type) As Object
            If destinationType = GetType(InstanceDescriptor) AndAlso TypeOf value Is VerticalMarker Then

                Dim constructor As System.Reflection.ConstructorInfo = GetType(VerticalMarker).GetConstructor(New Type() {GetType(Double)})

                Dim marker = TryCast(value, VerticalMarker)
                Dim descriptor = New InstanceDescriptor(constructor, New Object() {marker.X}, True)

                Return descriptor
            End If
            Return MyBase.ConvertTo(context, culture, value, destinationType)
        End Function

    End Class
End Namespace