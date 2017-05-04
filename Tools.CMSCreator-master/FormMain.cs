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
            if (radioButtonSign.Enabled)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
                    foreach (string fileLoc in filePaths)
                    {
                        try
                        {
                            #region
                            if (radioButtonSign.Enabled)
                            {
                                // Create a UnicodeEncoder to convert between byte array and string.
                                ASCIIEncoding ByteConverter = new ASCIIEncoding();

                                string dataString;

                                // Create byte arrays to hold original, encrypted, and decrypted data.
                                byte[] originalData = ReadFile(fileLoc);
                                byte[] signedData;

                                // Create a new instance of the RSACryptoServiceProvider class 
                                // and automatically create a new key-pair.
                                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                                // Export the key information to an RSAParameters object.
                                // You must pass true to export the private key for signing.
                                // However, you do not need to export the private key
                                // for verification.
                                RSAParameters Key = RSAalg.ExportParameters(true);
                                XmlDocument XMLdoc = new XmlDocument();

                                FileStream fileXml = new FileStream(Directory.GetCurrentDirectory() + "/keys" + i++ + ".xml", FileMode.Create);

                                using (StreamWriter writerf = new StreamWriter(fileXml, Encoding.UTF8))
                                {
                                    writerf.Write(RSAalg.ToXmlString(true));
                                }


                                fileXml.Close();


                                // Hash and sign the data.
                                signedData = HashAndSignBytes(originalData, Key);

                                WriteFile(Directory.GetCurrentDirectory() + "/signData" + i + ".dat", signedData);

                                // Verify the data and display the result to the 
                                // console.
                                if (VerifySignedHash(originalData, signedData, Key))
                                {
                                    Console.WriteLine("The data was verified.");
                                    MessageBox.Show(string.Format("File {0} alredy signed (sign).", fileLoc));
                                }
                                else
                                {
                                    Console.WriteLine("The data does not match the signature.");
                                }
                                #endregion


                            }
                            else
                            {
                                #region

                                byte[] signData = ReadFile(fileLoc);
                                using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
                                {
                                    Stream myStream;
                                    openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
                                    openFileDialog1.Filter = "(*.xml)|*.xml|All files (*.*)|*.*";
                                    openFileDialog1.FilterIndex = 2;
                                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                                    {
                                        try
                                        {
                                            if ((myStream = openFileDialog1.OpenFile()) != null)
                                            {
                                                using (myStream)
                                                {
                                                    RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                                                    RSAalg.FromXmlString(System.IO.File.ReadAllText(openFileDialog1.FileName));
                                                    RSAParameters Key = RSAalg.ExportParameters(true);
                                                    if (VerifySignedHash(originalData, signData, Key))
                                                    {
                                                        Console.WriteLine("The data was verified.");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("The data does not match the signature.");
                                                    }

                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Error: Could not read file from disk: " + ex.Message);
                                        }
                                    }
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
        }
    }
}
