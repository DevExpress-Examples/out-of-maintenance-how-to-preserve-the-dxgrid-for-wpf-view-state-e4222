Imports Microsoft.VisualBasic
Imports DXGridRowStateHelper.Models
Imports System
Imports System.Collections.ObjectModel
Imports System.ComponentModel

Namespace DXGridRowStateHelper.ViewModels
	Public Class MainWindowVM
		Implements INotifyPropertyChanged
		#Region "Constructors"

		Public Sub New()
			Dim random As New Random()
			Dim random2 As New Random()

			For i As Integer = 0 To 99
				Me.Customers.Add(New Customer() With {.CustomerId = i, .RandomId = random.Next(1, 10), .RandomId2 = random.Next(1, 3), .FirstName = "testFirstName", .LastName = "testLastName"})
			Next i
		End Sub

		#End Region

		#Region "Properties"

		#Region "Customers Property"

		Private m_Customers As New ObservableCollection(Of Customer)()
		Public Property Customers() As ObservableCollection(Of Customer)
			Get
				Return Me.m_Customers
			End Get
			Set(ByVal value As ObservableCollection(Of Customer))
				Me.m_Customers = value

				Me.OnPropertyChanged("Customers")
			End Set
		End Property

		#End Region

		#Region "CurrentCustomer Property"

		Private m_CurrentCustomer As Customer = Nothing
		Public Property CurrentCustomer() As Customer
			Get
				Return Me.m_CurrentCustomer
			End Get
			Set(ByVal value As Customer)
				If Me.m_CurrentCustomer Is value Then
					Return
				End If

				Me.m_CurrentCustomer = value

				Me.OnPropertyChanged("CurrentCustomer")
			End Set
		End Property

		#End Region

		#End Region

		#Region "INotifyPropertyChanged Implementation"

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Private Sub OnPropertyChanged(ByVal propertyName As String)
			Dim handler = Me.PropertyChangedEvent
			If handler IsNot Nothing Then
				handler(Me, New PropertyChangedEventArgs(propertyName))
			End If
		End Sub

		#End Region
	End Class
End Namespace