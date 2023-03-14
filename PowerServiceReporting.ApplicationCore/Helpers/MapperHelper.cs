using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class MapperHelper
    {
        public static List<TDestination> MapList<TSource, TDestination>(this IMapper mapper, List<TSource> source)
        {
            return source.Select(x => mapper.Map<TDestination>(x)).ToList();
        }
    }
}
