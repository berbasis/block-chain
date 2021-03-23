using System;
using System.Security.Cryptography;
using System.Text;

namespace KTPBlockChain.Models
{
    public class Block
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string DataHash { get; set; }
        public string PrevHash { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }

        public const int Difficulty = 3;
        public const string BlockSalt1 = "really makes you think";
        public const string BlockSalt2 = "really activates my almonds";

        public string CreateHash()
        {
            var sha256 = new SHA256Managed();

            if(Id % 2 == 1)
            {
                byte[] bytes = Encoding.ASCII.GetBytes($"{Id}-{TimeStamp}-{DataHash}-{PrevHash}-{Nonce}-{BlockSalt1}");
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }    
            else
            {
                byte[] bytes = Encoding.ASCII.GetBytes($"{Id}-{TimeStamp}-{DataHash}-{PrevHash}-{Nonce}-{BlockSalt2}");
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }    
        }

        public bool IsValidHash()
        {
            string Valid = new string('0', Difficulty);
            return Hash.StartsWith(Valid);
        }

        public void Mine()
        {
            Nonce = 0;
            Hash = CreateHash();
            while (!IsValidHash())
            {
                Nonce++;
                Hash = CreateHash();
            }
        }

    }


}
