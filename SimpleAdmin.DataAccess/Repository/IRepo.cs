namespace SimpleAdmin.DataAccess.Repository;
public interface IRepo<T,U>
{
    IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate, List<string> include = null);
    Task<T> Get(U Id);
    Task Save(T user,int userId);
    Task Insert(T user);
    Task Update(T user);
    Task Delete(U Id);
}

