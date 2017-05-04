using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;


namespace Tools.CMSCreator
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            string storeName = ConfigurationManager.AppSettings.Get("storeName");
            StoreLocation storeLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), ConfigurationManager.AppSettings.Get("storeLocation"));
            X509FindType x509FindType = (X509FindType)Enum.Parse(typeof(X509FindType), ConfigurationManager.AppSettings.Get("x509FindType"));
            string findValue = ConfigurationManager.AppSettings.Get("findValue");

           
        }

       

        

        /// <summary>
        /// Создать объект хранилища
        /// </summary>
      

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            if (radioButtonSign.Enabled) //Подпись
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                    foreach (string fileLoc in filePaths)
                    {
                        try
                        {
                            #region
                            if (radioButtonSign.Checked)
                            {
                                // Конвертер в юникод
                                ASCIIEncoding ByteConverter = new ASCIIEncoding();

                                

                                // Create byte arrays to hold original, encrypted, and decrypted data.
                                byte[] originalData = ReadFile(fileLoc);
                                byte[] signedData;

                                // яснопонятно
                                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                                //Экспортирование параметров в xml файл
                                RSAParameters Key = RSAalg.ExportParameters(true);
                                

                                FileStream fileXml = new FileStream(Directory.GetCurrentDirectory() + "/keys" + i + ".xml", FileMode.Create);

                                using (StreamWriter writerf = new StreamWriter(fileXml, Encoding.UTF8))
                                {
                                    writerf.Write(RSAalg.ToXmlString(true));
                                }
                                  fileXml.Close();


                                // Хеширем и подписываем
                                signedData = HashAndSignBytes(originalData, Key);

                                WriteFile(Directory.GetCurrentDirectory() + "/signData" + i++ + ".dat", signedData);

                                // Verify the data and display the result to the 
                                // console.
                                if (VerifySignedHash(originalData, signedData, Key))
                                {
                                    Console.WriteLine("The data was verified.");
                                    MessageBox.Show(string.Format("File {0} alredy signed (sign).", fileLoc));
                                }
                                else
                                {
                                    MessageBox.Show("The data does not match the signature.");
                                }
                                #endregion


                            }
                            
                        }



                        catch (ArgumentNullException)
                        {
                            Console.WriteLine("The data was not signed or verified");

                        }

                    }

                }
            }
            if (radioButtonCheck.Checked) //Проверка
            {
                #region 
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    byte[] signData = null;
                    byte[] originalData = null;

                    RSACryptoServiceProvider AlgRSA = new RSACryptoServiceProvider();
                    RSAParameters Key = AlgRSA.ExportParameters(true);

                    

                    string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                    foreach (string fileLoc in filePaths)
                    {
                        if (fileLoc.EndsWith("xml"))
                        {
                            FileStream fileXml = new FileStream(fileLoc, FileMode.Open);
                            using (StreamReader readerxml = new StreamReader(fileXml))
                            {
                                AlgRSA.FromXmlString(readerxml.ReadToEnd()); //считываем ключи
                                Key = AlgRSA.ExportParameters(true);
                            }

                        }

                        if (!(fileLoc.EndsWith("xml") || fileLoc.EndsWith("dat")))
                        {
                            originalData = ReadFile(fileLoc);

                        }

                        if (fileLoc.EndsWith("dat"))
                        {
                            signData = ReadFile(fileLoc);
                        }

                    }

                    if (VerifySignedHash(originalData, signData, Key))
                    {
                        MessageBox.Show("The sign is verified.");
                    }
                    else
                    {
                        MessageBox.Show("The sign does not match original data with this keys.");
                    }

                    #endregion
                }
            }
        }
                
            
        

        private byte[] ReadFile(string path)
        {
            byte[] content = null;
            try
            {
                content = File.ReadAllBytes(path);
            }
            catch (IOException e)
            {
                MessageBox.Show(
                    string.Format("Не удалось прочитать файл {0}\n\n{1}", path, e.Message),
                    "Ошибка открытия файла");
            }
            return content;
        }

        private void WriteFile(string path, byte[] fileContent)
        {
            try
            {
                File.WriteAllBytes(path, fileContent);
            }
            catch (IOException e)
            {
                MessageBox.Show(
                    string.Format("Не удалось записать данные в файл {0}\n\n{1}", path, e.Message),
                    "Ошибка записи в файл");
            }
        }

        

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Создание экземпляра алгоритма с нужными ключами
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Хеширование и подпись через алгоритм SHA1
                return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                MessageBox.Show(
                    string.Format("", e.Message),
                    "Ошибка создания подписи");

                return null;
            }
        }

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                // Создание экземпляра алгоритма с нужными ключами
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Проверка подлинности подписи
                return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

            }
            catch (CryptographicException e)
            {
                MessageBox.Show(
                    string.Format("", e.Message),
                    "Ошибка проверки подписи");

                return false;
            }
            catch (Exception)
            { MessageBox.Show("Не все файлы приложены для проверки");
                return false; }
        }

      /*  private void RadioButtonCheck_CheckedChanged(object sender, EventArgs e) //
        {
            if (radioButtonCheck.Checked)
            {
                labelTXT.Enabled = true;
                textBox1.Enabled = true;

            }
            else
            {
                labelTXT.Enabled = false;
                textBox1.Enabled = false;
            }
        }*/ 

        
    }
}
