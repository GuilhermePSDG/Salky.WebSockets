using Salky.WebSockets.Contracts;
using Salky.WebSockets.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace Salky.WebSockets.Exensions
{

    public static class StorageExtensions
    {
        public static void Add<T>(this IStorage storage, T value) where T : notnull
        {
            storage.Add(GetTypeName<T>(), value);
        }
        public static bool Remove<T>(this IStorage storage)
        {
            return storage.Remove(GetTypeName<T>());
        }
        public static T Get<T>(this IStorage storage)
        {
            if (storage.TryGet<T>(out var res))
                return res;
            throw new KeyNotFoundException();
        }
        public static bool TryGet<T>(this IStorage storage, [NotNullWhen(true)] out T? result)
        {
            switch ((T?)storage.Get(GetTypeName<T>()))
            {
                case JsonObject objt:
                    result = objt.Deserialize<T>();
                    return result is not null;
                case T res:
                    result = res;
                    return true;
                case null:
                    result = default;
                    return false;
                default:
                    throw new UnSuportedTypeException();
            }
        }

        private static string GetTypeName<T>()
        {
            return typeof(T).Name;
        }
    }
}
