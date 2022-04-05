namespace SimpleAdmin.API.XunitTest;

public class CategoryControllerTest
{
    CategoryRepo repo;
    CategoryController controller;
    public CategoryControllerTest()
    {
        string dbName = Guid.NewGuid().ToString();
        DbFixture dbFixture = new DbFixture(dbName);
        repo = new CategoryRepo(dbFixture.DBOption);
        dbFixture.AddDummyData(repo.Context);
        controller = new CategoryController(repo);
        Mock.AuthorizedAccess(dbFixture, dbName);
    }
    [Theory]
    [InlineData(0,1,"","")]
    [InlineData(0,0,"","")]
    [InlineData(10,500,"","")]
    [InlineData(0, 10, "name;seotitle;url", "test")]
    [InlineData(0, 10, "name;seotitle", "test")]
    [InlineData(0, 10, "name;url", "test")]
    [InlineData(0, 10, "name", "test")]
    [InlineData(0, 10, "url", "test")]
    [InlineData(0, 10, "seotitle", "test")]
    [InlineData(0, 10, "url;name", "test")]
    [InlineData(0, 10, "url;name", "Cat 1")]
    public void Get_ShoulGetData(int skip,int take,string filters,string filtertxt)
    {
        
        OkObjectResult result= (OkObjectResult) controller.Get(skip, take, filters, filtertxt);
        List<CategoryModel> categories = result.Value as List<CategoryModel>;
        List<CategoryModel> filterCategories = categories;
        if (categories != null && filtertxt!=null)
        {
            if (filters.ToLower().Contains("name"))
            {
                filterCategories = filterCategories.Where(c => c.Name.Contains(filtertxt)).ToList();
            }

            if (filters.ToLower().Contains("seotitle"))
            {
                filterCategories = filterCategories.Where(c => 
                 c.seotitle.Contains(filtertxt)).ToList();
            }
            if (filters.ToLower().Contains("url"))
            {
                filterCategories = categories.Where(c => c.Url.Contains(filtertxt)).ToList();
            }
        }
        Assert.NotNull(categories);
        Assert.Equal(categories, filterCategories);

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
    [InlineData(0, 10, "urll", "test")]
    [InlineData(0, 10, "seo;full name", "test")]

    public void Get_WithWrongArgument_ShouldThrow(int skip, int take, string filters, string filtertxt)
    {
        Assert.Throws<ArgumentException>(() => controller.Get(skip, take, filters, filtertxt));
        
    }
    [Theory]
    [InlineData("", "")]
    [InlineData("name;seotitle;url", "test")]
    [InlineData("name;seotitle", "test")]
    [InlineData("name;url", "test")]
    [InlineData("name", "test")]
    [InlineData("url", "test")]
    [InlineData("seotitle", "test")]
    [InlineData("url;name", "test")]
    [InlineData("url;name", "Cat 1")]
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
        CategoryModel users = result.Value as CategoryModel;

        Assert.NotNull(users);
    }

    [Theory]
    [MemberData(nameof(GetUserToSave))]
    public async void Save_ShoulSaveData(CategoryModel category)
    {
        OkResult result = (OkResult)await controller.Save(category);
        Assert.NotNull(result);
        Category dbCategory =  repo.Get(c => c.Name == category.Name).FirstOrDefault();
        Assert.NotNull(dbCategory);
    }
  
    public static IEnumerable<object[]> GetUserToSave
    {

        get
        {
            yield return new object[] {
                    new CategoryModel
                    {
                        description="Description",
                        Name = "cat test 1",
                        seotitle="cat title",
                        Url="cate-test-1",
                        Photo="//"
                    }
                };
            yield return new object[] {
                    new CategoryModel
                    {
                        description="Description 2",
                        Name = "cat test 2",
                        seotitle="cat title 2",
                        Url="cate-test-2",
                        Photo="//"
                    }
                };
        }
    }

    [Fact]
    public async void Delete_ShoulRemoveData()
    {
        OkResult result = (OkResult)await controller.Delete(2);
        Assert.NotNull(result);
        Category dbUser = await repo.Get(2);
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
