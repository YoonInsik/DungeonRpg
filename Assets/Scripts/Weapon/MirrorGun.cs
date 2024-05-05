using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class MirrorGun : WeaponBase
{
    public GameObject mirrorMirrorBulletPrefab;
    public float interval = 0.2f;
    float elapsedTime = 0.0f;
    public Player player;

    private IObjectPool<MirrorBullet> pool;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        pool = new ObjectPool<MirrorBullet>(CreateMirrorBullet, OnGetMirrorBullet, OnReleaseMirrorBullet, OnDestroyMirrorBullet, maxSize:30);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > interval)
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
        //var mirrorMirrorBullet = Instantiate(mirrorMirrorBulletPrefab, transform.position, Quaternion.identity).GetComponent<MirrorBullet>();
        var mirrorMirrorBullet = pool.Get();
        mirrorMirrorBullet.transform.position = transform.position + dir.normalized;
        mirrorMirrorBullet.Shoot(dir.normalized);
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
