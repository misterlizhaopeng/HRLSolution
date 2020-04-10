using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace HRL.Common.Utlity
{
    public enum AESBits
    {
        BITS128,
        BITS192,
        BITS256
    }


    public class AESEncryptor
    {
        private string fPassword;
        private AESBits fEncryptionBits;
        private byte[] fSalt = new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 };
        /// <summary>
        /// Initialize new AESEncryptor.
        /// </summary>
        /// <param name="password">The password to use for encryption/decryption.</param>
        /// <param name="encryptionBits">Encryption bits (128,192,256).</param>
        public AESEncryptor(string password, AESBits encryptionBits)
        {
            fPassword = password;
            fEncryptionBits = encryptionBits;
        }
        /// <summary>
        /// Initialize new AESEncryptor.
        /// </summary>
        /// <param name="password">The password to use for encryption/decryption.</param>
        /// <param name="encryptionBits">Encryption bits (128,192,256).</param>
        /// <param name="salt">Salt bytes. Bytes length must be 15.</param>
        public AESEncryptor(string password, AESBits encryptionBits, byte[] salt)
        {
            fPassword = password;
            fEncryptionBits = encryptionBits;
            fSalt = salt;
        }
        private byte[] iEncrypt(byte[] data, byte[] key, byte[] iV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }
        /// <summary>
        /// Encrypt string with AES algorith.
        /// </summary>
        /// <param name="data">String to encrypt.</param>
        public string Encrypt(string data)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(data);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            switch (fEncryptionBits)
            {
                case AESBits.BITS128:
                    return Convert.ToBase64String(iEncrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16)));
                case AESBits.BITS192:
                    return Convert.ToBase64String(iEncrypt(clearBytes, pdb.GetBytes(24), pdb.GetBytes(16)));
                case AESBits.BITS256:
                    return Convert.ToBase64String(iEncrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16)));
            }
            return null;
        }
        /// <summary>
        /// Encrypt byte array with AES algorithm.
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        public byte[] Encrypt(byte[] data)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            switch (fEncryptionBits)
            {
                case AESBits.BITS128:
                    return iEncrypt(data, pdb.GetBytes(16), pdb.GetBytes(16));
                case AESBits.BITS192:
                    return iEncrypt(data, pdb.GetBytes(24), pdb.GetBytes(16));
                case AESBits.BITS256:
                    return iEncrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));
            }
            return null;
        }
        private byte[] iDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }
        /// <summary>
        /// Decrypt string with AES algorithm.
        /// </summary>
        /// <param name="data">Encrypted string.</param>
        public string Decrypt(string data)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(data);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            switch (fEncryptionBits)
            {
                case AESBits.BITS128:
                    return System.Text.Encoding.Unicode.GetString(iDecrypt(dataToDecrypt, pdb.GetBytes(16), pdb.GetBytes(16)));
                case AESBits.BITS192:
                    return System.Text.Encoding.Unicode.GetString(iDecrypt(dataToDecrypt, pdb.GetBytes(24), pdb.GetBytes(16)));
                case AESBits.BITS256:
                    return System.Text.Encoding.Unicode.GetString(iDecrypt(dataToDecrypt, pdb.GetBytes(32), pdb.GetBytes(16)));
            }
            return null;
        }
        /// <summary>
        /// Decrypt byte array with AES algorithm.
        /// </summary>
        /// <param name="data">Encrypted byte array.</param>
        public byte[] Decrypt(byte[] data)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            switch (fEncryptionBits)
            {
                case AESBits.BITS128:
                    return iDecrypt(data, pdb.GetBytes(16), pdb.GetBytes(16));
                case AESBits.BITS192:
                    return iDecrypt(data, pdb.GetBytes(24), pdb.GetBytes(16));
                case AESBits.BITS256:
                    return iDecrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));
            }
            return null;
        }
        /// <summary>
        /// Encryption/Decryption password.
        /// </summary>
        public string Password
        {
            get { return fPassword; }
            set { fPassword = value; }
        }
        /// <summary>
        /// Encryption/Decryption bits.
        /// </summary>
        public AESBits EncryptionBits
        {
            get { return fEncryptionBits; }
            set { fEncryptionBits = value; }
        }
        /// <summary>
        /// Salt bytes (bytes length must be 15).
        /// </summary>
        public byte[] Salt
        {
            get { return fSalt; }
            set { fSalt = value; }
        }
    }


    public class MD5Encryptor
    {
        public MD5Encryptor()
        {
        }
        public string GetMD5(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(data));
        }
        public string GetMD5(string data)
        {
            return GetMD5(ASCIIEncoding.Default.GetBytes(data));
        }
    }




    public class TripleDESEncryptor
    {
        private string fPassword;
        private byte[] fSalt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        /// <summary>
        /// Initialize new TripleDESEncryptor.
        /// </summary>
        /// <param name="password">The password to use for encryption/decryption.</param>
        public TripleDESEncryptor(string password)
        {
            fPassword = password;
        }
        /// <summary>
        /// Initialize new TripleDESEncryptor.
        /// </summary>
        /// <param name="password">The password to use for encryption/decryption.</param>
        /// <param name="salt">Salt bytes. Bytes length must be 13.</param>
        public TripleDESEncryptor(string password, byte[] salt)
        {
            fPassword = password;
            fSalt = salt;
        }
        private byte[] iEncrypt(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            TripleDES alg = TripleDES.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// Encrypt string with TripleDES algorith.
        /// </summary>
        /// <param name="data">String to encrypt.</param>
        /// <returns>Encrypted string.</returns>
        public string Encrypt(string data)
        {
            byte[] dataToEncrypt = System.Text.Encoding.Unicode.GetBytes(data);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            return Convert.ToBase64String(iEncrypt(dataToEncrypt, pdb.GetBytes(16), pdb.GetBytes(8)));
        }
        /// <summary>
        /// Encrypt byte array with TripleDES algorithm.
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        /// <returns>Encrypted bytes.</returns>
        public byte[] Encrypt(byte[] data)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            return iEncrypt(data, pdb.GetBytes(16), pdb.GetBytes(8));
        }
        private byte[] iDecrypt(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream ms = new MemoryStream();
            TripleDES alg = TripleDES.Create();
            alg.Key = key;
            alg.IV = iv;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// Decrypt string with TripleDES algorithm.
        /// </summary>
        /// <param name="data">Encrypted string.</param>
        /// <returns>Decrypted string.</returns>
        public string Decrypt(string data)
        {
            byte[] cipherBytes = Convert.FromBase64String(data);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            return System.Text.Encoding.Unicode.GetString(iDecrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(8)));
        }
        /// <summary>
        /// Decrypt byte array with TripleDES algorithm.
        /// </summary>
        /// <param name="data">Encrypted byte array.</param>
        /// <returns>Decrypted byte array.</returns>
        public byte[] Decrypt(byte[] data)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(fPassword, fSalt);
            return iDecrypt(data, pdb.GetBytes(16), pdb.GetBytes(8));
        }
        /// <summary>
        /// Encryption/Decryption password.
        /// </summary>
        public string Password
        {
            get { return fPassword; }
            set { fPassword = value; }
        }
        /// <summary>
        /// Salt bytes (bytes length must be 15).
        /// </summary>
        public byte[] Salt
        {
            get { return fSalt; }
            set { fSalt = value; }
        }
    }
}
