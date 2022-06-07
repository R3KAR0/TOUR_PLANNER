using LAUER_SWEN2_TOUR_PLANNER.BL.Tours;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Serilog;
using iTextSharp;
using iText.IO.Image;
//using System.Drawing;

namespace LAUER_SWEN2_TOUR_PLANNER.BL.Reports
{
    public class ReportCreator
    {
        public void CreateSummaryReport()
        {
            try
            {
                Log.Information("Start creating summery report!");
                string filename = $"Summery_report_{DateTime.Now.ToString("yyyyMMddHHmm")}.pdf";

                PdfWriter writer = new PdfWriter(filename);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                var tours = TourLogic.GetAllToursWithTourLogs();

                if (tours != null && tours.Count > 0)
                {
                    GenerateTourSummary(document, tours);
                }

                document.Close();
                Log.Information("Summery report created successfully!");
            }
            catch (Exception)
            {

                Log.Error("Exception encountered while creating summery_report!");
            }

        }

        public void GenerateReportForOneTour(Tour tour)
        {

            string filename = $"{DateTime.Now.ToString("yyyyMMddHHmm")}_{tour.Name}.pdf";

            PdfWriter writer = new PdfWriter(filename);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            GenerateReport(document, tour);

            document.Close();
        }

        public void GenerateReport(Document document, Tour tour)
        {


            AddTourInfo(document, tour);

            AddImage(document, tour.PictureBytes);

            AddLogTable(document, tour.Logs);


        }

        public void GenerateTourSummary(Document document, List<Tour> tours)
        {

            foreach (Tour tour in tours)
            {
                AddTourInfo(document, tour);
                AddImage(document, tour.PictureBytes);
                document.Add(new AreaBreak());

            }


        }

        public System.Drawing.Image GetImageFromBytes(Byte[] pictureBytes)
        {
            using (var ms = new MemoryStream(pictureBytes))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        public string GetTimeStamp()
        {

            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        void AddNewLine(Document document)
        {
            document.Add(new Paragraph(""));
        }

        static Cell getHeaderCell(String s)
        {
            return new Cell().Add(new Paragraph(s))
                .SetBold()
                .SetBackgroundColor(ColorConstants.WHITE)
                .SetFontSize(16)
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD));
        }

        void AddTourInfo(Document document, Tour tour)
        {
            Paragraph TourName = new Paragraph(tour.Name)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.RED);
            document.Add(TourName);
            Paragraph Description = new Paragraph("Description").SetBold();
            document.Add(Description);
            document.Add(new Paragraph(tour.Description));

            AddNewLine(document);

            document.Add(new Paragraph(tour.TransportType.ToString()));

            AddNewLine(document);

            document.Add(new Paragraph("From: " + tour.From));
            document.Add(new Paragraph("To: " + tour.To));

            AddNewLine(document);
            AddNewLine(document);
            AddNewLine(document);
        }

        void AddLogTable(Document document, List<TourLog> tourLogList)
        {
            Paragraph tableHeader = new Paragraph("TourLogs")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLACK)
                    .SetBackgroundColor(ColorConstants.WHITE);
            document.Add(tableHeader);

            Table table = new Table(UnitValue.CreatePercentArray(6)).UseAllAvailableWidth();
            table.AddHeaderCell(getHeaderCell("Date"));
            table.AddHeaderCell(getHeaderCell("Duration"));
            table.AddHeaderCell(getHeaderCell("Distance"));
            table.AddHeaderCell(getHeaderCell("Rating"));
            table.AddHeaderCell(getHeaderCell("Difficulty"));
            table.AddHeaderCell(getHeaderCell("Comment"));

            table.SetFontSize(11).SetBackgroundColor(ColorConstants.WHITE);

            foreach (var tl in tourLogList)
            {
                table.AddCell(tl.CreationDate.ToString());
                table.AddCell(tl.TotalTime.ToString());
                table.AddCell(tl.TourRating.ToString());
                table.AddCell(tl.Difficulty.ToString());
                table.AddCell(tl.Comment);
            }

            document.Add(table);

            AddNewLine(document);

        }

        void AddImage(Document document, Byte[] img)
        {

            ImageData imageData = ImageDataFactory.Create(img);
            Image image = new Image(imageData);

            image.ScaleAbsolute(400, 400).SetHorizontalAlignment(HorizontalAlignment.CENTER);

            document.Add(image);

            AddNewLine(document);
            AddNewLine(document);
        }

    }
}
