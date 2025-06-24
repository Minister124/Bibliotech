using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Events;
using Bibliotech.Core.ValueObjects;

namespace Bibliotech.Core.Entities;

public class User : AggregateRoot<UserId>, IAuditableEntity
{
          public string Email { get; private set; } = string.Empty;
          public string PasswordHash { get; private set; } = string.Empty;
          public string FirstName { get; private set; } = string.Empty;
          public string LastName { get; private set; } = string.Empty;
          public bool IsEmailVerified { get; private set; }
          public string? EmailVerificationToken { get; private set; }
          public DateTime? EmailVerifiedAt { get; private set; }
          public string? PasswordResetToken { get; private set; }
          public DateTime? PasswordResetTokenExpiry { get; private set; }
          public DateTime? LastActiveDate { get; private set; }
          public UserStatus Status { get; private set; }
          public DateTime CreatedAt { get; private set; }
          public DateTime? UpdatedAt { get; private set; }
          public string? CreatedBy { get; private set; }
          public string? UpdatedBy { get; private set; }

          public List<string> Roles { get; private set; } = new();

          private User() { }//Constructor for EF Core

          public static User Create(string email, string passwordHash, string firstName, string lastName)
          {
                    var user = new User
                    {
                              Id = UserId.New(),
                              Email = email.ToLowerInvariant(),
                              PasswordHash = passwordHash,
                              FirstName = firstName,
                              LastName = lastName,
                              IsEmailVerified = false,
                              EmailVerificationToken = Guid.NewGuid().ToString(),
                              Status = UserStatus.Active,
                              CreatedAt = DateTime.UtcNow,
                              LastActiveDate = DateTime.UtcNow
                    };

                    user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Email));
                    return user;
          }

          public void VerifyEmail()
          {
                    IsEmailVerified = true;
                    EmailVerifiedAt = DateTime.UtcNow;
                    EmailVerificationToken = null;
                    UpdatedAt = DateTime.UtcNow;

                    AddDomainEvent(new UserEmailVerifiedEvent(Id, Email));
          }

          public void UpdateLastActiveDate()
          {
                    LastActiveDate = DateTime.UtcNow;
                    UpdatedAt = DateTime.UtcNow;
          }

          public void AddRole(string role)
          {
                    if (!Roles.Contains(role))
                    {
                              Roles.Add(role);
                              UpdatedAt = DateTime.UtcNow;
                              AddDomainEvent(new UserRoleAddedEvent(Id, role));
                    }
          }

          public void RemoveRole(string role)
          {
                    if (Roles.Contains(role))
                    {
                              Roles.Remove(role);
                              UpdatedAt = DateTime.UtcNow;
                              AddDomainEvent(new UserRoleRemovedEvent(Id, role));
                    }
          }

          public bool HasRole(string role)
          {
                    return Roles.Contains(role);
          }

          public void ResetPassword(string newPasswordHash)
          {
                    PasswordHash = newPasswordHash;
                    PasswordResetToken = null;
                    PasswordResetTokenExpiry = null;
                    UpdatedAt = DateTime.UtcNow;
          }

          public void resetPasswordToken(TimeSpan? expiry = null)
          {
                    PasswordResetToken = Guid.NewGuid().ToString();
                    PasswordResetTokenExpiry = DateTime.UtcNow.Add(expiry ?? TimeSpan.FromHours(1));
                    UpdatedAt = DateTime.UtcNow;
          }
}

public enum UserStatus
{
          Active = 1,
          Inactive = 2,
          Suspended = 3,
          Deleted = 4
}
