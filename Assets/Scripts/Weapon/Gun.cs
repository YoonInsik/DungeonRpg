using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Gun : WeaponBase
{
    public GameObject bulletPrefab;
    public float interval = 0.2f;
    float elapsedTime = 0.0f;
    public Player player;

    private IObjectPool<Bullet> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > interval * player.ATKCooldownDelicacy())
        {
            elapsedTime = 0.0f;
            Fire();
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        //var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        var bullet = pool.Get();
        bullet.transform.position = transform.position + dir.normalized;
        bullet.Shoot(dir.normalized);
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
