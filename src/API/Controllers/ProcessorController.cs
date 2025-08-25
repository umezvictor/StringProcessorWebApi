using Application.Abstractions.Authentication;
using Application.Features.StringProcessor.Command;
using Domain.Procesor;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared;
using Shared.Requests;
using Webly.Jobs;


namespace Webly.Controllers
{


    [EnableRateLimiting(Constants.RateLimitingPolicy)]
    [Authorize]
    public class ProcessorController(IBackgroundJobClient backgroundJobClient, IUserContext userContext) : BaseController
    {

        [HttpPost("process-string")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ProcessString([FromBody] CreateProcessStringRequestCommand command, CancellationToken cancellationToken)
        {
            if (await Mediator.Send(command))
            {
                string jobId = backgroundJobClient.Enqueue<StringProcessorJob>(
                job => job.ExecuteAsync(userContext.UserId.ToString(), cancellationToken));
                return Ok(new Result<string>(jobId, true, Error.None));
            }
            return BadRequest(new Result<string>(null, false, ProcessStringErrors.BadRequest));
        }


        [HttpPost("cancel-job")]
        public async Task<IActionResult> CancelBackgroundJob([FromBody] CancelJobRequest request)
        {
            await Task.Run(() =>
            {
                //mark job as deleted and trigger the job cancellation
                BackgroundJob.Delete(request.JobId);
            });
            return Ok(new Result<string>(null, true, Error.None));
        }

    }


}
