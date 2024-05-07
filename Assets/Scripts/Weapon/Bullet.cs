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
    private bool isReleased = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        isReleased = false;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.fixedDeltaTime * speed);
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
        if (managedPool == null)
        {
            Debug.LogError("ManagedPool is not set properly.");
        }
        else
        {
            Debug.Log("ManagedPool set successfully.");
        }
    }

    public void Shoot(Vector3 dir)
    {
        direction = dir;
        Invoke("DestroyBullet", 5f);
    }

    public void DestroyBullet()
    {
        if (!isReleased)
        {
            if (managedPool == null)
            {
                Debug.LogError("ManagedPool is null when trying to release the bullet.");
                return; // Early exit to avoid the null reference exception
            }
            managedPool.Release(this);
            isReleased = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DestroyBullet();
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyBullet");
    }
}
