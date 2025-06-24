using System;
using Bibliotech.Core.Entities;
using Bibliotech.Core.ValueObjects;

namespace Bibliotech.Core.Repositories;

public interface IUserRepository
{
          Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

          Task<bool> AnyUsers();
          Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
          Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken cancellationToken = default);
          Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default);
          Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
          Task AddAsync(User user, CancellationToken cancellationToken = default);
          Task UpdateAsync(User user, CancellationToken cancellationToken = default);
          Task DeleteAsync(User user, CancellationToken cancellationToken = default);
          Task<IEnumerable<User>> GetActiveUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
