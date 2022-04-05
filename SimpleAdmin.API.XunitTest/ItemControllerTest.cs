namespace SimpleAdmin.API.XunitTest;

public class ItemControllerTest
{
    ItemRepo repo;
    ItemController controller;
    public ItemControllerTest()
    {
        string dbName = Guid.NewGuid().ToString();
        DbFixture dbFixture = new DbFixture(dbName);
        repo = new ItemRepo(dbFixture.DBOption);
        dbFixture.AddDummyData(repo.Context);
        controller = new ItemController(repo);
        Mock.AuthorizedAccess(dbFixture, dbName);
    }
    [Theory]
    [InlineData(0,1,"","")]
    [InlineData(0,0,"","")]
    [InlineData(10,500,"","")]
    [InlineData(0, 10, "title;categoryname;url", "test")]
    [InlineData(0, 10, "title;categoryname", "test")]
    [InlineData(0, 10, "title;url", "test")]
    [InlineData(0, 10, "title", "test")]
    [InlineData(0, 10, "url", "test")]
    [InlineData(0, 10, "categoryname", "test")]
    [InlineData(0, 10, "url;title", "test")]
    [InlineData(0, 10, "url;categoryname", "Cat 1")]
    public void Get_ShoulGetData(int skip,int take,string filters,string filtertxt)
    {
        
        OkObjectResult result= (OkObjectResult) controller.Get(skip, take, filters, filtertxt);
        List<ItemModel> items = result.Value as List<ItemModel>;
        List<ItemModel> filterItems = items;
        if (items != null && filtertxt!=null)
        {
            if (filters.ToLower().Contains("title"))
            {
                filterItems = filterItems.Where(c => c.title.Contains(filtertxt)).ToList();
            }

            if (filters.ToLower().Contains("categoryname"))
            {
                filterItems = filterItems.Where(c => 
                 c.Category.Name.Contains(filtertxt)).ToList();
            }
            if (filters.ToLower().Contains("url"))
            {
                filterItems = items.Where(c => c.Url.Contains(filtertxt)).ToList();
            }
            if (filters.ToLower().Contains("seotitle"))
            {
                filterItems = items.Where(c => c.seotitle.Contains(filtertxt)).ToList();
            }
        }
        Assert.NotNull(items);
        Assert.Equal(items, filterItems);

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
    [InlineData("title;categoryname;url", "test")]
    [InlineData("title;categoryname", "test")]
    [InlineData("title;url", "test")]
    [InlineData("title", "test")]
    [InlineData("url", "test")]
    [InlineData("categoryname", "test")]
    [InlineData("url;title", "test")]
    [InlineData("url;categoryname", "Cat 1")]
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
    [MemberData(nameof(GetGuids))]

    public async void GetById_ShoulGetData(Guid id)
    {
        OkObjectResult result = (OkObjectResult) await controller.GetById(id);
        ItemModel users = result.Value as ItemModel;

        Assert.NotNull(users);
    }
    public static IEnumerable<object[]> GetGuids
    {

        get
        {
            yield return new object[] {
                    new Guid("f699c009-20ad-411f-bf46-4ba178faeacc")
                };
            yield return new object[] {
                    Guid.NewGuid()
                };
        }
    }

    [Theory]
    [MemberData(nameof(GetUserToSave))]
    public async void Save_ShoulSaveData(ItemModel item)
    {
        OkResult result = (OkResult)await controller.Save(item);
        Assert.NotNull(result);
        Item dbCategory =  repo.Get(c => c.Title == item.title).FirstOrDefault();
        Assert.NotNull(dbCategory);
    }
  
    public static IEnumerable<object[]> GetUserToSave
    {

        get
        {
            yield return new object[] {
                    new ItemModel
                    {
                        description="Description",
                        title = "item 1",
                        summary = "summary 1",
                        seotitle="seo title",
                        Url="item1",
                        Photo="//"
                    }
                };
            yield return new object[] {
                    new ItemModel
                    {
                        description="Description",
                        title = "item 2",
                        summary = "summary 2",
                        seotitle="seo title",
                        Url="item2",
                        Photo="//"
                    }
                };
        }
    }

    [Fact]
    public async void Delete_ShoulRemoveData()
    {
        OkResult result = (OkResult)await controller.Delete(new Guid("f699c009-20ad-411f-bf46-4ba178faeacc"));
        Assert.NotNull(result);
        Item dbUser = await repo.Get(new Guid("f699c009-20ad-411f-bf46-4ba178faeacc"));
        Assert.Null(dbUser);
    }
}
