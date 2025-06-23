using System;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Repositories;

public interface IRefreshTokenRepository
{
          Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
          Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
          Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
          Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
          Task RemoveAllUserTokenAsync(string userId, CancellationToken cancellationToken = default);
          Task CleanupExpiredTokenAsync(CancellationToken cancellationToken = default);
} 
