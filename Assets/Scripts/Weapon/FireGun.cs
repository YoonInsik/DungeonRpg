using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireGun : WeaponBase
{
    public GameObject fireBulletPrefab;

    private IObjectPool<FireBullet> pool;
    private Vector3 direction;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<FireBullet>(CreateFireBullet, OnGetFireBullet, OnReleaseFireBullet, OnDestroyFireBullet, maxSize: 30);
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

        var fireBullet = pool.Get();
        fireBullet.transform.position = transform.position + direction.normalized;
        fireBullet.transform.localScale = fireBullet.transform.localScale * player.ATKRangeDelicacy();
        fireBullet.Shoot(direction.normalized, CalculateDamage(), data.speed);

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
