﻿/*
 * Copyright (C) 2023 Crypter File Transfer
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
 * Contact the current copyright holder to discuss commercial license options.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crypter.Common.Contracts;
using Crypter.Common.Contracts.Features.Transfer;
using Crypter.Crypto.Common.StreamEncryption;
using EasyMonads;

namespace Crypter.Common.Client.Interfaces.Requests
{
   public interface IMessageTransferRequests
   {
      Task<Either<UploadTransferError, UploadTransferResponse>> UploadMessageTransferAsync(Maybe<string> recipientUsername, UploadMessageTransferRequest uploadRequest, Func<EncryptionStream> encryptionStreamOpener, bool withAuthentication);
      Task<Maybe<List<UserReceivedMessageDTO>>> GetReceivedMessagesAsync();
      Task<Maybe<List<UserSentMessageDTO>>> GetSentMessagesAsync();
      Task<Either<TransferPreviewError, MessageTransferPreviewResponse>> GetAnonymousMessagePreviewAsync(string hashId);
      Task<Either<TransferPreviewError, MessageTransferPreviewResponse>> GetUserMessagePreviewAsync(string hashId, bool withAuthentication);
      Task<Either<DownloadTransferCiphertextError, StreamDownloadResponse>> GetAnonymousMessageCiphertextAsync(string hashId, byte[] proof);
      Task<Either<DownloadTransferCiphertextError, StreamDownloadResponse>> GetUserMessageCiphertextAsync(string hashId, byte[] proof, bool withAuthentication);
   }
}
