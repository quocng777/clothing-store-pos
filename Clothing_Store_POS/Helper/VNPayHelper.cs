using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Clothing_Store_POS.Helper
{
    public static class VNPayHelper
    {
        // Tạo chuỗi dữ liệu cần băm
        public static string CreateHashData(SortedDictionary<string, string> parameters, string hashSecret)
        {
            StringBuilder data = new StringBuilder();

            // Loại bỏ tham số vnp_SecureHash khi tạo chuỗi
            foreach (var param in parameters)
            {
                if (param.Key != "vnp_SecureHash") // Bỏ qua vnp_SecureHash khi tạo chuỗi hash
                {
                    if (data.Length > 0)
                    {
                        data.Append("&");
                    }
                    data.Append(param.Key + "=" + param.Value);
                }
            }

            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hashSecret)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data.ToString()));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // Tạo chuỗi query string từ danh sách tham số
        public static string BuildQueryString(SortedDictionary<string, string> parameters)
        {
            StringBuilder queryBuilder = new StringBuilder();
            foreach (var param in parameters)
            {
                if (queryBuilder.Length > 0)
                {
                    queryBuilder.Append("&");
                }
                queryBuilder.Append(HttpUtility.UrlEncode(param.Key) + "=" + HttpUtility.UrlEncode(param.Value));
            }
            return queryBuilder.ToString();
        }

        // Tạo URL thanh toán VNPay
        public static string GeneratePaymentUrl(string baseUrl, SortedDictionary<string, string> parameters, string hashSecret)
        {
            string queryString = BuildQueryString(parameters);
            string secureHash = CreateHashData(parameters, hashSecret);  // Tính toán lại vnp_SecureHash
            return $"{baseUrl}?{queryString}&vnp_SecureHash={secureHash}";
        }

        public static string CreatePaymentUrl(Double totalAmount)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = "http://localhost:5000/callback";

            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", "RF0CB68N");
            pay.AddRequestData("vnp_Amount", (totalAmount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode","VND");
            pay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", $"Thanh toan clothing store");
            pay.AddRequestData("vnp_OrderType", "pay");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "U2SDDL7N5CPE5EOB8QARV18IE5BAPNNX");

            return paymentUrl;
        }
    }
}
