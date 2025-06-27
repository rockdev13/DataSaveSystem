using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace SaveLoadSystem
{
    public class DataCacheManager
    {
        public Dictionary<string, object> Cache;

        public DataCacheManager()
        {
            Cache = new Dictionary<string, object>();
        }

        private readonly object _cacheLock = new object();

        public void SetValue(string key, object value)
        {
            lock (_cacheLock)
            {
                Cache[key] = value;
            }
        }

        public T GetValue<T>(string key)
        {
            if (!Cache.TryGetValue(key, out var value)) return default;

            if (value is T directValue) return directValue;

            return JsonConvert.DeserializeObject<T>(value.ToString(), JsonConverters.JsonSettings);
        }

        public void RemoveValue(string key)
        {
            Cache.Remove(key);
        }

        public Dictionary<string, object> FromJson(string json)
        {
            Cache = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return Cache;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Cache, JsonConverters.JsonSettings);
        }
    }
}

