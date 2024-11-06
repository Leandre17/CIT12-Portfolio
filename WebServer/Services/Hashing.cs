using System;
using System.Security.Cryptography;
using System.Text;

namespace WebServer.Services
{
    public class Hashing
    {
   
        protected const int SaltBitSize = 64;
        protected const byte SaltByteSize = SaltBitSize / 8;
        protected const int HashBitSize = 256;
        protected const int HashByteSize = HashBitSize / 8;
    
        private readonly RandomNumberGenerator _rand = RandomNumberGenerator.Create();
      
        public (string hash, string salt) Hash(string password)
        {
            byte[] salt = new byte[SaltByteSize];
            _rand.GetBytes(salt);
            string saltString = Convert.ToHexString(salt);

  
            string hash = HashSHA256(password, saltString);
            return (hash, saltString);
        }
        public bool Verify(string loginPassword, string hashedRegisteredPassword, string saltString)
        {
            string hashedLoginPassword = HashSHA256(loginPassword, saltString);
            return hashedRegisteredPassword == hashedLoginPassword;
        }

        
        private string HashSHA256(string password, string saltString)
        {
            byte[] hashInput = Encoding.UTF8.GetBytes(saltString + password);
            using (SHA256 sha256 = SHA256.Create())  
            {
                byte[] hashOutput = sha256.ComputeHash(hashInput);  
                return Convert.ToHexString(hashOutput);
            }
        }
    }
}
