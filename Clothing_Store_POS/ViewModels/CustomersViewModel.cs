using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class CustomersViewModel : INotifyPropertyChanged
    {
        public readonly CustomerDAO _customerDAO;
        public ObservableCollection<Customer> Customers { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string Keyword { get; set; }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private int _perPage;
        public int PerPage
        {
            get => _perPage;
            set
            {
                _perPage = value;
                OnPropertyChanged(nameof(PerPage));
            }
        }

        public CustomersViewModel()
        {
            this._customerDAO = new CustomerDAO();
            CurrentPage = 1;
            PerPage = 6;
            Customers = new ObservableCollection<Customer>();
        }

        public async Task LoadCustomers(bool useNoTracking = false)
        {
            var pagedResult = await _customerDAO.GetCustomers(CurrentPage, PerPage, Keyword, useNoTracking);
            TotalPages = pagedResult.TotalPages;
            TotalItems = pagedResult.TotalItems;
            if (CurrentPage > TotalPages)
            {
                CurrentPage = TotalPages;
                pagedResult = await _customerDAO.GetCustomers(CurrentPage, PerPage, Keyword, useNoTracking);
                TotalItems = pagedResult.TotalItems;
            }

            // update products
            Customers.Clear();
            foreach (var c in pagedResult.Items)
            {
                Customers.Add(c);
            }
        }
        public void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                _ = LoadCustomers(false);
            }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                _ = LoadCustomers(false);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DeleteACustomer(int customerId)
        {

            Customer customer = null;
            foreach (var c in Customers)
            {
                if (c.Id == customerId)
                {
                    customer = c;
                    break;
                }
            }

            if (customer == null)
            {
                return;
            }

            _customerDAO.DeleteCustomerById(customerId);
            Customers.Remove(customer);
            TotalItems--;
        }
    }
}
