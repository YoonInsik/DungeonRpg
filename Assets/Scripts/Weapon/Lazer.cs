using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Lazer : WeaponBase
{
    Player player;
    public Vector3 direction;
    float speed = 10.0f;
    private IObjectPool<Lazer> managedPool;
    private bool isReleased = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        baseDamage = 6.0f;
        attackScale = transform.localScale;
    }

    private void OnEnable()
    {
        isReleased = false;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime * player.ATKSpeedDelicacy(), Space.World);
    }

    public void SetManagedPool(IObjectPool<Lazer> pool)
    {
        managedPool = pool;
    }

    public void Shoot(Vector3 direction)
    {
        this.direction = direction; // 방향을 저장
        transform.localScale = attackScale * player.ATKRangeDelicacy();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90); // 스프라이트가 바라볼 각도 조정
        Invoke("DestroyLazer", 2.0f); // 2초 후 로켓 파괴
    }

    public void DestroyLazer()
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
        CancelInvoke("DestroyLazer");
    }
}
