Imports Microsoft.VisualBasic
Imports System.ComponentModel

Namespace DXGridRowStateHelper.Models
	Public Class Customer
		Implements INotifyPropertyChanged
		#Region "CustomerId Property"

		Private m_CustomerId As Integer = 0
		Public Property CustomerId() As Integer
			Get
				Return Me.m_CustomerId
			End Get
			Set(ByVal value As Integer)
				Me.m_CustomerId = value

				Me.OnPropertyChanged("CustomerId")
			End Set
		End Property

		#End Region

		#Region "RandomId Property"

		Private m_RandomId As Integer = 0
		Public Property RandomId() As Integer
			Get
				Return Me.m_RandomId
			End Get
			Set(ByVal value As Integer)
				Me.m_RandomId = value

				Me.OnPropertyChanged("RandomId")
			End Set
		End Property

		#End Region

		#Region "RandomId2 Property"

		Private m_RandomId2 As Integer = 0
		Public Property RandomId2() As Integer
			Get
				Return Me.m_RandomId2
			End Get
			Set(ByVal value As Integer)
				Me.m_RandomId2 = value

				Me.OnPropertyChanged("RandomId2")
			End Set
		End Property

		#End Region

		#Region "FirstName Property"

		Private m_FirstName As String = String.Empty
		Public Property FirstName() As String
			Get
				Return Me.m_FirstName
			End Get
			Set(ByVal value As String)
				Me.m_FirstName = value

				Me.OnPropertyChanged("FirstName")
			End Set
		End Property

		#End Region

		#Region "LastName Property"

		Private m_LastName As String = String.Empty
		Public Property LastName() As String
			Get
				Return Me.m_LastName
			End Get
			Set(ByVal value As String)
				Me.m_LastName = value

				Me.OnPropertyChanged("LastName")
			End Set
		End Property

		#End Region

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Private Sub OnPropertyChanged(ByVal propertyName As String)
			Dim handler = Me.PropertyChangedEvent
			If handler IsNot Nothing Then
				handler(Me, New PropertyChangedEventArgs(propertyName))
			End If
		End Sub
	End Class
End Namespace