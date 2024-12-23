using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Helper
{
    public class PaymentHandler
    {
        public event Action<string> PaymentReceived;

        public void StartHttpListener()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/callback/");
            listener.Start();
            Console.WriteLine("Listening for VNPay callback...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                string queryString = request.Url.Query;
                Console.WriteLine("Received callback: " + queryString);

                HttpListenerResponse response = context.Response;
                string responseString = "<html><body>Payment received. You can close this window.</body></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();

                PaymentReceived?.Invoke(queryString);
            }
        }
    }
}
