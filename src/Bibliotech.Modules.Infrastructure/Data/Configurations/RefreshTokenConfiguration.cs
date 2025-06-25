using System;
using Bibliotech.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bibliotech.Modules.Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
          public void Configure(EntityTypeBuilder<RefreshToken> builder)
          {
                    builder.ToTable("RefreshTokens", "bibliotech");

                    builder.HasKey(rt => rt.Id);

                    builder.Property(rt => rt.Id)
                              .HasMaxLength(36)
                              .IsRequired();

                    builder.Property(rt => rt.UserId)
                              .HasMaxLength(36)
                              .IsRequired();

                    builder.Property(rt => rt.Token)
                              .HasMaxLength(500)
                              .IsRequired();

                    builder.Property(rt => rt.DeviceInfo)
                              .HasMaxLength(1000)
                              .IsRequired();

                    builder.Property(rt => rt.IpAddress)
                              .HasMaxLength(45)
                              .IsRequired();

                    builder.Property(rt => rt.RevokedBy)
                              .HasMaxLength(36);

                    builder.Property(rt => rt.ReplaceByToken)
                              .HasMaxLength(500);

                    builder.HasIndex(rt => rt.Token)
                              .IsUnique()
                              .HasDatabaseName("IX_RefreshTokens_Token");

                    builder.HasIndex(rt => rt.UserId)
                              .HasDatabaseName("IX_RefreshTokens_UserId");

                    builder.HasIndex(rt => new { rt.UserId, rt.IsRevoked })
                              .HasDatabaseName("IX_RefreshTokens_IsRevoked");

                    builder.HasIndex(rt => rt.ExpiryDate)
                              .HasDatabaseName("IX_RefreshTokens_ExpiryDate");
                    
                    builder.Property(rt => rt.IsRevoked)
                              .HasDefaultValue(false);
                    
                    builder.Property(rt => rt.CreatedAt)
                              .HasDefaultValueSql("CURRENT_TIMESTAMP");
          }
}
