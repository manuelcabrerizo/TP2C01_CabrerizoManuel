using System;
using System.Collections.Generic;
using System.Diagnostics;

public class PoolAllocator<T>
{
    public class PoolObject
    {
        public T obj;
        public PoolObject next;
        public bool isFree;
    }

    private PoolObject free = null;
    private Func<T> onCreate = null;
    private Action<T> onDestroy = null;
    private Action<T> onGet = null;
    private Action<T> onRelease = null;

    // dictionary use to get references to PoolObject from the T type object
    // use for quick look up on the release
    private Dictionary<T, PoolObject> toPoolObject;

    private int spawCount = 0;

    public PoolAllocator(Func<T> onCreate, Action<T> onDestroy, Action<T> onGet, Action<T> onRelease)
    {
        toPoolObject = new Dictionary<T, PoolObject>();
        free = null;
        this.onCreate = onCreate;
        this.onDestroy = onDestroy;
        this.onGet = onGet;
        this.onRelease = onRelease;
    }

    public int GetSpawnCount()
    {
        return spawCount;
    }

    public T Get()
    {
        PoolObject po = null;
        if(free == null)
        {
            // Alloc a new object
            po = new PoolObject();
            po.obj = onCreate();
            toPoolObject.Add(po.obj, po);
        }
        else
        {
            // use the first object from the free list
            po = free;
            Debug.Assert(po.isFree == true);
            free = free.next;
        }
        // se to default values
        po.next = null;
        po.isFree = false;
        onGet(po.obj);
        spawCount++;
        return po.obj;
    }

    public void Release(T obj)
    {
        PoolObject po = toPoolObject[obj];
        Debug.Assert(po != null, "object allocated from another pool");
        Debug.Assert(po.isFree == false, "object has already been released");
        po.isFree = true;
        onRelease(po.obj);

        // add the object to the free linklist
        po.next = free;
        free = po; 

        spawCount--;
    }
}