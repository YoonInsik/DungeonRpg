using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ShotGun : WeaponBase
{
    public GameObject bulletPrefab;

    private IObjectPool<Bullet> pool;
    private Vector3 direction;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.position = player.transform.position + offset;

        if (player?.scanner?.nearestTarget == null) return;
        direction = player.scanner.nearestTarget.position - transform.position;

        if (direction.magnitude <= data.range)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            Fire();
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void Fire()
    {
        if (elapsedTime < data.interval * player.ATKCooldownDelicacy()) return;

        // �Ѿ��� �߻��ϴ� �⺻ ���� ����
        FireBullet(direction.normalized); // �⺻ �������� �Ѿ� �߻�

        // �Ѿ��� �߰��� �翷���� 15���� �߻�
        FireBullet(Quaternion.Euler(0, 0, 10) * direction.normalized); // ������ 15��
        FireBullet(Quaternion.Euler(0, 0, -10) * direction.normalized); // ���� 15��
        
        elapsedTime = 0.0f;
    }

    void FireBullet(Vector3 direction)
    {
        var bullet = pool.Get();
        bullet.transform.position = transform.position + direction;
        bullet.transform.localScale = bullet.transform.localScale * player.ATKRangeDelicacy();
        bullet.Shoot(direction, CalculateDamage(), data.speed);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        bullet.SetManagedPool(pool);
        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
