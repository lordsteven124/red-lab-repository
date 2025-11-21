using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ProductManagement.API.Models;

namespace ProductManagement.API.Services
{
    public class ProductPdfService
    {
        public byte[] GeneratePdfReport(List<Product> products)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .AlignCenter()
                        .Text("Reporte de Productos")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            // Tabla de productos
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3); // Nombre
                                    columns.RelativeColumn(4); // Descripción
                                    columns.RelativeColumn(2); // Precio
                                    columns.RelativeColumn(2); // Estado
                                });

                                // Encabezado de la tabla
                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Nombre");
                                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Descripción");
                                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Precio");
                                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Estado");
                                });

                                // Filas de productos
                                foreach (var product in products)
                                {
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(product.Nombre);
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(product.Descripcion ?? "N/A");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"${product.Precio:F2}");
                                    table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(product.Estado ? "Activo" : "Inactivo");
                                }
                            });

                            // Resumen
                            column.Item().PaddingTop(10).AlignRight().Text(text =>
                            {
                                text.Span("Total de productos: ").SemiBold();
                                text.Span(products.Count.ToString());
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generado el ");
                            x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}