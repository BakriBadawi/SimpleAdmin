namespace SimpleAdmin.API.XunitTest;

public class UserControllerTest
{
    UserRepo repo;
    UserController controller;
    public UserControllerTest()
    {
        string dbName = Guid.NewGuid().ToString();
        DbFixture dbFixture = new DbFixture(dbName);
        repo = new UserRepo(dbFixture.DBOption);
        dbFixture.AddDummyData(repo.Context);
        controller = new UserController(repo);
        Mock.AuthorizedAccess(dbFixture, dbName);
    }
    [Theory]
    [InlineData(0,1,"","")]
    [InlineData(0,0,"","")]
    [InlineData(10,500,"","")]
    [InlineData(0, 10, "fullname;email", "test")]
    [InlineData(0, 10, "fullname", "test")]
    [InlineData(0, 10, "email", "test")]
    [InlineData(0, 10, "email;fullname", "test")]
    [InlineData(0, 10, "email;Fullname", "admin")]
    public void Get_ShoulGetData(int skip,int take,string filters,string filtertxt)
    {
        
        OkObjectResult result= (OkObjectResult) controller.Get(skip, take, filters, filtertxt);
        List<UserModel> users = result.Value as List<UserModel>;
        List<UserModel> filterUser = users;
        if (users!=null && filtertxt!=null)
        {
            if (filters.ToLower().Contains("email"))
            {
                filterUser = filterUser.Where(c => 
                 c.email.Contains(filtertxt)).ToList();
            }
            if (filters.ToLower().Contains("fullname"))
            {
                filterUser = filterUser.Where(c => c.fullName.Contains(filtertxt)).ToList();
            }
        }
        Assert.NotNull(users);
        Assert.Equal(users, filterUser);

    }
    [Theory]
    [InlineData(-1, 1, "", "")]
    [InlineData(0, -1, "", "")]
    [InlineData(10, 501, "", "")]
   
    public void Get_WithWrongArgument_ShouldReturn400(int skip, int take, string filters, string filtertxt)
    {
        ObjectResult result = (ObjectResult)controller.Get(skip, take, filters, filtertxt);
        Assert.Equal(400,result.StatusCode);
    }
    [Theory]
    [InlineData(0, 10, "undefined", "test")]
    [InlineData(0, 10, "emaill", "test")]
    [InlineData(0, 10, "email;full name", "test")]

    public void Get_WithWrongArgument_ShouldThrow(int skip, int take, string filters, string filtertxt)
    {
        Assert.Throws<ArgumentException>(() => controller.Get(skip, take, filters, filtertxt));
        
    }
    [Theory]
    [InlineData("", "")]
    [InlineData("fullname;email", "test")]
    [InlineData("fullname", "test")]
    [InlineData("email", "test")]
    [InlineData("email;fullname", "test")]
    [InlineData("email;Fullname", "admin")]
    public void GetCount_ShoulGetCount(string filters, string filtertxt)
    {
        OkObjectResult result = (OkObjectResult)controller.GetCount(filters, filtertxt);
        int? count = result.Value as int?;
        Assert.NotNull(count);
    }
    [Theory]
    [InlineData("undefined", "test")]
    [InlineData("emaill", "test")]
    [InlineData("email;full name", "test")]

    public void GetCount_WithWrongArgument_ShouldThrow(string filters, string filtertxt)
    {
        Assert.Throws<ArgumentException>(() => controller.GetCount( filters, filtertxt));

    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(0)]
    public async void GetById_ShoulGetData(int id)
    {
        OkObjectResult result = (OkObjectResult) await controller.GetById(id);
        UserModel users = result.Value as UserModel;

        Assert.NotNull(users);
    }

    [Theory]
    [MemberData(nameof(GetUserToSave))]
    public async void Save_ShoulSaveData(UserModel user)
    {
        OkResult result = (OkResult)await controller.Save(user);
        Assert.NotNull(result);
        User dbUser =  repo.Get(c => c.FullName == user.fullName).FirstOrDefault();
        Assert.NotNull(dbUser);
    }
  
    public static IEnumerable<object[]> GetUserToSave
    {

        get
        {
            yield return new object[] {
                    new UserModel
                    {
                        email = "test@test.com",
                        fullName = "SaveUser1",
                        isEnabled = true,
                        password = "1123",
                        photo = "//"
                    }
                };
            yield return new object[] {
                    new UserModel
                    {
                        id=1,
                        email = "test@test.com",
                        fullName = "SaveUser2",
                        isEnabled = true,
                        password = "1123",
                        photo = "//"
                    }
                };
        }
    }

    [Fact]
    public async void Delete_ShoulRemoveData()
    {
        OkResult result = (OkResult)await controller.Delete(2);
        Assert.NotNull(result);
        User dbUser = await repo.Get(2);
        Assert.Null(dbUser);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(0)]
    public async void Delete_RelatedUserShouldthrow(int id)
    {
       await Assert.ThrowsAnyAsync<Exception>(async () => await controller.Delete(id));
     
        
    }
}
