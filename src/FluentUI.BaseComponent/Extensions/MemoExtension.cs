using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FluentUI.Extensions
{
    // https://trenki2.github.io/blog/2018/12/31/memoization-in-csharp/

    public static class Memoizer
    {
        public static Func<R?>? Memoize<R>(Func<R?> func)
        {
            if (func == null)
                throw new Exception("Supplied function cannot be null.");

            object? cache = null;
            return () =>
            {
                if (cache == null)
                    cache = func();
                return (R)cache;
            };
        }
        
        public static Func<A, R> Memoize<A, R>(Func<A, R> func)
        {
            var cache = new Dictionary<A, R>();
            return a =>
            {
                if (cache.TryGetValue(a, out R value))
                    return value;
                value = func(a);
                cache.Add(a, value);
                return value;
            };
        }

        public static Func<A,B,R> Memoize<A,B,R>(Func<A,B,R> func)
        {
            
            var cache = new Dictionary<(A, B), R>();
            return (a,b) =>
            {
                if (cache.TryGetValue((a,b), out R value))
                    return value;
                value = func(a,b);
                cache.Add((a,b), value);
                return value;
            };
        }


        public static Func<A, R> ThreadSafeMemoize<A, R>(Func<A, R> func)
        {
            var cache = new ConcurrentDictionary<A, R>();
            return argument => cache.GetOrAdd(argument, func(argument));
        }

        public static Func<A,B, R> ThreadSafeMemoize<A,B, R>(Func<A,B, R> func)
        {
            var cache = new ConcurrentDictionary<(A,B), R>();
            return (a, b) =>
            {
                if (cache.TryGetValue((a, b), out R value))
                    return value;
                value = func(a, b);
                cache.TryAdd((a, b), value);
                return value;
            };
        }
    }

    public static class MemoizerExtensions
    {
        public static Func<R> Memoize<R>(this Func<R> func)
        {
            return Memoizer.Memoize(func);
        }

        public static Func<A, R> Memoize<A, R>(this Func<A, R> func)
        {
            return Memoizer.Memoize(func);
        }
        public static Func<A,B, R> Memoize<A,B, R>(this Func<A,B, R> func)
        {
            return Memoizer.Memoize(func);
        }

        public static Func<A, R> ThreadSafeMemoize<A, R>(this Func<A, R> func)
        {
            return Memoizer.ThreadSafeMemoize(func);
        }

        public static Func<A,B, R> ThreadSafeMemoize<A,B, R>(this Func<A,B, R> func)
        {
            return Memoizer.ThreadSafeMemoize(func);
        }
    }
}

