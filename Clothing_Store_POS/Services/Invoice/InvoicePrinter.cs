using Clothing_Store_POS.Models;
using Microsoft.UI.Xaml;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Clothing_Store_POS.Services.Invoice
{
    public class InvoicePrinter
    {
        public static async Task GenerateAndSaveInvoice(InvoiceModel invoiceModel)
        {
            string fileName = $"Invoice_{invoiceModel.Id}_{DateTime.Now:yyMMddhhmmss}.pdf";
            var savePicker = new FileSavePicker
            {
                SuggestedFileName = fileName,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var window = (Application.Current as App).MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

            savePicker.FileTypeChoices.Add("PDF Files", new List<string> { ".pdf" });
            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                var document = new InvoiceDocument(invoiceModel);
                document.GeneratePdf(file.Path);
                await Windows.System.Launcher.LaunchFileAsync(file);
            }
        }
    }
}
