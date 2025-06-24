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

          public async Task AddAsync(User user, CancellationToken cancellationToken = default)
          {
                    await _context.Users.AddAsync(user, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
          }

          public async Task<bool> AnyUsers()
          {
                    return await _context.Users.AnyAsync();
          }

          public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
          {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync(cancellationToken);
          }

          public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.AnyAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
          }

          public async Task<IEnumerable<User>> GetActiveUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.Where(u => u.Status == UserStatus.Active)
                                        .OrderByDescending(u => u.LastActiveDate)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);
          }

          public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
          }

          public async Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token, cancellationToken);
          }

          public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);          }

          public async Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken cancellationToken = default)
          {
                    return await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == token && u.PasswordResetTokenExpiry > DateTime.UtcNow, cancellationToken);
          }

          public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
          {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync(cancellationToken);
          }
}
