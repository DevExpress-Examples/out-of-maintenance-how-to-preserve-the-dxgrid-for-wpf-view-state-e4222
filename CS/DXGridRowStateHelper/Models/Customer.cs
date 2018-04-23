using System.ComponentModel;

namespace DXGridRowStateHelper.Models
{
    public class Customer : INotifyPropertyChanged
    {
        #region CustomerId Property

        private int m_CustomerId = 0;
        public int CustomerId
        {
            get
            {
                return this.m_CustomerId;
            }
            set
            {
                this.m_CustomerId = value;

                this.OnPropertyChanged("CustomerId");
            }
        }

        #endregion

        #region RandomId Property

        private int m_RandomId = 0;
        public int RandomId
        {
            get
            {
                return this.m_RandomId;
            }
            set
            {
                this.m_RandomId = value;

                this.OnPropertyChanged("RandomId");
            }
        }

        #endregion

        #region RandomId2 Property

        private int m_RandomId2 = 0;
        public int RandomId2
        {
            get
            {
                return this.m_RandomId2;
            }
            set
            {
                this.m_RandomId2 = value;

                this.OnPropertyChanged("RandomId2");
            }
        }

        #endregion

        #region FirstName Property

        private string m_FirstName = string.Empty;
        public string FirstName
        {
            get
            {
                return this.m_FirstName;
            }
            set
            {
                this.m_FirstName = value;

                this.OnPropertyChanged("FirstName");
            }
        }

        #endregion

        #region LastName Property

        private string m_LastName = string.Empty;
        public string LastName
        {
            get
            {
                return this.m_LastName;
            }
            set
            {
                this.m_LastName = value;

                this.OnPropertyChanged("LastName");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}