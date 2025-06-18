using System;
using Bibliotech.Core.Entities;
using Bibliotech.Core.Repositories;
using Bibliotech.Core.ValueObjects;
using Bibliotech.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bibliotech.Modules.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
          private readonly BibliotechDbContext _context;

          public UserRepository(BibliotechDbContext context)
          {
                    _context = context;
          }

          public Task AddAsync(User user, CancellationToken cancellationToken = default)
          {
                    throw new NotImplementedException();
          }

          public Task DeleteAsync(User user, CancellationToken cancellationToken = default)
          {
                    throw new NotImplementedException();
          }

          public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
          {
                    throw new NotImplementedException();
          }

          public Task<IEnumerable<User>> GetActiveUsersAsync(int pageNumber, int PageSize, CancellationToken cancellationToken = default)
          {
                    throw new NotImplementedException();
          }

          public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
          }

          public Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken cancellationToken = default)
          {
                    throw new NotImplementedException();
          }

          public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);          }

          public Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default)
          {
                    throw new NotImplementedException();
          }

          public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
          {
                    throw new NotImplementedException();
          }
}
