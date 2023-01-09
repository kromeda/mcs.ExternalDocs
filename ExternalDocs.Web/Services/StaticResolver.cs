namespace ExternalDocs.Web.Services;

public class StaticResolver
{
    private readonly IWebHostEnvironment _environment;
    private readonly IDictionary<string, string> _cachedMarkups;

    public StaticResolver(IWebHostEnvironment environment)
    {
        _environment = environment;
        _cachedMarkups = new ConcurrentDictionary<string, string>();
    }

    public async Task<string> GetMarkup(string fileName)
    {
        if (!_cachedMarkups.TryGetValue(fileName, out string content))
        {
            string markupPath = Path.Combine(_environment.WebRootPath, "markup", fileName);
            content = await File.ReadAllTextAsync(markupPath, Encoding.UTF8);
            _cachedMarkups.Add(fileName, content);
        }

        return content;
    }
}
