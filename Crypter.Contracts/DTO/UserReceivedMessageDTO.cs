﻿using System;

namespace Crypter.Contracts.DTO
{
   public class UserReceivedMessageDTO
   {
      public Guid Id { get; set; }
      public string Subject { get; set; }
      public Guid SenderId { get; set; }
      public string SenderUsername { get; set; }
      public string SenderPublicAlias { get; set; }
      public DateTime ExpirationUTC { get; set; }

      public UserReceivedMessageDTO(Guid id, string subject, Guid senderId, string senderUsername, string senderPublicAlias, DateTime expirationUTC)
      {
         Id = id;
         Subject = subject;
         SenderId = senderId;
         SenderUsername = senderUsername;
         SenderPublicAlias = senderPublicAlias;
         ExpirationUTC = expirationUTC;
      }
   }
}
