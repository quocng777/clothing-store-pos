using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class CustomerViewModel
    {
        private readonly CustomerDAO _customerDAO;

        public CustomerViewModel()
        {
            this._customerDAO = new CustomerDAO();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Save()
        {
            // handle saving image

            var customer = new Customer
            {
                Name = this.Name,
                Email = this.Email,
                Phone = this.Phone
            };

            return this._customerDAO.AddCustomer(customer);
        }

        public void Clear()
        {
            Name = String.Empty;
            Email = String.Empty;
            Phone = String.Empty;
        }

        public void Update()
        {
            var savedCustomer = _customerDAO.FindCustomerById(this.Id);
            if (savedCustomer == null)
            {
                throw new Exception("Customer not found");
            }

            savedCustomer.Name = this.Name;
            savedCustomer.Email = this.Email;
            savedCustomer.Phone = this.Phone;

            _customerDAO.UpdateCustomer(savedCustomer);
        }
    }
}
