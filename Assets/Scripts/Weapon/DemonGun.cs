using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DemonGun : WeaponBase
{
    public GameObject demonBulletPrefab;

    private IObjectPool<DemonBullet> pool;
    private Vector3 direction;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<DemonBullet>(CreateDemonBullet, OnGetDemonBullet, OnReleaseDemonBullet, OnDestroyDemonBullet, maxSize: 30);
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

        var demonBullet = pool.Get();
        demonBullet.transform.position = transform.position + direction.normalized;
        demonBullet.transform.localScale = demonBullet.transform.localScale * player.ATKRangeDelicacy();
        demonBullet.Shoot(direction.normalized, CalculateDamage(), data.speed);

        elapsedTime = 0.0f;
    }

    private DemonBullet CreateDemonBullet()
    {
        DemonBullet demonBullet = Instantiate(demonBulletPrefab).GetComponent<DemonBullet>();
        demonBullet.SetManagedPool(pool);
        return demonBullet;
    }

    private void OnGetDemonBullet(DemonBullet demonBullet)
    {
        demonBullet.gameObject.SetActive(true);
    }

    private void OnReleaseDemonBullet(DemonBullet demonBullet)
    {
        demonBullet.gameObject.SetActive(false);
    }

    private void OnDestroyDemonBullet(DemonBullet demonBullet)
    {
        Destroy(demonBullet.gameObject);
    }
}
