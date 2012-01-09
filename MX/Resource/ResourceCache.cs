using System;
using System.Collections.Generic;

namespace MX
{

    public interface IResource<T>
    {
        T Load(string path);
    }

    public static class ResourceCache<T> where T : IResource<T>, new()
    {
        private static Dictionary<string, T> Cache = new Dictionary<string, T>();

        public static T Get(string key)
        {
            if (!Cache.ContainsKey(key))
                Cache.Add(key,(T)(new T()).Load(key));

            return (Cache.ContainsKey(key) ? Cache[key] : default(T));
        }
    }

}