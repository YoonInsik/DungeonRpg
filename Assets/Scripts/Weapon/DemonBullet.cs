using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DemonBullet : MonoBehaviour
{
    Player player;
    Vector3 direction;
    float damage;
    float speed = 10.0f;
    private IObjectPool<DemonBullet> managedPool;
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

    public void SetManagedPool(IObjectPool<DemonBullet> pool)
    {
        managedPool = pool;
    }

    public void Shoot(Vector3 dir, float _damage, float _speed)
    {
        direction = dir;
        damage = _damage;
        speed = _speed;
        transform.localScale = attackScale * player.ATKRangeDelicacy();
        Invoke("DestroyDemonBullet", 5f);
    }

    public void DestroyDemonBullet()
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
            DestroyDemonBullet();
            collision.GetComponent<Enemy>().Damaged(damage);
            player.HP += Mathf.RoundToInt(damage * 0.1f);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyDemonBullet");
    }
}
