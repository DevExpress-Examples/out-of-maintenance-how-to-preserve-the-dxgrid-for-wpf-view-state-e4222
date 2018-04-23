using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Windows.Documents;

namespace DXGridRowStateHelper.Models
{
    public class RowStateHelper
    {
        private GridControl gridControl;
        private string keyFieldName;

        private int topRowIndex;

        private ArrayList expandedList;
        private ArrayList selectionList;
        private ArrayList masterRowsList;
        private ArrayList groupRowList;

        

        public ArrayList ExpandedList
        {
            get
            {
                if (expandedList == null)
                    expandedList = new ArrayList();
                return expandedList;
            }
        }

        public ArrayList SelectionList
        {
            get
            {
                if (selectionList == null)
                    selectionList = new ArrayList();
                return selectionList;
            }
        }

        public ArrayList MasterRowsList
        {
            get
            {
                if (masterRowsList == null)
                    masterRowsList = new ArrayList();
                return masterRowsList;
            }
        }

        public ArrayList GroupRowList {
            get {
                if(groupRowList == null)
                    groupRowList = new ArrayList();
                return groupRowList;
            }
        }


        public RowStateHelper(GridControl vw, string keyField)
        {
            gridControl = vw;
            keyFieldName = keyField;
        }

        #region Saving Information
        public void SaveViewInfo(int dataSourceCount)
        {
            SaveExpandedMasterRows(MasterRowsList, dataSourceCount);
            SaveExpansionView(ExpandedList);
            SaveSelectionView(SelectionList);
            SaveVisibleIndex();
        }

        public void LoadViewInfo()
        {
            LoadExpandedMasterRows(MasterRowsList);
            LoadExpansionView(ExpandedList);
            LoadSelectionView(SelectionList);
            LoadVisibleIndex();
        }

        private void SaveExpandedMasterRows(ArrayList expandedList, int dataSourceCount)
        {
            expandedList.Clear();

            for (int i = 0; i < dataSourceCount; i++)
            {
                int rowHandle = gridControl.GetRowHandleByListIndex(i);
                if (gridControl.IsMasterRowExpanded(rowHandle))
                    expandedList.Add(rowHandle);
            }
        }

        

        private void SaveExpansionView(ArrayList expandedGroupsList)
        {
            if (gridControl.GroupCount == 0) return;
            GroupRowList.Clear();
            for (int i = -1; i > int.MinValue; i--)
            {
                if (!gridControl.IsValidRowHandle(i)) break;
                if (gridControl.IsGroupRowExpanded(i))
                {
                    GroupRowList.Add(i);
                }
            }
        }

        private void SaveSelectionView(ArrayList selectionList)
        {
            selectionList.Clear();

            int[] selectedRows = ((TableView)gridControl.View).GetSelectedRowHandles();
            for (int i = 0; i < selectedRows.Length; i++)
            {
                selectionList.Add(selectedRows[i]);
            }
            selectionList.Add(gridControl.View.FocusedRowHandle);
        }

        public void SaveVisibleIndex()
        {
            topRowIndex = gridControl.View.TopRowIndex;
        }

        #endregion
        #region Loading Information
        private void LoadExpandedMasterRows(ArrayList expandedList)
        {

            gridControl.CollapseAllGroups();
            for (int i = 0; i < expandedList.Count; i++)
            {
                gridControl.ExpandMasterRow((int)expandedList[i]);
            }

        }

        private void LoadExpansionView(ArrayList expandedGroupsList)
        {
            if (gridControl.GroupCount == 0) return;

            gridControl.CollapseAllGroups();

            foreach(int grouIndex in GroupRowList) {
                gridControl.ExpandGroupRow(grouIndex);
            }
        }

        private void LoadSelectionView(ArrayList selectionList)
        {
            ((TableView)gridControl.View).BeginSelection();
            try
            {
                ((TableView)gridControl.View).ClearSelection();
                for (int i = 0; i < selectionList.Count; i++)
                {
                    if (i == selectionList.Count - 1)
                        ((TableView)gridControl.View).FocusedRowHandle = Convert.ToInt32(selectionList[i]);
                    else
                        ((TableView)gridControl.View).SelectRow(Convert.ToInt32(selectionList[i]));
                }
            }
            finally
            {
                ((TableView)gridControl.View).EndSelection();
            }
        }

        public void LoadVisibleIndex()
        {
            gridControl.View.TopRowIndex = topRowIndex;
        }
        #endregion
    }
}