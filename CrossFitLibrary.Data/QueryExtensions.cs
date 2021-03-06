using System;
using System.Linq;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Data;

public static class QueryExtensions
{

        public static int LatestVersion<TSource>(this IQueryable<TSource> source, int offset = 0)
        where TSource : VersionedModel
        {
            return source.Max(x => x.Version) + offset;
        }
}
