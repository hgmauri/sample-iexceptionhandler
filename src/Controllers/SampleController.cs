using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Sample.IExceptionHandler.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
	[HttpGet]
	public IActionResult GetAsync()
	{
		throw new ArgumentException("Teste erro");
	}
}
