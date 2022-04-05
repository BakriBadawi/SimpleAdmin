namespace SimpleAdmin.DataAccess.Repository;
public class ItemRepo : BaseRepo, IRepo<Item, Guid>
{
    public ItemRepo(DbContextOptions<SimpleadminContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public IQueryable<Item> Get(System.Linq.Expressions.Expression<Func<Item, bool>> predicate, List<string> include = null)
    {
        if (predicate == null)
        {
            predicate = c => 1 == 1;
        }
        IQueryable<Item> query = Context.Item;
        if (include != null)
        {
            foreach (string item in include)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    query= query.Include(item);
                }
            }
        }
        return query.Where(predicate);
    }
    public Task<Item> Get(Guid Id)
    {
        return Context.Item.FirstOrDefaultAsync(c => c.Id == Id);
    }
    public async Task Save(Item item,int userId)
    {
        item.CreatedBy = userId;
        item.LastModifiedBy = userId;

        if (item.Id == Guid.Empty)
        {
            await Insert(item);
            return;
        }
        await Update(item);
    }
    public async Task Update(Item item)
    {
        try
        {
            Item oldItem = await Get(item.Id);
            if (oldItem == null)
            {
                throw new ArgumentException("Item with provided Id can't be found");
            }

            item.CreatedBy = oldItem.CreatedBy;
            item.CreatedDate = oldItem.CreatedDate;
            item.LastModifiedDate = DateTime.Now;

            Context.Entry(await Context.Set<Item>().FindAsync(item.Id)).State = EntityState.Detached;
            Context.Entry(item).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task Insert(Item item)
    {
        Context.Item.Add(item);
        await Context.SaveChangesAsync();
    }
    public async Task Delete(Guid Id)
    {
        Item oldItem = await Get(Id);
        if (oldItem == null)
        {
            throw new ArgumentException("Item with provided Id can't be found");
        }
        Context.Item.Remove(await Get(Id));
        await Context.SaveChangesAsync();
    }

}

