using System.Security.Cryptography;
using System.Text;

namespace DoughnutBank.Utils
{
    public class DiffieHellman
    {
        private Aes aes = null;
        private ECDiffieHellmanCng diffieHellman = null;
        private readonly byte[] publicKey;
        public DiffieHellman()
        {
            this.aes = new AesCryptoServiceProvider();
            this.diffieHellman = new ECDiffieHellmanCng
            {
                KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
                HashAlgorithm = CngAlgorithm.Sha256,

            };

            this.publicKey = this.diffieHellman.PublicKey.ToByteArray();
        }

        public byte[] PublicKey
        {
            get { return this.publicKey; }
        }

        public byte[] IV
        {
            get { return this.aes.IV; }
        }

        
        public byte[] Encrypt(byte[] otherPartyPublicKey, string message)
        {
            var derivedKey = ComputeDerivedKey(otherPartyPublicKey);

            SetAesKey(derivedKey);

            byte[] encryptedByteArray = MessageToEncryptedByteArrayUsingAes(message);
            return encryptedByteArray;
        }

        public string Decrypt(byte[] otherPartyPublicKey, byte[] encryptedMessage, byte[] iv)
        {
            var derivedKey = ComputeDerivedKey(otherPartyPublicKey);
           
            SetAesKey(derivedKey);
            SetAesIV(iv);

            string decryptedMessage = CryptedMessageToDecryptedStringUsingAes(encryptedMessage);
            return decryptedMessage;
        }


        private byte[] ComputeDerivedKey(byte[] otherPartyPublicKey)
        {
            var key = CngKey.Import(otherPartyPublicKey, CngKeyBlobFormat.EccPublicBlob);
            return diffieHellman.DeriveKeyMaterial(key);
        }

        private void SetAesKey(byte[] key)
        {
            aes.Key = key;
        }

        private void SetAesIV(byte[] iv)
        {
            aes.IV = iv;
        }

        private byte[] MessageToEncryptedByteArrayUsingAes(string message)
        {
            using var cipherText = new MemoryStream();

            using var encryptor = aes.CreateEncryptor();

            using var cryptoStream = new CryptoStream(cipherText, encryptor, CryptoStreamMode.Write);
                    
            byte[] ciphertextMessage = Encoding.UTF8.GetBytes(message);
            cryptoStream.Write(ciphertextMessage, 0, ciphertextMessage.Length);
                    
            return cipherText.ToArray();
        }

        private string CryptedMessageToDecryptedStringUsingAes(byte[] encryptedMessage)
        {
            using var plainText = new MemoryStream();

            using var decryptor = this.aes.CreateDecryptor();

            using var cryptoStream = new CryptoStream(plainText, decryptor, CryptoStreamMode.Write);
                    
            cryptoStream.Write(encryptedMessage, 0, encryptedMessage.Length);

            return Encoding.UTF8.GetString(plainText.ToArray());    
        }



    }
}
