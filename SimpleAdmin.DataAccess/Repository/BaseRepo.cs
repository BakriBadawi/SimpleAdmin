namespace SimpleAdmin.DataAccess.Repository;
public class BaseRepo: IDisposable
{
    public DbContextOptions<SimpleadminContext> DbContextOptions { get; }
    public BaseRepo(DbContextOptions<SimpleadminContext> dbContextOptions)
    {
        DbContextOptions = dbContextOptions;
    }
    private SimpleadminContext context;
    public SimpleadminContext Context
    {
        get
        {
            if (context == null)
            {
                context = new SimpleadminContext(DbContextOptions);
            }
            return context;
        }
    }
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
       disposed = true;
    }
    public void Dispose()
    {
        if (context!=null)
        {
            context.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}

