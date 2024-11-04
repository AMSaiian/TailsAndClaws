using System.Reflection;

namespace TailsAndClaws.Application;

public static class ApplicationAssemblyType
{
    public static readonly Assembly Reference = typeof(ApplicationAssemblyType).Assembly;
}
