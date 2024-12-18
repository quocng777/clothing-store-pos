using Clothing_Store_POS.DAOs;
using Clothing_Store_POS.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Clothing_Store_POS.ViewModels
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        public readonly OrderDAO _orderDAO;
        public ObservableCollection<OrderViewModel> Orders { get; set; }
        public ObservableCollection<int> PageNumbers { get; set; }
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

        public OrdersViewModel()
        {
            this._orderDAO = new OrderDAO();
            CurrentPage = 1;
            PerPage = 10;
            Orders = new ObservableCollection<OrderViewModel>();
            PageNumbers = new ObservableCollection<int>();
            _ = LoadOrders();
        }

        public OrdersViewModel(int customerId)
        {
            this._orderDAO = new OrderDAO();
            CurrentPage = 1;
            PerPage = 10;
            Orders = new ObservableCollection<OrderViewModel>();
            PageNumbers = new ObservableCollection<int>();
            _ = LoadOrdersByCustomerId(customerId);
        }

        public void DeleteAnOrder(int orderId)
        {
            OrderViewModel order = null;
            foreach (var o in Orders)
            {
                if (o.Id == orderId)
                {
                    order = o;
                    break;
                }
            }

            if (order == null)
            {
                return;
            }

            _orderDAO.DeleteOrderById(orderId);
            Orders.Remove(order);
        }

        public async Task LoadOrders()
        {
            var pagedResult = await _orderDAO.GetListOrders(CurrentPage, PerPage, Keyword);
            TotalPages = pagedResult.TotalPages;
            TotalItems = pagedResult.TotalItems;

            if (CurrentPage > TotalPages)
            {
                CurrentPage = 1;
                pagedResult = await _orderDAO.GetListOrders(CurrentPage, PerPage, Keyword);
                TotalPages = pagedResult.TotalPages;
            }

            // update page numbers
            PageNumbers.Clear();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageNumbers.Add(i);
            }

            // update products
            Orders.Clear();
            foreach (var order in pagedResult.Items)
            {
                Orders.Add(new OrderViewModel(order));
            }
        }

        public async Task LoadOrdersByCustomerId(int customerId)
        {
            var pagedResult = await _orderDAO.GetListOrdersByCustomerId(CurrentPage, PerPage, customerId);
            TotalPages = pagedResult.TotalPages;
            TotalItems = pagedResult.TotalItems;

            if (CurrentPage > TotalPages)
            {
                CurrentPage = 1;
                pagedResult = await _orderDAO.GetListOrdersByCustomerId(CurrentPage, PerPage, customerId);
                TotalPages = pagedResult.TotalPages;
            }

            // update page numbers
            PageNumbers.Clear();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageNumbers.Add(i);
            }

            // update products
            Orders.Clear();
            foreach (var order in pagedResult.Items)
            {
                Orders.Add(new OrderViewModel(order));
            }
        }

        public void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                _ = LoadOrders();
            }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                _ = LoadOrders();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
