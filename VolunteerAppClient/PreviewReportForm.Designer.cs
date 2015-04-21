namespace VolunteerAppClient
{
    partial class PreviewReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ReportViewerBrowser = new System.Windows.Forms.WebBrowser();
            this.PrintButton = new System.Windows.Forms.Button();
            this.ClosePreviewButton = new System.Windows.Forms.Button();
            this.DocumentPrinter = new System.Drawing.Printing.PrintDocument();
            this.PrinterDialogBox = new System.Windows.Forms.PrintDialog();
            this.SuspendLayout();
            // 
            // ReportViewerBrowser
            // 
            this.ReportViewerBrowser.AllowNavigation = false;
            this.ReportViewerBrowser.AllowWebBrowserDrop = false;
            this.ReportViewerBrowser.IsWebBrowserContextMenuEnabled = false;
            this.ReportViewerBrowser.Location = new System.Drawing.Point(0, 0);
            this.ReportViewerBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.ReportViewerBrowser.Name = "ReportViewerBrowser";
            this.ReportViewerBrowser.Size = new System.Drawing.Size(874, 733);
            this.ReportViewerBrowser.TabIndex = 0;
            this.ReportViewerBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // PrintButton
            // 
            this.PrintButton.BackColor = System.Drawing.Color.PaleGreen;
            this.PrintButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintButton.Location = new System.Drawing.Point(888, 50);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(100, 40);
            this.PrintButton.TabIndex = 1;
            this.PrintButton.Text = "Print";
            this.PrintButton.UseVisualStyleBackColor = false;
            this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
            // 
            // ClosePreviewButton
            // 
            this.ClosePreviewButton.BackColor = System.Drawing.Color.Salmon;
            this.ClosePreviewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClosePreviewButton.Location = new System.Drawing.Point(888, 120);
            this.ClosePreviewButton.Name = "ClosePreviewButton";
            this.ClosePreviewButton.Size = new System.Drawing.Size(100, 40);
            this.ClosePreviewButton.TabIndex = 2;
            this.ClosePreviewButton.Text = "Close";
            this.ClosePreviewButton.UseVisualStyleBackColor = false;
            this.ClosePreviewButton.Click += new System.EventHandler(this.ClosePreviewButton_Click);
            // 
            // DocumentPrinter
            // 
            this.DocumentPrinter.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.DocumentPrinter_PrintPage);
            // 
            // PrinterDialogBox
            // 
            this.PrinterDialogBox.UseEXDialog = true;
            // 
            // PreviewReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 734);
            this.Controls.Add(this.ClosePreviewButton);
            this.Controls.Add(this.PrintButton);
            this.Controls.Add(this.ReportViewerBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "PreviewReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Preview : [file]";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser ReportViewerBrowser;
        private System.Windows.Forms.Button PrintButton;
        private System.Windows.Forms.Button ClosePreviewButton;
        private System.Drawing.Printing.PrintDocument DocumentPrinter;
        private System.Windows.Forms.PrintDialog PrinterDialogBox;
    }
}