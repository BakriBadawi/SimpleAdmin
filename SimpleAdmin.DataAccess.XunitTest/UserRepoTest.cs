namespace SimpleAdmin.DataAccess.XunitTest;
public class UserRepoTest
{
    UserRepo repo;
    public UserRepoTest()
    {
        DbFixture dbFixture= new DbFixture();
        repo = new UserRepo(new DbFixture().DBOption);
        dbFixture.AddDummyData(repo.Context);
    }
    [Theory]
    [MemberData(nameof(GetData))]

    public async Task Get_ShoudGetData(System.Linq.Expressions.Expression<Func<User, bool>> predicate, List<string> include)
    {
        List<User> data = await repo.Get(predicate, include).ToListAsync();
        Assert.NotEmpty(data);
    }
    public static IEnumerable<object[]> GetData
    {

        get
        {
            System.Linq.Expressions.Expression<Func<User, bool>> predicate = c => 1 == 1;
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
        User newUser = new User
        {
            Id = 0,
            FullName = "test",
            Email = "ee@ee.com",
            Password = "123",
            IsEnabled = false
        };
        await repo.Save(newUser,1);
        User dbUser = await repo.Get(newUser.Id);
        Assert.NotNull(dbUser);
    }

    [Theory]
    [MemberData(nameof(GetInsertUserData))]
    public async Task Save_WithMissingParameterShouldThrowException(User newUser)
    {
        await Assert.ThrowsAnyAsync<Exception>(async () => await repo.Save(newUser,1));
    }
    public static IEnumerable<object[]> GetInsertUserData
    {

        get
        {
            yield return new object[] {
                    new User
                    {
                        Id = 0,
                        Email = "ee@ee.com",
                        Password = "123",
                        IsEnabled = false
                    }
                };
            yield return new object[] {
                    new User
                    {
                        Id = 0,
                        FullName = "test1",
                        Password = "123",
                        IsEnabled = false
                    }
                };
            yield return new object[] {
                    new User
                    {
                        Id = 0,
                        FullName = "test1",
                        Email = "ee@ee.com",
                        IsEnabled = false
                    }
                };
        }
    }
    [Fact]
    public async Task Save_WithIdShouldUpdate()
    {
        User newUser = new User
        {
            Id = 1,
            FullName = "test1",
            Email = "ee@ee.com",
            Password = "123",
            IsEnabled = false
        };
        await repo.Save(newUser,1);
        User dbUser = await repo.Get(newUser.Id);
        Assert.Equal(dbUser.FullName, newUser.FullName);
    }
    [Fact]
    public async Task Save_WithNonExsistIdShouldThrowException()
    {
        User newUser = new User
        {
            Id = 5,
            FullName = "test1",
            Email = "ee@ee.com",
            Password = "123",
            IsEnabled = false
        };
        await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Save(newUser,1));
    }
    [Fact]
    public async Task Delete_WithNonExsistIdShouldThrowException()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(async () => await repo.Delete(50));
    }
    [Fact]
    public async Task Delete_WithRelationsShouldThrowException()
    {
        await Assert.ThrowsAnyAsync<Exception>(async ()=>await repo.Delete(1));
    }
    [Fact]
    public async Task Delete_ShouldRemoveUser()
    {
        await repo.Delete(2);
        Assert.Null(await repo.Get(2));
    }
}