using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Caching.Models
{
    public static class InMemoryRepository
    {
        private const long UnsavedId = default(long);
        private static readonly Dictionary<Type, Dictionary<long, IAmModelOfEntity>> Storage = new Dictionary<Type, Dictionary<long, IAmModelOfEntity>>();
        private static long IdGeneratorSeed = 1;

        static InMemoryRepository()
        {
            var allModelTypes = Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.GetInterfaces().Contains(typeof(IAmModelOfEntity)));

            foreach (var modelType in allModelTypes)
            {
                Storage[modelType] = new Dictionary<long, IAmModelOfEntity>();
            }
        }

        public static void Save(IAmModelOfEntity obj)
        {
            var id = GetProp<long>(obj);

            if (id == UnsavedId)
            {
                id = GetNextId();
                SetProp(obj, id);
                Storage[obj.GetType()][id] = obj;
                return;
            }

            ApplyVersioning(obj);
            Storage[obj.GetType()][id] = obj;
        }

        public static T Get<T>(long id)
        {
            return (T)Storage[typeof(T)][id];
        }

        // infrastructure
        private static void ApplyVersioning(object obj)
        {
            if (obj is IVersionable)
            {
                var version = GetProp<long>(obj, "Version");
                version++;
                SetProp(obj, version, "Version");
            }

            if (obj is ITimeVersionable)
            {
                SetProp(obj, DateTime.Now, "LastModified");
            }
        }

        private static T GetProp<T>(object obj, string name = "Id")
        {
            var type = obj.GetType();
            var idProp = type.GetProperty(name);

            return (T)idProp.GetValue(obj, null);
        }

        private static void SetProp<T>(object obj, T value, string name = "Id")
        {
            var type = obj.GetType();
            var idProp = type.GetProperty(name);

            idProp.SetValue(obj, value, null);
        }

        private static long GetNextId()
        {
            return IdGeneratorSeed++;
        }
    }
}