using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Email_Verification
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async void btnValidateEmails_Click(object sender, EventArgs e)
        {
            lstResults.Items.Clear();
            var emails = txtEmails.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var email in emails)
            {
                string trimmedEmail = email.Trim();
                if (IsValidEmail(trimmedEmail))
                {
                    string status = await VerifyEmail(trimmedEmail);
                    lstResults.Items.Add($"{trimmedEmail} - {status}");
                }
                else
                {
                    lstResults.Items.Add($"{trimmedEmail} - Ge�ersiz Format");
                }
            }
        }

        private async Task<string> VerifyEmail(string email)
        {
            string apiKey = "1Yo2ylyxWBAQwauOOMPY1";  // API anahtar�n�z� buraya ekleyin
            string url = $"https://apps.emaillistverify.com/api/verifyEmail?secret={apiKey}&email={email}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Yan�t� kontrol et ve uygun mesaj� d�nd�r
                    switch (responseBody.ToLower())  // Metni k���k harfe �evirerek kontrol edelim
                    {
                        case "ok":
                            return "Ge�erli ve Ger�ekte Var: Adres t�m testleri ge�ti.";
                        case "email_disabled":
                            return "Ge�ersiz veya Bulunamad�: Adres bir veya daha fazla testi ge�emedi.";
                        default:
                            return $"Bilinmeyen Hata: Yan�t tan�nm�yor. Gelen yan�t: {responseBody}";
                    }
                }
                else
                {
                    return "API iste�i ba�ar�s�z oldu";
                }
            }
        }

        private void btnLoadFromExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Dosyalar�|*.xlsx;*.xls";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var emailList = ReadEmailsFromExcel(openFileDialog.FileName);
                    txtEmails.Text = string.Join(Environment.NewLine, emailList);
                }
            }
        }

        private List<string> ReadEmailsFromExcel(string filePath)
        {
            List<string> emails = new List<string>();
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        var email = reader.GetString(0);
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            emails.Add(email);
                        }
                    }
                }
            }
            return emails;
        }

        private void btnCopyResults_Click(object sender, EventArgs e)
        {
            
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Dosyas�|*.xlsx";
                saveFileDialog.Title = "Sonu�lar� Excel Olarak Kaydet";
                saveFileDialog.FileName = "EmailVerificationResults.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        
                        var worksheet = package.Workbook.Worksheets.Add("Sonu�lar");

                        
                        worksheet.Cells[1, 1].Value = "Email";
                        worksheet.Cells[1, 2].Value = "Durum";

                       
                        for (int i = 0; i < lstResults.Items.Count; i++)
                        {
                            var item = lstResults.Items[i].ToString().Split(" - ");
                            if (item.Length == 2)
                            {
                                worksheet.Cells[i + 2, 1].Value = item[0]; 
                                worksheet.Cells[i + 2, 2].Value = item[1]; 
                            }
                        }

                        // Kolon geni�liklerini ayarla
                        worksheet.Column(1).Width = 50; 
                        worksheet.Column(2).Width = 50; 

                       
                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fi);
                    }

                    MessageBox.Show("Sonu�lar Excel'e aktar�ld�!", "Kay�t Ba�ar�l�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


    }
}
