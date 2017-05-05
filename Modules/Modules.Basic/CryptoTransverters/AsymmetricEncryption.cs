using Modules.Basic.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Basic.CryptoTransverters
{
    /// <summary>
    /// 非对称加密
    /// </summary>
    public class AsymmetricEncryption : IEncrypt, IDecrypt
    {
        public static AsymmetricEncryption Default { get; set; }
        static AsymmetricEncryption()
        {
            string publicRSAKey = ConfigurationManager.AppSettings["AsymmetricEncryptionPublicKey"];
            string privateRSAKey = ConfigurationManager.AppSettings["AsymmetricEncryptionPrivateKey"];
            Default = new AsymmetricEncryption(publicRSAKey, privateRSAKey);
        }

        protected HashAlgorithm HA { get; set; }
        protected RSACryptoServiceProvider RSACSP { get; set; }
        protected int DecryptCount { get; set; }
        protected int EncryptCount { get; set; }

        public AsymmetricEncryption()
        {
            HA = GetHashAlgorithm();
            RSACSP = GetRSACryptoServiceProvider();
            DecryptCount = RSACSP.KeySize / 8;
            EncryptCount = DecryptCount - 11;
        }

        public AsymmetricEncryption(string publicKeyBlobString, string privateKeyBlobString)
        {
            HA = GetHashAlgorithm();
            RSACSP = GetRSACryptoServiceProvider();
            byte[] publicKeyBlob = GetBlob(publicKeyBlobString);
            byte[] privateKeyBlob = GetBlob(privateKeyBlobString);
            RSACSP.ImportCspBlob(publicKeyBlob);
            RSACSP.ImportCspBlob(privateKeyBlob);
            DecryptCount = RSACSP.KeySize / 8;
            EncryptCount = DecryptCount - 11;
        }

        public AsymmetricEncryption(byte[] publicKeyBlob, byte[] privateKeyBlob)
        {
            HA = GetHashAlgorithm();
            RSACSP = GetRSACryptoServiceProvider();
            RSACSP.ImportCspBlob(publicKeyBlob);
            RSACSP.ImportCspBlob(privateKeyBlob);
            DecryptCount = RSACSP.KeySize / 8;
            EncryptCount = DecryptCount - 11;
        }

        protected virtual HashAlgorithm GetHashAlgorithm()
        {
            return SHA256.Create();
        }

        protected virtual RSACryptoServiceProvider GetRSACryptoServiceProvider()
        {
            return new RSACryptoServiceProvider();
        }

        protected virtual byte[] GetBlob(string blob)
        {
            return blob.FromBase64String();
        }


        #region Encrypt
        /// <summary>
        /// 使用RSA进行非对称加密
        /// </summary>
        /// <param name="inputBuffer">待加密的字节数组</param>
        /// <param name="keyBlob">RSA公钥Blob字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] inputBuffer, byte[] keyBlob)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportCspBlob(keyBlob);
                if (inputBuffer.Length <= EncryptCount)
                {
                    return RSA.Encrypt(inputBuffer, false);
                }
                else
                {
                    List<byte> list = new List<byte>();
                    while (inputBuffer.Any())
                    {
                        list.AddRange(RSA.Encrypt(inputBuffer.Take(EncryptCount).ToArray(), false));
                        inputBuffer = inputBuffer.Skip(EncryptCount).ToArray();
                    }
                    return list.ToArray();
                }
            }
        }

        /// <summary>
        /// 使用RSA进行非对称加密
        /// </summary>
        /// <param name="inputBuffer">待加密的字节数组</param>
        /// <param name="keyBlobString">RSA公钥Blob字符串（Base64）</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] inputBuffer, string keyBlobString)
        {
            byte[] keyBlob = keyBlobString.FromBase64String();
            return Encrypt(inputBuffer, keyBlob);
        }

        /// <summary>
        /// 使用RSA进行非对称加密，使用默认公钥
        /// </summary>
        /// <param name="inputBuffer">待加密的字节数组</param>
        /// <returns>加密后的字节数组</returns>
        public byte[] Encrypt(byte[] inputBuffer)
        {
            if (inputBuffer.Length <= EncryptCount)
            {
                return RSACSP.Encrypt(inputBuffer, false);
            }
            else
            {
                List<byte> list = new List<byte>();
                while (inputBuffer.Any())
                {
                    list.AddRange(RSACSP.Encrypt(inputBuffer.Take(EncryptCount).ToArray(), false));
                    inputBuffer = inputBuffer.Skip(EncryptCount).ToArray();
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// 使用RSA进行非对称加密
        /// </summary>
        /// <param name="inString">待加密的字符串（UTF8）</param>
        /// <param name="keyBlob">RSA公钥Blob字节数组</param>
        /// <returns>加密后的字符串（Base64）</returns>
        public string Encrypt(string inString, byte[] keyBlob)
        {
            byte[] inputBuffer = inString.FromUTF8String();
            return Encrypt(inputBuffer, keyBlob).ToBase64String();
        }

        /// <summary>
        /// 使用RSA进行非对称加密
        /// </summary>
        /// <param name="inString">待加密的字符串（UTF8）</param>
        /// <param name="keyBlobString">RSA公钥Blob字符串（Base64）</param>
        /// <returns>加密后的字符串（Base64）</returns>
        public string Encrypt(string inString, string keyBlobString)
        {
            byte[] keyBlob = keyBlobString.FromBase64String();
            return Encrypt(inString, keyBlob);
        }

        /// <summary>
        /// 使用RSA进行非对称加密，使用默认公钥
        /// </summary>
        /// <param name="inString">待加密的字符串（UTF8）</param>
        /// <returns>加密后的字符串（Base64）</returns>
        public string Encrypt(string inString)
        {
            byte[] inputBuffer = inString.FromUTF8String();
            return Encrypt(inputBuffer).ToBase64String();
        }
        #endregion
        #region Decrypt
        /// <summary>
        /// 使用RSA进行非对称解密
        /// </summary>
        /// <param name="inputBuffer">待解密的字节数组</param>
        /// <param name="keyBlob">RSA私钥Blob字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] inputBuffer, byte[] keyBlob)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportCspBlob(keyBlob);
                if (inputBuffer.Length <= DecryptCount)
                {
                    return RSA.Decrypt(inputBuffer, false);
                }
                else
                {
                    List<byte> list = new List<byte>();
                    while (inputBuffer.Any())
                    {
                        list.AddRange(RSA.Decrypt(inputBuffer.Take(DecryptCount).ToArray(), false));
                        inputBuffer = inputBuffer.Skip(DecryptCount).ToArray();
                    }
                    return list.ToArray();
                }
            }
        }

        /// <summary>
        /// 使用RSA进行非对称解密
        /// </summary>
        /// <param name="inputBuffer">待解密的字节数组</param>
        /// <param name="keyBlobString">RSA私钥Blob字符串（Base64）</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] inputBuffer, string keyBlobString)
        {
            byte[] keyBlob = keyBlobString.FromBase64String();
            return Decrypt(inputBuffer, keyBlob);
        }

        /// <summary>
        /// 使用RSA进行非对称解密，使用默认私钥
        /// </summary>
        /// <param name="inputBuffer">待解密的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        public byte[] Decrypt(byte[] inputBuffer)
        {
            if (inputBuffer.Length <= DecryptCount)
            {
                return RSACSP.Decrypt(inputBuffer, false);
            }
            else
            {
                List<byte> list = new List<byte>();
                while (inputBuffer.Any())
                {
                    list.AddRange(RSACSP.Decrypt(inputBuffer.Take(DecryptCount).ToArray(), false));
                    inputBuffer = inputBuffer.Skip(DecryptCount).ToArray();
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// 使用RSA进行非对称解密
        /// </summary>
        /// <param name="inString">待解密的字符串（Base64）</param>
        /// <param name="keyBlob">RSA私钥Blob字节数组</param>
        /// <returns>解密后的字符串（UTF8）</returns>
        public string Decrypt(string inString, byte[] keyBlob)
        {
            byte[] inputBuffer = inString.FromBase64String();
            return Decrypt(inputBuffer, keyBlob).ToUTF8String();
        }

        /// <summary>
        /// 使用RSA进行非对称解密
        /// </summary>
        /// <param name="inString">待解密的字符串（Base64）</param>
        /// <param name="keyBlobString">RSA私钥Blob字符串（Base64）</param>
        /// <returns>解密后的字符串（UTF8）</returns>
        public string Decrypt(string inString, string keyBlobString)
        {
            byte[] keyBlob = keyBlobString.FromBase64String();
            return Decrypt(inString, keyBlob);
        }

        /// <summary>
        /// 使用RSA进行非对称解密，使用默认私钥
        /// </summary>
        /// <param name="inString">待解密的字符串（Base64）</param>
        /// <returns>解密后的字符串（UTF8）</returns>
        public string Decrypt(string inString)
        {
            byte[] inputBuffer = inString.FromBase64String();
            return Decrypt(inputBuffer).ToUTF8String();
        }
        #endregion
    }
}
