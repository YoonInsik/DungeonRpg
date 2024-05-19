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
        if(elapsedTime > data.interval)
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

        // 총알을 발사하는 기본 방향 설정
        FireBullet(dir.normalized); // 기본 방향으로 총알 발사

        // 총알을 추가로 양옆으로 15도씩 발사
        FireBullet(Quaternion.Euler(0, 0, 10) * dir.normalized); // 오른쪽 15도
        FireBullet(Quaternion.Euler(0, 0, -10) * dir.normalized); // 왼쪽 15도
        
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
