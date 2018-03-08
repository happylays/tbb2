using System;
using System.Collections.Generic;

namespace GameFramework
{
    public static class ReferencePool {
        private static readonly IDictionary<string, Queue<IReference>> s_ReferencePool = new Dictionary<string, Queue<IReference>>();

        public static void ClearAll() {
            lock (s_ReferencePool)
            {
                s_ReferencePool.Clear();
            }
        }
        public static void Clear(Type referenceType) {

            if (referenceType == null)
            {
                throw;
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw;
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw;
            }

            lock (s_ReferencePool)
            {
                GetReferencePool(referenceType.FullName).Clear();
            }
        }
        public static void Clear<T>() where T : class, IReference {
            lock (s_ReferencePool)
            {
                GetReferencePool(typeof(T).FullName).Clear();
            }
        }
        public static int Count() {
            lock (s_ReferencePool)
            {
                return s_ReferencePool.Count;
            }
        }
        public static int Count<T>() {
            lock (s_ReferencePool)
            {
                return GetReferencePool(typeof(T).FullName).Count;
            }
        }
        public static int Count(Type referenceType) {
            
            if (referenceType == null)
            {
                throw;
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw;
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw;
            }

            lock (s_ReferencePool)
            {
                return GetReferencePool(referenceType.FullName).Count;
            }
        }
        public static T Acquire<T>() where T : class, IReference, new() {
            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
                if (referencePool.Count > 0)
                {
                    return (T)referencePool.Dequeue();
                }
            }

            return new T();
        }
        public static IReference Acquire(Type referenceType) {
            if (referenceType == null) {
                throw;
            }
            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw;
            }
            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw;
            }

            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
                if (referencePool.Count > 0)
                {
                    return referencePool.Dequeue();
                }
            }

            return (IReference)Activator.CreateInstance(referenceType);
        }
        public static void Release<T>(T reference) where T : class, IReference {
            if (reference == null)
            {
                throw;
            }

            reference.Clear();
            lock (s_ReferencePool)
            {
                GetReferencePool(typeof(T).FullName).Enqueue(reference);
            }
        }
        public static void Release(Type referenceType, IReference reference) {
            if (referenceType == null)
            {

            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {

            }
            if (reference == null)
            {
                
            }

            Type type = reference.GetType();
            if (referenceType != type)
            {
                throw;
            }

            reference.Clear();
            lock (s_ReferencePool)
            {
                GetReferencePool(referenceType.FullName).Enqueue(reference);
            }
        }
        public static void Add<T>(int count) where T : class, IReference, new() {
            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
                while (count-- > 0)
                {
                    referencePool.Enqueue(new T());
                }
            }
        }
        public static vid Add(Type referenceType, int count)
        {
            if (referenceType == null)
            {

            }
            if (!referenceType.IsClass || referenceType.IsAbstract)
            {

            }
            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {

            }

            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
                while (count-- > 0)
                {
                    referencePool.Enqueue((IReference)Activator.CreateInstance(referenceType));
                }
            }

        }

        public static void Remove<T>(int count) {
            lock (s_ReferencePool) {
                Queue<IReference> referencePool = GetReferencePool(typeof(T).FullName);
                if (referencePool.Count < count)
                {
                    count = referencePool.Count;
                }

                while (count-- > 0)
                {
                    referencePool.Dequeue();
                }
            }
        }
        public static void Remove(Type referenceType, int count) {

            if (referenceType == null)
            {

            }
            if (!referenceType.IsClass || referenceType.IsAbstract)
            {

            }
            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {

            }

            lock (s_ReferencePool)
            {
                Queue<IReference> referencePool = GetReferencePool(referenceType.FullName);
                if (referencePool.Count < count)
                {
                    count = referencePool.Count;
                }

                while (count-- > 0)
                {
                    referencePool.Dequeue();
                }
            }
        }
        private static Queue<IReference> GetReferencePool(string fullName) {
            Queue<IReference> referencePool = null;
            if (!s_ReferencePool.TryGetValue(fullName, out referencePool))
            {
                referencePool = new Queue<IReference>();
                s_ReferencePool.Add(fullName, referencePool);
            }

            return referencePool;
        }

    }

}