using ELearningPlatform_Data.Infracstructure.Interfaces;
using ELearningPlatform_Domain.Helper.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.Handler.Users
{
    public abstract class UserBaseHandler : BaseHandler
    {
        private readonly string key = UserConstants.Key;

        // Constructor to initialize the UnitOfWork from the base class
        protected UserBaseHandler(IHttpContextAccessor httpContext, IUnitOfWork unitOfWork) : base(httpContext, unitOfWork)
        {
        }

        protected async Task<bool> CheckUserExist(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            return await _unitOfWork.Users.CheckExist(x => x.Username == username);
        }

        /// <summary>
        /// Method to encrypt a string using TripleDES encryption algorithm 
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns>
        /// Return the encrypted result as a base64-encoded string
        /// </returns>
        protected string Encrypt(string toEncrypt)
        {
            // Boolean flag to decide whether to hash the key or not
            bool useHashing = true;
            byte[] keyArray;
            // Convert the string to encrypt into a byte array
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            // If hashing is used, create an MD5 hash of the key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                // Hash the key string and store it in the keyArray
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
            {
                // If not hashing, use the raw key as byte array
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            // Initialize TripleDES encryption algorithm with the key
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;                  // Set the key
            tdes.Mode = CipherMode.ECB;           // Use ECB (Electronic Codebook) mode for encryption
            tdes.Padding = PaddingMode.PKCS7;     // Apply PKCS7 padding

            // Create an encryptor to perform the encryption
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            // Encrypt the byte array and store the result
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Method to decrypt a string using TripleDES encryption algorithm
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns>
        /// Return the decrypted result as a UTF-8 string
        /// </returns>
        protected string Decrypt(string toDecrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            // Convert the base64-encoded string back to a byte array
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            // If hashing is used, create an MD5 hash of the key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                // Hash the key string and store it in the keyArray
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
            {
                // If not hashing, use the raw key as byte array
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            // Initialize TripleDES decryption algorithm with the key
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;                 // Set the key
            tdes.Mode = CipherMode.ECB;          // Use ECB (Electronic Codebook) mode for decryption
            tdes.Padding = PaddingMode.PKCS7;    // Apply PKCS7 padding

            // Create a decryptor to perform the decryption
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            // Decrypt the byte array and store the result
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
