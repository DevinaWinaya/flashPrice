using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace othersFx.Crypto.Hash
{
    public static class Hasher
    {

        /// <summary>
        /// Computes hash of input string with specified <see cref="Encoding"/> and returns hexadecimal string representation of hash in lowercase
        /// </summary>
        /// <typeparam name="THashAlgorithm">The type that derives from <see cref="HashAlgorithm"/> to use</typeparam>
        /// <param name="input">The string to compute hash from</param>
        /// <param name="encoding">The encoding to use to get </param>
        /// <returns>The hashed string in lowercase hexadecimal format</returns>
        private static string GetHash<THashAlgorithm>(string input, Encoding encoding) where THashAlgorithm: HashAlgorithm, new()
        {
            var hashResult = GetHashBytes<THashAlgorithm>(input, encoding);
            var sb = new StringBuilder();
            foreach (byte b in hashResult)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        // No need to return IEnumerable<byte>, it's intentional
        private static byte[] GetHashBytes<THashAlgorithm>(string input, Encoding encoding)
            where THashAlgorithm : HashAlgorithm, new()
        {
            using (var hasher = new THashAlgorithm())
            {
                return hasher.ComputeHash(encoding.GetBytes(input));
            }
        }


        public static string GetSha256Hash(string input, Encoding encoding)
        {
            return GetHash<SHA256Managed>(input, encoding);
        }
    }
}
