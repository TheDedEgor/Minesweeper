using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sapper.Infrastructure.Extensions
{
    internal static class MyExtensions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> enumerable) => new(enumerable);
    }
}
