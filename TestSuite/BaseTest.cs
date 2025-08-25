using Application.Features.StringProcessor.Command;
using Domain.Users;
using Infrastructure.Database;
using System.Reflection;
using Webly;

namespace TestSuite
{
    public abstract class BaseTest
    {
        protected static readonly Assembly DomainAssembly = typeof(User).Assembly;
        protected static readonly Assembly ApplicationAssembly = typeof(CreateProcessStringRequestCommand).Assembly;
        protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
        protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;
    }
}
