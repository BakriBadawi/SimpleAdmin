namespace SimpleAdmin.DataAccess.XunitTest;
public class DbFixture
{
    private DbContextOptions<SimpleadminContext> dBOption;

    public  DbContextOptions<SimpleadminContext> DBOption
    {
        get
        {
            if (dBOption == null)
            {
                dBOption = new DbContextOptionsBuilder<SimpleadminContext>()
                                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                  .Options;
            }
            return dBOption;
        }
    }
    public void AddDummyData(SimpleadminContext context)
    {
        context.User.Add(new User()
        {
            Email = "Admin@Admin.com",
            FullName = "Admin",
            Password = "#####",
            IsEnabled = true,
        });
        context.User.Add(new User()
        {
            Email = "Admin2@Admin.com",
            FullName = "Admin2",
            Password = "#####",
            IsEnabled = true,
        });
        context.Category.Add(new Category()
        {
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            Name = "Cat 1",
        });
        context.Category.Add(new Category()
        {
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            Name = "Cat 2",
        });
        context.Item.Add(new Item()
        {
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            CategoryId = 1,
            Summary = "Summary",
            Title = "Title",
            Id = new Guid("f699c009-20ad-411f-bf46-4ba178faeacc")
        });
        context.SaveChanges();
    }
}