Imports Microsoft.VisualBasic
Imports DXGridRowStateHelper.Models
Imports DXGridRowStateHelper.ViewModels
Imports System.Windows

Namespace DXGridRowStateHelper
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			Me.InitializeComponent()

			Me.DataContext = viewModel

			Me.helper = New RowStateHelper(Me.grdCustomers, "CustomerId")
		End Sub

		Private viewModel As New MainWindowVM()
		Private helper As RowStateHelper

		Private Sub Button_Load_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Me.helper.LoadViewInfo()
		End Sub

		Private Sub Button_Save_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			Me.helper.SaveViewInfo(Me.viewModel.Customers.Count)
		End Sub
	End Class
End Namespace