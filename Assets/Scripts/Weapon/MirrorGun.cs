using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class MirrorGun : WeaponBase
{
    public GameObject mirrorMirrorBulletPrefab;

    private IObjectPool<MirrorBullet> pool;
    private Vector3 direction;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<MirrorBullet>(CreateMirrorBullet, OnGetMirrorBullet, OnReleaseMirrorBullet, OnDestroyMirrorBullet, maxSize:30);
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

        var mirrorMirrorBullet = pool.Get();
        mirrorMirrorBullet.transform.position = transform.position + direction.normalized;
        mirrorMirrorBullet.transform.localScale = mirrorMirrorBullet.transform.localScale * player.ATKRangeDelicacy();
        mirrorMirrorBullet.Shoot(direction.normalized, CalculateDamage(), data.speed);

        elapsedTime = 0.0f;
    }

    private MirrorBullet CreateMirrorBullet()
    {
        MirrorBullet mirrorMirrorBullet = Instantiate(mirrorMirrorBulletPrefab).GetComponent<MirrorBullet>();
        mirrorMirrorBullet.SetManagedPool(pool);
        return mirrorMirrorBullet;
    }

    private void OnGetMirrorBullet(MirrorBullet mirrorMirrorBullet)
    {
        mirrorMirrorBullet.gameObject.SetActive(true);
    }

    private void OnReleaseMirrorBullet(MirrorBullet mirrorMirrorBullet)
    {
        mirrorMirrorBullet.gameObject.SetActive(false);
    }

    private void OnDestroyMirrorBullet(MirrorBullet mirrorMirrorBullet)
    {
        Destroy(mirrorMirrorBullet.gameObject);
    }
}
