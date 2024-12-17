using Clothing_Store_POS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace Clothing_Store_POS.Pages.Statistics
{
    public class TopItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultProductTemplate { get; set; }
        public DataTemplate FirstPositionProductTemplate { get; set; }
        public DataTemplate DefaultUserTemplate { get; set; }
        public DataTemplate FirstPositionUserTemplate { get; set; }
        public DataTemplate DefaultCustomerTemplate { get; set; }
        public DataTemplate FirstPositionCustomerTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ProductStatsDto product)
            {
                return product.Index == 1 ? FirstPositionProductTemplate : DefaultProductTemplate;
            }
            else if (item is UserStatsDto user)
            {
                return user.Index == 1 ? FirstPositionUserTemplate : DefaultUserTemplate;
            }
            else if (item is CustomerStatsDto customer)
            {
                return customer.Index == 1 ? FirstPositionCustomerTemplate : DefaultCustomerTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
