using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Webly.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>() ?? null!;

        protected string GetUserId()
        {
            return User.Claims.First(i => i.Type == "UserId").Value;
        }

        protected string GetEmail()
        {
            return User.Claims.First(i => i.Type == "Email").Value;
        }
    }
}
