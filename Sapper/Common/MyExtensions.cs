using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Minesweeper.Common
{
    internal static class MyExtensions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> enumerable) => new(enumerable);
    }
}
