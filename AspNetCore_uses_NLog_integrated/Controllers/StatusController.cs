using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore_uses_NLog_integrated.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class StatusController : ControllerBase
	{
		private readonly ILogger<StatusController> _logger;

		public StatusController(ILogger<StatusController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public string Get()
		{
			_logger.LogInformation("AspNetCore_uses_NLog_integrated - status endpoint has been called");
			return "Hallo!!! AspNetCore_uses_NLog_integrated!!!";
		}
	}
}
