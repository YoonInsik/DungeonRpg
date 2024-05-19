using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Lazer : MonoBehaviour
{
    Player player;
    Vector3 direction;
    float damage = 0;
    float speed = 10.0f;
    Vector3 attackScale;

    private IObjectPool<Lazer> managedPool;
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
        transform.Translate(direction * speed * Time.fixedDeltaTime * player.ATKSpeedDelicacy(), Space.World);
    }

    public void SetManagedPool(IObjectPool<Lazer> pool)
    {
        managedPool = pool;
    }

    public void Shoot(Vector3 dir, float _damage, float _speed)
    {
        damage = _damage;
        speed = _speed;
        direction = dir; // ������ ����
        transform.localScale = attackScale * player.ATKRangeDelicacy();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90); // ��������Ʈ�� �ٶ� ���� ����
        Invoke("DestroyLazer", 2.0f); // 2�� �� ���� �ı�
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
            collision.GetComponent<Enemy>().Damaged(damage);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DestroyLazer");
    }
}
