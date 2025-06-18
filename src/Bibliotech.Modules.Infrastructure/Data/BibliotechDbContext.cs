using System;
using Bibliotech.Core.Abstractions;
using Bibliotech.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bibliotech.Modules.Infrastructure.Data;

public class BibliotechDbContext : DbContext
{
          public BibliotechDbContext(DbContextOptions<BibliotechDbContext> options) : base(options)
          {

          }

          public DbSet<User> Users { get; set; } = null!;

          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
                    base.OnModelCreating(modelBuilder);

                    modelBuilder.ApplyConfigurationsFromAssembly(typeof(BibliotechDbContext).Assembly);

                    modelBuilder.HasDefaultSchema("bibliotech");
          }

          public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
          {
                    var entries = ChangeTracker.Entries<IAuditableEntity>();

                    foreach (var entry in entries)
                    {
                              switch (entry.State)
                              {
                                        case EntityState.Added:
                                                  entry.Property(c => c.CreatedAt).CurrentValue = DateTime.UtcNow;
                                                  break;
                                        case EntityState.Modified:
                                                  entry.Property(u => u.UpdatedAt).CurrentValue = DateTime.UtcNow;
                                                  break;

                              }
                    }

                    return await base.SaveChangesAsync(cancellationToken);
          }
}
