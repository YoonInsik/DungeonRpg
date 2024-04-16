using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Bomb : MonoBehaviour
{
    Player player;
    float speed = 5.0f;
    Vector3 direction;

    private IObjectPool<Bomb> managedPool;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    public void Shoot(Vector3 dir)
    {
        direction = dir;
    }
    public void SetManagedPool(IObjectPool<Bomb> pool)
    {
        managedPool = pool;
    }

    public void DestroyBomb()
    {
        managedPool.Release(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
        }
    }
}
