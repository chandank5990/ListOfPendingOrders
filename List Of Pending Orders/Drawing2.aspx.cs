using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

using System.Drawing.Imaging;
using PQScan.PDFToImage;
using Spire.Pdf;
using System.Net;
using System.Net.NetworkInformation;

namespace Points_Pending_Supplier
{
    public partial class Drawing2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string testdrawing = Request.QueryString["testdrawing"];
            //if (testdrawing.Contains("kkk"))
            //{
            //    if (count == 0)
            //    {
            if (!IsPostBack)
            {
                Button1_Click(null, null);
            }
            //        count++;
            //    }
            //}
        }

        public void Button1_Click(object sender, EventArgs e)
        {
            try
            {

                string Article = Request.QueryString["Article"];
                if (Article.Length > 6)
                {

                    string pdfpath = "W:\\test\\Access\\Planos\\" + Article.Substring(0, 6) + "\\" + Article + ".PC.pdf";
                    if (File.Exists(pdfpath))
                    {

                        PdftoIMG(pdfpath);
                        //string outputpath = Server.MapPath("~/Images/UIDimage/New.jpg");
                        //string ghostScriptPath = Server.MapPath("~/Images/gs904w32.exe");
                        //PdfToJpg(ghostScriptPath,pdfpath, outputpath);


                        Image1.ImageUrl = "~/Images/UIDimage/New.jpg";

                        //Iframe.Attributes.Add("src", "Access/Planos/" + TextBoxArticle.Text.Substring(0, 6) + "/" + TextBoxArticle.Text + ".PC.pdf");
                    }

                    else
                    {
                        Image1.ImageUrl = "~/Images/draft.jpg";

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string Article = Request.QueryString["Article"];
            if (Article.Length > 6)
            {

                string pdfpath = "W:\\test\\Access\\Planos\\" + Article.Substring(0, 6) + "\\" + Article + ".PT" + ".pdf";
                if (File.Exists(pdfpath))
                {

                    //WebClient client = new WebClient();
                    //Byte[] buffer = client.DownloadData(pdfPath);
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-length", buffer.Length.ToString());
                    //Response.BinaryWrite(buffer);
                    // Iframe.Attributes.Add("src", "Planos/" + TextBoxArticle.Text.Substring(0, 6) + "/" + TextBoxArticle.Text + ".PT.pdf");
                    PdftoIMG(pdfpath);
                    Image1.ImageUrl = "~/Images/UIDimage/New.jpg";
                }

                else
                {
                    //Iframe.Attributes.Add("src", "Planos/draft.pdf");
                    Image1.ImageUrl = "~/Images/draft.jpg";

                }
            }
        }


        //void PdftoIMG(string pdfpath)
        //{

        //    try
        //    {
        //        // string pdfFileName = Server.MapPath(pdfpath);
        //        string pdfFileName = pdfpath;

        //        // Create an instance of PQScan.PDFToImage.PDFDocument object.
        //        PDFDocument pdfDoc = new PDFDocument();

        //        // Load PDF document.
        //        pdfDoc.LoadPDF(pdfFileName);

        //        // Prepare response.
        //        //Response.Clear();
        //        //Response.ContentType = "image/jpeg";

        //        // Render first page of the document to the output image.
        //        System.Drawing.Image jpgImage = pdfDoc.ToImage(0);

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            // Save image to jpg format.
        //            jpgImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            // Show jpg image to the aspx web page.
        //            //Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);

        //            Bitmap bmp = (Bitmap)System.Drawing.Image.FromStream(ms);
        //            bmp = new Bitmap(bmp); // make a copy of it

        //            ms.Close();


        //            // ...



        //            if (File.Exists(Server.MapPath("~/Images/UIDimage/New.jpg")))
        //            {
        //                //File.Delete(HttpContext.Current.Server.MapPath("~/Images/UIDimage/New.jpg"));

        //                bmp.Save(Server.MapPath("~/Images/UIDimage/New.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
        //            }
        //            else
        //            {
        //                bmp.Save(Server.MapPath("~/Images/UIDimage/New.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // File.Delete(HttpContext.Current.Server.MapPath("~/Images/UIDimage/New.jpg"));
        //        Response.Write(ex.ToString());

        //    }
        //}







        void PdftoIMG(string pdfpath)
        {
            Spire.Pdf.PdfDocument pdfdocument = new Spire.Pdf.PdfDocument();
            pdfdocument.LoadFromFile(pdfpath);
            //for (int i = 0; i < pdfdocument.Pages.Count; i++)
            //{
            System.Drawing.Image image = pdfdocument.SaveAsImage(0, 96, 96);
            image.Save(string.Format(Server.MapPath("~/Images/UIDimage/New.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg));
            //}  

        }


        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    //IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

        //void pdftest()
        //{
        //     try
        //     {

        //         string Article = Request.QueryString["Article"];
        //         if (Article.Length > 6)
        //         {

        //             string pdfPath = "W:\\test\\Access\\Planos\\" + Article.Substring(0, 6) + "\\" + Article + ".PC.pdf";
        //             //string path = Server.MapPath(pdfPath);
        //             WebClient client = new WebClient();
        //             Byte[] buffer = client.DownloadData(pdfPath);
        //             if (buffer != null)
        //             {
        //                 Response.ContentType = "application/pdf";
        //                 Response.AddHeader("content-length", buffer.Length.ToString());
        //                 Response.BinaryWrite(buffer);
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Response.Write(ex.ToString());
        //     } 
        // }

        protected void Button3_Click1(object sender, EventArgs e)
        {

            string UID = Request.QueryString["UID"];
            PdfDocument doc = new PdfDocument();
            PdfSection section = doc.Sections.Add();
            PdfPageBase page = doc.Pages.Add();
            Spire.Pdf.Graphics.PdfImage image = Spire.Pdf.Graphics.PdfImage.FromFile(Server.MapPath("~/Images/UIDimage/New.jpg"));


            float widthFitRate = image.PhysicalDimension.Width / page.Canvas.ClientSize.Width;
            float heightFitRate = image.PhysicalDimension.Height / page.Canvas.ClientSize.Height;
            float fitRate = Math.Max(widthFitRate, heightFitRate);
            float fitWidth = image.PhysicalDimension.Width / fitRate;
            float fitHeight = image.PhysicalDimension.Height / fitRate;

            //page.Canvas.DrawImage(image, 0, 0, fitWidth, fitHeight);
            page.Canvas.DrawImage(image, -6, 0, fitWidth + 20, fitHeight + 70);

            doc.SaveToFile(Server.MapPath("~/Images/New.pdf"));
            doc.Close();
            string strURL = "~/Images/New.pdf";
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            //response.AddHeader(" Content-Disposition", "attachment;filename=\"" + Server.MapPath(strURL) + "\"");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + UID + ".pdf");
            byte[] data = req.DownloadData(Server.MapPath(strURL));
            response.BinaryWrite(data);
            response.End();

        }



    }
}