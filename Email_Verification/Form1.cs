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
                    lstResults.Items.Add($"{trimmedEmail} - Geçersiz Format");
                }
            }
        }

        private async Task<string> VerifyEmail(string email)
        {
            string apiKey = "1Yo2ylyxWBAQwauOOMPY1";  // API anahtarýnýzý buraya ekleyin
            string url = $"https://apps.emaillistverify.com/api/verifyEmail?secret={apiKey}&email={email}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Yanýtý kontrol et ve uygun mesajý döndür
                    switch (responseBody.ToLower())  // Metni küçük harfe çevirerek kontrol edelim
                    {
                        case "ok":
                            return "Geçerli ve Gerçekte Var: Adres tüm testleri geçti.";
                        case "email_disabled":
                            return "Geçersiz veya Bulunamadý: Adres bir veya daha fazla testi geçemedi.";
                        default:
                            return $"Bilinmeyen Hata: Yanýt tanýnmýyor. Gelen yanýt: {responseBody}";
                    }
                }
                else
                {
                    return "API isteði baþarýsýz oldu";
                }
            }
        }

        private void btnLoadFromExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Dosyalarý|*.xlsx;*.xls";
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
                saveFileDialog.Filter = "Excel Dosyasý|*.xlsx";
                saveFileDialog.Title = "Sonuçlarý Excel Olarak Kaydet";
                saveFileDialog.FileName = "EmailVerificationResults.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        
                        var worksheet = package.Workbook.Worksheets.Add("Sonuçlar");

                        
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

                        // Kolon geniþliklerini ayarla
                        worksheet.Column(1).Width = 50; 
                        worksheet.Column(2).Width = 50; 

                       
                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fi);
                    }

                    MessageBox.Show("Sonuçlar Excel'e aktarýldý!", "Kayýt Baþarýlý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


    }
}
