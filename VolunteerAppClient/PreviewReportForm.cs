using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolunteerAppClient
{
    public partial class PreviewReportForm : Form
    {
        private string CurrentReport;
        public PreviewReportForm(string path)
        {
            InitializeComponent();

            CurrentReport = path;
            this.Text = this.Text.Replace("[file]", new FileInfo(path).Name);
            ReportViewerBrowser.Url = new Uri(string.Format("file:///{0}", path));
        }

        private void PrintReport()
        {
            PrinterDialogBox.Document = DocumentPrinter;
            if (PrinterDialogBox.ShowDialog() == DialogResult.OK)
            {
                DocumentPrinter.PrinterSettings = PrinterDialogBox.PrinterSettings;
                DocumentPrinter.Print();
            }
        }

        private void ClosePreviewButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            ReportViewerBrowser.ShowPrintDialog();
        }

        private void DocumentPrinter_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float linesPerPage = 0;
            float yPosition = 0;
            int count = 0;
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;
            string line = null;
            Font printFont = this.Font;
            SolidBrush myBrush = new SolidBrush(Color.Black);
            linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics);
            StreamReader myReader = new StreamReader(CurrentReport);


            while (count < linesPerPage && ((line = myReader.ReadLine()) != null))
            {
                yPosition = topMargin + (count * printFont.GetHeight(e.Graphics));
                e.Graphics.DrawString(line, printFont, myBrush, leftMargin, yPosition, new StringFormat());
                count++;
            }
            if (line != null)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
            myBrush.Dispose();
        }
    }
}
