
using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    public delegate LinkedList<T> ReleaseObjetFilterCallback<T>(LinkedList<T> candidateObjects, int toReleaseCount, DateTime expireTime) where T : ObjectBase;
}