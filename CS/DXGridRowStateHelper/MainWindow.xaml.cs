using DXGridRowStateHelper.Models;
using DXGridRowStateHelper.ViewModels;
using System.Windows;

namespace DXGridRowStateHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = viewModel;

            this.helper = new RowStateHelper(this.grdCustomers, "CustomerId");
        }

        private MainWindowVM viewModel = new MainWindowVM();
        private RowStateHelper helper;

        private void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            this.helper.LoadViewInfo();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            this.helper.SaveViewInfo(this.viewModel.Customers.Count);
        }
    }
}