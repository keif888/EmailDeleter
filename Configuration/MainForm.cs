using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Configuration
{
    public partial class FormMain : Form
    {

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnKeyGen_Click(object sender, EventArgs e)
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                tbKeyBase64.Text = Convert.ToBase64String(aes.Key);
                tbKey.Text = System.Text.Encoding.ASCII.GetString(aes.Key);
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            byte[] value = System.Text.Encoding.UTF8.GetBytes(tbValue.Text);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(tbKeyBase64.Text);
                aes.GenerateIV();
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(value, 0, value.Length);
                    }
                    tbResult.Text = Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private void tbKey_Leave(object sender, EventArgs e)
        {
            tbKeyBase64.Text = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tbKey.Text));
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            byte[] value = Convert.FromBase64String(tbValue.Text);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(tbKeyBase64.Text);
                byte[] iv = new byte[aes.IV.Length];
                int numBytesToRead = aes.IV.Length, encryptLength = value.Length - aes.IV.Length;
                Array.Copy(value, 0, iv, 0, numBytesToRead);
                aes.IV = iv;
                byte[] encryptedBytes = new byte[encryptLength];
                Array.Copy(value, numBytesToRead, encryptedBytes, 0, encryptLength);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    tbResult.Text = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
