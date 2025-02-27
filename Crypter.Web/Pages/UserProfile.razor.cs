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

using System.Threading.Tasks;
using Crypter.Common.Client.Interfaces.HttpClients;
using Crypter.Common.Client.Interfaces.Services;
using Microsoft.AspNetCore.Components;

namespace Crypter.Web.Pages
{
   public partial class UserProfileBase : ComponentBase
   {
      [Inject]
      private ICrypterApiClient CrypterApiService { get; set; }

      [Inject]
      private IUserSessionService UserSessionService { get; set; }

      [Parameter]
      public string Username { get; set; }

      protected Shared.Modal.UploadFileTransferModal FileModal { get; set; }
      protected Shared.Modal.UploadMessageTransferModal MessageModal { get; set; }

      protected bool Loading;
      protected bool IsProfileAvailable;
      protected string Alias;
      protected string About;
      protected string ProperUsername;
      protected bool AllowsFiles;
      protected bool AllowsMessages;
      protected byte[] UserPublicKey;
      protected bool EmailVerified;

      protected override void OnInitialized()
      {
         Loading = true;
      }

      protected override async Task OnParametersSetAsync()
      {
         Loading = true;
         await PrepareUserProfileAsync();
         Loading = false;
      }

      protected async Task PrepareUserProfileAsync()
      {
         bool isLoggedIn = await UserSessionService.IsLoggedInAsync();
         var response = await CrypterApiService.User.GetUserProfileAsync(Username, isLoggedIn);
         response.DoRight(x =>
         {
            Alias = x.Alias;
            About = x.About;
            ProperUsername = x.Username;
            AllowsFiles = x.ReceivesFiles;
            AllowsMessages = x.ReceivesMessages;
            UserPublicKey = x.PublicKey;
            EmailVerified = x.EmailVerified;
         });

         IsProfileAvailable = response.Match(
            false,
            right => right.PublicKey is not null);
      }
   }
}
