using System.Runtime.CompilerServices;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    /// <summary>
    /// Helper for code reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets the current method name from async function.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
    }
}
