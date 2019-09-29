using System;
using System.Collections.Generic;

namespace Core {
    public static class KthLargestElement<T> {
        private static Random _random = null;
        public static T GetKth(List<T> Elements,int k, Predicate<Tuple<T,T>> Small) {
            if (_random == null) _random = new Random();
            if (Elements.Count == 1) return Elements[0];
            var randomComparer = Elements[_random.Next(Elements.Count)];
            var SmallElements = Elements.FindAll(x => Small(new Tuple<T, T>(x, randomComparer)));
            if (SmallElements.Count == k - 1) return randomComparer;
            if (SmallElements.Count >= k) return GetKth(SmallElements, k, Small);
            var NoSmallElements = Elements.FindAll(x => Small(new Tuple<T, T>(randomComparer, x)));
            return k > Elements.Count - NoSmallElements.Count + 1
                ? GetKth(NoSmallElements, k + NoSmallElements.Count - Elements.Count, Small)
                : randomComparer;
        }
    }
}