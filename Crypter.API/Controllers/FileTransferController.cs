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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Crypter.API.Attributes;
using Crypter.API.Contracts;
using Crypter.API.Controllers.Base;
using Crypter.Common.Contracts;
using Crypter.Common.Contracts.Features.Transfer;
using Crypter.Core.Services;
using EasyMonads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypter.API.Controllers
{
   [ApiController]
   [Route("api/file/transfer")]
   public class FileTransferController : TransferControllerBase
   {
      public FileTransferController(ITransferDownloadService transferDownloadService, ITransferUploadService transferUploadService, ITokenService tokenService, IUserTransferService userTransferService)
         : base(transferDownloadService, transferUploadService, tokenService, userTransferService) { }

      [HttpPost]
      [MaybeAuthorize]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadTransferResponse))]
      [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> UploadFileTransferAsync([FromQuery] string username, [FromForm] UploadFileTransferReceipt request)
      {
         Maybe<Guid> senderId = _tokenService.TryParseUserId(User);
         Maybe<string> maybeUsername = string.IsNullOrEmpty(username)
            ? Maybe<string>.None
            : username;
         using Stream ciphertextStream = request.Ciphertext.OpenReadStream();

         return await _transferUploadService.UploadFileTransferAsync(senderId, maybeUsername, request.Data, ciphertextStream)
            .MatchAsync(
               left: MakeErrorResponse,
               right: Ok,
               neither: MakeErrorResponse(UploadTransferError.UnknownError));
      }

      [HttpGet("received")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserReceivedFileDTO>))]
      [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
      public async Task<IActionResult> GetReceivedFilesAsync(CancellationToken cancellationToken)
      {
         Guid userId = _tokenService.ParseUserId(User);
         List<UserReceivedFileDTO> result = await _userTransferService.GetUserReceivedFilesAsync(userId, cancellationToken);
         return Ok(result);
      }

      [HttpGet("sent")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserSentFileDTO>))]
      [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
      public async Task<IActionResult> GetSentFilesAsync(CancellationToken cancellationToken)
      {
         Guid userId = _tokenService.ParseUserId(User);
         List<UserSentFileDTO> result = await _userTransferService.GetUserSentFilesAsync(userId, cancellationToken);
         return Ok(result);
      }

      [HttpGet("preview/anonymous")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageTransferPreviewResponse))]
      [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> GetAnonymousFilePreviewAsync([FromQuery] string id, CancellationToken cancellationToken)
      {
         return await _transferDownloadService.GetAnonymousFilePreviewAsync(id, cancellationToken)
            .MatchAsync(
               MakeErrorResponse,
               Ok,
               MakeErrorResponse(TransferPreviewError.UnknownError));
      }

      [HttpGet("preview/user")]
      [MaybeAuthorize]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageTransferPreviewResponse))]
      [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> GetUserFilePreviewAsync([FromQuery] string id, CancellationToken cancellationToken)
      {
         Maybe<Guid> userId = _tokenService.TryParseUserId(User);
         return await _transferDownloadService.GetUserFilePreviewAsync(id, userId, cancellationToken)
            .MatchAsync(
               left: MakeErrorResponse,
               right: Ok,
               neither: MakeErrorResponse(TransferPreviewError.UnknownError));
      }

      [HttpGet("ciphertext/anonymous")]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
      [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
      [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> GetAnonymousFileCiphertextAsync([FromQuery] string id, [FromQuery] string proof)
      {
         return await DecodeProof(proof)
            .BindAsync(async decodedProof => await _transferDownloadService.GetAnonymousFileCiphertextAsync(id, decodedProof))
            .MatchAsync(
               MakeErrorResponse,
               x => new FileStreamResult(x, "application/octet-stream"),
               MakeErrorResponse(DownloadTransferCiphertextError.UnknownError));
      }

      [HttpGet("ciphertext/user")]
      [MaybeAuthorize]
      [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
      [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
      [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
      public async Task<IActionResult> GetUserFileCiphertextAsync([FromQuery] string id, [FromQuery] string proof)
      {
         Maybe<Guid> userId = _tokenService.TryParseUserId(User);
         return await DecodeProof(proof)
            .BindAsync(async decodedProof => await _transferDownloadService.GetUserFileCiphertextAsync(id, decodedProof, userId))
            .MatchAsync(
               MakeErrorResponse,
               x => new FileStreamResult(x, "application/octet-stream"),
               MakeErrorResponse(DownloadTransferCiphertextError.UnknownError));
      }
   }
}
