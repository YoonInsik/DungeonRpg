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

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > data.interval * player.ATKCooldownDelicacy())
        {
            Fire();
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;

        if (dir.magnitude > data.range)
            return;

        // �Ѿ��� �߻��ϴ� �⺻ ���� ����
        FireBullet(dir.normalized); // �⺻ �������� �Ѿ� �߻�

        // �Ѿ��� �߰��� �翷���� 15���� �߻�
        FireBullet(Quaternion.Euler(0, 0, 10) * dir.normalized); // ������ 15��
        FireBullet(Quaternion.Euler(0, 0, -10) * dir.normalized); // ���� 15��
        
        elapsedTime = 0.0f;
    }

    void FireBullet(Vector3 direction)
    {
        var bullet = pool.Get();
        bullet.transform.position = transform.position + direction;
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
