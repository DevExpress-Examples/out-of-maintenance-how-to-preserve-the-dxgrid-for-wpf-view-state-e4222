// Developer Express Code Central Example:
// How to preserve the DXGrid for WPF view state
// 
// This example demonstrates how to save/load the groups and master rows expansion
// state, selection, focused row and position of the focused row.
// All
// functionality implemented in custom class named RowStateHelper, that contains
// methods for performing these operations.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E4222

using DevExpress.Xpf.Grid;
using System.Collections;
using System.Collections.Generic;

namespace SaveWpfGridState {
    public class RowStateHelper {
        GridControl gridControl;
        string keyFieldName;

        object focusedMasterKey;
        object focusedDetailKey;
        List<object> focusedGroupValues;

        ArrayList selectionList;
        ArrayList masterRowList;
        List<GroupInfo> groupRowList;

        public ArrayList SelectionList
        {
            get
            {
                if (selectionList == null)
                    selectionList = new ArrayList();
                return selectionList;
            }
        }
        public ArrayList MasterRowList
        {
            get
            {
                if (masterRowList == null)
                    masterRowList = new ArrayList();
                return masterRowList;
            }
        }
        public List<GroupInfo> GroupRowList
        {
            get
            {
                if (groupRowList == null)
                    groupRowList = new List<GroupInfo>();
                return groupRowList;
            }
        }

        public RowStateHelper(GridControl vw, string keyField) {
            gridControl = vw;
            keyFieldName = keyField;
        }

        public void SaveViewInfo() {
            SaveExpandedMasterRows();
            SaveExpansionView();
            //SaveSelectionView();
            SaveSelection();
        }
        public void LoadViewInfo() {
            LoadExpandedMasterRows();
            LoadExpansionView();
            //LoadSelectionView();
            LoadSelection();
            ClearVariables();
        }
        void ClearVariables() {
            focusedMasterKey = null;
            focusedDetailKey = null;
            focusedGroupValues = null;
            SelectionList.Clear();
            MasterRowList.Clear();
            GroupRowList.Clear();
        }

        #region master rows
        void SaveExpandedMasterRows() {
            MasterRowList.Clear();
            for (int i = 0; i < gridControl.DataController.ListSourceRowCount; i++) {
                var rowHandle = gridControl.GetRowHandleByListIndex(i);
                if (gridControl.IsMasterRowExpanded(rowHandle))
                    MasterRowList.Add(GetKeyByRowHandle(rowHandle));
            }
        }
        void LoadExpandedMasterRows() {
            for (int i = 0; i < gridControl.DataController.ListSourceRowCount; i++) {
                var rowHandle = gridControl.GetRowHandleByListIndex(i);
                gridControl.CollapseMasterRow(rowHandle);
            }
            foreach (var key in MasterRowList) {
                var test = GetRowHandleByKey(key);
                var datarow = gridControl.DataController.GetRow(0);
                gridControl.ExpandMasterRow(GetRowHandleByKey(key));
            }
        }
        #endregion

        #region group rows
        void SaveExpansionView() {
            if (gridControl.GroupCount == 0)
                return;
            GroupRowList.Clear();
            for (int i = -1; i > int.MinValue; i--) {
                if (!gridControl.IsValidRowHandle(i))
                    break;
                if (gridControl.GetRowLevelByRowHandle(i) == 0)
                    GroupRowList.Add(GetGroupInfo(i));
            }
        }
        GroupInfo GetGroupInfo(int rowHandle) {
            var info = new GroupInfo {
                Value = gridControl.GetGroupRowValue(rowHandle),
                IsExpanded = gridControl.IsGroupRowExpanded(rowHandle)
            };
            for (var i = 0; i < gridControl.GetChildRowCount(rowHandle); i++) {
                var childRowHandle = gridControl.GetChildRowHandle(rowHandle, i);
                if (gridControl.IsGroupRowHandle(childRowHandle))
                    info.Children.Add(GetGroupInfo(childRowHandle));
            }
            return info;
        }

        void LoadExpansionView() {
            if (gridControl.GroupCount == 0)
                return;
            gridControl.CollapseAllGroups();
            for (int i = -1; i > int.MinValue; i--) {
                if (!gridControl.IsValidRowHandle(i))
                    break;
                if (gridControl.GetRowLevelByRowHandle(i) == 0)
                    ExpandGroupRow(i, GroupRowList);
            }
        }
        void ExpandGroupRow(int rowHandle, List<GroupInfo> infoList) {
            var value = gridControl.GetGroupRowValue(rowHandle);
            var info = infoList.Find(i => i.Value.Equals(value));
            if (info == null)
                return;
            if (info.IsExpanded)
                gridControl.ExpandGroupRow(rowHandle);
            for (int i = 0; i < gridControl.GetChildRowCount(rowHandle); i++) {
                var childRowHandle = gridControl.GetChildRowHandle(rowHandle, i);
                if (gridControl.IsGroupRowHandle(childRowHandle))
                    ExpandGroupRow(childRowHandle, info.Children);
            }
            infoList.Remove(info);
        }
        #endregion

