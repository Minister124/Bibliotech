using System;
using Bibliotech.Core.Models;
using Bibliotech.Core.Repositories;
using Bibliotech.Modules.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bibliotech.Modules.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
          private readonly BibliotechDbContext _context;

          private readonly ILogger<RefreshTokenRepository> _logger;

          public RefreshTokenRepository(BibliotechDbContext context, ILogger<RefreshTokenRepository> logger)
          {
                    _context = context;
                    _logger = logger;
          }

          public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
          {
                    try
                    {
                              await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
                              await _context.SaveChangesAsync(cancellationToken);

                              _logger.LogDebug("Refresh token added for user {UserId}", refreshToken.UserId);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error adding refresh token for user {UserId}", refreshToken.UserId);
                              throw;
                    }
          }

          public async Task CleanupExpiredTokenAsync(CancellationToken cancellationToken = default)
          {
                    try
                    {
                              var expiredTokens = await _context.RefreshTokens.Where(rt => rt.ExpiryDate <= DateTime.UtcNow)
                                                            .ToListAsync(cancellationToken);

                              if (expiredTokens.Any())
                              {
                                        _context.RefreshTokens.RemoveRange(expiredTokens);
                                        await _context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("Cleaned up {Count} expired refresh tokens", expiredTokens.Count);
                              }
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error cleaning up expired refresh tokens");
                              throw;
                    }
          }

          public async Task<RefreshToken> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
          {
                    try
                    {
                              return await _context.RefreshTokens.AsNoTracking()
                                        .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error retrieving refresh token");
                              throw;
                    }
          }

          public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
          {
                    try
                    {
                              return await _context.RefreshTokens.AsNoTracking().Where(rt => rt.UserId == userId)
                                                  .OrderByDescending(rt => rt.CreatedAt)
                                                  .ToListAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error retrieving refresh tokens for user {UserId}", userId);
                              throw;
                    }
          }

          public async Task RevokeAllUserTokenAsync(string userId, CancellationToken cancellationToken = default)
          {
                    try
                    {
                              var activeTokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId && !rt.IsRevoked)
                                                                      .ToListAsync(cancellationToken);

                              foreach (var token in activeTokens)
                              {
                                        token.IsRevoked = true;
                                        token.RevokedAt = DateTime.UtcNow;
                                        token.RevokedBy = "System";
                              }

                              if (activeTokens.Any())
                              {
                                        await _context.SaveChangesAsync(cancellationToken);
                                        _logger.LogInformation("Revoked {Count} refresh tokens for user {UserId}", activeTokens.Count, userId);
                              }
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error revoking all tokens for user {UserId}", userId);
                              throw;
                    }
          }

          public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
          {
                    try
                    {
                              _context.RefreshTokens.Update(refreshToken);
                              await _context.SaveChangesAsync(cancellationToken);

                              _logger.LogDebug("Refresh token updated refresh token {TokenId}", refreshToken.Id);
                    }
                    catch (Exception ex)
                    {
                              _logger.LogError(ex, "Error updating refresh token {TokenId}", refreshToken.Id);
                              throw;
                    }
          }
}
