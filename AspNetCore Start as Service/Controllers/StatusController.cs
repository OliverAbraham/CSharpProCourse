using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAlsServiceStarten.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class StatusController : ControllerBase
	{
		[HttpGet]
		public string Get()
		{
			return "Hallo";
		}
	}
}
