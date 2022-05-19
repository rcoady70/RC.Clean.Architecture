using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Crypto
{
    public class RanNumber
    {
        /// <summary>
        /// Generate 32 byte random no rumber
        /// </summary>
        /// <returns></returns>
        public static string Generate32ByteRandonNo()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
