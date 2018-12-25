using System;
using System.Collections.Generic;

namespace Dados
{
    public static class IListExtensions
    {
        private static Random _random = new Random();


        public static T GetRandomElement<T>(this IList<T> list)
        {
            var index = _random.Next(0, list.Count);

            return list[index];
        }
    }
}
