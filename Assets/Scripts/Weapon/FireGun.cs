using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireGun : WeaponBase
{
    public GameObject fireBulletPrefab;

    private IObjectPool<FireBullet> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<FireBullet>(CreateFireBullet, OnGetFireBullet, OnReleaseFireBullet, OnDestroyFireBullet, maxSize: 30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > data.interval)
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

        //var fireBullet = Instantiate(fireBulletPrefab, transform.position, Quaternion.identity).GetComponent<FireBullet>();
        var fireBullet = pool.Get();
        fireBullet.transform.position = transform.position + dir.normalized;
        fireBullet.Shoot(dir.normalized, CalculateDamage(), data.speed);

        elapsedTime = 0.0f;
    }

    private FireBullet CreateFireBullet()
    {
        FireBullet fireBullet = Instantiate(fireBulletPrefab).GetComponent<FireBullet>();
        fireBullet.SetManagedPool(pool);
        return fireBullet;
    }

    private void OnGetFireBullet(FireBullet fireBullet)
    {
        fireBullet.gameObject.SetActive(true);
    }

    private void OnReleaseFireBullet(FireBullet fireBullet)
    {
        fireBullet.gameObject.SetActive(false);
    }

    private void OnDestroyFireBullet(FireBullet fireBullet)
    {
        Destroy(fireBullet.gameObject);
    }
}
