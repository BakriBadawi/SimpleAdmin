namespace SimpleAdmin.DataAccess.Repository;
public class CategoryRepo : BaseRepo, IRepo<Category,int>
{
    public CategoryRepo(DbContextOptions<SimpleadminContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    public IQueryable<Category> Get(System.Linq.Expressions.Expression<Func<Category, bool>> predicate, List<string> include = null)
    {
        if (predicate == null)
        {
            predicate = c => 1 == 1;
        }
        IQueryable<Category> query = Context.Category;
        if (include != null)
        {
            foreach (string item in include)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    query=query.Include(item);
                }
            }
        }
        return query.Where(predicate);
    }
    public Task<Category> Get(int Id)
    {
        return Context.Category.FirstOrDefaultAsync(c => c.Id == Id);
    }
    public async Task Save(Category category,int userId)
    {
        category.CreatedBy = userId;
        category.LastModifiedBy = userId;

        if (category.Id == 0)
        {
            await Insert(category);
            return;
        }
        await Update(category);
    }
    public async Task Update(Category category)
    {
        try
        {
            Category oldCategory = await Get(category.Id);
            if (oldCategory == null)
            {
                throw new ArgumentException("Category with provided Id can't be found");
            }

            category.CreatedBy = oldCategory.CreatedBy;
            category.CreatedDate = oldCategory.CreatedDate;
            category.LastModifiedDate = DateTime.Now;

            Context.Entry(await Context.Set<Category>().FindAsync(category.Id)).State = EntityState.Detached;
            Context.Entry(category).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task Insert(Category Category)
    {
        Context.Category.Add(Category);
        await Context.SaveChangesAsync();
    }
    public async Task Delete(int Id)
    {
        Category oldCategory = await Get(Id);
        if (oldCategory == null)
        {
            throw new ArgumentException("Category with provided Id can't be found");
        }
        Context.Category.Remove(await Get(Id));
        await Context.SaveChangesAsync();
    }

}

