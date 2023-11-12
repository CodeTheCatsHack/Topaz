using Microsoft.EntityFrameworkCore;

namespace Scaffold.Context;

public partial class TopazContext : AbstractContext<TopazContext>
{
    public TopazContext(DbContextOptions<TopazContext> options)
        : base(options)
    {
        ChangeTracker.AutoDetectChangesEnabled = true;
        ChangeTracker.LazyLoadingEnabled = true;
    }
}