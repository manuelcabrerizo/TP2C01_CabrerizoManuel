using System;
using System.Collections.Generic;
using System.Diagnostics;

public class PoolAllocator<T>
{
    public class PoolObject
    {
        public T obj;
        public PoolObject prev;
        public PoolObject next;

        public bool isFree;
    }

    private PoolObject first = null;
    private PoolObject free = null;

    private Func<T> onCreate = null;
    private Action<T> onDestroy = null;
    private Action<T> onGet = null;
    private Action<T> onRelease = null;

    private Dictionary<T, PoolObject> toPoolObject;

    public PoolAllocator(Func<T> onCreate, Action<T> onDestroy, Action<T> onGet, Action<T> onRelease)
    {
        toPoolObject = new Dictionary<T, PoolObject>();
        first = null;
        free = null;
        this.onCreate = onCreate;
        this.onDestroy = onDestroy;
        this.onGet = onGet;
        this.onRelease = onRelease;
    }

    public void Clear()
    {
        PoolObject po = first;
        while(po != null)
        {
            onRelease(po.obj);
            onDestroy(po.obj);
            toPoolObject.Remove(po.obj);
            po.obj = default;
            po = po.next;
        }
        toPoolObject.Clear();
        first = null;
        free = null;
    }

    public T Get()
    {
        PoolObject po = null;
        if(free == null)
        {
            // Alloc a new game object
            po = new PoolObject();
            po.obj = onCreate();
            toPoolObject.Add(po.obj, po);
        }
        else
        {
            // use the first free
            po = free;
            Debug.Assert(po.isFree == true);
            free = free.next;
        }

        po.next = null;
        po.prev = null;
        po.isFree = false;
        onGet(po.obj);

        if(first == null)
        {
            first = po;
        }
        else
        {
            first.prev = po;
            po.next = first;
            first = po;
        }

        return first.obj;
    }

    public void Release(T obj)
    {
        // add it to the free list
        PoolObject po = toPoolObject[obj];
        Debug.Assert(po != null, "object allocated from another pool");
        Debug.Assert(po.isFree == false, "object has already been released");
        po.isFree = true;
        if(first == po)
        {
            first = po.next;
        }

        if(free != null)
        {
            free.prev = po;
            po.next = free;
        }
        
        onRelease(po.obj);

        // remove it from the double linklist
        if(po.prev != null)
        {
            po.prev.next = po.next;
        }

        if(po.next != null)
        {
            po.next.prev = po.prev; 
        }
        free = po; 
    }
}
