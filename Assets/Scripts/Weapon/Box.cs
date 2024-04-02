using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Box : MonoBehaviour
{
    float damage = 1.0f;
    float elapsedTime = 0.0f;
    float interval = 3.5f;

    private IObjectPool<Box> managedPool;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > interval)
        {
            elapsedTime = 0.0f;
            DestroyBox();
        }
    }

    public void SetManagedPool(IObjectPool<Box> pool)
    {
        managedPool = pool;
    }

    public void DestroyBox()
    {
        managedPool.Release(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }
}
