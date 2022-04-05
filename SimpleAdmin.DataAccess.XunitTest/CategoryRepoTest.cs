namespace SimpleAdmin.DataAccess.XunitTest;
public class CategoryRepoTest
{
    CategoryRepo repo;
    public CategoryRepoTest()
    {
        DbFixture dbFixture = new DbFixture();
        repo = new CategoryRepo(new DbFixture().DBOption);
        dbFixture.AddDummyData(repo.Context);
    }
    [Theory]
    [MemberData(nameof(GetData))]

    public async Task Get_ShoudGetData(System.Linq.Expressions.Expression<Func<Category, bool>> predicate, List<string> include)
    {
        List<Category> data = await repo.Get(predicate, include).ToListAsync();
        Assert.NotEmpty(data);
    }
    public static IEnumerable<object[]> GetData
    {

        get
        {
            System.Linq.Expressions.Expression<Func<Category, bool>> predicate = c => 1 == 1;
            yield return new object[] { predicate, new List<string> { "" } };
            yield return new object[] { null, null };
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public async Task GetID_ShoudGetDataOrNull(int id)
    {
        var result = await repo.Get(id);
        if (result == null)
        {
            Assert.Null(result);
        }
        else { Assert.Equal(id, result.Id); }
    }
    [Fact]
    public async Task Save_WithZeroIdShouldInsert()
    {
        Category newCategory = new Category
        {
            CreatedBy = 0,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            Name = "Cat 1",
        };
        await repo.Save(newCategory, 1);
        Category dbCategory = await repo.Get(newCategory.Id);
        Assert.NotNull(dbCategory);
    }

    [Theory]
    [MemberData(nameof(GetInsertCategoryData))]
    public async Task Save_WithMissingParameterShouldThrowException(Category newCategory)
    {
        await Assert.ThrowsAnyAsync<Exception>(async () => await repo.Save(newCategory,1));
    }
    public static IEnumerable<object[]> GetInsertCategoryData
    {

        get
        {
            yield return new object[] {
                    new Category
                    {
                        Id = 0,
                        CreatedBy = 1,
                        Description = "Description",
                        IsEnabled = true,
                    }
                };
        }
    }
    [Fact]
    public async Task Save_WithIdShouldUpdate()
    {
        Category newCategory = new Category
        {
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            Name = "Cat Update",
        };
        await repo.Save(newCategory,1);
        Category dbCategory = await repo.Get(newCategory.Id);
        Assert.Equal(dbCategory.Name, newCategory.Name);
    }
    [Fact]
    public async Task Save_WithNonExsistIdShouldThrowException()
    {
        Category newCategory = new Category
        {
            Id=9,
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            Name = "Cat Update",
        };
        await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Save(newCategory,1));
    }
    [Fact]
    public async Task Delete_WithNonExsistIdShouldThrowException()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Delete(30));
    }
    [Fact]
    public async Task Delete_WithRelationsShouldThrowException()
    {
        await Assert.ThrowsAnyAsync<Exception>(async () => await repo.Delete(1));
    }
    [Fact]
    public async Task Delete_ShouldRemoveCategory()
    {
        await repo.Delete(2);
        Assert.Null(await repo.Get(2));

    }
}