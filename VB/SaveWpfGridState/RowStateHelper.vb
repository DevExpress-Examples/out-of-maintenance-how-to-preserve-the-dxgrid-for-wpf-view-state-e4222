' Developer Express Code Central Example:
' How to preserve the DXGrid for WPF view state
' 
' This example demonstrates how to save/load the groups and master rows expansion
' state, selection, focused row and position of the focused row.
' All
' functionality implemented in custom class named RowStateHelper, that contains
' methods for performing these operations.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E4222

Imports DevExpress.Xpf.Grid
Imports System.Collections
Imports System.Collections.Generic

Namespace SaveWpfGridState
    Public Class RowStateHelper
        Private gridControl As GridControl
        Private keyFieldName As String

        Private focusedMasterKey As Object
        Private focusedDetailKey As Object
        Private focusedGroupValues As List(Of Object)


        Private selectionList_Renamed As ArrayList

        Private masterRowList_Renamed As ArrayList

        Private groupRowList_Renamed As List(Of GroupInfo)

        Public ReadOnly Property SelectionList() As ArrayList
            Get
                If selectionList_Renamed Is Nothing Then
                    selectionList_Renamed = New ArrayList()
                End If
                Return selectionList_Renamed
            End Get
        End Property
        Public ReadOnly Property MasterRowList() As ArrayList
            Get
                If masterRowList_Renamed Is Nothing Then
                    masterRowList_Renamed = New ArrayList()
                End If
                Return masterRowList_Renamed
            End Get
        End Property
        Public ReadOnly Property GroupRowList() As List(Of GroupInfo)
            Get
                If groupRowList_Renamed Is Nothing Then
                    groupRowList_Renamed = New List(Of GroupInfo)()
                End If
                Return groupRowList_Renamed
            End Get
        End Property

        Public Sub New(ByVal vw As GridControl, ByVal keyField As String)
            gridControl = vw
            keyFieldName = keyField
        End Sub

        Public Sub SaveViewInfo()
            SaveExpandedMasterRows()
            SaveExpansionView()
            'SaveSelectionView();
            SaveSelection()
        End Sub
        Public Sub LoadViewInfo()
            LoadExpandedMasterRows()
            LoadExpansionView()
            'LoadSelectionView();
            LoadSelection()
            ClearVariables()
        End Sub
        Private Sub ClearVariables()
            focusedMasterKey = Nothing
            focusedDetailKey = Nothing
            focusedGroupValues = Nothing
            SelectionList.Clear()
            MasterRowList.Clear()
            GroupRowList.Clear()
        End Sub

        #Region "master rows"
        Private Sub SaveExpandedMasterRows()
            MasterRowList.Clear()
            For i As Integer = 0 To gridControl.DataController.ListSourceRowCount - 1
                Dim rowHandle = gridControl.GetRowHandleByListIndex(i)
                If gridControl.IsMasterRowExpanded(rowHandle) Then
                    MasterRowList.Add(GetKeyByRowHandle(rowHandle))
                End If
            Next i
        End Sub
        Private Sub LoadExpandedMasterRows()
            For i As Integer = 0 To gridControl.DataController.ListSourceRowCount - 1
                Dim rowHandle = gridControl.GetRowHandleByListIndex(i)
                gridControl.CollapseMasterRow(rowHandle)
            Next i
            For Each key In MasterRowList
                Dim test = GetRowHandleByKey(key)
                Dim datarow = gridControl.DataController.GetRow(0)
                gridControl.ExpandMasterRow(GetRowHandleByKey(key))
            Next key
        End Sub
        #End Region

        #Region "group rows"
        Private Sub SaveExpansionView()
            If gridControl.GroupCount = 0 Then
                Return
            End If
            GroupRowList.Clear()
            For i As Integer = -1 To Integer.MinValue + 1 Step -1
                If Not gridControl.IsValidRowHandle(i) Then
                    Exit For
                End If
                If gridControl.GetRowLevelByRowHandle(i) = 0 Then
                    GroupRowList.Add(GetGroupInfo(i))
                End If
            Next i
        End Sub
        Private Function GetGroupInfo(ByVal rowHandle As Integer) As GroupInfo
            Dim info = New GroupInfo With { _
                .Value = gridControl.GetGroupRowValue(rowHandle), _
                .IsExpanded = gridControl.IsGroupRowExpanded(rowHandle) _
            }
            Dim i = 0
            Do While i < gridControl.GetChildRowCount(rowHandle)
                Dim childRowHandle = gridControl.GetChildRowHandle(rowHandle, i)
                If gridControl.IsGroupRowHandle(childRowHandle) Then
                    info.Children.Add(GetGroupInfo(childRowHandle))
                End If
                i += 1
            Loop
            Return info
        End Function

        Private Sub LoadExpansionView()
            If gridControl.GroupCount = 0 Then
                Return
            End If
            gridControl.CollapseAllGroups()
            For i As Integer = -1 To Integer.MinValue + 1 Step -1
                If Not gridControl.IsValidRowHandle(i) Then
                    Exit For
                End If
                If gridControl.GetRowLevelByRowHandle(i) = 0 Then
                    ExpandGroupRow(i, GroupRowList)
                End If
            Next i
        End Sub
        Private Sub ExpandGroupRow(ByVal rowHandle As Integer, ByVal infoList As List(Of GroupInfo))
            Dim value = gridControl.GetGroupRowValue(rowHandle)
            Dim info = infoList.Find(Function(x) Equals(x.Value, value))
            If info Is Nothing Then
                Return
            End If
            If info.IsExpanded Then
                gridControl.ExpandGroupRow(rowHandle)
            End If
            Dim i As Integer = 0
            Do While i < gridControl.GetChildRowCount(rowHandle)
                Dim childRowHandle = gridControl.GetChildRowHandle(rowHandle, i)
                If gridControl.IsGroupRowHandle(childRowHandle) Then
                    ExpandGroupRow(childRowHandle, info.Children)
                End If
                i += 1
            Loop
            infoList.Remove(info)
        End Sub
        #End Region

        #Region "focused row"
        Private Sub SaveSelection()
            focusedDetailKey = Nothing
            focusedMasterKey = Nothing
            focusedGroupValues = Nothing

            Dim focusedRowHandle As Integer
            If gridControl.View.IsFocusedView Then
                focusedDetailKey = Nothing
                focusedRowHandle = gridControl.View.FocusedRowHandle
                If gridControl.IsGroupRowHandle(focusedRowHandle) Then
                    focusedGroupValues = GetGroupValues(focusedRowHandle)
                    Return
                End If
            Else
                Dim focusedView = gridControl.View.FocusedView
                Dim detailRowHandle = focusedView.FocusedRowHandle
                focusedDetailKey = GetKeyByRowHandle(detailRowHandle, CType(focusedView.DataControl, GridControl))
                focusedRowHandle = CType(focusedView.DataControl, GridControl).GetMasterRowHandle()
            End If
            focusedMasterKey = GetKeyByRowHandle(focusedRowHandle)
        End Sub

        Private Sub LoadSelection()
            Dim masterRowHandle = If(focusedGroupValues Is Nothing, GetRowHandleByKey(focusedMasterKey), GetGroupRowHandleByValue())
            If focusedDetailKey Is Nothing Then
                gridControl.View.FocusedRowHandle = masterRowHandle
                Return
            End If
            If Not gridControl.IsMasterRowExpanded(masterRowHandle) Then
                gridControl.ExpandMasterRow(masterRowHandle)
            End If
            Dim detailControl = TryCast(gridControl.GetDetail(masterRowHandle), GridControl)
            If detailControl Is Nothing Then
                Return
            End If
            detailControl.View.MoveFocusedRow(GetRowHandleByKey(focusedDetailKey, detailControl))
        End Sub

        Private Function GetGroupValues(ByVal rowHandle As Integer) As List(Of Object)
            Dim list = New List(Of Object)()
            Do While rowHandle <> DataControlBase.InvalidRowHandle
                list.Insert(0, gridControl.GetGroupRowValue(rowHandle))
                rowHandle = gridControl.GetParentRowHandle(rowHandle)
            Loop
            Return list
        End Function

        Private Function GetGroupRowHandleByValue() As Integer
            If focusedGroupValues.Count = 0 Then
                Return DataControlBase.InvalidRowHandle
            End If
            Dim i As Integer = -1
            Do While gridControl.IsValidRowHandle(i)
                If gridControl.GetRowLevelByRowHandle(i) = 0 Then
                    Dim rowHandle = FindGroupRowHandle(i, 0)
                    If rowHandle <> DataControlBase.InvalidRowHandle Then
                        Return rowHandle
                    End If
                End If
                i -= 1
            Loop
            Return DataControlBase.InvalidRowHandle
        End Function
        Private Function FindGroupRowHandle(ByVal rowHandle As Integer, ByVal level As Integer) As Integer
            Dim value = gridControl.GetGroupRowValue(rowHandle)
            If Not Equals(value, focusedGroupValues(level)) Then
                Return DataControlBase.InvalidRowHandle
            End If
            If focusedGroupValues.Count - 1 <> level Then
                Dim i As Integer = 0
                Do While i < gridControl.GetChildRowCount(rowHandle)
                    Dim childRowHandle = gridControl.GetChildRowHandle(rowHandle, i)
                    If Not gridControl.IsGroupRowHandle(childRowHandle) Then
                        i += 1
                        Continue Do
                    End If
                    Dim result = FindGroupRowHandle(childRowHandle, level + 1)
                    If result <> DataControlBase.InvalidRowHandle Then
                        Return result
                    End If
                    i += 1
                Loop
            End If
            Return rowHandle
        End Function
        #End Region

        #Region "selection"
        Private Sub SaveSelectionView()
            SelectionList.Clear()
            Dim selectedRows = gridControl.GetSelectedRowHandles()
            For i As Integer = 0 To selectedRows.Length - 1
                SelectionList.Add(GetKeyByRowHandle(selectedRows(i)))
            Next i
        End Sub

        Private Sub LoadSelectionView()
            gridControl.BeginSelection()
            Try
                gridControl.UnselectAll()
                For i As Integer = 0 To SelectionList.Count - 1
                    gridControl.SelectItem(GetRowHandleByKey(SelectionList(i)))
                Next i
            Finally
                gridControl.EndSelection()
            End Try
        End Sub
        #End Region

        Private Function GetKeyByRowHandle(ByVal rowHandle As Integer) As Object
            Return GetKeyByRowHandle(rowHandle, gridControl)
        End Function
        Private Function GetKeyByRowHandle(ByVal rowHandle As Integer, ByVal grid As GridControl) As Object
            Return grid.GetCellValue(rowHandle, keyFieldName)
        End Function

        Private Function GetRowHandleByKey(ByVal key As Object) As Integer
            Return GetRowHandleByKey(key, gridControl)
        End Function
        Private Function GetRowHandleByKey(ByVal key As Object, ByVal grid As GridControl) As Integer
            Return grid.FindRowByValue(keyFieldName, key)
        End Function
    End Class
    Public Class GroupInfo
        Public Property Value() As Object
        Public Property IsExpanded() As Boolean
        Private privateChildren As New List(Of GroupInfo)()
        Public Property Children() As List(Of GroupInfo)
            Get
                Return privateChildren
            End Get
            Private Set(ByVal value As List(Of GroupInfo))
                privateChildren = value
            End Set
        End Property
    End Class
End Namespace
