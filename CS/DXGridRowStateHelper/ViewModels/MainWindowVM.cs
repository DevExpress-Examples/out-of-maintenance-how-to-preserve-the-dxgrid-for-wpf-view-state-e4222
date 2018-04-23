using DXGridRowStateHelper.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DXGridRowStateHelper.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindowVM()
        {
            Random random = new Random();
            Random random2 = new Random();

            for (int i = 0; i < 100; i++)
            {
                this.Customers.Add(new Customer() 
                { 
                    CustomerId = i, 
                    RandomId = random.Next(1, 10),
                    RandomId2 = random.Next(1, 3),
                    FirstName = "TestFirstName", 
                    LastName = "TestLastName" 
                });
            }
        }

        #endregion

        #region Properties

        #region Customers Property

        private ObservableCollection<Customer> m_Customers = new ObservableCollection<Customer>();
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return this.m_Customers;
            }
            set
            {
                this.m_Customers = value;

                this.OnPropertyChanged("Customers");
            }
        }

        #endregion

        #region CurrentCustomer Property

        private Customer m_CurrentCustomer = null;
        public Customer CurrentCustomer
        {
            get
            {
                return this.m_CurrentCustomer;
            }
            set
            {
                if (this.m_CurrentCustomer == value)
                {
                    return;
                }

                this.m_CurrentCustomer = value;

                this.OnPropertyChanged("CurrentCustomer");
            }
        }

        #endregion

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}