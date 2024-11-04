namespace Email_Verification
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtEmails;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.Button btnValidateEmails;
        private System.Windows.Forms.Button btnLoadFromExcel;
        private System.Windows.Forms.Button btnCopyResults; // Yeni buton tanımı

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtEmails = new TextBox();
            lstResults = new ListBox();
            btnValidateEmails = new Button();
            btnLoadFromExcel = new Button();
            btnCopyResults = new Button();
            SuspendLayout();
            // 
            // txtEmails
            // 
            txtEmails.Location = new Point(14, 14);
            txtEmails.Margin = new Padding(4, 3, 4, 3);
            txtEmails.Multiline = true;
            txtEmails.Name = "txtEmails";
            txtEmails.Size = new Size(493, 115);
            txtEmails.TabIndex = 0;
            // 
            // lstResults
            // 
            lstResults.FormattingEnabled = true;
            lstResults.ItemHeight = 15;
            lstResults.Location = new Point(14, 173);
            lstResults.Margin = new Padding(4, 3, 4, 3);
            lstResults.Name = "lstResults";
            lstResults.Size = new Size(493, 184);
            lstResults.TabIndex = 1;
            // 
            // btnValidateEmails
            // 
            btnValidateEmails.Location = new Point(14, 138);
            btnValidateEmails.Margin = new Padding(4, 3, 4, 3);
            btnValidateEmails.Name = "btnValidateEmails";
            btnValidateEmails.Size = new Size(140, 27);
            btnValidateEmails.TabIndex = 2;
            btnValidateEmails.Text = "E-postaları Doğrula";
            btnValidateEmails.UseVisualStyleBackColor = true;
            btnValidateEmails.Click += btnValidateEmails_Click;
            // 
            // btnLoadFromExcel
            // 
            btnLoadFromExcel.Location = new Point(193, 138);
            btnLoadFromExcel.Margin = new Padding(4, 3, 4, 3);
            btnLoadFromExcel.Name = "btnLoadFromExcel";
            btnLoadFromExcel.Size = new Size(140, 27);
            btnLoadFromExcel.TabIndex = 3;
            btnLoadFromExcel.Text = "Excel'den Yükle";
            btnLoadFromExcel.UseVisualStyleBackColor = true;
            btnLoadFromExcel.Click += btnLoadFromExcel_Click;
            // 
            // btnCopyResults
            // 
            btnCopyResults.Location = new Point(367, 138);
            btnCopyResults.Margin = new Padding(4, 3, 4, 3);
            btnCopyResults.Name = "btnCopyResults";
            btnCopyResults.Size = new Size(140, 27);
            btnCopyResults.TabIndex = 4;
            btnCopyResults.Text = "Sonuçları Excel'e Aktar ";
            btnCopyResults.UseVisualStyleBackColor = true;
            btnCopyResults.Click += btnCopyResults_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(530, 370);
            Controls.Add(btnCopyResults);
            Controls.Add(btnLoadFromExcel);
            Controls.Add(btnValidateEmails);
            Controls.Add(lstResults);
            Controls.Add(txtEmails);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Email Verification";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
