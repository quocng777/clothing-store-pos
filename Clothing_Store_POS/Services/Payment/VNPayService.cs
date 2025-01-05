using Clothing_Store_POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Services.Payment
{
    class VNPayService
    {
        private static VNPayService _instance;
        public PaymentHandler PaymentHandler { get; private set; }

        private VNPayService()
        {
            PaymentHandler = new PaymentHandler();
        }

        public static VNPayService Instance => _instance ??= new VNPayService();
    }
}
