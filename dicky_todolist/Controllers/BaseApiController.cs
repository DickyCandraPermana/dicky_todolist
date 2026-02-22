using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dicky_todolist.Controllers
{
    // Controllers/BaseApiController.cs
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected Guid GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userId!);
        }
    }
}
