using Microsoft.EntityFrameworkCore;

namespace SimpleAdmin.API.XunitTest;

public class DbFixture
{
    private string _DBName;
    public DbFixture(string DBName)
    {
        _DBName = DBName;
    }
    private DbContextOptions<SimpleadminContext> dBOption;

    public DbContextOptions<SimpleadminContext> DBOption
    {
        get
        {
            if (dBOption == null)
            {
                dBOption = new DbContextOptionsBuilder<SimpleadminContext>()
                                  .UseInMemoryDatabase(databaseName: _DBName)
                                  .Options;
            }
            return dBOption;
        }
    }
    public void AddDummyData(SimpleadminContext context)
    {
        context.User.Add(new User()
        {
            Id=1,
            Email = "Eamil@domian.com",
            FullName = "Admin",
            Password = "$2a$11$BYPotUMQz4HBM7bBdZ848eXh59cmygLXcnZ5c3XEVeN5M1Jb40faG",
            IsEnabled = true,
        });
        context.User.Add(new User()
        {
            Id = 2,
            Email = "Eamil2@domian.com",
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