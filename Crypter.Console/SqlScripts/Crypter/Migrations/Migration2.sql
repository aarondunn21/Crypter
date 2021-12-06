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

BEGIN;

   -- Alter UserX25519KeyPair

   ALTER TABLE IF EXISTS public."UserX25519KeyPair"
      ADD COLUMN "ClientIV" text COLLATE pg_catalog."default";

   -- Alter UserEd25519KeyPair

   ALTER TABLE IF EXISTS public."UserEd25519KeyPair"
      ADD COLUMN "ClientIV" text COLLATE pg_catalog."default";

   -- Delete rows from UserX25519KeyPair

   DELETE FROM public."UserX25519KeyPair";

   -- Delete rows from UserX25519KeyPair

   DELETE FROM public."UserEd25519KeyPair";

   -- Update schema version

   UPDATE public."Schema" SET "Version" = 2, "Updated" = CURRENT_TIMESTAMP;

COMMIT;