using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Utils
{
    public class Crypto
    {
        private string kPassword;
       
        
        // Generate a new key pair
        /// <summary>
        ///Generate Key pair and write those to seperate files
        /// </summary>
        /// <param name="publicKeyFileName"></param>
        /// <param name="privateKeyFileName"></param>
       
        // Encrypt a file
        /// <summary>
        /// Encrypt a file with private key
        /// </summary>
        /// <param name="publicKeyFileName"></param>
        /// <param name="plainFileName"></param>
        /// <param name="encryptedFileName"></param>
       
        // Decrypt a file
        /// <summary>
        /// Decrypt a file with a public key
        /// </summary>
        /// <param name="privateKeyFileName"></param>
        /// <param name="encryptedFileName"></param>
        /// <param name="plainFileName"></param>
        public static string Decrypt(string privateKeyFileName, string encryptedFileName)
        {
            // Variables
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamReader privateKeyFile = null;
            FileStream encryptedFile = null;
            StreamWriter plainFile = null;
            string privateKeyText = "";
            string plainText = "";
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            try
            {
                // Select target CSP
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL 
                //cspParams.ProviderName; // CSP name
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                // Read private/public key pair from file
                privateKeyFile = File.OpenText(privateKeyFileName);
                privateKeyText = privateKeyFile.ReadToEnd();

                // Import private/public key pair
                rsaProvider.FromXmlString(privateKeyText);

                // Read encrypted text from file
                encryptedFile = File.OpenRead(encryptedFileName);
                encryptedBytes = new byte[encryptedFile.Length];
                encryptedFile.Read(encryptedBytes, 0, (int)encryptedFile.Length);

                // Decrypt text
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                // Write decrypted text to file
                //plainFile = File.CreateText(plainFileName);
                plainText = Encoding.Unicode.GetString(plainBytes);
                
            }
            catch (Exception ex)
            {
                // Any errors? Show them
                Console.WriteLine("Exception decrypting file! More info:");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Do some clean up if needed
                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
                if (encryptedFile != null)
                {
                    encryptedFile.Close();
                }
                if (plainFile != null)
                {
                    plainFile.Close();
                }
            }
            return plainText;

        } // Decrypt
    }
}
