using System.Reflection;

namespace AMSaiian.Shared.Application;

public static class SharedApplicationAssemblyType
{
    public static readonly Assembly Reference = typeof(SharedApplicationAssemblyType).Assembly;
}
