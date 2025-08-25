using Shared;

namespace Domain.Procesor
{
    public static class ProcessStringErrors
    {

        public static readonly Error BadRequest = Error.Problem(
          "BadRequest",
          "Enter a valid input string");
    }
}
