namespace SimpleAdmin.API.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class UploaderController : Controller
{
    public static IWebHostEnvironment _environment;
    public IConfiguration _configuration;
    public UploaderController(IWebHostEnvironment environment,IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<string> Upload(IFormFile file, string path = "")
    {
        if (file.Length > 0)
        {
            try
            {
                string extraPath = DateTime.Now.ToString("yyyy/MM/dd");
                path = Path.Combine(path, extraPath);
                path = Path.Combine("uploads", path);
                
                string extension = Path.GetExtension(file.FileName).ToLower();
                List<string> Allowex = _configuration.GetSection("AllowedExtention").Get<List<string>>();
                    
                if (!Allowex.Contains(extension))
                {
                    return null;
                }
                string directoryPath = Path.Combine(@_environment.WebRootPath, @path.Replace('/', '\\'));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                
                string newName = Guid.NewGuid().GetHashCode() + extension;
                path = Path.Combine(@path, @newName);

                using (FileStream filestream = System.IO.File.Create(Path.Combine(@_environment.WebRootPath, @path.Replace('/', '\\'))))
                {
                    await file.CopyToAsync(filestream);
                    filestream.Flush();

                }
                return "/" + path.Replace('\\', '/');
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        else
        {
            return"";
        }

    }
}
