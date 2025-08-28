using Application.Abstractions.Authentication;
using Application.Features.StringProcessor.Command;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared;
using Shared.Requests;
using Webly.SignalR.Jobs;


namespace Webly.Controllers
{
    [EnableRateLimiting(Constants.RateLimitingPolicy)]
    [Authorize]

    public class ProcessorController(IBackgroundJobClient backgroundJobClient,
        IUserContext userContext) : BaseController
    {

        [HttpPost("process-string")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ProcessString([FromBody] CreateProcessStringRequest request,
            [FromHeader(Name = "X-Idempotency-Key")] string requestId,
            CancellationToken cancellationToken)
        {

            if (!Guid.TryParse(requestId, out Guid parsedRequestId))
            {
                return BadRequest();
            }

            var command = new CreateProcessStringRequestCommand(parsedRequestId, request.Input);
            var response = await Mediator.Send(command, cancellationToken);

            if (response.IsSuccess)
            {
                string jobId = backgroundJobClient.Enqueue<StringProcessorJob>(
                    job => job.ExecuteAsync(userContext.UserId.ToString(), cancellationToken));
                return Ok(new Result<string>(jobId, true, Error.None));

            }

            return BadRequest(response);

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
