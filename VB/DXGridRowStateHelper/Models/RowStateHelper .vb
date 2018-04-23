Imports Microsoft.VisualBasic
Imports DevExpress.Xpf.Grid
Imports System
Imports System.Collections
Imports System.Windows.Documents

Namespace DXGridRowStateHelper.Models
	Public Class RowStateHelper
		Private gridControl As GridControl
		Private keyFieldName As String

		Private topRowIndex As Integer

		Private expandedList_Renamed As ArrayList
		Private selectionList_Renamed As ArrayList
		Private masterRowsList_Renamed As ArrayList
		Private groupRowList_Renamed As ArrayList



		Public ReadOnly Property ExpandedList() As ArrayList
			Get
				If expandedList_Renamed Is Nothing Then
					expandedList_Renamed = New ArrayList()
				End If
				Return expandedList_Renamed
			End Get
		End Property

		Public ReadOnly Property SelectionList() As ArrayList
			Get
				If selectionList_Renamed Is Nothing Then
					selectionList_Renamed = New ArrayList()
				End If
				Return selectionList_Renamed
			End Get
		End Property

		Public ReadOnly Property MasterRowsList() As ArrayList
			Get
				If masterRowsList_Renamed Is Nothing Then
					masterRowsList_Renamed = New ArrayList()
				End If
				Return masterRowsList_Renamed
			End Get
		End Property

		Public ReadOnly Property GroupRowList() As ArrayList
			Get
				If groupRowList_Renamed Is Nothing Then
					groupRowList_Renamed = New ArrayList()
				End If
				Return groupRowList_Renamed
			End Get
		End Property


		Public Sub New(ByVal vw As GridControl, ByVal keyField As String)
			gridControl = vw
			keyFieldName = keyField
		End Sub

		#Region "Saving Information"
		Public Sub SaveViewInfo(ByVal dataSourceCount As Integer)
			SaveExpandedMasterRows(MasterRowsList, dataSourceCount)
			SaveExpansionView(ExpandedList)
			SaveSelectionView(SelectionList)
			SaveVisibleIndex()
		End Sub

		Public Sub LoadViewInfo()
			LoadExpandedMasterRows(MasterRowsList)
			LoadExpansionView(ExpandedList)
			LoadSelectionView(SelectionList)
			LoadVisibleIndex()
		End Sub

		Private Sub SaveExpandedMasterRows(ByVal expandedList As ArrayList, ByVal dataSourceCount As Integer)
			expandedList.Clear()

			For i As Integer = 0 To dataSourceCount - 1
				Dim rowHandle As Integer = gridControl.GetRowHandleByListIndex(i)
				If gridControl.IsMasterRowExpanded(rowHandle) Then
					expandedList.Add(rowHandle)
				End If
			Next i
		End Sub



		Private Sub SaveExpansionView(ByVal expandedGroupsList As ArrayList)
			If gridControl.GroupCount = 0 Then
				Return
			End If
			GroupRowList.Clear()
			For i As Integer = -1 To Integer.MinValue + 1 Step -1
				If (Not gridControl.IsValidRowHandle(i)) Then
					Exit For
				End If
				If gridControl.IsGroupRowExpanded(i) Then
					GroupRowList.Add(i)
				End If
			Next i
		End Sub

		Private Sub SaveSelectionView(ByVal selectionList As ArrayList)
			selectionList.Clear()

			Dim selectedRows() As Integer = (CType(gridControl.View, TableView)).GetSelectedRowHandles()
			For i As Integer = 0 To selectedRows.Length - 1
				selectionList.Add(selectedRows(i))
			Next i
			selectionList.Add(gridControl.View.FocusedRowHandle)
		End Sub

		Public Sub SaveVisibleIndex()
			topRowIndex = gridControl.View.TopRowIndex
		End Sub

		#End Region
		#Region "Loading Information"
		Private Sub LoadExpandedMasterRows(ByVal expandedList As ArrayList)

			gridControl.CollapseAllGroups()
			For i As Integer = 0 To expandedList.Count - 1
				gridControl.ExpandMasterRow(CInt(Fix(expandedList(i))))
			Next i

		End Sub

		Private Sub LoadExpansionView(ByVal expandedGroupsList As ArrayList)
			If gridControl.GroupCount = 0 Then
				Return
			End If

			gridControl.CollapseAllGroups()

			For Each grouIndex As Integer In GroupRowList
				gridControl.ExpandGroupRow(grouIndex)
			Next grouIndex
		End Sub

		Private Sub LoadSelectionView(ByVal selectionList As ArrayList)
			CType(gridControl.View, TableView).BeginSelection()
			Try
				CType(gridControl.View, TableView).ClearSelection()
				For i As Integer = 0 To selectionList.Count - 1
					If i = selectionList.Count - 1 Then
						CType(gridControl.View, TableView).FocusedRowHandle = Convert.ToInt32(selectionList(i))
					Else
						CType(gridControl.View, TableView).SelectRow(Convert.ToInt32(selectionList(i)))
					End If
				Next i
			Finally
				CType(gridControl.View, TableView).EndSelection()
			End Try
		End Sub

		Public Sub LoadVisibleIndex()
			gridControl.View.TopRowIndex = topRowIndex
		End Sub
		#End Region
	End Class
End Namespace