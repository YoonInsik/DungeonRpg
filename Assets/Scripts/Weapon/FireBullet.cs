using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class FireBullet : WeaponBase
{
    Player player;
    public Vector3 direction;
    float speed = 5.0f;
    private IObjectPool<FireBullet> managedPool;
    private bool isReleased = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        baseDamage = 1.0f;
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

    public void SetManagedPool(IObjectPool<FireBullet> pool)
    {
        managedPool = pool;
    }

    public void Shoot(Vector3 dir)
    {
        direction = dir;
        transform.localScale = attackScale * player.ATKRangeDelicacy();
        Invoke("DestroyFireBullet", 1.5f);
    }

    public void DestroyFireBullet()
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
            float damage = CalculateDamage();
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyFireBullet");
    }
}
