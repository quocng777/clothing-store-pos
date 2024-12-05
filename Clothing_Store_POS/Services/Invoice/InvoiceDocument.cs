using Clothing_Store_POS.Converters;
using Clothing_Store_POS.Models;
using Microsoft.UI.Xaml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Clothing_Store_POS.Services.Invoice
{
    public class InvoiceDocument : IDocument
    {
        public InvoiceModel Model { get; set; }

        public InvoiceDocument(InvoiceModel model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Darken1);
            var infoStyle = TextStyle.Default.FontSize(10).FontColor(Colors.Grey.Darken2);
            var shopNameStyle = TextStyle.Default.FontSize(12).Bold().FontColor(Colors.Black);


            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().PaddingBottom(10).Text($"Invoice #{Model.Id}").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span($"Issue date: {Model.CreatedAt:d}").Style(infoStyle);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span($"Created by: {Model.User.FullName}").Style(infoStyle);
                    });
                });


                row.RelativeItem().PaddingTop(-5).AlignRight().Column(column =>
                {
                    column.Item().Height(50).Width(50).Image("Assets/logo.png").FitArea();
                    column.Item().Text("TQT Store").Style(shopNameStyle);
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

                column.Item().Width(100).BorderBottom(1).PaddingBottom(5).Text("Customer").SemiBold();
                if (Model.Customer != null)
                {
                    column.Item().Element(ComposeCustomer);
                }
                else
                {
                    column.Item().Text("None");
                }

                column.Item().Element(ComposeTable);

                column.Item().Element(ComposeGrandTotal);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Product");
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                    header.Cell().Element(CellStyle).AlignCenter().Text("Quantity");
                    header.Cell().Element(CellStyle).AlignCenter().Text("Discount(%)");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                        .DefaultTextStyle(x => x.SemiBold())
                        .BorderBottom(1)
                        .PaddingVertical(5)
                        .BorderColor(Colors.Grey.Darken1);
                    }
                });

                foreach (var item in Model.InvoiceItems)
                {
                    table.Cell().Element(CellStyle).Text($"{Model.InvoiceItems.IndexOf(item) + 1}");
                    table.Cell().Element(CellStyle).Text(item.Product.Name);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{PriceToVNDConverter.convertToVND(item.Product.Price)}");
                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.Quantity}");
                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.DiscountPercentage}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{PriceToVNDConverter.convertToVND(item.TotalPrice)}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container
                        .BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingVertical(5);
                    }
                }
            });
        }

        void ComposeGrandTotal(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text($"{PriceToVNDConverter.convertToVND(Model.OriginalPrice)}").AlignRight();

                column.Item().PaddingVertical(5).Row(row =>
                {
                    row.RelativeItem().Text("Discount:").AlignLeft();
                    row.ConstantItem(100).Text($"- {Model.DiscountPercentage} %").AlignRight();
                });

                column.Item().PaddingVertical(5).Row(row =>
                {
                    row.RelativeItem().Text("Tax:").AlignLeft();
                    row.ConstantItem(100).Text($" + {Model.TaxPercentage} %").AlignRight();
                });

                column.Item().BorderTop(1).PaddingVertical(10).Row(row =>
                {
                    row.RelativeItem().Text("Total:").AlignLeft().Bold();
                    row.ConstantItem(100).Text($"{PriceToVNDConverter.convertToVND(Model.FinalPrice)}").AlignRight().Bold();
                });
            });
        }

        void ComposeCustomer(IContainer container)
        {
            var valueStyle = TextStyle.Default.FontColor(Colors.Black);

            container.PaddingBottom(10).Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Name: {Model.Customer.Name}").Style(valueStyle);
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Email: {Model.Customer.Email}").Style(valueStyle);
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Phone: {Model.Customer.Phone}").Style(valueStyle);
                });
            });
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Grey.Lighten4).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Notes").FontSize(14);
                column.Item().Text("Please...");
            });
        }
    }
}
