using CSharpGetStarted.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CSharpGetStarted.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
    }
}
