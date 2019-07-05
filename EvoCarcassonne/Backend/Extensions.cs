using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace EvoCarcassonne.Backend
{
    public static class Extensions
    {
        public static T RemoveAndGet<T>(this IList<T> list, int index)
        {
            lock (list)
            {
                var value = list[index];
                list.RemoveAt(index);
                return value;
            }
        }
    }
}
