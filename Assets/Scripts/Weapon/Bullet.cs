using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Pool;
using SHS;

public class Bullet : WeaponBase
{
    Player player;
    public Vector3 direction;
    float speed = 10.0f;
    private IObjectPool<Bullet> managedPool;
    private bool isReleased = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        baseDamage = 5.0f;
        attackScale = transform.localScale;
    }

    private void OnEnable()
    {
        isReleased = false;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.fixedDeltaTime * speed * player.ATKSpeedDelicacy());
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    public void Shoot(Vector3 dir)
    {
        direction = dir;
        transform.localScale = attackScale * player.ATKRangeDelicacy();
        Invoke("DestroyBullet", 5f);
    }

    public void DestroyBullet()
    {
        if (!isReleased)
        {
            managedPool.Release(this);
            isReleased = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DestroyBullet();
            float damage = CalculateDamage();
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyBullet");
    }
}