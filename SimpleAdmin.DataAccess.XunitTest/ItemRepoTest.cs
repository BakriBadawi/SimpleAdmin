namespace SimpleAdmin.DataAccess.XunitTest;
public class ItemRepoTest
{
    ItemRepo repo;
    public ItemRepoTest()
    {
        DbFixture dbFixture = new DbFixture();
        repo = new ItemRepo(new DbFixture().DBOption);
        dbFixture.AddDummyData(repo.Context);
    }
    [Theory]
    [MemberData(nameof(GetData))]

    public async Task Get_ShoudGetData(System.Linq.Expressions.Expression<Func<Item, bool>> predicate, List<string> include)
    {
        List<Item> data = await repo.Get(predicate, include).ToListAsync();
        Assert.NotEmpty(data);
    }
    public static IEnumerable<object[]> GetData
    {

        get
        {
            System.Linq.Expressions.Expression<Func<Item, bool>> predicate = c => 1 == 1;
            yield return new object[] { predicate, new List<string> { "" } };
            yield return new object[] { null, null };
        }
    }

    [Theory]
    [MemberData(nameof(GetGuids))]
    public async Task GetID_ShoudGetDataOrNull(Guid id)
    {
        var result = await repo.Get(id);
        if (result == null)
        {
            Assert.Null(result);
        }
        else { Assert.Equal(id, result.Id); }
    }
    public static IEnumerable<object[]> GetGuids
    {

        get
        {
            yield return new object[] {new Guid("f699c009-20ad-411f-bf46-4ba178faeacc") };
            yield return new object[] {new Guid("22db6649-49b5-4cc7-90d7-f0b55de023ee") };
            yield return new object[] {Guid.Empty };
        }
    }
    [Fact]
    public async Task Save_WithEmptyIdShouldInsert()
    {
        Item newItem = new Item
        {
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            CategoryId = 1,
            Summary = "Summary",
            Title = "Title",
        };
        await repo.Save(newItem, 1);
        Item dbItem = await repo.Get(newItem.Id);
        Assert.NotNull(dbItem);
    }

    [Theory]
    [MemberData(nameof(GetInsertItemData))]
    public async Task Save_WithMissingParameterShouldThrowException(Item newItem)
    {
        await Assert.ThrowsAnyAsync<Exception>(async () => await repo.Save(newItem, 1));
    }
    public static IEnumerable<object[]> GetInsertItemData
    {

        get
        {
            yield return new object[] {
                    new Item
                    {
                        CreatedBy = 1,
                        Description = "Description",
                        LastModifiedBy = 1,
                        CategoryId = 1,
                        Summary = "Summary",
                    }
                };
        }
    }
    [Fact]
    public async Task Save_WithIdShouldUpdate()
    {
        Item newItem = new Item
        {
            Id = new Guid("f699c009-20ad-411f-bf46-4ba178faeacc"),
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            CategoryId = 1,
            Summary = "Summary",
            Title = "Title Updated",
        };
        await repo.Save(newItem, 1);
        Item dbItem = await repo.Get(newItem.Id);
        Assert.Equal(dbItem.Title, newItem.Title);
    }
    [Fact]
    public async Task Save_WithNonExsistIdShouldThrowException()
    {
        Item newItem = new Item
        {
            Id = new Guid("9a6aeabf-cac7-4ac8-a36c-f963570c9199"),
            CreatedBy = 1,
            Description = "Description",
            IsEnabled = true,
            LastModifiedBy = 1,
            CategoryId = 1,
            Summary = "Summary",
            Title = "Title Updated",
        };
        await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Save(newItem, 1));
    }
    [Fact]
    public async Task Delete_WithNonExsistIdShouldThrowException()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Delete(new Guid("9a6aeabf-cac7-4ac8-a36c-f963570c9199")));
    }
    [Fact]
    public async Task Delete_ShouldRemoveItem()
    {
        await repo.Delete(new Guid("f699c009-20ad-411f-bf46-4ba178faeacc"));
        Assert.Null(await repo.Get(new Guid("f699c009-20ad-411f-bf46-4ba178faeacc")));

    }
}