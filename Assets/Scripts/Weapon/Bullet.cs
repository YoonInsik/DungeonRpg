using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    Player player;

    public Vector3 direction;
    float speed = 10.0f;

    private IObjectPool<Bullet> managedPool;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.fixedDeltaTime * speed);
    }

    public void SetManagedPool(IObjectPool<Bullet> Pool)
    {
        managedPool = Pool;
    }

    public void Shoot(Vector3 dir)
    {
        direction = dir;

        Invoke("DestroyBullet", 5f);
    }

    public void DestroyBullet()
    {
        managedPool.Release(this);
    }



}
