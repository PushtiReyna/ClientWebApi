using DataLayer.Entities;
using DTO.Salary;
using Helper.CommonModel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Document = iTextSharp.text.Document;

using System.Globalization;


namespace BusinessLayer
{
    public class SalaryBLL
    {
        public readonly ClientApiDbContext _db;
        public SalaryBLL(ClientApiDbContext db)
        {
            _db = db;
        }

        public CommonResponse SalaryClient(SalaryClientReqDTO salaryClientReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                SalaryClientResDTO salaryClientResDTO = new SalaryClientResDTO();

                var clientDetail = _db.ClientMsts.FirstOrDefault(x => x.Id == salaryClientReqDTO.Id && x.IsDelete == false);

                if (clientDetail != null)
                {
                    SalaryMst salaryMst = new SalaryMst();
                    salaryMst.Id = salaryClientReqDTO.Id;
                    // salaryMst.Month = salaryClientReqDTO.Month.Month;
                    // salaryMst.Year = salaryClientReqDTO.Month.Year;

                    salaryMst.Month = salaryClientReqDTO.Month;
                    salaryMst.Year = salaryClientReqDTO.Year;

                    salaryMst.FixedBasicSalary = (decimal)(clientDetail.Ctc) * ((decimal)(40.0 / 100.0));

                    salaryMst.FixedHra = salaryMst.FixedBasicSalary * ((decimal)0.4);

                    salaryMst.FixedConveyanceAllowance = 1600;

                    salaryMst.FixedMedicalAllowance = 1250;

                    salaryMst.AdditionalHraAllowance = 2400;

                    salaryMst.TotalDays = 29;

                    salaryMst.PayableDays = salaryMst.TotalDays;

                    var PayableDays_TotalDays = salaryMst.PayableDays / salaryMst.TotalDays;

                    salaryMst.GrossSalaryPayable = ((decimal)(clientDetail.Ctc) * PayableDays_TotalDays);

                    salaryMst.Basic = (salaryMst.FixedBasicSalary * PayableDays_TotalDays);

                    var GrossSalaryPayable_Basic = (salaryMst.GrossSalaryPayable - salaryMst.Basic);

                    var FixedHra_PayableDays_TotalDays = (salaryMst.FixedHra * PayableDays_TotalDays);

                    var basic = ((decimal)0.4) * salaryMst.Basic;

                    // HouseRentAllowance = IF((O7-P7)<0.4*P7,O7-P7,G7*N7/K7)
                    if (GrossSalaryPayable_Basic < basic)
                    {
                        salaryMst.HouseRentAllowance = GrossSalaryPayable_Basic;
                    }
                    else
                    {
                        salaryMst.HouseRentAllowance = FixedHra_PayableDays_TotalDays;
                    }
                    //Pf = IF((F7)<15001,P7*12%,0)
                    if (salaryMst.FixedBasicSalary < 15001)
                    {
                        salaryMst.PfOne = salaryMst.Basic * ((decimal)0.12);
                    }
                    else
                    {
                        salaryMst.PfOne = 0;
                    }

                    salaryMst.PfTwo = salaryMst.PfOne;

                    salaryMst.EmployerContPf = salaryMst.PfTwo;

                    var HouseRentAllowance_EmployerContPf = salaryMst.HouseRentAllowance - salaryMst.EmployerContPf;

                    //ConveyanceAllowance = IF((O7-P7-Q7-R7)<1600*N7/K7,(O7-P7-Q7-R7),H7*N7/K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf) < 1600 * PayableDays_TotalDays)
                    {
                        salaryMst.ConveyanceAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf;
                    }
                    else
                    {
                        salaryMst.ConveyanceAllowance = salaryMst.FixedConveyanceAllowance * PayableDays_TotalDays;
                    }

                    // MedicalAllowance = IF((O7-P7-Q7-R7-S7)<1250*N7/K7,O7-P7-Q7-R7-S7,I7*N7/K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance) < 1250 * PayableDays_TotalDays)
                    {
                        salaryMst.MedicalAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance;
                    }
                    else
                    {
                        salaryMst.MedicalAllowance = salaryMst.FixedMedicalAllowance * PayableDays_TotalDays; ;
                    }

                    // Additional HRA Allowance =  IF((O7 - P7 - Q7 - R7 - S7 - T7) < 2400 * N7 / K7, O7 - P7 - Q7 - R7 - S7 - T7, J7 * N7 / K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance - salaryMst.MedicalAllowance) < 2400 * PayableDays_TotalDays)
                    {
                        salaryMst.SalaryAdditionalHraAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance - salaryMst.MedicalAllowance;
                    }
                    else
                    {
                        salaryMst.SalaryAdditionalHraAllowance = salaryMst.AdditionalHraAllowance * PayableDays_TotalDays;
                    }

                    //FlexibleAllowance=O7-SUM(P7:U7) //O7 - (P7 + Q7 + R7 + S7 + T7 + U7)
                    salaryMst.FlexibleAllowance = salaryMst.GrossSalaryPayable - (salaryMst.Basic + salaryMst.HouseRentAllowance + salaryMst.EmployerContPf + salaryMst.ConveyanceAllowance + salaryMst.MedicalAllowance + salaryMst.SalaryAdditionalHraAllowance);

                    salaryMst.Incentives = 0;


                    // Total = SUM(P7:V7)+W7 // P7 + Q7 + R7 + S7 + T7 + U7 + V7 + W7
                    salaryMst.Total = (decimal)(salaryMst.Basic + salaryMst.HouseRentAllowance + salaryMst.EmployerContPf + salaryMst.ConveyanceAllowance + salaryMst.MedicalAllowance + salaryMst.SalaryAdditionalHraAllowance + salaryMst.FlexibleAllowance + salaryMst.Incentives);

                    salaryMst.Esic = 0;

                    //Pt = =IF(X7<6000,0,IF(X7<9000,80,IF(X7<12000,150,IF(X7>=12000,200))))
                    if (salaryMst.Total < 6000)
                    {
                        salaryMst.Pt = 0;
                    }
                    else if (salaryMst.Total < 9000)
                    {
                        salaryMst.Pt = 80;
                    }
                    else if (salaryMst.Total < 12000)
                    {
                        salaryMst.Pt = 150;
                    }
                    else
                    {
                        salaryMst.Pt = 200;
                    }

                    salaryMst.Advances = 0;
                    salaryMst.IncomeTax = 0;

                    //=SUM(Y7:AE7) Y7 + Z7 + AA7 + AB7 + AC7 + AD7 + AE7

                    salaryMst.TotalDed = salaryMst.PfOne + salaryMst.PfTwo + salaryMst.Pt;

                    // ActualNetPayable X7-AF7
                    salaryMst.ActualNetPayable = salaryMst.Total - salaryMst.TotalDed;

                    salaryMst.IsActive = true;
                    salaryMst.CreatedBy = 1;
                    salaryMst.CreatedOn = DateTime.Now;

                    _db.SalaryMsts.Add(salaryMst);
                    _db.SaveChanges();

                    salaryClientResDTO.Salaryid = salaryMst.Salaryid;

                    response.Data = salaryClientResDTO;
                    response.Status = true;
                    response.Message = "Data added successfully";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "client id is not valid";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }

        public CommonResponse DownloadSalary(DownloadSalaryReqDTO downloadSalaryReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var salaryDetail = _db.SalaryMsts.FirstOrDefault(x => x.Id == downloadSalaryReqDTO.UserId && x.Month == downloadSalaryReqDTO.Month && x.Year == downloadSalaryReqDTO.Year);
                if (salaryDetail != null)
                {
                    var clientDetail = _db.ClientMsts.FirstOrDefault(x => x.Id == salaryDetail.Id);
                    if (clientDetail != null)
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);

                        if (System.IO.File.Exists("D:\\SalarySlip.pdf"))
                        {
                            System.IO.File.Delete("D:\\SalarySlip.pdf");
                        }
                        FileStream FS = new FileStream("D:\\SalarySlip.pdf", FileMode.Create);

                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, FS);

                        pdfDoc.Open();

                        PdfPCell cell = new PdfPCell();

                        //Image
                        PdfPTable HeaderPlot = new PdfPTable(new float[] { 10F });
                        string path = "C:\\Users\\Reyna\\Pictures\\Saved Pictures\\book5.jfif";
                        FileInfo f2 = new FileInfo(path);
                        FileStream fs = new FileStream(f2.FullName,FileMode.Open, FileAccess.Read);
                        BinaryReader rdr = new BinaryReader(fs);
                        byte[] fileData = rdr.ReadBytes((int)fs.Length);

                        Image image = Image.GetInstance(fileData);
                        image.ScaleAbsolute(50, 50); //adjusting image size
                        image.Alignment = Element.ALIGN_CENTER;
                        cell = new PdfPCell(image)
                        {
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_TOP,
                            VerticalAlignment = Element.ALIGN_TOP
                        };
                        HeaderPlot.AddCell(cell);
                        Font font = FontFactory.GetFont("Calibri Light", 8f, Font.NORMAL,iTextSharp.text.BaseColor.BLACK);//Initializing font
                        cell = new PdfPCell(new Phrase("202/401, Iscon Atria 2, Opp GET, Gotri, Vadodara-390021", font))
                        {
                            Border = 0,
                            HorizontalAlignment = Element.ALIGN_RIGHT,
                            VerticalAlignment = Element.ALIGN_TOP
                        };
                        HeaderPlot.AddCell(cell);
                        pdfDoc.Add(HeaderPlot);
                        HeaderPlot.WriteSelectedRows(0, -1, pdfDoc.Left + 40, pdfDoc.Top - 835, pdfWriter.DirectContent);


                        //name and address
                        Paragraph header = new Paragraph("Reyna Solutions LLP", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        header.SpacingBefore = 10f;
                        header.IndentationLeft = 200f;
                        pdfDoc.Add(header);

                        header = new Paragraph("202/401, Iscon Atria 2, Opp GET, Gotri, Vadodara-390021", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        header.IndentationLeft = 100f;
                        pdfDoc.Add(header);

                        //DateTimeFormatInfo.CurrentInfo.GetMonthName(salaryDetail.Month)

                        header = new Paragraph("Pay Slip for the month of" + " " + DateTimeFormatInfo.CurrentInfo.GetMonthName(salaryDetail.Month) + "/" + salaryDetail.Year, new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        header.IndentationLeft = 160f;
                        header.SpacingAfter = 30f;
                        pdfDoc.Add(header);

                        //firsttable
                        PdfPTable FirstTable = new PdfPTable(4);
                        FirstTable.DefaultCell.BorderWidth = 0;
                        FirstTable.WidthPercentage = 100;

                        #region MyRegion
                        //FirstTable.AddCell("Emp ID");
                        //FirstTable.AddCell(salaryDetail.Id.ToString());
                        //FirstTable.AddCell("Pay Days");
                        //FirstTable.AddCell(salaryDetail.PayableDays.ToString());
                        //FirstTable.AddCell("DOJ");
                        //FirstTable.AddCell("01/07/2023");
                        //FirstTable.AddCell("Department");
                        //FirstTable.AddCell("0");
                        //FirstTable.AddCell("Employee Name");
                        //FirstTable.AddCell(clientDetail.Fullname);
                        //FirstTable.AddCell("Present Days");
                        //FirstTable.AddCell("29.00");
                        //FirstTable.AddCell("Designation");
                        //FirstTable.AddCell("0");
                        //FirstTable.AddCell("A/C No");
                        //FirstTable.AddCell("248645748865748"); 
                        #endregion

                        header = new Paragraph("Emp ID", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Paragraph Data = new Paragraph(salaryDetail.Id.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        header = new Paragraph("Pay Days", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Data = new Paragraph(salaryDetail.PayableDays.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        header = new Paragraph("DOJ ", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Data = new Paragraph("01/07/2023", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        header = new Paragraph("Department", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Data = new Paragraph("0", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        header = new Paragraph("Employee Name", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Data = new Paragraph(clientDetail.Fullname, new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        header = new Paragraph("Present Days", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Data = new Paragraph("29.00", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        header = new Paragraph("Designation", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Data = new Paragraph("0", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        header = new Paragraph("A/C No", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(header);
                        Data = new Paragraph("248645748865748", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        FirstTable.AddCell(Data);

                        FirstTable.SpacingAfter = 12f;
                        pdfDoc.Add(FirstTable);

                        //second table
                        PdfPTable secondTable = new PdfPTable(4);
                        secondTable.WidthPercentage = 100;

                        //1st Row
                        cell = new PdfPCell();
                        header = new Paragraph("Earnings", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(header);
                        cell.PaddingLeft = 30f;
                        cell.PaddingRight = 20f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        header = new Paragraph("Amount", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(header);
                        cell.PaddingLeft = 30f;
                        cell.PaddingRight = 20f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        header = new Paragraph("Deduction ", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(header);
                        cell.PaddingLeft = 30f;
                        cell.PaddingRight = 20f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        header = new Paragraph("Amount", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(header);
                        cell.PaddingLeft = 30f;
                        cell.PaddingRight = 20f;
                        secondTable.AddCell(cell);

                        //2nd Row
                        cell = new PdfPCell();
                        Data = new Paragraph("Basic", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.Basic.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.PaddingLeft = 90f;
                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        secondTable.AddCell(cell);

                        //secondTable.AddCell("Basic");
                        //secondTable.AddCell(salaryDetail.Basic.ToString());
                        // secondTable.AddCell("PF");
                        //secondTable.AddCell(salaryDetail.PfOne.ToString());

                        cell = new PdfPCell();
                        Data = new Paragraph("PF", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.PfOne.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        //3rd row
                        cell = new PdfPCell();
                        Data = new Paragraph("HRA", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.FixedHra.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("PF", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.PfTwo.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        //  secondTable.AddCell("HRA");
                        // secondTable.AddCell(salaryDetail.FixedHra.ToString());
                        // secondTable.AddCell("PF");
                        // secondTable.AddCell(salaryDetail.PfTwo.ToString());

                        //4th row
                        cell = new PdfPCell();
                        Data = new Paragraph("Conveyance", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.FixedConveyanceAllowance.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("PF_Employer", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.EmployerContPf.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        // secondTable.AddCell("Conveyance");
                        // secondTable.AddCell(salaryDetail.FixedConveyanceAllowance.ToString());
                        //secondTable.AddCell("PF_Employer");
                        // secondTable.AddCell(salaryDetail.EmployerContPf.ToString());

                        //5th row
                        cell = new PdfPCell();
                        Data = new Paragraph("Medical", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.FixedMedicalAllowance.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("");
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("");
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        //secondTable.AddCell("Medical");
                        //secondTable.AddCell(salaryDetail.FixedMedicalAllowance.ToString());
                        //secondTable.AddCell("");
                        //secondTable.AddCell("");

                        //6th row
                        cell = new PdfPCell();
                        Data = new Paragraph("Flexible B", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("2590.00", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("");
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("");
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        secondTable.AddCell(cell);

                        //secondTable.AddCell("Flexible B");
                        //secondTable.AddCell("2590.00");
                        //secondTable.AddCell("");
                        //secondTable.AddCell("");

                        //7th row
                        cell = new PdfPCell();
                        Data = new Paragraph("PF_Employer", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthTop = 0f;
                        cell.BorderWidthBottom = 0f;
                        cell.PaddingLeft = 10f;
                        cell.PaddingBottom = 40f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph(salaryDetail.EmployerContPf.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.BorderWidthTop = 0f;
                        cell.BorderWidthBottom = 0f;
                        cell.PaddingBottom = 40f;
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("");
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingBottom = 40f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("");
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingBottom = 40f;
                        secondTable.AddCell(cell);

                        //secondTable.AddCell("PF_Employer");
                        //secondTable.AddCell(salaryDetail.EmployerContPf.ToString());
                        //secondTable.AddCell("");
                        //secondTable.AddCell("");

                        //8th row
                        cell = new PdfPCell();
                        Data = new Paragraph("Total", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(Data);
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        var total = salaryDetail.Basic + salaryDetail.FixedHra + salaryDetail.FixedConveyanceAllowance + salaryDetail.FixedMedicalAllowance + (decimal)2590.00 + salaryDetail.EmployerContPf;
                        cell = new PdfPCell();
                        Data = new Paragraph(total.ToString(), new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(Data);
                        cell.PaddingLeft = 70f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("Total", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(Data);
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        var totalTwo = salaryDetail.PfOne + salaryDetail.PfTwo + salaryDetail.EmployerContPf;
                        cell = new PdfPCell();
                        Data = new Paragraph(totalTwo.ToString(), new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD));
                        cell.AddElement(Data);
                        cell.PaddingLeft = 90f;
                        secondTable.AddCell(cell);

                        //9th row
                        cell = new PdfPCell();
                        Data = new Paragraph("Net", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD));
                        cell.AddElement(Data);
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        var net = total - totalTwo;
                        cell = new PdfPCell();
                        Data = new Paragraph(net.ToString(), new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.Colspan = 3;
                        cell.AddElement(Data);
                        cell.BorderWidthLeft = 0f;
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        secondTable.AddCell(cell);

                        //10th row
                        cell = new PdfPCell();
                        Data = new Paragraph("In Words", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD));
                        cell.AddElement(Data);
                        cell.BorderWidthRight = 0f;
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        cell.PaddingLeft = 10f;
                        secondTable.AddCell(cell);

                        string word = NumberToWords((int)net);
                        cell = new PdfPCell();
                        Data = new Paragraph(word, new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        cell.Colspan = 3;
                        cell.BorderWidthLeft = 0f;
                        cell.BorderWidthBottom = 0f;
                        cell.BorderWidthTop = 0f;
                        secondTable.AddCell(cell);

                        //11th row
                        cell = new PdfPCell();
                        Data = new Paragraph("");
                        cell.AddElement(Data);
                        cell.Colspan = 3;
                        cell.BorderWidthLeft = 1f;
                        cell.BorderWidthBottom = 1f;
                        cell.BorderWidthTop = 0f;
                        cell.BorderWidthRight = 0f;
                        cell.PaddingBottom = 10f;
                        secondTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("Signature", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD));
                        cell.AddElement(Data);
                        cell.BorderWidthBottom = 1f;
                        cell.BorderWidthRight = 1f;
                        cell.BorderWidthTop = 0f;
                        cell.BorderWidthLeft = 0f;
                        cell.PaddingLeft = 10f;
                        cell.PaddingBottom = 10f;
                        secondTable.AddCell(cell);

                        secondTable.SpacingAfter = 12f;
                        pdfDoc.Add(secondTable);


                        PdfPTable thirdTable = new PdfPTable(7);

                        thirdTable.WidthPercentage = 55;
                        thirdTable.HorizontalAlignment = Element.ALIGN_LEFT;

                        //1st row
                        header = new Paragraph("Lev.Type", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        thirdTable.AddCell(header);

                        header = new Paragraph("Op.Bal", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        thirdTable.AddCell(header);

                        header = new Paragraph("Allot.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        thirdTable.AddCell(header);

                        header = new Paragraph("Avail.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        thirdTable.AddCell(header);

                        header = new Paragraph("Encash.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        thirdTable.AddCell(header);

                        header = new Paragraph("Adj.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        thirdTable.AddCell(header);

                        header = new Paragraph("Cl.Ba.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        thirdTable.AddCell(header);

                        //2nd row
                        cell = new PdfPCell();
                        Data = new Paragraph("Leave", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        thirdTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("0.00", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        thirdTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("1.5", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        thirdTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("0", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        thirdTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("0", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        thirdTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("0", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        thirdTable.AddCell(cell);

                        cell = new PdfPCell();
                        Data = new Paragraph("1.5", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        cell.AddElement(Data);
                        thirdTable.AddCell(cell);

                        thirdTable.SpacingAfter = 12f;
                        pdfDoc.Add(thirdTable);

                        PdfPTable fourTable = new PdfPTable(5);

                        fourTable.WidthPercentage = 55;
                        fourTable.HorizontalAlignment = Element.ALIGN_RIGHT;

                        header = new Paragraph("Advance", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        fourTable.AddCell(header);

                        header = new Paragraph("Taken", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        fourTable.AddCell(header);

                        header = new Paragraph("Op.Bal.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        fourTable.AddCell(header);

                        header = new Paragraph("EMI/Rct.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        fourTable.AddCell(header);

                        header = new Paragraph("Cl.Bal.", new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL));
                        fourTable.AddCell(header);

                        pdfDoc.Add(fourTable);

                        pdfWriter.CloseStream = false;
                        pdfDoc.Close();

                        FS.Close();

                        DownloadSalaryResDTO downloadSalaryResDTO = new DownloadSalaryResDTO();
                        downloadSalaryResDTO.UserId = clientDetail.Id;

                        response.Data = downloadSalaryResDTO;
                        response.Status = true;
                        response.Message = "pdf download successfully";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "pdf not download";
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    }
                }
                else
                {
                    response.Message = "user detail is not exists";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

    }
}
