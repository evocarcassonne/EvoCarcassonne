using System;
using System.Collections.Generic;

namespace EvoCarcassonne
{
    public static class Mediator
    {
        private static readonly IDictionary<string, List<Action<object>>> Dictionary =
            new Dictionary<string, List<Action<object>>>();

        public static void Subscribe(string token, Action<object> callback)
        {
            if (!Dictionary.ContainsKey(token))
            {
                var list = new List<Action<object>>();
                list.Add(callback);
                Dictionary.Add(token, list);
            }
            else
            {
                var found = false;
                foreach (var item in Dictionary[token])
                {
                    if (item.Method.ToString() == callback.Method.ToString())
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    Dictionary[token].Add(callback);
                }
            }
        }

        public static void Unsubscribe(string token, Action<object> callback)
        {
            if (Dictionary.ContainsKey(token))
            {
                Dictionary[token].Remove(callback);
            }
        }

        public static void Notify(string token, object args = null)
        {
            if (Dictionary.ContainsKey(token))
            {
                foreach (var callback in Dictionary[token])
                {
                    callback(args);
                }
            }
        }
    }
}
