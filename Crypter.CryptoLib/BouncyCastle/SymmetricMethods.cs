﻿using Crypter.CryptoLib.Enums;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Crypter.CryptoLib.BouncyCastle
{
   public static class SymmetricMethods
   {
      /// <summary>
      /// Generate a new AES key of the given size
      /// </summary>
      /// <returns></returns>
      public static KeyParameter GenerateKey(AesKeySize keySize)
      {
         var algorithm = $"AES{(int)keySize}";
         var generator = GeneratorUtilities.GetKeyGenerator(algorithm);
         byte[] symmetricKey = generator.GenerateKey();
         return new KeyParameter(symmetricKey);
      }

      /// <summary>
      /// Generate a 128-bit IV
      /// </summary>
      /// <remarks>
      /// Be aware that AES uses 128-bit block sizes.
      /// This is true for both AES128 and AES256.
      /// The size of the IV should be equal to the block size.
      /// </remarks>
      /// <returns>
      /// An array of 16 random bytes
      /// </returns>
      public static byte[] GenerateIV()
      {
         SecureRandom random = new SecureRandom();
         return random.GenerateSeed(16);
      }

      /// <summary>
      /// Encrypt some bytes using AES/CTR/NoPadding
      /// </summary>
      /// <param name="inBytes"></param>
      /// <param name="key"></param>
      /// <param name="iv"></param>
      /// <returns></returns>
      public static byte[] Encrypt(byte[] inBytes, KeyParameter key, byte[] iv)
      {
         IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
         cipher.Init(true, new ParametersWithIV(key, iv));

         byte[] ciphertext = new byte[cipher.GetOutputSize(inBytes.Length)];
         var length = cipher.ProcessBytes(inBytes, 0, inBytes.Length, ciphertext, 0);
         cipher.DoFinal(ciphertext, length);
         cipher.Reset();
         return ciphertext;
      }

      /// <summary>
      /// Decrypt some bytes using AES/CTR/NoPadding
      /// </summary>
      /// <param name="inBytes"></param>
      /// <param name="key"></param>
      /// <param name="iv"></param>
      /// <returns></returns>
      public static byte[] Decrypt(byte[] inBytes, KeyParameter key, byte[] iv)
      {
         IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
         cipher.Init(false, new ParametersWithIV(key, iv));

         byte[] plaintext = new byte[cipher.GetOutputSize(inBytes.Length)];
         var length = cipher.ProcessBytes(inBytes, 0, inBytes.Length, plaintext, 0);
         cipher.DoFinal(plaintext, length);
         cipher.Reset();
         return plaintext;
      }
   }
}
