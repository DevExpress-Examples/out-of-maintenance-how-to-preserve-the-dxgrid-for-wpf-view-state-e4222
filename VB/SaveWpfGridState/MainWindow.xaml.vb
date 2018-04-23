Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes

Namespace SaveWpfGridState
    Partial Public Class MainWindow
        Inherits Window
        Implements INotifyPropertyChanged

        Public Sub New()
            InitializeComponent()
            rowStateHelper = New RowStateHelper(grdMaster, "id")
            DataContext = Me
        End Sub

        Public ReadOnly Property Data() As List(Of MasterDatum)
            Get
                Return _data
            End Get
        End Property
        Private _data As List(Of MasterDatum)

        Private rowStateHelper As RowStateHelper

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Private Sub RaisePropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Private Sub btnRefreshData_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            RefreshData()
        End Sub

        Private Sub RefreshData()
            _data = New List(Of MasterDatum)()
            For i As Integer = 1 To 10
                _data.Add(New MasterDatum() With { _
                    .id = i, _
                    .MasterDescription = String.Format("Master_Desc_{0}", i), _
                    .GroupField = String.Format("Group_{0}", (i Mod 2) + 1) _
                })
            Next i
            rowStateHelper.SaveViewInfo()
            RaisePropertyChanged("Data")
            rowStateHelper.LoadViewInfo()
        End Sub
    End Class

    Public Class MasterDatum
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
                ChildData = New List(Of ChildDatum)()
                For i As Integer = 0 To 11
                    ChildData.Add(New ChildDatum() With { _
                        .id = (_id * 1000) + i, _
                        .ChildDescription = String.Format("Child_Desc_{0}", (_id * 1000) + i) _
                    })
                Next i
            End Set
        End Property
        Private _id As Integer
        Public Property MasterDescription() As String
        Public Property GroupField() As String
        Public Property ChildData() As List(Of ChildDatum)
    End Class

    Public Class ChildDatum
        Public Property id() As Integer
        Public Property ChildDescription() As String
    End Class
End Namespace
