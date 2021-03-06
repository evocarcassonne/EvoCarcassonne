﻿using System.Collections.Generic;
namespace DotNetCoreWebApi.Backend
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
