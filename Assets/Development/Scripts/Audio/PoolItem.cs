using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolItem : MonoBehaviour
{
    private Pooling _Pool;

    public Pooling Pool
    {
        set { _Pool = value; }
    }

    protected abstract void Reset();

    public virtual void Initiate()
    {
        Reset();
        gameObject.SetActive(true);
    }

    public virtual void ReturnToPool()
    {
        _Pool.AddItem(this);
    }
}
