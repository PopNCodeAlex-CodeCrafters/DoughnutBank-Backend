using DoughnutBank.Utils;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoughnutBank.Tests.UnitTests
{
    public class DiffieHellmanTests
    {
        [Fact]
        public void Encrypt_encryptedMessageShouldBeDifferentFromOriginal()
        {
            //arrange
            var messageToEncrypt = "This is just a random message";
            var diffieHellmanOtherParty = new DiffieHellman();
            var otherPartyPublicKey = diffieHellmanOtherParty.PublicKey;

            //act
            var diffieHellman = new DiffieHellman();
            var encryptedMessageAsByteArray = diffieHellman.Encrypt(otherPartyPublicKey, messageToEncrypt);

            //assert
            var areEquals = encryptedMessageAsByteArray.Equals(messageToEncrypt);
            areEquals.Should().Be(false);
        }

        [Theory]
        [InlineData("ThisIsJustAMessage")]
        [InlineData("I am doing this alone")]
        [InlineData("980 675")]
        public void EncryptDecrypt_encryptedMessageShouldAllowToBeDecrypted(string messageAliceEncryts)
        {
            //arrange
            var BobDiffieHellman = new DiffieHellman();
            var BobPublicKey = BobDiffieHellman.PublicKey;

            var AliceDiffieHellman = new DiffieHellman();
            var AlicePublicKey = AliceDiffieHellman.PublicKey;
            var AliceIV = AliceDiffieHellman.IV;

            //act
            var encryptedMessageByAlice = AliceDiffieHellman.Encrypt(BobPublicKey, messageAliceEncryts);

            string decryptedMessageByBob = BobDiffieHellman.Decrypt(AlicePublicKey, encryptedMessageByAlice, AliceIV);


            //assert
            var areEquals = messageAliceEncryts.Equals(decryptedMessageByBob);
            areEquals.Should().Be(true);
        }
    }
}