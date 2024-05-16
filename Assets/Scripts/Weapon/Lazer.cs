using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Lazer : MonoBehaviour
{
    Player player;
    public Vector3 direction;
    float speed = 10.0f;
    float damage = 1.0f;
    private IObjectPool<Lazer> managedPool;
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
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    public void SetManagedPool(IObjectPool<Lazer> pool)
    {
        managedPool = pool;
    }

    public void Shoot(Vector3 direction)
    {
        this.direction = direction; // ������ ����
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