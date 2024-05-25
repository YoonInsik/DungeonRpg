using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using SHS;

public class Bullet : MonoBehaviour
{
    Player player;
    Vector3 direction;
    float damage = 0;
    float speed = 10.0f;
    Vector3 attackScale;
    
    private IObjectPool<Bullet> managedPool;
    private bool isReleased = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
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

    public void Shoot(Vector3 dir, float _damage, float _speed)
    {
        direction = dir;
        damage = _damage;
        speed = _speed;
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
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyBullet");
    }
}