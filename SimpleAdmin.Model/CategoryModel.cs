namespace SimpleAdmin.Model;
public class CategoryModel
{
    private Category category;
    public CategoryModel()
    {
        category = new Category();
    }
    public CategoryModel(Category initCategory)
    {
        if (initCategory != null)
        {
            category = initCategory;
        }
        else
        {
            category = new Category();
        }
    }
    [JsonIgnore]
    public Category DBCategory
    {
        get { return category; }
    }
    public int id
    {
        get { return category.Id; }
        set { category.Id = value; }
    }
    [Required]
    public string description
    {
        get { return category.Description; }
        set { category.Description = value; }
    }
    [Required]
    public string Name
    {
        get { return category.Name; }
        set { category.Name = value; }
    }
    public string Photo
    {
        get { return category.Photo; }
        set { category.Photo = value; }
    }
    public string Seodescription
    {
        get { return category.Seodescription; }
        set { category.Seodescription = value; }
    }
    
    public string seokeywords
    {
        get { return category.Seokeywords; }
        set { category.Seokeywords = value; }
    }
    
    public string seotitle
    {
        get { return category.Seotitle; }
        set { category.Seotitle = value; }
    }
    public string Url
    {
        get { return category.Url; }
        set { category.Url = value; }
    }
    public bool isEnabled
    {
        get { return category.IsEnabled; }
        set { category.IsEnabled = value; }
    }
    
}