        #region focused row
        void SaveSelection() {
            focusedDetailKey = null;
            focusedMasterKey = null;
            focusedGroupValues = null;

            int focusedRowHandle;
            if (gridControl.View.IsFocusedView) {
                focusedDetailKey = null;
                focusedRowHandle = gridControl.View.FocusedRowHandle;
                if (gridControl.IsGroupRowHandle(focusedRowHandle)) {
                    focusedGroupValues = GetGroupValues(focusedRowHandle);
                    return;
                }
            }
            else {
                var focusedView = gridControl.View.FocusedView;
                var detailRowHandle = focusedView.FocusedRowHandle;
                focusedDetailKey = GetKeyByRowHandle(detailRowHandle, (GridControl)focusedView.DataControl);
                focusedRowHandle = ((GridControl)focusedView.DataControl).GetMasterRowHandle();
            }
            focusedMasterKey = GetKeyByRowHandle(focusedRowHandle);
        }

        void LoadSelection() {
            var masterRowHandle = (focusedGroupValues == null) ? GetRowHandleByKey(focusedMasterKey) : GetGroupRowHandleByValue();
            if (focusedDetailKey == null) {
                gridControl.View.FocusedRowHandle = masterRowHandle;
                return;
            }
            if (!gridControl.IsMasterRowExpanded(masterRowHandle))
                gridControl.ExpandMasterRow(masterRowHandle);
            var detailControl = gridControl.GetDetail(masterRowHandle) as GridControl;
            if (detailControl == null)
                return;
            detailControl.View.MoveFocusedRow(GetRowHandleByKey(focusedDetailKey, detailControl));
        }

        List<object> GetGroupValues(int rowHandle) {
            var list = new List<object>();
            while (rowHandle != DataControlBase.InvalidRowHandle) {
                list.Insert(0, gridControl.GetGroupRowValue(rowHandle));
                rowHandle = gridControl.GetParentRowHandle(rowHandle);
            }
            return list;
        }

        int GetGroupRowHandleByValue() {
            if (focusedGroupValues.Count == 0)
                return DataControlBase.InvalidRowHandle;
            for (int i = -1; gridControl.IsValidRowHandle(i); i--) {
                if (gridControl.GetRowLevelByRowHandle(i) == 0) {
                    var rowHandle = FindGroupRowHandle(i, 0);
                    if (rowHandle != DataControlBase.InvalidRowHandle)
                        return rowHandle;
                }
            }
            return DataControlBase.InvalidRowHandle;
        }
        int FindGroupRowHandle(int rowHandle, int level) {
            var value = gridControl.GetGroupRowValue(rowHandle);
            if (!value.Equals(focusedGroupValues[level]))
                return DataControlBase.InvalidRowHandle;
            if (focusedGroupValues.Count - 1 != level) {
                for (int i = 0; i < gridControl.GetChildRowCount(rowHandle); i++) {
                    var childRowHandle = gridControl.GetChildRowHandle(rowHandle, i);
                    if (!gridControl.IsGroupRowHandle(childRowHandle))
                        continue;
                    var result = FindGroupRowHandle(childRowHandle, level + 1);
                    if (result != DataControlBase.InvalidRowHandle)
                        return result;
                }
            }
            return rowHandle;
        }
        #endregion

        #region selection
        void SaveSelectionView() {
            SelectionList.Clear();
            var selectedRows = gridControl.GetSelectedRowHandles();
            for (int i = 0; i < selectedRows.Length; i++) {
                SelectionList.Add(GetKeyByRowHandle(selectedRows[i]));
            }
        }

        void LoadSelectionView() {
            gridControl.BeginSelection();
            try {
                gridControl.UnselectAll();
                for (int i = 0; i < SelectionList.Count; i++)
                    gridControl.SelectItem(GetRowHandleByKey(SelectionList[i]));
            }
            finally {
                gridControl.EndSelection();
            }
        }
        #endregion

        object GetKeyByRowHandle(int rowHandle) {
            return GetKeyByRowHandle(rowHandle, gridControl);
        }
        object GetKeyByRowHandle(int rowHandle, GridControl grid) {
            return grid.GetCellValue(rowHandle, keyFieldName);
        }

        int GetRowHandleByKey(object key) {
            return GetRowHandleByKey(key, gridControl);
        }
        int GetRowHandleByKey(object key, GridControl grid) {
            return grid.FindRowByValue(keyFieldName, key);
        }
    }
    public class GroupInfo {
        public GroupInfo() {
            Children = new List<GroupInfo>();
        }
        public object Value { get; set; }
        public bool IsExpanded { get; set; }
        public List<GroupInfo> Children { get; private set; }
    }
}