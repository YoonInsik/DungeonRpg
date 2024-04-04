using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Pool;
using SHS;

public class Bullet : MonoBehaviour
{
    Player player;

    public Vector3 direction;
    float speed = 10.0f;
    float damage = 1.0f;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DestroyBullet();
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }


}
