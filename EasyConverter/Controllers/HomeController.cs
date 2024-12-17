using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyConverter.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConverterService _converter;

    public HomeController(ILogger<HomeController> logger, IConverterService converter)
    {
        _logger = logger;
        _converter = converter;
    }

    public IActionResult Index()
    {
        return View();
    }

    [RequestSizeLimit(50000000)]
    [HttpPost]
    public async Task<IActionResult> Index(IFormFile file, CancellationToken cancellationToken)
    {
        _logger.LogDebug("HomeController -> POST Index start");

        await using var stream = file.OpenReadStream();
        var resultBytes = Array.Empty<byte>();
        
        try
        {
            resultBytes = await _converter.ConvertAsync(stream, true, cancellationToken);
        }
        catch
        {
            stream.Close();
        }

        _logger.LogDebug("HomeController -> POST Index end");

        return File(resultBytes, "video/webm", file.FileName + "_" + DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + ".webm");
    }
}
