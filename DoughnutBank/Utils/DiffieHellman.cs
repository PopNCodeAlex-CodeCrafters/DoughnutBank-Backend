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

            var decryptedMessage = CryptedMessageToDecryptedStringUsingAes(encryptedMessage);
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
            byte[] encryptedMessage;

            using var encryptedMessageStream = new MemoryStream();
            
            using var encryptor = this.aes.CreateEncryptor();
                
            using var cryptoStream = new CryptoStream(encryptedMessageStream, encryptor, CryptoStreamMode.Write);
                    
            using (StreamWriter sw = new StreamWriter(cryptoStream))
             sw.Write(message);

            encryptedMessage = encryptedMessageStream.ToArray();
            return encryptedMessage;
        }

        private string CryptedMessageToDecryptedStringUsingAes(byte[] encryptedMessage)
        {
            using var encryptedMessageStream = new MemoryStream(encryptedMessage);

            using var decryptor = this.aes.CreateDecryptor();

            using var cryptoStream = new CryptoStream(encryptedMessageStream, decryptor, CryptoStreamMode.Read);

            using StreamReader reader = new StreamReader(cryptoStream);

            string plainText = reader.ReadToEnd();
            return plainText;
        }
    }
}
