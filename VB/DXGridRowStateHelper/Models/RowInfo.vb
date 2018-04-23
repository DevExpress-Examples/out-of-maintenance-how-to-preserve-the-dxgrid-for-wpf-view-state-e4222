Imports Microsoft.VisualBasic
Imports System
Namespace DXGridRowStateHelper.Models
	Public Class RowInfo
		Private privateId As Object
		Public Property Id() As Object
			Get
				Return privateId
			End Get
			Set(ByVal value As Object)
				privateId = value
			End Set
		End Property
		Private privateLevel As Integer
		Public Property Level() As Integer
			Get
				Return privateLevel
			End Get
			Set(ByVal value As Integer)
				privateLevel = value
			End Set
		End Property
	End Class
End Namespace