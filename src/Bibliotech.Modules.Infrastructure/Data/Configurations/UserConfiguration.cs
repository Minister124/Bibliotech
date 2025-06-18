using System;
using Bibliotech.Core.Entities;
using Bibliotech.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bibliotech.Modules.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
          public void Configure(EntityTypeBuilder<User> builder)
          {
                    builder.ToTable("users");

                    builder.HasKey(x => x.Id);

                    builder.Property(x => x.Id)
                              .HasConversion(
                                        userId => userId.Value,
                                        value => UserId.From(value)
                              ).ValueGeneratedNever();
                    
                    builder.Property(x => x.Email)
                              .IsRequired()
                              .HasMaxLength(255);

                    builder.Property(x => x.PasswordHash)
                              .IsRequired()
                              .HasMaxLength(255);
                    
                    builder.Property(x => x.FirstName)
                              .IsRequired()
                              .HasMaxLength(100);

                    builder.Property(x => x.LastName)
                              .IsRequired()
                              .HasMaxLength(100);

                    builder.Property(x => x.EmailVerificationToken)
                              .HasMaxLength(500);

                    builder.Property(x => x.PasswordResetToken)
                              .HasMaxLength(500);

                    builder.Property(x => x.Status)
                              .HasConversion<int>();
                    
                    builder.Property(x => x.CreatedAt)
                              .IsRequired();

                    builder.Property(x => x.CreatedBy)
                              .HasMaxLength(100);
                    
                    builder.Property(x => x.UpdatedBy)
                              .HasMaxLength(100);

                    builder.HasIndex(x => x.Email)
                              .IsUnique()
                              .HasDatabaseName("ix_users_email");

                    builder.HasIndex(x => x.EmailVerificationToken)
                              .HasDatabaseName("ix_users_email_verification_token");
                    
                    builder.HasIndex(x => x.Status)
                              .HasDatabaseName("ix_users_status");
                    
                    builder.HasIndex(x => x.PasswordResetToken)
                              .HasDatabaseName("ix_users_password_reset_token");
                    
                    builder.HasIndex(x => x.CreatedAt)
                              .HasDatabaseName("ix_users_created_at");

                    builder.Ignore(x => x.DomainEvents);
          }
}
