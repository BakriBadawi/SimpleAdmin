namespace SimpleAdmin.DataAccess.Repository;
public class UserRepo : BaseRepo, IRepo<User,int>
{
    public UserRepo(DbContextOptions<SimpleadminContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public IQueryable<User> Get(System.Linq.Expressions.Expression<Func<User, bool>> predicate, List<string> include = null)
    {
        if (predicate==null)
        {
            predicate = c => 1 == 1;
        }
        IQueryable<User> query = Context.User;
        if (include != null)
        {
            foreach (string item in include)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    query = query.Include(item);
                }
            }
        }
        return query.Where(predicate);
    }
    
    public Task<User> Get(int Id)
    {
        return Context.User.FirstOrDefaultAsync(c => c.Id == Id);
    }
    public async Task Save(User user,int userId)
    {
        user.CreatedBy = userId;
        user.LastModifiedBy = userId;

        if (user.Id == 0)
        {
            await Insert(user);
            return;
        }
        await Update(user);
    }
    public async Task Update(User user)
    {
        User oldUser = await Get(user.Id);
        
        if (oldUser == null)
        {
            throw new ArgumentException("User with provided Id can't be found");
        }

        user.CreatedBy = oldUser.CreatedBy;
        user.CreatedDate= oldUser.CreatedDate;
        user.LastModifiedDate = DateTime.Now;

        Context.Entry(await Context.Set<User>().FindAsync(user.Id)).State = EntityState.Detached;
        Context.Entry(user).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }
    public async Task Insert(User user)
    {
        Context.User.Add(user);
        await Context.SaveChangesAsync();
    }
    public async Task Delete(int Id)
    {
        User oldUser = await Get(Id);
        if (oldUser == null)
        {
            throw new ArgumentException("User with provided Id can't be found");
        }
        Context.User.Remove(oldUser);
        await Context.SaveChangesAsync();
    }
}

