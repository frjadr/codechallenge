using AutoMapper;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class AutoMapperHelper
    {
        public static List<TDestination> MapList<TSource, TDestination>(this IMapper mapper, List<TSource> source)
        {
            return source.Select(x => mapper.Map<TDestination>(x)).ToList();
        }
    }
}
