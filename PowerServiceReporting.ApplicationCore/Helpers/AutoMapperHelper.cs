using AutoMapper;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    /// <summary>
    /// AutoMapper support helper.
    /// </summary>
    public static class AutoMapperHelper
    {
        /// <summary>
        /// Heplps Autommaper to map two lists via ceneric implementation.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapList<TSource, TDestination>(this IMapper mapper, List<TSource> source)
        {
            return source.Select(x => mapper.Map<TDestination>(x)).ToList();
        }
    }
}
