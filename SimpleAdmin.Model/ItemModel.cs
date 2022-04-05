namespace SimpleAdmin.Model;
public class ItemModel
{
    private Item item;
    public ItemModel()
    {
        item = new Item();
    }
    public ItemModel(Item initItem)
    {
        item = initItem;
    }
    [JsonIgnore]
    public Item DBItem
    {
        get { return item; }
    }
    public Guid id
    {
        get { return item.Id; }
        set { item.Id = value; }
    }
    [Required]
    public string title
    {
        get { return item.Title; }
        set { item.Title = value; }
    }
    private CategoryModel _category;
    public CategoryModel Category
    {
        get {
            if (_category==null)
            {
                _category = new CategoryModel(item.Category);
            }
            return _category;
        }
        set {
            CategoryId = value.id;
            _category = value;
        }
    }
    [JsonIgnore]
    public int CategoryId
    {
        get {return (Category != null) ? Category.id : item.CategoryId; }

        set {
            item.CategoryId = value; 
        }
    }

    [Required]
    public string description
    {
        get { return item.Description; }
        set { item.Description = value; }
    }
    [Required]
    public string summary
    {
        get { return item.Summary; }
        set { item.Summary = value; }
    }
    public bool isEnabled
    {
        get { return item.IsEnabled; }
        set { item.IsEnabled = value; }
    }
    public string Photo
    {
        get { return item.Photo; }
        set { item.Photo = value; }
    }
    public string Seodescription
    {
        get { return item.Seodescription; }
        set { item.Seodescription = value; }
    }
    
    public string seokeywords
    {
        get { return item.Seokeywords; }
        set { item.Seokeywords = value; }
    }
    
    public string seotitle
    {
        get { return item.Seotitle; }
        set { item.Seotitle = value; }
    }
    public string Url
    {
        get { return item.Url; }
        set { item.Url = value; }
    } 
}
