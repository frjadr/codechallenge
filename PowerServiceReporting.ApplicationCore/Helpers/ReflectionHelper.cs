using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
    }
}
