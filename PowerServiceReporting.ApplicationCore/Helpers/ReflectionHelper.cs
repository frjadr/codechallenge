using System.Runtime.CompilerServices;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
    }
}
