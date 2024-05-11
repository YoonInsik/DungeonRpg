using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireGun : MonoBehaviour
{
    public GameObject fireBulletPrefab;
    public float interval = 1.0f;
    float elapsedTime = 0.0f;
    public Player player;

    private IObjectPool<FireBullet> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<FireBullet>(CreateFireBullet, OnGetFireBullet, OnReleaseFireBullet, OnDestroyFireBullet, maxSize: 30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > interval)
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
        //var fireBullet = Instantiate(fireBulletPrefab, transform.position, Quaternion.identity).GetComponent<FireBullet>();
        var fireBullet = pool.Get();
        fireBullet.transform.position = transform.position + dir.normalized;
        fireBullet.Shoot(dir.normalized);
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
