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

using System.Text.RegularExpressions;
using Crypter.Common.Primitives.Enums;
using Crypter.Common.Primitives.ValidationHandlers;
using EasyMonads;
using ValueOf;

namespace Crypter.Common.Primitives
{
   public class Username : ValueOf<string, Username>
   {
      private static readonly Regex _validCharacters = new(@"^[a-zA-Z0-9_\-]+$");

      /// <summary>
      /// Do not use this.
      /// </summary>
      public Username()
      {
      }

      protected override void Validate()
      {
         StringPrimitiveValidationHandler.ThrowIfInvalid(CheckValidation, Value);
      }

      protected override bool TryValidate()
      {
         return CheckValidation(Value)
            .IsNone;
      }

      public static Maybe<StringPrimitiveValidationFailure> CheckValidation(string value)
      {
         if (value is null)
         {
            return StringPrimitiveValidationFailure.IsNull;
         }

         if (string.IsNullOrWhiteSpace(value))
         {
            return StringPrimitiveValidationFailure.IsEmpty;
         }

         if (value.Length > 32)
         {
            return StringPrimitiveValidationFailure.TooLong;
         }

         if (!_validCharacters.IsMatch(value))
         {
            return StringPrimitiveValidationFailure.InvalidCharacters;
         }

         return Maybe<StringPrimitiveValidationFailure>.None;
      }
   }
}
