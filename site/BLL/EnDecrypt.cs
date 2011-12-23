using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BLL
{
    public class EnDecrypt
    {
		private byte[] Defaultkey = Encoding.ASCII.GetBytes("603deb1015ca71be2b73aef0857d7781");
        private byte[] DefaultIV = Encoding.ASCII.GetBytes("0001020304050607");

        public byte[] Key { get; set; }
        public byte[] IV { get; set; }

        public EnDecrypt(byte[] key, byte[] iv)
        {
            Key = key;
            IV = iv;
        }

        public EnDecrypt()
        {
            Key = Defaultkey;
            IV = DefaultIV;
        }

        public byte[] EncryptToByteArray(string plainStr)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = 256;
            aesEncryption.BlockSize = 256;
            aesEncryption.Mode = CipherMode.CFB;
            aesEncryption.Padding = PaddingMode.Zeros;
            aesEncryption.IV = this.IV;
            aesEncryption.Key = this.Key;

            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(plainStr);

            ICryptoTransform crypto = aesEncryption.CreateEncryptor();

            // The result of the encryption and decryption
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);

            return cipherText;
        }
        public string EncryptToString(string plainStr)
        {
            return Convert.ToBase64String(EncryptToByteArray(plainStr));
        }

        public string DecryptFromByteArray(byte[] encryptedByteArray)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = 256;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CFB;
            aesEncryption.Padding = PaddingMode.Zeros;
            aesEncryption.IV = IV;
            aesEncryption.Key = this.Key;

            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
            return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedByteArray, 0, encryptedByteArray.Length));
        }

        public string Decrypt(string encryptedText)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = 256;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CFB;
            aesEncryption.Padding = PaddingMode.Zeros;
            aesEncryption.IV = IV;
            aesEncryption.Key = this.Key;

            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64CharArray(encryptedText.ToCharArray(), 0, encryptedText.Length);

            return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        }
    }
}
