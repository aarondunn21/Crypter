﻿/*
 * Copyright (C) 2021 Crypter File Transfer
 * 
 * This file is part of the Crypter file transfer project.
 * 
 * Crypter is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * The Crypter source code is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * You can be released from the requirements of the aforementioned license
 * by purchasing a commercial license. Buying such a license is mandatory
 * as soon as you develop commercial activities involving the Crypter source
 * code without disclosing the source code of your own applications.
 * 
 * Contact the current copyright holder to discuss commerical license options.
 */

using Crypter.Contracts.Enum;
using Crypter.Contracts.Requests;
using Crypter.Contracts.Responses;
using Crypter.Core.Interfaces;
using Crypter.Core.Services;
using Crypter.CryptoLib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Crypter.API.Services
{
   public class DownloadService
   {
      private readonly IBaseTransferService<IMessageTransferItem> MessageService;
      private readonly IBaseTransferService<IFileTransferItem> FileService;
      private readonly IUserService UserService;
      private readonly IUserProfileService UserProfileService;
      private readonly ITransferItemStorageService MessageTransferItemStorageService;
      private readonly ITransferItemStorageService FileTransferItemStorageService;
      private readonly ISimpleEncryptionService SimpleEncryptionService;
      private readonly ISimpleHashService SimpleHashService;
      private readonly Func<byte[], byte[]> ItemDigestFunction;

      public DownloadService(
         IConfiguration configuration,
         IBaseTransferService<IMessageTransferItem> messageService,
         IBaseTransferService<IFileTransferItem> fileService,
         IUserService userService,
         IUserProfileService userProfileService,
         ISimpleEncryptionService simpleEncryptionService,
         ISimpleHashService simpleHashService
         )
      {
         MessageService = messageService;
         FileService = fileService;
         UserService = userService;
         UserProfileService = userProfileService;
         MessageTransferItemStorageService = new TransferItemStorageService(configuration["EncryptedFileStore:Location"], TransferItemType.Message);
         FileTransferItemStorageService = new TransferItemStorageService(configuration["EncryptedFileStore:Location"], TransferItemType.File);
         SimpleEncryptionService = simpleEncryptionService;
         SimpleHashService = simpleHashService;
         ItemDigestFunction = SimpleHashService.DigestSha256;
      }

      public async Task<IActionResult> GetMessagePreviewAsync(GetTransferPreviewRequest request, Guid requestorId, CancellationToken cancellationToken)
      {
         var possibleMessage = await MessageService.ReadAsync(request.Id, cancellationToken);
         if (possibleMessage is null)
         {
            return new NotFoundObjectResult(
               new MessagePreviewResponse(null, 0, default, null, null, default, default, default, default));
         }

         var messageBelongsToSomeoneElse = possibleMessage.Recipient != Guid.Empty && possibleMessage.Recipient != requestorId;
         if (messageBelongsToSomeoneElse)
         {
            return new NotFoundObjectResult(
               new MessagePreviewResponse(null, 0, default, null, null, default, default, default, default));
         }

         string senderUsername = null;
         string senderAlias = null;
         if (possibleMessage.Sender != Guid.Empty)
         {
            var possibleUser = await UserService.ReadAsync(possibleMessage.Sender, cancellationToken);
            if (possibleUser != null)
            {
               senderUsername = possibleUser.Username;
            }

            var possibleUserProfile = await UserProfileService.ReadAsync(possibleMessage.Sender, cancellationToken);
            if (possibleUserProfile != null)
            {
               senderAlias = possibleUserProfile.Alias;
            }
         }

         return new OkObjectResult(
            new MessagePreviewResponse(possibleMessage.Subject, possibleMessage.Size, possibleMessage.Sender, senderUsername, senderAlias, possibleMessage.Recipient, possibleMessage.X25519PublicKey, possibleMessage.Created, possibleMessage.Expiration));
      }

      public async Task<IActionResult> GetFilePreviewAsync(GetTransferPreviewRequest request, Guid requestorId, CancellationToken cancellationToken)
      {
         var possibleFile = await FileService.ReadAsync(request.Id, cancellationToken);
         if (possibleFile is null)
         {
            return new NotFoundObjectResult(
               new FilePreviewResponse(null, null, 0, default, null, null, default, default, default, default));
         }

         var fileBelongsToSomeoneElse = possibleFile.Recipient != Guid.Empty && possibleFile.Recipient != requestorId;
         if (fileBelongsToSomeoneElse)
         {
            return new NotFoundObjectResult(
               new FilePreviewResponse(null, null, 0, default, null, null, default, default, default, default));
         }

         string senderUsername = null;
         string senderAlias = null;
         if (possibleFile.Sender != Guid.Empty)
         {
            var possibleUser = await UserService.ReadAsync(possibleFile.Sender, cancellationToken);
            if (possibleUser != null)
            {
               senderUsername = possibleUser.Username;
            }

            var possibleUserProfile = await UserProfileService.ReadAsync(possibleFile.Sender, cancellationToken);
            if (possibleUserProfile != null)
            {
               senderAlias = possibleUserProfile.Alias;
            }
         }

         return new OkObjectResult(
            new FilePreviewResponse(possibleFile.FileName, possibleFile.ContentType, possibleFile.Size, possibleFile.Sender, senderUsername, senderAlias, possibleFile.Recipient, possibleFile.X25519PublicKey, possibleFile.Created, possibleFile.Expiration));
      }

      public async Task<IActionResult> GetMessageCiphertextAsync(GetTransferCiphertextRequest request, Guid requestorId, CancellationToken cancellationToken)
      {
         var possibleMessage = await MessageService.ReadAsync(request.Id, cancellationToken);
         if (possibleMessage is null)
         {
            return new NotFoundObjectResult(
               new GetTransferCiphertextResponse(DownloadCiphertextResult.NotFound, null, null));
         }

         var messageBelongsToSomeoneElse = possibleMessage.Recipient != Guid.Empty && possibleMessage.Recipient != requestorId;
         if (messageBelongsToSomeoneElse)
         {
            return new NotFoundObjectResult(
               new GetTransferCiphertextResponse(DownloadCiphertextResult.NotFound, null, null));
         }

         // Remove server-side encryption
         var serverDecryptionKey = Convert.FromBase64String(request.ServerDecryptionKeyBase64);
         var cipherTextServer = await MessageTransferItemStorageService.ReadAsync(request.Id, cancellationToken);
         var cipherTextClient = SimpleEncryptionService.Decrypt(serverDecryptionKey, possibleMessage.ServerIV, cipherTextServer);

         // Compare digests AFTER removing server-side encryption
         var cipherTextClientDigest = ItemDigestFunction(cipherTextClient);
         var digestsMatch = SimpleHashService.CompareDigests(possibleMessage.ServerDigest, cipherTextClientDigest);
         if (!digestsMatch)
         {
            return new BadRequestObjectResult(
                new GetTransferCiphertextResponse(DownloadCiphertextResult.ServerDecryptionFailed, null, null));
         }

         if (possibleMessage.Recipient == Guid.Empty)
         {
            await MessageService.DeleteAsync(request.Id, cancellationToken);
            MessageTransferItemStorageService.Delete(request.Id);
         }

         return new OkObjectResult(
             new GetTransferCiphertextResponse(DownloadCiphertextResult.Success, Convert.ToBase64String(cipherTextClient), possibleMessage.ClientIV));
      }

      public async Task<IActionResult> GetFileCiphertextAsync(GetTransferCiphertextRequest request, Guid requestorId, CancellationToken cancellationToken)
      {
         var possibleFile = await FileService.ReadAsync(request.Id, cancellationToken);
         if (possibleFile is null)
         {
            return new NotFoundObjectResult(
               new GetTransferCiphertextResponse(DownloadCiphertextResult.NotFound, null, null));
         }

         var fileBelongsToSomeoneElse = possibleFile.Recipient != Guid.Empty && possibleFile.Recipient != requestorId;
         if (fileBelongsToSomeoneElse)
         {
            return new NotFoundObjectResult(
               new GetTransferCiphertextResponse(DownloadCiphertextResult.NotFound, null, null));
         }

         // Remove server-side encryption
         var serverDecryptionKey = Convert.FromBase64String(request.ServerDecryptionKeyBase64);
         var cipherTextServer = await FileTransferItemStorageService.ReadAsync(request.Id, cancellationToken);
         var cipherTextClient = SimpleEncryptionService.Decrypt(serverDecryptionKey, possibleFile.ServerIV, cipherTextServer);

         // Compare digests AFTER removing server-side encryption
         var cipherTextClientDigest = ItemDigestFunction(cipherTextClient);
         var digestsMatch = SimpleHashService.CompareDigests(possibleFile.ServerDigest, cipherTextClientDigest);
         if (!digestsMatch)
         {
            return new BadRequestObjectResult(
                new GetTransferCiphertextResponse(DownloadCiphertextResult.ServerDecryptionFailed, null, null));
         }

         if (possibleFile.Recipient == Guid.Empty)
         {
            await FileService.DeleteAsync(request.Id, cancellationToken);
            FileTransferItemStorageService.Delete(request.Id);
         }

         return new OkObjectResult(
             new GetTransferCiphertextResponse(DownloadCiphertextResult.Success, Convert.ToBase64String(cipherTextClient), possibleFile.ClientIV));
      }

      public async Task<IActionResult> GetMessageSignatureAsync(GetTransferSignatureRequest request, Guid requestorId, CancellationToken cancellationToken)
      {
         var possibleMessage = await MessageService.ReadAsync(request.Id, cancellationToken);
         if (possibleMessage is null)
         {
            return new NotFoundObjectResult(
               new GetTransferSignatureResponse(DownloadSignatureResult.NotFound, null, null));
         }

         var messageBelongsToSomeoneElse = possibleMessage.Recipient != Guid.Empty && possibleMessage.Recipient != requestorId;
         if (messageBelongsToSomeoneElse)
         {
            return new NotFoundObjectResult(
               new GetTransferSignatureResponse(DownloadSignatureResult.NotFound, null, null));
         }

         return new OkObjectResult(
            new GetTransferSignatureResponse(DownloadSignatureResult.Success, possibleMessage.Signature, possibleMessage.Ed25519PublicKey));
      }

      public async Task<IActionResult> GetFileSignatureAsync(GetTransferSignatureRequest request, Guid requestorId, CancellationToken cancellationToken)
      {
         var possibleFile = await FileService.ReadAsync(request.Id, cancellationToken);
         if (possibleFile is null)
         {
            return new NotFoundObjectResult(
               new GetTransferSignatureResponse(DownloadSignatureResult.NotFound, null, null));
         }

         var messageBelongsToSomeoneElse = possibleFile.Recipient != Guid.Empty && possibleFile.Recipient != requestorId;
         if (messageBelongsToSomeoneElse)
         {
            return new NotFoundObjectResult(
               new GetTransferSignatureResponse(DownloadSignatureResult.NotFound, null, null));
         }

         return new OkObjectResult(
            new GetTransferSignatureResponse(DownloadSignatureResult.Success, possibleFile.Signature, possibleFile.Ed25519PublicKey));
      }
   }
}
