using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaveWpfGridState
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            rowStateHelper = new RowStateHelper(grdMaster, "id");
            DataContext = this;
        }

        public List<MasterDatum> Data
        {
            get
            {
                return _data;
            }
        }
        List<MasterDatum> _data;

        RowStateHelper rowStateHelper;

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            _data = new List<MasterDatum>();
            for (int i = 1; i < 11; i++)
            {
                _data.Add(new MasterDatum() {
                    id = i,
                    MasterDescription = string.Format("Master_Desc_{0}", i),
                    GroupField = string.Format("Group_{0}", (i % 2) + 1)
                });
            }
            rowStateHelper.SaveViewInfo();
            RaisePropertyChanged("Data");
            rowStateHelper.LoadViewInfo();
        }
    }

    public class MasterDatum
    {
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                ChildData = new List<ChildDatum>();
                for (int i = 0; i < 12; i++)
                {
                    ChildData.Add(new ChildDatum() {
                        id = (_id * 1000) + i,
                        ChildDescription = string.Format("Child_Desc_{0}",
                        (_id * 1000) + i)
                    });
                }
            }
        }
        private int _id;
        public string MasterDescription { get; set; }
        public string GroupField { get; set; }
        public List<ChildDatum> ChildData { get; set; }
    }

    public class ChildDatum
    {
        public int id { get; set; }
        public string ChildDescription { get; set; }
    }
}
