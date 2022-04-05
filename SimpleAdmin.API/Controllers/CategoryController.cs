namespace SimpleAdmin.API.Controllers;
[ApiController]
[Route("[controller]/[action]")]
[AutoLog,Authorize]
public class CategoryController : ControllerBase
{
    public DataAccess.Repository.IRepo<Category, int> _repo { get; }

    public CategoryController(IRepo<Category,int> repo)
    {
        _repo = repo;
    }

    [HttpGet("{skip}/{take}")]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(List<Category>))]

    public IActionResult Get(int skip, int take, string filters = "", string filtertxt = "")
    {
        if (skip < 0 || take < 0)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, "skip/take parameter value should be a positive integer");
        }
        if (take > 500)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, "take parameter value should be a less than 500");
        }
        IQueryable<Category> query = GetQuery(filters, filtertxt);
        return Ok(query.Skip(skip).Take(take).Select(c => new CategoryModel(c)).ToList());
    }
  
    [HttpGet]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(int))]
    public IActionResult GetCount(string filters , string filtertxt)
    {
        IQueryable<Category> query = GetQuery(filters, filtertxt);
        return Ok(query.Count());
    }
    [HttpGet("{id}")]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CategoryModel))]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(new CategoryModel(await _repo.Get(id)));
    }
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] CategoryModel Category)
    {
        if (!ModelState.IsValid)
        {
            throw new ArgumentException(ModelState.GetErrorMessages());
        }
        await _repo.Save(Category.DBCategory, AuthorizeAttribute.AuthUser.User.id);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.Delete(id);
        return Ok();
    }
    
    private IQueryable<Category> GetQuery(string filters, string filtertxt)
    {
        List<string> filtelst = new List<string>();
        if (!string.IsNullOrEmpty(filters))
        {
            filtelst = filters.ToLower().Split(';').ToList();
        }
        IQueryable<Category> query = _repo.Get(c => 1 == 1);
        if (filtelst != null && filtelst.Count != 0 && !string.IsNullOrEmpty(filtertxt))
        {
            foreach (string item in filtelst)
            {
                switch (item)
                {
                    case "name":
                        query = query.Where(c => c.Name.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    case "seotitle":
                        query = query.Where(c => c.Seotitle.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    case "url":
                        query = query.Where(c => c.Url.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    default:
                        if (item.Trim() != "")
                        {
                            throw new ArgumentException("The filters parameter should have either name, seotitle, url or it is combination separated by ;");
                        }
                        break;
                }
            }
        }

        return query;
    }
}
