﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

#nullable disable

namespace Crypter.Core.Migrations
{
   [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Crypter.Core.Entities.AnonymousFileTransferEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("KeyExchangeNonce")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Proof")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("AnonymousFileTransfer", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.AnonymousMessageTransferEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("KeyExchangeNonce")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Proof")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AnonymousMessageTransfer", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserConsentEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<int>("ConsentType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Deactivated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.ToTable("UserConsent", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserContactEntity", b =>
                {
                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("Owner");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("uuid")
                        .HasColumnName("Contact");

                    b.HasKey("OwnerId", "ContactId");

                    b.HasIndex("ContactId");

                    b.ToTable("UserContact", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserEmailVerificationEntity", b =>
                {
                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Code")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("VerificationKey")
                        .HasColumnType("bytea");

                    b.HasKey("Owner");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("UserEmailVerification", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<short>("ClientPasswordVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)0);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("citext");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<short>("ServerPasswordVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)0);

                    b.Property<string>("Username")
                        .HasColumnType("citext");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserFailedLoginEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.ToTable("UserFailedLogin", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserFileTransferEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("KeyExchangeNonce")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Proof")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PublicKey")
                        .HasColumnType("bytea");

                    b.Property<Guid?>("RecipientId")
                        .HasColumnType("uuid")
                        .HasColumnName("Recipient");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uuid")
                        .HasColumnName("Sender");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("UserFileTransfer", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserKeyPairEntity", b =>
                {
                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("Nonce")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PrivateKey")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PublicKey")
                        .HasColumnType("bytea");

                    b.HasKey("Owner");

                    b.ToTable("UserKeyPair", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserMasterKeyEntity", b =>
                {
                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("EncryptedKey")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Nonce")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("RecoveryProof")
                        .HasColumnType("bytea");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Owner");

                    b.ToTable("UserMasterKey", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserMessageTransferEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("KeyExchangeNonce")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Proof")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PublicKey")
                        .HasColumnType("bytea");

                    b.Property<Guid?>("RecipientId")
                        .HasColumnType("uuid")
                        .HasColumnName("Recipient");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uuid")
                        .HasColumnName("Sender");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("UserMessageTransfer", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserNotificationSettingEntity", b =>
                {
                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<bool>("EmailNotifications")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableTransferNotifications")
                        .HasColumnType("boolean");

                    b.HasKey("Owner");

                    b.ToTable("UserNotificationSetting", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserPrivacySettingEntity", b =>
                {
                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<bool>("AllowKeyExchangeRequests")
                        .HasColumnType("boolean");

                    b.Property<int>("ReceiveFiles")
                        .HasColumnType("integer");

                    b.Property<int>("ReceiveMessages")
                        .HasColumnType("integer");

                    b.Property<int>("Visibility")
                        .HasColumnType("integer");

                    b.HasKey("Owner");

                    b.ToTable("UserPrivacySetting", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserProfileEntity", b =>
                {
                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<string>("About")
                        .HasColumnType("text");

                    b.Property<string>("Alias")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.HasKey("Owner");

                    b.ToTable("UserProfile", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserRecoveryEntity", b =>
                {
                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Code")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("VerificationKey")
                        .HasColumnType("bytea");

                    b.HasKey("Owner");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("UserRecovery", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserTokenEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Owner")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Owner");

                    b.ToTable("UserToken", (string)null);
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserConsentEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithMany("Consents")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserContactEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "Contact")
                        .WithMany("Contactors")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Crypter.Core.Entities.UserEntity", "Owner")
                        .WithMany("Contacts")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserEmailVerificationEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithOne("EmailVerification")
                        .HasForeignKey("Crypter.Core.Entities.UserEmailVerificationEntity", "Owner")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserFailedLoginEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithMany("FailedLoginAttempts")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserFileTransferEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "Recipient")
                        .WithMany("ReceivedFileTransfers")
                        .HasForeignKey("RecipientId");

                    b.HasOne("Crypter.Core.Entities.UserEntity", "Sender")
                        .WithMany("SentFileTransfers")
                        .HasForeignKey("SenderId");

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserKeyPairEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithOne("KeyPair")
                        .HasForeignKey("Crypter.Core.Entities.UserKeyPairEntity", "Owner")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserMasterKeyEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithOne("MasterKey")
                        .HasForeignKey("Crypter.Core.Entities.UserMasterKeyEntity", "Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserMessageTransferEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "Recipient")
                        .WithMany("ReceivedMessageTransfers")
                        .HasForeignKey("RecipientId");

                    b.HasOne("Crypter.Core.Entities.UserEntity", "Sender")
                        .WithMany("SentMessageTransfers")
                        .HasForeignKey("SenderId");

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserNotificationSettingEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithOne("NotificationSetting")
                        .HasForeignKey("Crypter.Core.Entities.UserNotificationSettingEntity", "Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserPrivacySettingEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithOne("PrivacySetting")
                        .HasForeignKey("Crypter.Core.Entities.UserPrivacySettingEntity", "Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserProfileEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithOne("Profile")
                        .HasForeignKey("Crypter.Core.Entities.UserProfileEntity", "Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserRecoveryEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithOne("Recovery")
                        .HasForeignKey("Crypter.Core.Entities.UserRecoveryEntity", "Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserTokenEntity", b =>
                {
                    b.HasOne("Crypter.Core.Entities.UserEntity", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("Owner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Crypter.Core.Entities.UserEntity", b =>
                {
                    b.Navigation("Consents");

                    b.Navigation("Contactors");

                    b.Navigation("Contacts");

                    b.Navigation("EmailVerification");

                    b.Navigation("FailedLoginAttempts");

                    b.Navigation("KeyPair");

                    b.Navigation("MasterKey");

                    b.Navigation("NotificationSetting");

                    b.Navigation("PrivacySetting");

                    b.Navigation("Profile");

                    b.Navigation("ReceivedFileTransfers");

                    b.Navigation("ReceivedMessageTransfers");

                    b.Navigation("Recovery");

                    b.Navigation("SentFileTransfers");

                    b.Navigation("SentMessageTransfers");

                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
