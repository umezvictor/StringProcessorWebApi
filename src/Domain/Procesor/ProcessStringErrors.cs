using Shared;

namespace Domain.Procesor
{
    public static class ProcessStringErrors
    {

        public static readonly Error BadRequest = Error.Problem(
          "BadRequest",
          "Enter a valid input string");

        public static readonly Error TooManyRequests = Error.Problem(
         "TooManyRequests",
         "You can only process one request at a time");
    }
}
