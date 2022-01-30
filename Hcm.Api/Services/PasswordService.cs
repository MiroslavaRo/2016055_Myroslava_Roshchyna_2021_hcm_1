using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hcm.Api.Services
{
    public class PasswordService
    {
        public string GenerateRandomPassword()
        {
            return Hash("Adminpass1_");
        }

        public bool IsValid(string password)
        {
            var match = Regex.Match(
                password,
                @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*#?&_\-.])[A-Za-z\d@$!%*#?&_\-.]{8,}$");

            return match.Success;
        }

        public bool AreEqual(string hashPassword, string plainPassword)
        {
            var hashString = Hash(plainPassword);
            return hashString == hashPassword;
        }

        public string Hash(string plainText)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plainText);
                var hashBytes = md5.ComputeHash(bytes);
                var stringBuilder = new StringBuilder();

                foreach (var @byte in hashBytes)
                {
                    stringBuilder.AppendFormat("{0:x2}", @byte);
                }

                return stringBuilder.ToString().ToLower();
            }
        }
    }
}
